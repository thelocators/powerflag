using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skainix.Syndication;

namespace PowerFlag
{
	public partial class Recent : Form
	{
		public Recent()
		{
			InitializeComponent();
		}

		public void LoadFeeds(IEnumerable<SyndicatedFeed> feeds)
		{
			richTextBox1.Clear();
			StringBuilder sb = new StringBuilder();
			foreach (SyndicatedFeed f in feeds)
			{
				var flagged = from SyndicatedFeed.Item i in f.FeedItems
							  where i.HasBeenRead
							  orderby i.PubDate
							  select i;

				sb.AppendFormat("Feed: {0}\r\n", f.Url);

				if (flagged != null && flagged.Count() > 0)
				{
					foreach (SyndicatedFeed.Item i in flagged)
					{
						sb.AppendFormat("\tFlagged: {0}\r\n", i.Title);
					}
				}

				sb.AppendLine();
			}


			richTextBox1.Text = sb.ToString();
		}
	}
}
