using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerFlag
{
	public partial class EditForm : Form
	{
		private string curUrl;
		private string curRule;

		public EditForm()
		{
			InitializeComponent();
		}

		private void EditForm_Load(object sender, EventArgs e)
		{
			loadFormFromFile();
		}

		private void loadFormFromFile()
		{
			loadFormFromFile(0);
		}

		private void loadFormFromFile(int selectedFeedIndex)
		{
			List<FeedToFlag> feeds = FeedToFlag.GetListFromFile(PowerFlag.SettingsFilepath);

			feedsLB.Items.Clear();
			var query = from ftf in feeds
						orderby ftf.Url
						select ftf.Url;

			if (query != null)
			{
				feedsLB.Items.AddRange(query.ToArray());
			}

			feedsLB.SelectedIndex = selectedFeedIndex;
		}

		private void feedsLB_SelectedIndexChanged(object sender, EventArgs e)
		{
			rulesLB.Items.Clear();

			curUrl = feedsLB.SelectedItem.ToString();

			urlTB.Text = curUrl;

			FeedToFlag ftf = getFeedToFlagFromFile(curUrl);
			ftf.FlagRules.Sort();
			rulesLB.Items.AddRange(ftf.FlagRules.ToArray());

			if (rulesLB.Items.Count > 0)
			{
				rulesLB.SelectedIndex = 0;
			}
			else
			{
				ruleTB.Clear();
				curRule = string.Empty;
			}
		}

		private FeedToFlag getFeedToFlagFromFile(string url)
		{
			List<FeedToFlag> feeds = FeedToFlag.GetListFromFile(PowerFlag.SettingsFilepath);

			return getFeedToFlagFromList(url, feeds);
		}

		private static FeedToFlag getFeedToFlagFromList(string url, List<FeedToFlag> feeds)
		{
			foreach (FeedToFlag ftf in feeds)
			{
				if (ftf.Url == url)
				{
					return ftf;
				}
			}

			return null;
		}

		private void rulesLB_SelectedIndexChanged(object sender, EventArgs e)
		{
			curRule = rulesLB.SelectedItem.ToString();
			ruleTB.Text = curRule;
		}

		private void newFeedBTN_Click(object sender, EventArgs e)
		{
			urlTB.Text = string.Empty;
			curUrl = string.Empty;
		}

		private void newRuleBTN_Click(object sender, EventArgs e)
		{
			ruleTB.Text = string.Empty;
			curRule = string.Empty;
		}

		private void deleteFeedBTN_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(urlTB.Text))
			{
				feedsLB.SelectedIndex = 0;
				return;
			}

			List<FeedToFlag> feeds = FeedToFlag.GetListFromFile(PowerFlag.SettingsFilepath);

			FeedToFlag toRemove = getFeedToFlagFromList(curUrl, feeds);

			if (toRemove == null)
			{
				MessageBox.Show(string.Format("Could not find feed {0} in settings file.", urlTB.Text));
				return;
			}

			feeds.Remove(toRemove);

			FeedToFlag.SaveListToFile(PowerFlag.SettingsFilepath, feeds);
			loadFormFromFile();
		}

		private void saveFeedBTN_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(curUrl))
			{
				addFeed();
			}
			else
			{
				updateFeed();
			}
		}

		private void updateFeed()
		{
			FeedToFlag feed = getFeedToFlagFromFile(curUrl);
			feed.Url = urlTB.Text;
		}

		private void addFeed()
		{
			FeedToFlag ftf = new FeedToFlag { Url = urlTB.Text };
			List<FeedToFlag> feeds = FeedToFlag.GetListFromFile(PowerFlag.SettingsFilepath);
			feeds.Add(ftf);
			FeedToFlag.SaveListToFile(PowerFlag.SettingsFilepath, feeds);
			loadFormFromFile();
		}

		private void updateRule()
		{
			List<FeedToFlag> feeds = FeedToFlag.GetListFromFile(PowerFlag.SettingsFilepath);
			FeedToFlag curFeed = getFeedToFlagFromList(curUrl, feeds);
			curFeed.FlagRules.Remove(curRule);
			curFeed.FlagRules.Add(ruleTB.Text);
			FeedToFlag.SaveListToFile(PowerFlag.SettingsFilepath, feeds);
			loadFormFromFile(feedsLB.SelectedIndex);
		}

		private void addRule()
		{
			List<FeedToFlag> feeds = FeedToFlag.GetListFromFile(PowerFlag.SettingsFilepath);
			FeedToFlag curFeed = getFeedToFlagFromList(curUrl, feeds);
			curFeed.FlagRules.Add(ruleTB.Text);
			FeedToFlag.SaveListToFile(PowerFlag.SettingsFilepath, feeds);
			loadFormFromFile(feedsLB.SelectedIndex);
		}

		private void saveRuleBTN_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(curRule))
			{
				addRule();
			}
			else
			{
				updateRule();
			}
		}

		private void deleteRuleBTN_Click(object sender, EventArgs e)
		{
			List<FeedToFlag> feeds = FeedToFlag.GetListFromFile(PowerFlag.SettingsFilepath);
			FeedToFlag curFeed = getFeedToFlagFromList(curUrl, feeds);
			curFeed.FlagRules.Remove(curRule);
			FeedToFlag.SaveListToFile(PowerFlag.SettingsFilepath, feeds);
			loadFormFromFile(feedsLB.SelectedIndex);
		}

		
	}
}
