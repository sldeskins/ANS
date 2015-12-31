using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;

namespace ANSRun
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting search");
            /*
            # Web scraping
            # ASNs (Autonomous System Numbers) are one of the building blocks of the
            # Internet. This project is to create a mapping from each ASN in use to the
            # company that owns it. For example, ASN 36375 is used by the University of
            # Michigan - http://bgp.he.net/AS36375
            # 
            # The site http://bgp.he.net/ has lots of useful information about ASNs. 
            # Starting at http://bgp.he.net/report/world crawl and scrape the linked country
            # reports to make a structure mapping each ASN to info about that ASN.
            # Sample structure:
            #   {3320: {'Country': 'DE',
            #     'Name': 'Deutsche Telekom AG',
            #     'Routes v4': 13547,
            #     'Routes v6': 268},
            #    36375: {'Country': 'US',
            #     'Name': 'University of Michigan',
            #     'Routes v4': 14,
            #     'Routes v6': 1}}
            #
            # When done, output the collected data to a json file.
            */
            Uri _bgp_base = new Uri("http://bgp.he.net");
            Uri _UriRelCountriesReport = new Uri("/report/world", UriKind.Relative);
            Uri _UirStartWalkCounties = new Uri(_bgp_base, _UriRelCountriesReport);

            HttpWebResponse responseCountries;
            if (Get(_UirStartWalkCounties, out responseCountries))
            {
                string countryPageData = GetResponseAsText(responseCountries);
                List<CountryInfo> countryList = GetCountryList(countryPageData);

                ProcessReportsForCountries(_bgp_base, countryList);
            }
            else {
                Console.WriteLine("Initial country page did not process");
            }

            // Check whether the new Uri is absolute or relative.
            if (!_UriRelCountriesReport.IsAbsoluteUri)
                Console.WriteLine("{0} is a relative Uri.", _UriRelCountriesReport);

            Console.ReadKey();
        }

        private static void ProcessReportsForCountries(Uri uribase, List<CountryInfo> countryList)
        {

            StringBuilder fileContents = new StringBuilder();
            
            foreach (CountryInfo countryInfo in countryList)
            {
                Uri _UriCountryReport = new Uri(uribase, countryInfo.reportLink);
                HttpWebResponse responseCountryReport;
                if (Get(_UriCountryReport, out responseCountryReport))
                {
                    string countryANSReportJson = ProcessCountryReport(countryInfo.CountryCodeCC, responseCountryReport);
                    fileContents.Append(countryANSReportJson + ",");
                }
                else {
                    Console.WriteLine("report for did not process");
                }
            }
            
        }


        public class CountryInfo
        {
            public string CountryDescription;
            public string CountryCodeCC;
            public int ASNs;
            public string reportLink;
        }
        public static List<CountryInfo> GetCountryList(string htmlText)
        {
            List<CountryInfo> countryList = new List<CountryInfo>();
            XmlDocument document = new XmlDocument();
            string rtext = Clean(htmlText);
            document.LoadXml(rtext);

            XmlNodeList countries2= document.SelectNodes("/html/body//div");
            Console.WriteLine("countries2 " + countries2.Count);

            XmlNode countries = document.SelectSingleNode("//div[@id='countries']"); 

            XmlNodeList rows = countries.SelectNodes(".//tbody/tr");
            Console.WriteLine("rows " + rows.Count);

            foreach (XmlNode row in rows)
            {
                CountryInfo countryInfo = new CountryInfo();
                XmlNodeList columns = row.SelectNodes("td");


                XmlNode countryDescritionNode = columns[0].SelectSingleNode("//img[@title]");
                countryInfo.CountryDescription = countryDescritionNode.Attributes.GetNamedItem("title").InnerText;

                countryInfo.CountryCodeCC = columns[1].InnerText.Trim();

                countryInfo.ASNs = GetIntFromString(columns[2].InnerText);

                XmlNode reportLinkNode = columns[3].SelectSingleNode("//a[@href]");
                countryInfo.reportLink = reportLinkNode.Attributes.GetNamedItem("href").InnerText;


                countryList.Add(countryInfo);
            }
            return countryList;
        }

      

        public static int GetIntFromString(string value)
        {
            return int.Parse(Regex.Replace(value.Trim(), @"[,]", ""));
        }
        public static string Clean(string htmlText)
        {
            return  CorrectForAmpersandCopy (CorrectForAttributeAmpersand(htmlText));
        }
        public static string CorrectForAmpersandCopy(string value)
        {
            return Regex.Replace(value.Trim(), "(&copy;)", "&#169;");

        }        public static string CorrectForAttributeAmpersand(string value)
        {
            //  return Regex.Replace(value.Trim(), "[&][^(amp;)|*]", "&amp;");
            // return Regex.Replace(value.Trim(), "(([&][^(&amp;)]){1})|(^[&]$)", "&amp;");
            // Regex.Replace(value, "(([&][^(amp;)]){1})|(^[&]$)", "&amp;");
            // string part1 = Regex.Replace(value, "(([&][^(amp;)]+))", "&amp;");
            // return part1;
            string[] parts = value.Split(new char[] { '&' });
            StringBuilder c = new StringBuilder();

            if (value.IndexOf('&') >= 0)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    c.Append(parts[i]);
                    if (i < (parts.Length - 1))
                    {
                        if (parts[i + 1].Length >= 4 && parts[i + 1].Substring(0, 4) == "amp;")
                        {
                            c.Append("&");
                        }
                        else if (parts[i + 1].IndexOf(';') < 0) { c.Append("&amp;"); }
                        else { c.Append("&"); }
                    }
                }
            }
            else { c.Append(value); }

            return c.ToString();
        }
        public class ANSInfo
        {
            public string ASN;
            public string ASN_Name;
            public int Adjacencies_v4;
            public int Routes_v4;
            public int Adjacencies_v6;
            public int Routes_v6;
        }
        private static string ProcessCountryReport(string countryCode, HttpWebResponse responseCountryReport)
        {
            string countryReportData = GetResponseAsText(responseCountryReport);
            List<ANSInfo> ANSInfoList = GetANSList(countryReportData);

            StringBuilder jInnards = new StringBuilder();
            foreach (ANSInfo aNSInfo in ANSInfoList)
            {
                jInnards.Append(JsonASNInfo(countryCode, aNSInfo) + ",");
            }

            return jInnards.ToString().Substring(0, jInnards.Length - 1);
        }
        public static string JsonASNInfo(string countryCode, ANSInfo ansInfo)
        {
            StringBuilder jInnards = new StringBuilder();
            JsonWriter jw = new JsonTextWriter(new StringWriter(jInnards));

            jw.WritePropertyName(ansInfo.ASN);

            jw.WriteStartObject();
            jw.WritePropertyName("Country");
            jw.WriteValue(countryCode);
            jw.WritePropertyName("Name");
            jw.WriteValue(ansInfo.ASN_Name);
            jw.WritePropertyName("Routes v4");
            jw.WriteValue(ansInfo.Routes_v4);
            jw.WritePropertyName("Routes v6");
            jw.WriteValue(ansInfo.Routes_v6);
            jw.WriteEndObject();

            return jInnards.ToString();
        }

        public String ToJSONRepresentation()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter jw = new JsonTextWriter(new StringWriter(sb));
            jw.Formatting = Newtonsoft.Json.Formatting.Indented;
            jw.WriteStartObject();
            jw.WritePropertyName("id");
            // jw.WriteValue(this.Id);
            jw.WritePropertyName("name");
            // jw.WriteValue(this.Name);

            jw.WritePropertyName("addresses");
            jw.WriteStartArray();

            jw.WriteEndArray();


            return sb.ToString();
        }

        public static List<ANSInfo> GetANSList(string htmlText)
        {
            List<ANSInfo> ansList = new List<ANSInfo>();
            XmlDocument document = new XmlDocument();
           
            document.LoadXml(Clean(htmlText));
            XmlNode countries = document.SelectSingleNode("//div[@id='country']");
            XmlNodeList rows = countries.SelectNodes("//tbody/tr");
            foreach (XmlNode row in rows)
            {
                ANSInfo ANSInfo = new ANSInfo();
                XmlNodeList columns = row.SelectNodes("td");

                XmlNode ansNode = columns[0].SelectSingleNode("./a");
                //countryInfo.CountryDescription = countryDescritionNode.Attributes.GetNamedItem("title").InnerText;
                ANSInfo.ASN = ansNode.InnerText.Trim();

                ANSInfo.ASN_Name = columns[1].InnerText.Trim();

                ANSInfo.Adjacencies_v4 = GetIntFromString(columns[2].InnerText);
                ANSInfo.Routes_v4 = GetIntFromString(columns[3].InnerText);
                ANSInfo.Adjacencies_v6 = GetIntFromString(columns[4].InnerText);
                ANSInfo.Routes_v6 = GetIntFromString(columns[5].InnerText);

                ansList.Add(ANSInfo);
            }
            return ansList;
        }
        class JsonInfo
        {
            [JsonIgnore]
            string ASN;

            public string Country;
            public string Name;

            [JsonProperty(PropertyName = "Routes v4")]
            public string Routesv4;

            [JsonProperty(PropertyName = "Routes v6")]
            public string Routesv6;
        }
        static bool Get(Uri uri, out HttpWebResponse response)
        {
            bool result = false;
            // Create a request for the URL.         
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            //HttpWebRequest request = WebRequest.CreateHttp("http://bgp.he.net/report/world");
            //WebRequest request = WebRequest.CreateHttp("http://google.com");

            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            //    Console.WriteLine("before");

            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            request.Accept = "text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko)";

            response = (HttpWebResponse)request.GetResponse();
            // Console.WriteLine("after");

            // Display the status.
            if (response.StatusDescription == "OK") { result = true; }
            Console.WriteLine(response.StatusDescription);


            return result;
        }
        static string GetResponseAsText(HttpWebResponse response)
        {  // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            // Console.WriteLine(responseFromServer);
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }
        static async Task<string> GetAsync(Uri uri)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri);

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("In async call");

            //    return await Task.Run(() => JsonObject.Parse(content));
            return await Task.Run(() => " ");
        }
    }
}