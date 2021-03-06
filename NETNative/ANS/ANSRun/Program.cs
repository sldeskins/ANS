﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using HtmlAgilityPack;

namespace ANSRun
{
    public class Program
    {
        static void Main(string[] args)
        {
           string outFolder= GetMakeOutputDirectory();
            string LogFile = outFolder+ @"\bgp_he_net_log.txt";
            ILogger logger = new LoggerFileConsole(LogFile);

            logger.Log("Starting search");

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

                List<CountryInfo> countryList = GetCountryListHTML(countryPageData, logger);

                ProcessReportsForCountries(_bgp_base, countryList, logger);
            }
            else {
                logger.Log("Initial country page did not process");
            }

            // // Check whether the new Uri is absolute or relative.
            // if (!_UriRelCountriesReport.IsAbsoluteUri)
            //     Console.WriteLine("{0} is a relative Uri.", _UriRelCountriesReport);
            logger.Log("Completed");
           // Console.ReadKey();
        }

        private static void ProcessReportsForCountries(Uri uribase, List<CountryInfo> countryList, ILogger logger)
        {
            StringBuilder fileContents = new StringBuilder();

            foreach (CountryInfo countryInfo in countryList)
            {
                Uri _UriCountryReport = new Uri(uribase, countryInfo.reportLink);
                HttpWebResponse responseCountryReport;
                if (Get(_UriCountryReport, out responseCountryReport))
                {
                    string countryANSReportJson = ProcessCountryReport(countryInfo.CountryCodeCC, responseCountryReport, logger);
                    if (!string.IsNullOrWhiteSpace(countryANSReportJson))
                    {
                        fileContents.Append(countryANSReportJson + ",");
                    }
                }
                else {
                    logger.Log("report for " + countryInfo.CountryCodeCC + "did not process");
                }
            }
            string outFolder = GetMakeOutputDirectory();
            string filename = outFolder + @"\bgp_he_net.json";
            WriteFileContents(filename, RemoveTrailingComma(fileContents));
        }

        private static void WriteFileContents(String filename, String fileContents)
        {
            File.Create(filename).Dispose();
            File.AppendAllText(filename, "{" + fileContents + "}");
        }

        public class CountryInfo
        {
            public string CountryDescription;
            public string CountryCodeCC;
            public int ASNs;
            public string reportLink;
        }

        public static List<CountryInfo> GetCountryListHTML(string htmlText, ILogger logger)
        {
            List<CountryInfo> countryList = new List<CountryInfo>();
            var html = new HtmlDocument();
            html.LoadHtml(htmlText); // load a string

            HtmlNode countries = html.GetElementbyId("countries");

            HtmlNodeCollection rows = countries.SelectNodes(".//tbody/tr");


            foreach (HtmlNode row in rows)
            {
                CountryInfo countryInfo = new CountryInfo();
                HtmlNodeCollection columns = row.SelectNodes("td");

                HtmlNode countryDescritionNode = columns[0].SelectSingleNode("//img[@title]");
                countryInfo.CountryDescription = countryDescritionNode.GetAttributeValue("title", "");

                countryInfo.CountryCodeCC = columns[1].InnerText.Trim();

                countryInfo.ASNs = GetIntFromString(columns[2].InnerText);

                HtmlNode reportLinkNode = columns[3].SelectSingleNode(".//a[@href]");
                countryInfo.reportLink = reportLinkNode.GetAttributeValue("href", "");


                countryList.Add(countryInfo);
            }
            logger.Log("There are " + countryList.Count + " counties.");
            return countryList;
        }

        public static int GetIntFromString(string value)
        {
            return int.Parse(Regex.Replace(value.Trim(), @"[,]", ""));
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
        private static string ProcessCountryReport(string countryCode, HttpWebResponse responseCountryReport, ILogger logger)

        {
            string countryReportData = GetResponseAsText(responseCountryReport);
            List<ANSInfo> ANSInfoList = GetANSListHTML(countryReportData);
            logger.Log("There are " + ANSInfoList.Count + " ANSs for " + countryCode + ".");


            StringBuilder jInnards = new StringBuilder();
            foreach (ANSInfo aNSInfo in ANSInfoList)
            {
                jInnards.Append(JsonASNInfo(countryCode, aNSInfo) + ",");
            }

            return RemoveTrailingComma(jInnards);
        }

        public static string RemoveTrailingComma(StringBuilder sb)
        {

            if (sb.Length > 0)
            {
                if (sb.ToString().Substring(sb.Length - 1) == ",") { return sb.ToString().Substring(0, sb.Length - 1); }
                else return sb.ToString();
            }

            else { return sb.ToString(); }

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

        public static List<ANSInfo> GetANSListHTML(string htmlText)
        {
            List<ANSInfo> ansList = new List<ANSInfo>();
            var html = new HtmlDocument();
            html.LoadHtml(htmlText);

            HtmlNode countries = html.GetElementbyId("country");
            HtmlNodeCollection rows = countries.SelectNodes(".//tbody/tr");
            if (rows != null)
            {
                foreach (HtmlNode row in rows)
                {
                    ANSInfo ANSInfo = new ANSInfo();
                    HtmlNodeCollection columns = row.SelectNodes("td");
                    columns = row.SelectNodes("td");

                    HtmlNode ansNode = columns[0].SelectSingleNode("./a");
                    ANSInfo.ASN = ansNode.InnerText.Trim();

                    ANSInfo.ASN_Name = columns[1].InnerText.Trim();

                    ANSInfo.Adjacencies_v4 = GetIntFromString(columns[2].InnerText);
                    ANSInfo.Routes_v4 = GetIntFromString(columns[3].InnerText);
                    ANSInfo.Adjacencies_v6 = GetIntFromString(columns[4].InnerText);
                    ANSInfo.Routes_v6 = GetIntFromString(columns[5].InnerText);

                    ansList.Add(ANSInfo);
                }
            }
            return ansList;
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
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            request.Accept = "text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko)";

            response = (HttpWebResponse)request.GetResponse();

            // Display the status.
            if (response.StatusDescription == "OK") { result = true; }
         
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

        public interface ILogger
        {
            void Log(string message);
        }
        public class LoggerFileConsole : ILogger
        {
            public string filename;
            public LoggerFileConsole(string filename)
            {
                this.filename = filename;
                File.Create(filename).Dispose();
            }
            public void Log(string message)
            {
                Console.WriteLine(message);
                File.AppendAllText(filename, message+Environment.NewLine);
            }
        }
        static string GetMakeOutputDirectory() {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string outputDirectory = baseDirectory + "\\Output";
            if (!Directory.Exists(outputDirectory)){
                Directory.CreateDirectory(outputDirectory);
            }
            return outputDirectory;
        }
    }
}