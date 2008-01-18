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

namespace PowerFlag
{
	public partial class PowerFlag : Form
	{
		public static readonly string SettingsFilepath = "settings.xml";
		public static readonly string FeedArchiveFilepath = "feedArchive.xml";
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


				List<FlaggedItem> flaggedItems = feedToFlag.SearchAndFlag(feed);
				logger.Debug("{0} items to flag . . .", flaggedItems.Count);
				foreach (FlaggedItem item in flaggedItems)
				{
					logger.Debug(string.Format("Flagged: {0}\r\n", item.Title));
				}


				int numRemoved = feed.PurgeOldItems(Properties.Settings.Default.NumItemsToArchive);
				logger.Debug(string.Format("{0} old items purged from feed.\r\n", numRemoved));

				logger.Debug("\r\n\r\n");
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
	}
}
