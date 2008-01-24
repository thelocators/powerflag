using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Skainix.Syndication
{
	public class SyndicatedFeed
	{
		private List<Item> feedItems;
		private string url;

		public List<Item> FeedItems
		{
			get
			{
				return feedItems;
			}
			set
			{
				feedItems = value;
			}
		}

		public string Url
		{
			get
			{
				return url;
			}
			set
			{
				url = value;
			}
		}



		public static SyndicatedFeed GetFeed(string lUrl)
		{
			SyndicatedFeed retVal = new SyndicatedFeed { url = lUrl };
			retVal.LoadFeed();
			return retVal;
		}


		public int PurgeOldItems(int numItemsToKeep)
		{
			if (FeedItems.Count <= numItemsToKeep)
			{
				return 0;
			}

			var itemsToPurge = (from i in FeedItems
								orderby i.PubDate ascending
								select i).Take(FeedItems.Count - numItemsToKeep);

			int numRemoved = 0;
			foreach (SyndicatedFeed.Item item in itemsToPurge)
			{
				if (FeedItems.Remove(item))
				{
					numRemoved++;
				}
			}

			return numRemoved;
		}

		public void LoadFeed()
		{	
			XElement feedSrc = XElement.Load(url);
			if (feedSrc.Name.ToString().ToLower() == "rss")
			{
				loadFromRSS(feedSrc);
				return;
			}

			if (feedSrc.Name.LocalName.ToLower() == "feed")
			{
				loadFromAtom(feedSrc);
				return;
			}

			if (feedSrc.Name.LocalName.ToLower() == "rdf")
			{
				loadFromRDF(feedSrc);
				return;
			}

			throw new ApplicationException(string.Format("Error in SyndicatedFeed.LoadFeed: Feed is not a supported type.  FeedSrc.Name:{0}, FeedSrc.Name.LocalName:{1}",
						feedSrc.Name.ToString(), feedSrc.Name.LocalName));			

		}



		private void loadFromAtom(XElement feedSrc)
		{
			XNamespace ns = feedSrc.GetDefaultNamespace().NamespaceName;
		
			var query = (from item in feedSrc.Elements(ns + "entry")
						 select new Item
						   {
							   Title = HtmlDecode(safeGetElementValue(item.Element(ns + "title"))),
							   Desc = HtmlDecode(safeGetElementValue(item.Element(ns + "content"))),
							   PubDate = DateTimeParser.Parse(HtmlDecode(safeGetElementValue(item.Element(ns + "issued")))),
							   Link = safeGetElementValue(item.Element(ns + "link").Element(ns + "href")),
							   Source = HtmlDecode(safeGetElementValue(item.Element(ns + "author")))
						   });


			addNewItemsToOld(query.AsEnumerable());
		}

		private void addNewItemsToOld(IEnumerable<Item> downloadedItems)
		{
			if (feedItems == null || feedItems.Count() < 1)
			{
				feedItems = downloadedItems.ToList();
				return;
			}

			List<Item> newItems = new List<Item>();

			bool isNew;
			foreach (Item newItem in downloadedItems)
			{
				isNew = true;
				foreach (Item oldItem in feedItems)
				{
					if (oldItem.IsTheSameAs(newItem))
					{
						isNew = false;
						break;
					}
				}

				if (isNew)
				{
					newItems.Add(newItem);
				}
			}

			feedItems.AddRange(newItems);
		}

		private string HtmlDecode(string src)
		{
			return System.Web.HttpUtility.HtmlDecode(src);
		}

		private void loadFromRDF(XElement feedSrc)
		{
			var query = from item in feedSrc.Elements("{http://purl.org/rss/1.0/}item")
						select new Item
						{
							Title = safeGetElementValue(item.Element("{http://purl.org/rss/1.0/}title")),
							Desc = string.Empty,
							PubDate = DateTimeParser.Parse(safeGetElementValue(item.Element("{http://purl.org/dc/elements/1.1/}date"))),
							Link = safeGetElementValue(item.Element("{http://purl.org/rss/1.0/}link"))
						};

			addNewItemsToOld(query.ToList());
		}

		private void loadFromRSS(XElement feedSrc)
		{
			var query = (from item in feedSrc.Elements("channel").Elements("item")
						 select new Item
						 {
							 Title = safeGetElementValue(item.Element("title")),
							 Desc = safeGetElementValue(item.Element("description")),
							 PubDate = DateTimeParser.Parse(safeGetElementValue(item.Element("pubDate"))),
							 Link = safeGetElementValue(item.Element("link")),
                             Source = safeGetElementValue(item.Element("source")),
							 Guid = safeGetElementValue(item.Element("guid")),
							 EnclosureUrl = getEnclosureUrlFromRssItemNode(item),
							 EnclosureLength = getEnclosureLengthFromRssItemNode(item),
							 EnclosureType = getEnclosureTypeFromRssItemNode(item)
						 });

			addNewItemsToOld(query.ToList());
		}

		private string getEnclosureTypeFromRssItemNode(XElement item)
		{
			XElement xe = getEnclosureElementFromRssItemNode(item);
			if (xe != null)
			{
				return safeGetAttributeValue(xe.Attribute("type"));
			}

			return null;
		}

		private string getEnclosureLengthFromRssItemNode(XElement item)
		{
			XElement xe = getEnclosureElementFromRssItemNode(item);
			if (xe != null)
			{
				return safeGetAttributeValue(xe.Attribute("length"));
			}

			return null;
		}

		private XElement getEnclosureElementFromRssItemNode(XElement item)
		{
			if (item != null)
			{
				return item.Element("enclosure");
			}
			else
			{
				return null;
			}
		}

		private string getEnclosureUrlFromRssItemNode(XElement item)
		{
			
			XElement xe = getEnclosureElementFromRssItemNode(item);
			if (xe != null)
			{
				return safeGetAttributeValue(xe.Attribute("url"));
			}

			return null;
		}

		private string safeGetAttributeValue(XAttribute xAttribute)
		{
			if (xAttribute != null && xAttribute.Value != null)
			{
				return System.Web.HttpUtility.HtmlDecode(xAttribute.Value);
			}

			return string.Empty;
		}

		private string safeGetElementValue(XElement xe)
		{
			if (xe != null && xe.Value != null)
			{
				return System.Web.HttpUtility.HtmlDecode(xe.Value);
			}

			return string.Empty;
		}
		

		public class Item
		{
			private string title;
			private string desc;
			private DateTime pubDate;
            private string link;
            private string source;

            public string Source
            {
                get
                {
                    return source;
                }
                set
                {
                    source = value;
                }
            }
            public string Link
            {
                get
                {
                    return link;
                }
                set
                {
                    link = value;
                }
            }
			public string Title
			{
				get
				{
					return title;
				}
				set
				{
					title = value;
				}
			}
			public string Desc
			{
				get
				{
					return desc;
				}
				set
				{
					desc = value;
				}
			}
			public DateTime PubDate
			{
				get
				{
					return pubDate;
				}
				set
				{
					pubDate = value;
				}
			}

			public string Guid
			{
				get;
				set;
			}

			public bool HasBeenRead
			{
				get;
				set;
			}

			public string EnclosureUrl
			{
				get;
				set;
			}

			public string EnclosureType
			{
				get;
				set;
			}

			public string EnclosureLength
			{
				get;
				set;
			}

			public Item()
			{
				HasBeenRead = false;
				Guid = string.Empty;
			}

			public override string ToString()
			{
				return string.Format("{0} - {1}\r\n{2}\r\n", this.Title, this.PubDate, this.Desc);
			}

			public bool IsTheSameAs(Item compare)
			{
				if (!string.IsNullOrEmpty(Guid) && !string.IsNullOrEmpty(compare.Guid))
				{
					return compare.Guid == Guid;
				}

				if (!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(compare.Title))
				{
					return compare.Title == Title;
				}

				return false;
			}
		}

	}
}
