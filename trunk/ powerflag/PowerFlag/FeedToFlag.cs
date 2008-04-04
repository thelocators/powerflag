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
			List<FlaggedItem> flaggedItems = flagItems(feed);

			return flaggedItems;
		}

		private List<FlaggedItem> flagItems(SyndicatedFeed feed)
		{
			//StringBuilder sb = new StringBuilder();
			//sb.Append("(");
			//foreach (string rule in FlagRules)
			//{
			//    sb.AppendFormat(@"(^|\W){0}($|\W)|", rule);
			//}

			//sb.Remove(sb.Length - 1, 1);

			//sb.Append(")");

			//string reStr = sb.ToString();
			//logger.Debug("Title Search Regex: {0}", reStr);
			//Regex re = new Regex(reStr, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			List<FlaggedItem> flaggedItems = new List<FlaggedItem>();


			Regex itemNumRe = new Regex(@".*\/(?<num>.*?).htm", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex resultRe = new Regex("thanks for flagging", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex urlRe = new Regex("(?<url>http://.*?/)", RegexOptions.IgnoreCase);

			if (!urlRe.IsMatch(feed.Url))
			{
				logger.Error("Error in FeedToFlag.flagItems: Could not parse base URL out of Feed URL: {0}", feed.Url);
				throw new ApplicationException(string.Format("Error in FeedToFlag.flagItems: Could not parse base URL out of Feed URL: {0}", feed.Url));
			}

			string baseUrl = urlRe.Match(feed.Url).Groups["url"].Value;
			string flagFormat = string.Concat(baseUrl, "flag/?flagCode=15&postingID={0}");

			foreach (SyndicatedFeed.Item item in feed.FeedItems)
			{
				if (item.HasBeenRead)
				{
					//this item has already been processed so move along
					continue;
				}

				Regex re;

				foreach (string rule in FlagRules)
				{
					string reStr = string.Format(@"(^|\W){0}($|\W)", rule);
					re = new Regex(reStr, RegexOptions.IgnoreCase);
					if (re.IsMatch(item.Title))
					{
						FlaggedItem fi = new FlaggedItem
						{
							FlaggedRegEx = re.ToString(),
							Title = item.Title,
							Url = item.Link
						};

						string itemNum = itemNumRe.Match(fi.Url).Groups["num"].Value;
						string link = string.Format(flagFormat, itemNum);
						string result = flagItem(link);
						if (resultRe.IsMatch(result))
						{
							flaggedItems.Add(fi);
							item.HasBeenRead = true;
						}

						break;
					}
				}

			}

			return flaggedItems;
		}


		//private void flagItems(SyndicatedFeed feed, List<FlaggedItem> flaggedItems)
		//{
		//    Regex itemNumRe = new Regex(@".*\/(?<num>.*?).htm", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		//    Regex resultRe = new Regex("thanks for flagging", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		//    Regex urlRe = new Regex("(?<url>http://.*?/)", RegexOptions.IgnoreCase);

		//    if (!urlRe.IsMatch(feed.Url))
		//    {
		//        logger.Error("Error in FeedToFlag.flagItems: Could not parse base URL out of Feed URL: {0}", feed.Url);
		//        return;
		//    }

		//    string baseUrl = urlRe.Match(feed.Url).Groups["url"].Value;
		//    string flagFormat = string.Concat(baseUrl, "flag/?flagCode=15&postingID={1}");

		//    flaggedItems.ForEach(fi =>
		//    {
		//        string itemNum = itemNumRe.Match(fi.Url).Groups["num"].Value;
		//        string link = string.Format(flagFormat, itemNum);
		//        string result = flagItem(link);
		//        if (resultRe.IsMatch(result))
		//        {
		//            fi.h
		//        }
		//    }
		//    );
				

		//}

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
