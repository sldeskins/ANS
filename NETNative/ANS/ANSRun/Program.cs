using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using Walker;
using Loggers;
using System.Threading;

namespace ANSRun
{
    public class Program
    {
        static void Main(string[] args)
        {
            string outFolder = GetMakeOutputDirectory();
            string LogFile = outFolder + @"\bgp_he_net_log.txt";
            ILogger logger = new LoggerFileConsole(LogFile);

            logger.Log("Starting search");


            //http://agilemanifesto.org/display/000000001.html
            //http://agilemanifesto.org/display/000000391.html
            Uri _bgp_base = new Uri("http://agilemanifesto.org/display");
            Uri _UriRelCountriesReport = new Uri("/report/world", UriKind.Relative);
            Uri _UirStartWalkCounties = new Uri(_bgp_base, _UriRelCountriesReport);

            // ProcessANS(_UirStartWalkCounties, _bgp_base, logger);
            int min = 1;
            int max = 391;

            for (int i = min; i < (max + 1); i++)
            {
                if (i % 10 == 0)
                {
                    Thread.Sleep(1000);
                }
                _UirStartWalkCounties = new Uri(_bgp_base, string.Format("/display/{0:000000000}.html", i));

                HttpWebResponse responseCountries;
                if (Walker.Walker.Get(_UirStartWalkCounties, out responseCountries))
                {
                    string countryPageData = Walker.Walker.GetResponseAsText(responseCountries);

                    if (countryPageData.ToLower().Contains("heila")  )
                    {
                        logger.Log("hit heila: " + i); 
                    }
                    if (  countryPageData.ToLower().Contains("eskins"))
                    {
                        logger.Log("hit eskins: " + i);
                    }
                    if (countryPageData.ToLower().Contains("sld"))
                    {
                        logger.Log("hit sld: " + i);
                    }
                }
                else
                {
                    logger.Log("not found: " + i);
                }
            }
            // // Check whether the new Uri is absolute or relative.
            // if (!_UriRelCountriesReport.IsAbsoluteUri)
            //     Console.WriteLine("{0} is a relative Uri.", _UriRelCountriesReport);
            logger.Log("Completed");
            // Console.ReadKey();
        }

        private static void ProcessANS(Uri _UirStartWalkCounties, Uri _bgp_base, ILogger logger)
        {
            HttpWebResponse responseCountries;
            if (Walker.Walker.Get(_UirStartWalkCounties, out responseCountries))
            {
                string countryPageData = Walker.Walker.GetResponseAsText(responseCountries);

                List<CountryInfo> countryList = Parser_ANSCountry.GetCountryListHTML(countryPageData, logger);

                ProcessReportsForCountries(_bgp_base, countryList, logger);
            }
            else
            {
                logger.Log("Initial country page did not process");
            }
        }

        private static void ProcessReportsForCountries(Uri uribase, List<CountryInfo> countryList, ILogger logger)
        {
            StringBuilder fileContents = new StringBuilder();

            foreach (CountryInfo countryInfo in countryList)
            {
                Uri _UriCountryReport = new Uri(uribase, countryInfo.reportLink);
                HttpWebResponse responseCountryReport;
                if (Walker.Walker.Get(_UriCountryReport, out responseCountryReport))
                {
                    string countryANSReportJson = Parser_ANSCountry.ProcessCountryReport(countryInfo.CountryCodeCC, responseCountryReport, logger);
                    if (!string.IsNullOrWhiteSpace(countryANSReportJson))
                    {
                        fileContents.Append(countryANSReportJson + ",");
                    }
                }
                else
                {
                    logger.Log("report for " + countryInfo.CountryCodeCC + "did not process");
                }
            }
            string outFolder = GetMakeOutputDirectory();
            string filename = outFolder + @"\bgp_he_net.json";
            WriteFileContents(filename, ParserHelper.RemoveTrailingComma(fileContents));
        }

        private static void WriteFileContents(String filename, String fileContents)
        {
            File.Create(filename).Dispose();
            File.AppendAllText(filename, "{" + fileContents + "}");
        }



        static string GetMakeOutputDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string outputDirectory = baseDirectory + "\\Output";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            return outputDirectory;
        }
    }
}