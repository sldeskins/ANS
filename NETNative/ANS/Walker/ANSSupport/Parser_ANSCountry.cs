using HtmlAgilityPack;
using Loggers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Walker
{
    public class Parser_ANSCountry
    {
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

                countryInfo.ASNs = ParserHelper.GetIntFromString(columns[2].InnerText);

                HtmlNode reportLinkNode = columns[3].SelectSingleNode(".//a[@href]");
                countryInfo.reportLink = reportLinkNode.GetAttributeValue("href", "");


                countryList.Add(countryInfo);
            }
            logger.Log("There are " + countryList.Count + " counties.");
            return countryList;
        }
        public static string ProcessCountryReport(string countryCode, HttpWebResponse responseCountryReport, ILogger logger)

        {
            string countryReportData =  Walker.GetResponseAsText(responseCountryReport);
            List<ANSInfo> ANSInfoList = GetANSListHTML(countryReportData);
            logger.Log("There are " + ANSInfoList.Count + " ANSs for " + countryCode + ".");


            StringBuilder jInnards = new StringBuilder();
            foreach (ANSInfo aNSInfo in ANSInfoList)
            {
                jInnards.Append(JsonASNInfo(countryCode, aNSInfo) + ",");
            }

            return ParserHelper.RemoveTrailingComma(jInnards);
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

                    ANSInfo.Adjacencies_v4 = ParserHelper.GetIntFromString(columns[2].InnerText);
                    ANSInfo.Routes_v4 = ParserHelper.GetIntFromString(columns[3].InnerText);
                    ANSInfo.Adjacencies_v6 = ParserHelper.GetIntFromString(columns[4].InnerText);
                    ANSInfo.Routes_v6 = ParserHelper.GetIntFromString(columns[5].InnerText);

                    ansList.Add(ANSInfo);
                }
            }
            return ansList;
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


    }
}
