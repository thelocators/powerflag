using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml;

namespace Skainix.Syndication
{
	/// <summary>
	/// Summary description for DateTimeExt.
	/// </summary>
	public static class DateTimeParser
	{
		
		private static String regExpression = "\\s*(?:(?:Mon|Tue|Wed|Thu|Fri|Sat|Sun)" + 
            "\\s*,\\s*)?(\\d{1,2})\\s+(Jan|Feb|Mar|Apr|May|Jun" +
            "|Jul|Aug|Sep|Oct|Nov|Dec)\\s+(\\d{2,})\\s+(\\d{2})\\s*:\\s*(\\d{2})\\s*(?::\\s*(\\d{2}))?" +
            "\\s+([+\\-]\\d{4}|UT|GMT|EST|EDT|CST|CDT|MST|MDT|PST|PDT|[A-IK-Z])";
		
		// same as the above but all on one line
		//private String regExpression = "\\s*(?:(?:Mon|Tue|Wed|Thu|Fri|Sat|Sun)\\s*,\\s*)?(\\d{1,2})\\s+(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\\s+(\\d{2,})\\s+(\\d{2})\\s*:\\s*(\\d{2})\\s*(?::\\s*(\\d{2}))?\\s+([+\\-]\\d{4}|UT|GMT|EST|EDT|CST|CDT|MST|MDT|PST|PDT|[A-IK-Z])";
		// test: regex not stored in variable 
		//private static Regex rfc2822 = new Regex( "\\s*(?:(?:Mon|Tue|Wed|Thu|Fri|Sat|Sun)\\s*,\\s*)?(\\d{1,2})\\s+(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\\s+(\\d{2,})\\s+(\\d{2})\\s*:\\s*(\\d{2})\\s*(?::\\s*(\\d{2}))?\\s+([+\\-]\\d{4}|UT|GMT|EST|EDT|CST|CDT|MST|MDT|PST|PDT|[A-IK-Z])", RegexOptions.Compiled );
		
