using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Records.Common.Model
{
	class IndividualSession
	{
		public class ProgressEventArgs
		{
			public ProgressEventArgs(double p) { Progress = p; }
			public double Progress { get; } // readonly
		}

		public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

		// Declare the event.
		public event ProgressEventHandler ProgressEvent;

		public void Main()
		{
			Run();
		}

		static void Run() {
			Console.WriteLine("DSV-Datenbank");
			Console.WriteLine("==================");
			for (int i = 100000; i < 999999; i++) {
				var t = new Thread(new (int x)=>{IndivRun(x) }, i);
				//IndivRun(i);
				 }
			//.
		}
		//Parallel.For(100000, 999999, (x) => {IndivRun(x)});

		public static void IndivRun(int id) {
			string url = "https://dsvdaten.dsv.de/Modules/Results/Individual.aspx?Lang=de-DE";
			string url_prefix = "https://dsvdaten.dsv.de/Modules/Results/";

			//RecordDB db = new RecordDB();

			//Console.WriteLine("XXXX");

			//var web = new HtmlWeb();
			//web.UseCookies = true;

			var doc = new HtmlDocument();
			//var cookies = new CookieContainer();
			var request = (HttpWebRequest)WebRequest.Create(url);
			//request.CookieContainer = cookies;

			var response = (HttpWebResponse)request.GetResponse();

			// Print the properties of each cookie.
			/*	foreach (Cookie cook in response.Cookies)
				{
					Console.WriteLine("Cookie:");
					Console.WriteLine($"{cook.Name} = {cook.Value}");
					Console.WriteLine($"Domain: {cook.Domain}");
					Console.WriteLine($"Path: {cook.Path}");
					Console.WriteLine($"Port: {cook.Port}");
					Console.WriteLine($"Secure: {cook.Secure}");

					Console.WriteLine($"When issued: {cook.TimeStamp}");
					Console.WriteLine($"Expires: {cook.Expires} (expired? {cook.Expired})");
					Console.WriteLine($"Don't save: {cook.Discard}");
					Console.WriteLine($"Comment: {cook.Comment}");
					Console.WriteLine($"Uri for comments: {cook.CommentUri}");
					Console.WriteLine($"Version: RFC {(cook.Version == 1 ? 2109 : 2965)}");

					// Show the string representation of the cookie.
					Console.WriteLine($"String: {cook}");
				}
				*/

			var dataStream = response.GetResponseStream();
			var reader = new StreamReader(dataStream);
			var responseFromServer = reader.ReadToEnd();
			doc.LoadHtml(responseFromServer);

			//Console.WriteLine("YYYYY");

			//Console.WriteLine(doc.DocumentNode.InnerHtml);

			var viewstate = doc.DocumentNode.SelectSingleNode("//input[@name='__VIEWSTATE']").Attributes[3].Value;
			var eventvali = doc.DocumentNode.SelectSingleNode("//input[@name='__EVENTVALIDATION']").Attributes[3].Value;
			var viewstgen = doc.DocumentNode.SelectSingleNode("//input[@name='__VIEWSTATEGENERATOR']").Attributes[3].Value;

			//Console.WriteLine("ZZZZZ");


			//Console.Write(i.ToString("000000") + ":\t");
			request = (HttpWebRequest)WebRequest.Create("https://dsvdaten.dsv.de/Modules/Results/Individual.aspx");
			request.Referer = "https://dsvdaten.dsv.de/Modules/Results/Individual.aspx";
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			//request.CookieContainer = cookies;
			//var spl = new string[] { "&#39;" };
			var postData = "";
			postData += "__VIEWSTATE=" + EncodeIt(viewstate);
			postData += "&__VIEWSTATEGENERATOR=" + Uri.EscapeDataString(viewstgen);
			postData += "&__EVENTVALIDATION=" + Uri.EscapeDataString(eventvali);
			postData += "&_firstnameTextBox=" + Uri.EscapeDataString("");
			postData += "&_lastnameTextBox=" + Uri.EscapeDataString("");
			postData += "&_regidTextBox=" + Uri.EscapeDataString(id.ToString("000000"));
			//postData += "&_regidTextBox=" + Uri.EscapeDataString("127006");
			postData += "&_submitButton=" + Uri.EscapeDataString("Suche und Zeige...");
			//postData += "&ctl00$ContentSection$hiddenTab=" + Uri.EscapeDataString("#clubs");

			var data = Encoding.ASCII.GetBytes(postData);


			/*Console.WriteLine("      POST: " + postData);
			Console.WriteLine("      __VIEWSTATE: " + viewstate);
			Console.WriteLine("      __VIEWSTATE: " + Uri.EscapeDataString(viewstate));
			Console.WriteLine("      __VIEWSTATE: " + EncodeIt(viewstate));
			Console.WriteLine("      __VIEWSTATEGENERATOR: " + viewstgen);
			Console.WriteLine("      __EVENTVALIDATION: " + eventvali);*/




			request.ContentLength = data.Length;
			using (var stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}
			request.AllowAutoRedirect = false;

			response = (HttpWebResponse)request.GetResponse();

			dataStream = response.GetResponseStream();
			reader = new StreamReader(dataStream);
			responseFromServer = reader.ReadToEnd();


			/*Console.WriteLine("      STATUS: " + response.StatusCode.ToString());
			Console.WriteLine("      METHOD: " + response.Method);
			Console.WriteLine("      SERVER: " + response.Server);*/


			if (response.StatusCode == HttpStatusCode.Found)
			{
				//Console.WriteLine("Found: " + response.Headers["Location"].ToString()); 
				var swimmer = ExtractSwimmer("https://dsvdaten.dsv.de" + response.Headers["Location"].ToString(), viewstate, viewstgen, eventvali);
				Console.WriteLine(id.ToString("000000") + ":\t" + swimmer);
			}
			else
			{
				//Console.WriteLine("");
				Console.WriteLine(id.ToString("000000") + ":\t" + "-");
				//Console.WriteLine("Not Found: " + responseFromServer);
			}
		}

		static string ExtractSwimmer(string url, string viewstate, string viewstgen, string eventvali)
		{

			var request = (HttpWebRequest)WebRequest.Create(url);

			request.Referer = "https://dsvdaten.dsv.de/Modules/Results/Individual.aspx";
			request.Method = "GET";


			var response = (HttpWebResponse)request.GetResponse();
            var dataStream = response.GetResponseStream();
			var reader = new StreamReader(dataStream);
			var responseFromServer = reader.ReadToEnd();

			var doc = new HtmlDocument();
			doc.LoadHtml(responseFromServer);

			var single_anchor = doc.DocumentNode.SelectSingleNode("//*[@id='ContentSection__headerLabel']");

			var swimmer = single_anchor.InnerText;
		
			return swimmer;
		}

		static string EncodeIt(string data)
		{
			int limit = 2000;
			StringBuilder sb = new StringBuilder();
			int loops = data.Length / limit;

			for (int i = 0; i <= loops; i++)
			{
				if (i < loops)
				{
					sb.Append(Uri.EscapeDataString(data.Substring(limit * i, limit)));
				}
				else
				{
					sb.Append(Uri.EscapeDataString(data.Substring(limit * i)));
				}
			}
			return sb.ToString();
		}

		static TimeSpan TimeSpanParse(string timestr)
		{
			var blocks = timestr.Split(':');
			var h = 0;
			var m = 0;
			var s = 0;
			var cs = 0;
			switch (blocks.Length)
			{
				case 3:
					h = int.Parse(blocks[blocks.Length - 3]);
					m = int.Parse(blocks[blocks.Length - 2]);
					s = int.Parse(blocks[blocks.Length - 1].Split(',')[0]);
					cs = int.Parse(blocks[blocks.Length - 1].Split(',')[1]);
					break;
				case 2:
					m = int.Parse(blocks[blocks.Length - 2]);
					s = int.Parse(blocks[blocks.Length - 1].Split(',')[0]);
					cs = int.Parse(blocks[blocks.Length - 1].Split(',')[1]);
					break;
				case 1:
					s = int.Parse(blocks[blocks.Length - 1].Split(',')[0]);
					cs = int.Parse(blocks[blocks.Length - 1].Split(',')[1]);
					break;
			}

			var ret = new TimeSpan(0, h, m, s, cs * 10);
			return ret;
		}

		MeetInfo ExtractMeetInfo(HtmlDocument meetDocument, string meetName)
		{
			//var MeetName = meetDocument.DocumentNode.SelectSingleNode("//span[@id='ContentSection__locationdataLabel']").InnerText;
			var MeetLocation = meetDocument.DocumentNode.SelectSingleNode("//span[@id='ContentSection__locationdataLabel']").InnerText;
			var MeetDateRange = meetDocument.DocumentNode.SelectSingleNode("//span[@id='ContentSection__datedataLabel']").InnerText;
			var MeetTiming = meetDocument.DocumentNode.SelectSingleNode("//span[@id='ContentSection__timingdataLabel']").InnerText;
			var MeetCourse = meetDocument.DocumentNode.SelectSingleNode("//span[@id='ContentSection__coursedataLabel']").InnerText;
			var MeetOrga = meetDocument.DocumentNode.SelectSingleNode("//span[@id='ContentSection__organizerdataLabel']").InnerText;

			MeetInfo result = new MeetInfo();
			string[] dotspl = MeetDateRange.Split('.');
			result.Year = int.Parse(dotspl[dotspl.Length - 1]);
			result.Location = MeetLocation;

			result.DateRange = MeetDateRange;
			result.Organizer = MeetOrga;
			result.Name = meetName;
			switch (MeetTiming)
			{
				case "Handzeit":
					result.Timing = TimingType.Manual;
					break;
				case "Automatisch":
					result.Timing = TimingType.Auto;
					break;
				case "Halbautomatisch":
					result.Timing = TimingType.HalfAuto;
					break;
			}
			result.Organizer = MeetOrga;
			switch (MeetCourse)
			{
				case "25m":
					result.CourseType = CourseType.Short;
					break;
				case "50m":
					result.CourseType = CourseType.Long;
					break;
				case "Freiwasser":
					result.CourseType = CourseType.OpenWater;
					break;
			}

			var resultClasses = meetDocument.DocumentNode.SelectNodes("//span[@id='ContentSection__eventsListBox']/option/@value");

			return result;
		}

		void AnalyseClub(string clubname, string cluburl, string xcid, RecordDB recorddb)
		{
			var web = new HtmlWeb();
			web.UseCookies = true;
			var doc = web.Load(cluburl);

			var clubNameLabel = doc.DocumentNode.SelectSingleNode("//span[@id='ContentSection__headerLabel']");
			var vidlabel = doc.DocumentNode.SelectSingleNode("//span[@id='ContentSection__clubidLabel']");
			var clubName = clubNameLabel.InnerText;
			var clubId = vidlabel.InnerText;

			var meets = doc.DocumentNode.SelectNodes("//a[starts-with(@href, '/Modules/Results/Meet.aspx?MeetID')]");
			if (meets == null) return;
			var meetcnt = 0;
			Console.Write("[00%]");

			foreach (HtmlNode meetlink in meets)
			{
				//Console.WriteLine("   Wettkampf: " + wklink.InnerText);
				Console.Write("\b\b\b\b\b");
				Console.Write("[" + ((100 * (meetcnt++)) / meets.Count).ToString("D2") + "%]");
				var target = meetlink.Attributes[0].Value;
				var referrer = cluburl;

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://dsvdaten.dsv.de" + target);
				request.Referer = cluburl;
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream dataStream = response.GetResponseStream();
				// Open the stream using a StreamReader for easy access.  
				StreamReader reader = new StreamReader(dataStream);
				// Read the content.  
				string responseFromServer = reader.ReadToEnd();

				var wkdoc = new HtmlDocument();
				wkdoc.LoadHtml(responseFromServer);

				MeetInfo meetInfo = ExtractMeetInfo(wkdoc, meetlink.InnerText);
				if (meetInfo.Timing != TimingType.Auto)
					continue;

				var clink = wkdoc.DocumentNode.SelectNodes("//a[@title='" + xcid + "']");
				if (clink == null) continue;
				var viewstate = wkdoc.DocumentNode.SelectSingleNode("//input[@name='__VIEWSTATE']").Attributes[3].Value;
				var eventvali = wkdoc.DocumentNode.SelectSingleNode("//input[@name='__EVENTVALIDATION']").Attributes[3].Value;
				var viewstgen = wkdoc.DocumentNode.SelectSingleNode("//input[@name='__VIEWSTATEGENERATOR']").Attributes[3].Value;

				request = (HttpWebRequest)WebRequest.Create("https://dsvdaten.dsv.de" + target);
				request.Referer = "https://dsvdaten.dsv.de" + target;
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";

				var spl = new string[] { "&#39;" };
				var postData = "__EVENTTARGET=" + Uri.EscapeDataString(clink[0].OuterHtml.Split(spl, StringSplitOptions.None)[1]);
				postData += "&__EVENTARGUMENT=";
				postData += "&__LASTFOCUS=";
				postData += "&__VIEWSTATE=" + EncodeIt(viewstate);
				postData += "&__VIEWSTATEGENERATOR=" + Uri.EscapeDataString(viewstgen);
				postData += "&__EVENTVALIDATION=" + Uri.EscapeDataString(eventvali);
				postData += "&ctl00$ContentSection$hiddenTab=" + Uri.EscapeDataString("#clubs");
				//Console.WriteLine("      POST: " + postData);
				var data = Encoding.ASCII.GetBytes(postData);


				request.ContentLength = data.Length;

				using (var stream = request.GetRequestStream())
				{
					stream.Write(data, 0, data.Length);
				}


				response = (HttpWebResponse)request.GetResponse();

				dataStream = response.GetResponseStream();
				reader = new StreamReader(dataStream);
				responseFromServer = reader.ReadToEnd();


				var wkcdoc = new HtmlDocument();
				wkcdoc.LoadHtml(responseFromServer);
				var single_anchor = wkcdoc.DocumentNode.SelectSingleNode("//*[@id='ContentSection__individualLabel']");
				var group_anchor = wkcdoc.DocumentNode.SelectSingleNode("//*[@id='ContentSection__relaysLabel']");
				var single_div = single_anchor.ParentNode.ParentNode;
				var group_div = group_anchor.ParentNode.ParentNode;

				//Console.WriteLine("     - " + single_anchor.InnerText);
				var rows = single_div.SelectNodes("./table/tbody/tr");
				if (rows != null)
					foreach (var row in rows)
					{
						var name = row.SelectSingleNode("./td[2]/a").InnerText.Trim();
						var surname = name.Split(',')[0].Trim();
						var prename = name.Split(',')[1].Trim();
						var sexstr = row.SelectSingleNode("./td[1]").InnerText.Trim();
						var sex = (sexstr.StartsWith('m'.ToString())) ? Sex.Male : Sex.Female;

						var birthstr = row.SelectSingleNode("./td[3]").InnerText.Trim();
						var birth = uint.Parse(birthstr);
						var disc = row.SelectSingleNode("./td[4]/span").InnerText.Replace("Vorlauf", "").Replace("Finale", "").Trim();
						var timestr = row.SelectSingleNode("./td[5]").InnerText.Replace("(Zw)", "").Trim();
						if (timestr == "DS" || timestr == "NA" || timestr == "AB" || timestr == "AU")
							continue;
						var time = TimeSpanParse(timestr);
						//Console.WriteLine("      " + name + " ("+ birth + "): \t"+ time + " " + disc);

						var records = recorddb.TestSingleRecords(Discipline.Parse(disc), sex, meetInfo.CourseType, birth, meetInfo.LastDate(), time);

						foreach (var record in records)
						{
							Console.WriteLine("      " + record.Surname + " (" + record.Agegroup.ToString() + "): \t" + record.Time.ToString() + " " + record.Discipline);
							var x = record;
							x.Surname = surname;
							x.Name = prename;
							x.Time = time;
							x.Age = birthstr;
							x.ClubId = clubId;
							x.Club = clubName;
							x.MeetName = meetInfo.Name;
							x.Date = meetInfo.DateRange.ToString();
							Console.WriteLine("    * " + name + " (" + birth + "): \t" + time + " " + disc);

						}
					}

				//Console.WriteLine("     - " + group_anchor.InnerText);
				rows = group_div.SelectNodes("./table/tbody/tr");
				if (rows != null)
					foreach (var row in rows)
					{
						var disc = row.SelectSingleNode("./td[1]/span").InnerText.Trim();
						var swimmers = row.SelectNodes("./td[2]/a");
						var names = "";
						if (swimmers != null)
							foreach (var swimmer in swimmers)
							{
								names += swimmer.InnerText.Trim() + ";";
							}
						var timestr = row.SelectSingleNode("./td[4]").InnerText.Replace("(Zw)", "").Trim();
						if (timestr == "DS" || timestr == "NA" || timestr == "AB" || timestr == "AU")
							continue;
						var time = TimeSpanParse(timestr);

						// The DSV database does not give the sex of the relay directly. 
						// To save download time. first check if we can have a record in any category


						foreach (Sex sex in new Sex[] { Sex.Male, Sex.Female, Sex.Mixed })
						{
							if (true)
							{
								var records = recorddb.TestTeamRecords(disc, sex, meetInfo.CourseType, meetInfo.LastDate(), time);

								foreach (var record in records)
								{

									Console.WriteLine("      " + clubname + ": \t" + time + " " + disc);
									Console.WriteLine("      " + record.Club + ": \t" + record.Time.ToString() + " " + record.Discipline);

								}
							}
						}
					}


			} // for each meet
			Console.Write("\b\b\b\b\b");
		}
	}
}
