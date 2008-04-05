using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NLog;
using Skainix.Syndication;
using Skainix.SerializationUtils;
using System.IO;
using System.Xml.Serialization;
using NLog.Targets;

namespace PowerFlag
{
	public partial class PowerFlag : Form
	{
		public static readonly string SettingsFilepath = string.Concat(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"\settings.xml");
		public static readonly string FeedArchiveFilepath = string.Concat(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"\feedArchive.xml");
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public PowerFlag()
		{
			InitializeComponent();
		}

		private static void createSettingsFile()
		{
			List<FeedToFlag> list = new List<FeedToFlag>();

			list.Add(new FeedToFlag
			{
				FlagRules = new List<string> { "buttons", "experienced producer" },
				 Url = "http://chicago.craigslist.org/chc/muc/index.rss" 
			});

			list.Add(new FeedToFlag
			{
				FlagRules = new List<string> { "buttons", "piano" },
				 Url = "http://chicago.craigslist.org/chc/msg/index.rss" 
			});

			FeedToFlag.SaveListToFile(SettingsFilepath, list);
			logger.Debug("New Settings file created.");
		}

		private void flagBTN_Click(object sender, EventArgs e)
		{
			DoFlagging();
		}

		internal static void DoFlagging()
		{
			//logger.Debug("\r\nSettingsFilepath:{0}\r\nFeedArchiveFilepath:{1}", SettingsFilepath, FeedArchiveFilepath);
			if (!File.Exists(SettingsFilepath))
			{
				logger.Debug("Settings file not found.  Creating new . . .");
				createSettingsFile();
			}

			SerializableDictionary<string, SyndicatedFeed> feedLookup = getFeedLookup();

			List<FeedToFlag> feeds = FeedToFlag.GetListFromFile(SettingsFilepath);

			foreach (FeedToFlag feedToFlag in feeds)
			{
				logger.Debug("Feed: {0}\r\n\r\n", feedToFlag.Url);

				if (!feedLookup.ContainsKey(feedToFlag.Url))
				{
					feedLookup.Add(feedToFlag.Url, new SyndicatedFeed { Url = feedToFlag.Url });
				}

				SyndicatedFeed feed = feedLookup[feedToFlag.Url];

				try
				{

					List<FlaggedItem> flaggedItems = feedToFlag.SearchAndFlag(feed); 
					logger.Debug("{0} items to flag . . .", flaggedItems.Count);
					foreach (FlaggedItem item in flaggedItems)
					{
						logger.Debug(string.Format("Flagged ({1}): {0}\r\n", item.Title, item.FlaggedRegEx));
					}


					int numRemoved = feed.PurgeOldItems(Properties.Settings.Default.NumItemsToArchive);
					logger.Debug(string.Format("{0} old items purged from feed.\r\n", numRemoved));

					logger.Debug("\r\n\r\n");
				}
				catch (Exception e)
				{
					logger.Error("Error reading feed {0}:{1}", feed.Url, e);
					continue;
				}
			
			}

			saveFeedLookup(feedLookup);
		}

		private static void saveFeedLookup(SerializableDictionary<string, SyndicatedFeed> feedLookup)
		{
			using (StreamWriter sw = new StreamWriter(FeedArchiveFilepath))
			{
				XmlSerializer xs = new XmlSerializer(typeof(SerializableDictionary<string, SyndicatedFeed>));
				xs.Serialize(sw, feedLookup);
			}
		}

		private static SerializableDictionary<string, SyndicatedFeed> getFeedLookup()
		{
			if (!File.Exists(FeedArchiveFilepath))
			{
				logger.Debug("No feed archive found.  Creating a new one.");
				return new SerializableDictionary<string, SyndicatedFeed>();
			}

			using (StreamReader sr = new StreamReader(FeedArchiveFilepath))
			{
				XmlSerializer xs = new XmlSerializer(typeof(SerializableDictionary<string, SyndicatedFeed>));
				return xs.Deserialize(sr) as SerializableDictionary<string, SyndicatedFeed>;
			}
		}

