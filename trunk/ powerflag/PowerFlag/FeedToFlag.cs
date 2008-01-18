using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using NLog;
using Skainix.Syndication;
using System.Text.RegularExpressions;

namespace PowerFlag
{
	[Serializable]
	public class FeedToFlag
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public SyndicatedFeed Feed
		{
			get;
			set;
		}

		public List<string> FlagRules
		{
			get;
			set;
		}

		public FeedToFlag()
		{
			FlagRules = new List<string>();
		}

		public List<FlaggedItem> SearchAndFlag()
		{
			Feed.LoadFeed();
			List<FlaggedItem> itemsToFlag = getItemsToFlag();

			flagItems(itemsToFlag);

			return itemsToFlag;
		}

		private List<FlaggedItem> getItemsToFlag()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("(");
			foreach (string rule in FlagRules)
			{
				sb.AppendFormat(@"\W{0}\W|", rule);
			}

			sb.Remove(sb.Length - 1, 1);

			sb.Append(")");

			string reStr = sb.ToString();
			logger.Debug("Title Search Regex: {0}", reStr);
			Regex re = new Regex(reStr, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			List<FlaggedItem> itemsToFlag = new List<FlaggedItem>();

			foreach (SyndicatedFeed.Item item in Feed.FeedItems)
			{
				if (item.HasBeenRead)
				{
					//this item has already been processed so move along
					continue;
				}
				if (re.IsMatch(item.Title))
				{
					itemsToFlag.Add(new FlaggedItem
					{
						FlaggedRegEx = re.ToString(),
						Title = item.Title,
						Url = item.Link
					});
					item.HasBeenRead = true;
				}

			}

			return itemsToFlag;
		}

		private void flagItems(List<FlaggedItem> flaggedItems)
		{
			Regex itemNumRe = new Regex(@".*\/(?<num>.*?).htm", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex resultRe = new Regex("thanks for flagging", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex urlRe = new Regex("(?<url>http://.*?/)", RegexOptions.IgnoreCase);

			if (!urlRe.IsMatch(Feed.Url))
			{
				logger.Error("Error in FeedToFlag.flagItems: Could not parse base URL out of Feed URL: {0}", Feed.Url);
				return;
			}

			string baseUrl = urlRe.Match(Feed.Url).Groups["url"].Value;

			foreach (FlaggedItem item in flaggedItems)
			{
				MatchCollection matches = itemNumRe.Matches(item.Url);
				if (matches != null && matches.Count == 1)
				{
					string itemNum = matches[0].Groups["num"].Value;
					string link = string.Format("{0}flag/?flagCode=15&postingID={1}", baseUrl, itemNum);
					string result = flagItem(link);
					if (!resultRe.IsMatch(result))
					{
						item.Title = string.Concat(item.Title, " -- COULD NOT FLAG!");
					}

				}
			}
		}

		private string flagItem(string link)
		{
			HttpWebRequest request = WebRequest.Create(link) as HttpWebRequest;
			HttpWebResponse response = request.GetResponse() as HttpWebResponse;

			using (StreamReader sr = new StreamReader(response.GetResponseStream()))
			{
				string result = sr.ReadToEnd();

				return result;
			}
		}

		private FlaggedItem checkFeedAgainstRule(string rule, SyndicatedFeed feed)
		{
			Regex titleRe = new Regex(rule, RegexOptions.Compiled | RegexOptions.IgnoreCase);

			foreach (SyndicatedFeed.Item item in feed.FeedItems)
			{
				if (titleRe.IsMatch(item.Title))
				{
					return new FlaggedItem
					{
						FlaggedRegEx = rule,
						Title = item.Title,
						Url = item.Link
					};
				}

			}

			return null;
		}


		public static List<FeedToFlag> GetListFromFile(string filepath)
		{
			using (StreamReader sr = new StreamReader(filepath))
			{
				XmlSerializer xs = new XmlSerializer(typeof(List<FeedToFlag>));
				return xs.Deserialize(sr) as List<FeedToFlag>;
			}
		}

		public static void SaveListToFile(string filepath, List<FeedToFlag> list)
		{
			using (StreamWriter sw = new StreamWriter(filepath))
			{
				XmlSerializer xs = new XmlSerializer(list.GetType());
				xs.Serialize(sw, list);
			}
		}
	}
}
