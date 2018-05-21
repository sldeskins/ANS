using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using Walker;
using Loggers;

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