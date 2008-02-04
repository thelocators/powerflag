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

		//public SyndicatedFeed Feed
		//{
		//    get;
		//    set;
		//}
		public string Url
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

		public List<FlaggedItem> SearchAndFlag(SyndicatedFeed feed)
		{
			feed.LoadFeed();
			List<FlaggedItem> itemsToFlag = getItemsToFlag(feed);

			flagItems(feed, itemsToFlag);

			return itemsToFlag;
		}

		private List<FlaggedItem> getItemsToFlag(SyndicatedFeed feed)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("(");
			foreach (string rule in FlagRules)
			{
				sb.AppendFormat(@"(^|\W){0}($|\W)|", rule);
			}

			sb.Remove(sb.Length - 1, 1);

			sb.Append(")");

			string reStr = sb.ToString();
			//logger.Debug("Title Search Regex: {0}", reStr);
			Regex re = new Regex(reStr, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			List<FlaggedItem> itemsToFlag = new List<FlaggedItem>();

			foreach (SyndicatedFeed.Item item in feed.FeedItems)
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

		private void flagItems(SyndicatedFeed feed, List<FlaggedItem> flaggedItems)
		{
			Regex itemNumRe = new Regex(@".*\/(?<num>.*?).htm", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex resultRe = new Regex("thanks for flagging", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex urlRe = new Regex("(?<url>http://.*?/)", RegexOptions.IgnoreCase);

			if (!urlRe.IsMatch(feed.Url))
			{
				logger.Error("Error in FeedToFlag.flagItems: Could not parse base URL out of Feed URL: {0}", feed.Url);
				return;
			}

			string baseUrl = urlRe.Match(feed.Url).Groups["url"].Value;

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
			EnsureFlagRulesAreUnique(list);
			foreach (FeedToFlag ftf in list)
			{
				ftf.FlagRules.Sort();
			}

			using (StreamWriter sw = new StreamWriter(filepath))
			{
				XmlSerializer xs = new XmlSerializer(list.GetType());
				xs.Serialize(sw, list);
			}
		}

		public static void EnsureFlagRulesAreUnique(List<FeedToFlag> list)
		{
			foreach (FeedToFlag ftf in list)
			{
				EnsureFlagRulesAreUnique(ftf);
			}
		}

		public static void EnsureFlagRulesAreUnique(FeedToFlag ftf)
		{
			HashSet<string> unique = new HashSet<string>(ftf.FlagRules);
			ftf.FlagRules = unique.ToList();
		}
	}
}
