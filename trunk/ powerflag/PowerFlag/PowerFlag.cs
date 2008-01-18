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

namespace PowerFlag
{
	public partial class PowerFlag : Form
	{
		public static readonly string SettingsFilepath = "settings.xml";
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public PowerFlag()
		{
			InitializeComponent();
			//NLog.Targets.RichTextBoxTarget target = new NLog.Targets.RichTextBoxTarget();
			//target.Layout = "${message}";
			//target.ControlName = "outputRTB";
			//target.FormName = "PowerFlag";
			//target.UseDefaultRowColoringRules = false;

			//NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
		}

		private static void createSettingsFile()
		{
			List<FeedToFlag> list = new List<FeedToFlag>();

			list.Add(new FeedToFlag
			{
				FlagRules = new List<string> { "buttons", "experienced producer" },
				Feed = new SyndicatedFeed { Url = "http://chicago.craigslist.org/chc/muc/index.rss" }
			});

			list.Add(new FeedToFlag
			{
				FlagRules = new List<string> { "buttons", "piano" },
				Feed = new SyndicatedFeed { Url = "http://chicago.craigslist.org/chc/msg/index.rss" }
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
			if (!System.IO.File.Exists(SettingsFilepath))
			{
				logger.Debug("Settings file not found.  Creating new . . .");
				createSettingsFile();
			}

			List<FeedToFlag> feeds = FeedToFlag.GetListFromFile(SettingsFilepath);

			foreach (FeedToFlag feed in feeds)
			{
				logger.Debug("Feed: {0}\r\n\r\n", feed.Feed.Url);

				int numRemoved = feed.Feed.PurgeOldItems(Properties.Settings.Default.NumItemsToArchive);
				logger.Debug(string.Format("{0} old items purged from feed.\r\n", numRemoved));

				List<FlaggedItem> flaggedItems = feed.SearchAndFlag();
				logger.Debug("{0} items to flag . . .", flaggedItems.Count);
				foreach (FlaggedItem item in flaggedItems)
				{
					logger.Debug(string.Format("Flagged: {0}\r\n", item.Title));
				}

				logger.Debug("\r\n\r\n");
			}

			FeedToFlag.SaveListToFile(SettingsFilepath, feeds);
		}

		private void settingsBTN_Click(object sender, EventArgs e)
		{

		}
	}
}