		private static Regex rfc2822 = new Regex(regExpression, RegexOptions.Compiled);
        private static ArrayList months = new ArrayList( 
											  new String[]
											  { "ZeroIndex", "Jan", "Feb", "Mar", "Apr", "May", "Jun", 
												"Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
														);
		
		private const int timeZones = 34;
		
		/// <summary>
		/// Converts an ISO 8601 date to a DateTime object. Helper method needed to 
		/// deal with timezone offset since they are unsupported by the 
		/// .NET Framework. 
		/// </summary>  
		/// <param name="datetime">DateTime string</param>
		/// <returns>DateTime instance</returns>
		/// <exception cref="FormatException">On format errors parsing the datetime</exception>
		public static DateTime ToDateTime( String datetime )
		{
			int timeIndex = datetime.IndexOf(":");
			if( timeIndex != -1 )
			{
				int tzoneIndex = datetime.IndexOf( "-", timeIndex );
				if( tzoneIndex == -1 )
				{
					tzoneIndex = datetime.IndexOf( "+", timeIndex );
					if( tzoneIndex != -1 ) // timezone is ahead of UTC
					{
						return AddOffset("+", datetime, tzoneIndex);
					}
				}
				else // timezone is behind UTC
				{
                    return AddOffset("-", datetime, tzoneIndex);
				}
			}

			return XmlConvert.ToDateTime(datetime, XmlDateTimeSerializationMode.Local);

		}


		/// <summary>
		/// Parse is able to parse RFC2822/RFC822 formatted dates.
		/// It has a fallback mechanism: if the string does not match,
		/// the normal DateTime.Parse() function is called.
		/// </summary>
		/// <param name="dateTimeString">DateTime String to parse</param>
		/// <returns>DateTime instance</returns>
		/// <exception cref="FormatException">On format errors parsing the datetime</exception>
		public static DateTime Parse( String dateTimeString )
		{
			if( dateTimeString == null || dateTimeString.Trim().Length == 0 )
			{
				//Return DateTime.Now.ToUniversalTime()
                //Changed to Nothing by Ian 4-3-05
				return new DateTime();
			}

			Match m = rfc2822.Match( dateTimeString );
			if( m.Success )
			{
				try
				{
					int dd = int.Parse( m.Groups[1].Value );
					int mth = months.IndexOf( m.Groups[2].Value );
					int yy = int.Parse( m.Groups[3].Value );

					// following year completion is compliant with RFC 2822.
					// yy = (yy < 50 ? 2000 + yy: (yy < 1000 ? 1900 + yy: yy))
					int hh = int.Parse( m.Groups[4].Value );
					int mm = int.Parse( m.Groups[5].Value );
					int ss = int.Parse( m.Groups[6].Value );
					String zone = m.Groups[7].Value;

					DateTime xd = new DateTime( yy, mth, dd, hh, mm, ss );

					return xd.AddHours( RFCTimeZoneToGMTBias( zone ) * -1 );

				}
				catch( Exception )
				{
					throw new FormatException("RES_ExceptionRFC2822ParseGroupsMessage");
				}
			}
			else
			{
                // fallback, if regex does not match
				return DateTime.Parse( dateTimeString );
			}
		}


		
		private struct TZB
		{
			public String Zone;
			public int Bias;

			public TZB( String z, int b )
			{
				Zone = z;
				Bias = b;

			}

		}

 
		private static TZB[] ZoneBias = 
		{
			new TZB("GMT", 0), new TZB("UT", 0), 
			new TZB("EST", -5 * 60), new TZB("EDT", -4 * 60), 
			new TZB("CST", -6 * 60), new TZB("CDT", -5 * 60), 
			new TZB("MST", -7 * 60), new TZB("MDT", -6 * 60), 
			new TZB("PST", -8 * 60), new TZB("PDT", -7 * 60), 
			new TZB("Z", 0), new TZB("A", -1 * 60), 
			new TZB("B", -2 * 60), new TZB("C", -3 * 60), 
			new TZB("D", -4 * 60), new TZB("E", -5 * 60), 
			new TZB("F", -6 * 60), new TZB("G", -7 * 60), 
			new TZB("H", -8 * 60), new TZB("I", -9 * 60), 
			new TZB("K", -10 * 60), new TZB("L", -11 * 60), 
			new TZB("M", -12 * 60), new TZB("N", 1 * 60), 
			new TZB("O", 2 * 60), new TZB("P", 3 * 60), 
			new TZB("Q", 4 * 60), new TZB("R", 3 * 60), 
			new TZB("S", 6 * 60), new TZB("T", 3 * 60), 
			new TZB("U", 8 * 60), new TZB("V", 3 * 60), 
			new TZB("W", 10 * 60), new TZB("X", 3 * 60), 
			new TZB("Y", 12 * 60)
		};

		
		private static double RFCTimeZoneToGMTBias( String zone )
		{
			String s;

			if( zone.IndexOfAny( "+-".ToCharArray() ) == 0 )	// +hhmm format
			{
				int fact;
				if( zone.Substring(0, 1) == "-" )
				{
					fact = -1;
				}
				else
				{
					fact = 1;
				}

				s = zone.Substring(1).TrimEnd();
				double hh = Math.Min( 23, int.Parse( s.Substring(0, 2) ) );
				double mm = Math.Min( 59, int.Parse( s.Substring(2, 2) ) ) / 60;

				return fact * (hh + mm);
			}
			else
			{
                // named format
				s = zone.ToUpper().Trim();
				for( int i = 0; i < timeZones; i++ )
				{
					if( ZoneBias[i].Zone.Equals(s) )
					{
                        return ZoneBias[i].Bias / 60;
					}
				}
			}

			return 0.0;
		}


		private static DateTime AddOffset( String offsetOp, String datetimeString, int tzoneIndex )
		{
			String[] offset = datetimeString.Substring( tzoneIndex + 1 ).Split( ":".ToCharArray() );
			String original = datetimeString;
			datetimeString = datetimeString.Substring( 0, tzoneIndex );

			if( datetimeString.IndexOf(":") == datetimeString.LastIndexOf(":") )	// check if seconds part is missing
			{
				datetimeString = datetimeString + ":00";
			}

			DateTime toReturn = XmlConvert.ToDateTime( datetimeString, XmlDateTimeSerializationMode.Local );

			try
			{
				switch( offsetOp )
				{
					case "+":
						toReturn = toReturn.Subtract( new TimeSpan( int.Parse(offset[0]), int.Parse(offset[1]), 0 ) );
						break;
					case "-":
						toReturn = toReturn.Add( new TimeSpan( int.Parse(offset[0]), int.Parse(offset[1]), 0 ) );
						break;
				}

				return toReturn;	// we treat all dates in feeds as if they are local time (later)
			}
			catch( IndexOutOfRangeException )
			{
				throw new FormatException("RES_ExceptionRFC2822InvalidTimezoneFormatMessage");
			}
		}

	}
}