		private void settingsBTN_Click(object sender, EventArgs e)
		{

		}

		private void importBTN_Click(object sender, EventArgs e)
		{
			openFileDialog1.AddExtension = true;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.CheckPathExists = true;
			openFileDialog1.DefaultExt = ".xml";
			openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			openFileDialog1.Multiselect = false;
			openFileDialog1.Title = "Import Settings";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				importSettings(openFileDialog1.FileName);
			}
		}

		private void importSettings(string newFilepath)
		{
			List<FeedToFlag> newSettings = FeedToFlag.GetListFromFile(newFilepath);
			if (newSettings == null || newSettings.Count < 1)
			{
				logger.Info("No settings to import in file '{0}'", newFilepath);
				return;
			}

			List<FeedToFlag> oldSettings = FeedToFlag.GetListFromFile(SettingsFilepath);
			Dictionary<string, FeedToFlag> lookup = oldSettings.ToDictionary(ftf => ftf.Url.ToLower());

			foreach (FeedToFlag ftf in newSettings)
			{
				logger.Info("Importing settings for feed: {0}", ftf.Url);
				if (lookup.ContainsKey(ftf.Url.ToLower()))
				{
					logger.Info("Feed already exists.  Importing FlagRules");
					HashSet<string> unique = new HashSet<string>(lookup[ftf.Url.ToLower()].FlagRules);
					foreach (string rule in ftf.FlagRules)
					{
						if (unique.Add(rule))
						{
							logger.Info("Adding Rule: {0}", rule);
						}
					}

					lookup[ftf.Url.ToLower()].FlagRules = new List<string>(unique);
				}
				else
				{
					logger.Info("Adding Feed and FlagRules.");
					oldSettings.Add(ftf);
				}
				logger.Info("Feed imported.");
			}

			FeedToFlag.EnsureFlagRulesAreUnique(oldSettings);

			FeedToFlag.SaveListToFile(SettingsFilepath, oldSettings);

			logger.Info("All settings imported.");
		}
		//private void importSettings(string newFilepath)
		//{
		//    List<FeedToFlag> newSettings = FeedToFlag.GetListFromFile(newFilepath);
		//    if (newSettings != null && newSettings.Count > 0)
		//    {
		//        List<FeedToFlag> oldSettings = FeedToFlag.GetListFromFile(SettingsFilepath);
		//        Dictionary<string, FeedToFlag> lookup = oldSettings.ToDictionary(ftg => ftg.Url.ToLower());

		//        FeedToFlag.EnsureFlagRulesAreUnique(newSettings);

		//        foreach (FeedToFlag ftg in newSettings)
		//        {
		//            logger.Info("Importing settings for feed: {0}", ftg.Url);
		//            if (lookup.ContainsKey(ftg.Url.ToLower()))
		//            {
		//                logger.Info("Feed already exists.  Importing FlagRules");
		//                lookup[ftg.Url.ToLower()].FlagRules.AddRange(ftg.FlagRules);
		//            }
		//            else
		//            {
		//                logger.Info("Adding Feed and FlagRules.");
		//                oldSettings.Add(ftg);
		//            }
		//        }

		//        FeedToFlag.EnsureFlagRulesAreUnique(oldSettings);

		//        FeedToFlag.SaveListToFile(SettingsFilepath, oldSettings);

		//        logger.Info("Settings imported.");
		//    }
		//}

		private void editBTN_Click(object sender, EventArgs e)
		{
			EditForm form = new EditForm();
			form.Show();
		}

		private void recentBTN_Click(object sender, EventArgs e)
		{
			SerializableDictionary<string, SyndicatedFeed> feedLookup = getFeedLookup();
			Recent r = new Recent();
			r.LoadFeeds(feedLookup.Values);
			r.Show();
		}
	}
}
