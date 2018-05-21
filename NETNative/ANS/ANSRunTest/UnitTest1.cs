using Microsoft.VisualStudio.TestTools.UnitTesting;
using ANSRun;
using System.Collections.Generic;
using System.Text;
using Loggers;
using Walker;

namespace ANSRunTest
{
    public class DummyLogger : ILogger
    {
        public void Log(string message) { }
    }
    [TestClass]
    public class UnitTest1
    {
        public ILogger logger = new DummyLogger();

        [TestMethod]
        public void TestMethod_GetCountryList()
        {
            string testrow = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">" + @"<html>
    <body>
        <div></div>
<div id='content'>
         <div></div>
        <div id='countries' class='tabdata'>
            <table id='table_countries' class='sortable'>
            <tbody>
            <tr>
                            <td>
                                <div class=""flag alignright floatright"">
                                    <img alt = ""United States"" src=""/images/flags/us.gif?1282328089"" title=""United States"" />
                                </div>
					            <div class=""down2 floatleft""> 
					                United States
                                </div>
				            </td>
				            <td class='centeralign'>
					            US
                            </td>
				            <td class=""alignright"">
					            24,964
				            </td>
				            <td class='centeralign'>
					            <a href = ""/country/US""> Report </a>
                            </td> 
                        </tr> 
            </tbody>
            </table>
        </div>
</div>
    </body>
</html>
            ";

            List<CountryInfo> countries = Parser_ANSCountry.GetCountryListHTML(testrow, logger);
            Assert.AreEqual(1, countries.Count);
            Assert.AreEqual("United States", countries[0].CountryDescription);
            Assert.AreEqual("US", countries[0].CountryCodeCC);
            Assert.AreEqual(24964, countries[0].ASNs);
            Assert.AreEqual("/country/US", countries[0].reportLink);
        }



        [TestMethod]
        public void TestMethod_GetCountryANSList()
        {
            string testrow = @"<html>
<div></div>
<div id='country' class='tabdata'>
	<h2><img alt=""Romania"" src="" / images / flags / ro.gif ? 1282328083"" title=""Romania"" /> Networks: Romania</h2>
  <table id = 'asns' class='sortable'>
	<thead>
	<tr>
	<th>ASN</th><th>Name</th><th>Adjacencies v4</th><th>Routes v4</th><th>Adjacencies v6</th><th>Routes v6</th>
	</tr>
	</thead>
	<tbody>
		<tr>
		<td><a href = ""/AS8708"" title= ""AS8708 - RCS &amp; RDS SA"" > AS8708 </a></td>
        <td>RCS & RDS SA</td>
		<td class='alignright'>372</td>
		<td class='alignright'>759</td>
		<td class='alignright'>71</td>
		<td class='alignright'>7</td>
		</tr>
	
		<tr>
		<td><a href = ""/AS9050"" title=""AS9050 - TELEKOM ROMANIA COMMUNICATION S.A"">AS9050</a></td>
		<td>TELEKOM ROMANIA COMMUNICATION S.A</td>
		<td class='alignright'>295</td>
		<td class='alignright'>1,920</td>
		<td class='alignright'>74</td>
		<td class='alignright'>25</td>
		</tr>
	
		<tr>
		<td><a href = ""/AS6663"" title=""AS6663 - Euroweb Romania SA"">AS6663</a></td>
		<td>Euroweb Romania SA</td>
		<td class='alignright'>252</td>
		<td class='alignright'>1,973</td>
		<td class='alignright'>95</td>
		<td class='alignright'>35</td>
		</tr>
</tbody>
</table>
</div>
</html>
            ";

            List<ANSInfo> ANSs = Parser_ANSCountry.GetANSListHTML(testrow);
            Assert.AreEqual(3, ANSs.Count);
            //
            Assert.AreEqual("AS8708", ANSs[0].ASN);
            Assert.AreEqual("RCS & RDS SA", ANSs[0].ASN_Name);
            Assert.AreEqual(372, ANSs[0].Adjacencies_v4);
            Assert.AreEqual(759, ANSs[0].Routes_v4);
            Assert.AreEqual(71, ANSs[0].Adjacencies_v6);
            Assert.AreEqual(7, ANSs[0].Routes_v6);
            //
            Assert.AreEqual("AS9050", ANSs[1].ASN);
            Assert.AreEqual("TELEKOM ROMANIA COMMUNICATION S.A", ANSs[1].ASN_Name);
            Assert.AreEqual(295, ANSs[1].Adjacencies_v4);
            Assert.AreEqual(1920, ANSs[1].Routes_v4);
            Assert.AreEqual(74, ANSs[1].Adjacencies_v6);
            Assert.AreEqual(25, ANSs[1].Routes_v6);

        }


        [TestMethod]
        public void TestMethod_JsonASNInfo()
        {
            string result = Parser_ANSCountry.JsonASNInfo("DE", new ANSInfo()
            {
                ASN = "1234",
                Routes_v4 = 12,
                Routes_v6 = 542,
                ASN_Name = "gun HO"
            });
            Assert.AreEqual("\"1234\":{\"Country\":\"DE\",\"Name\":\"gun HO\",\"Routes v4\":12,\"Routes v6\":542}", result);
        }

        [TestMethod]
        public void TestMethod_RemoveTrailingComma()
        {
            StringBuilder sb = new StringBuilder();
            Assert.AreEqual("", ParserHelper.RemoveTrailingComma(sb));

            //
            sb.Append(",");
            Assert.AreEqual("", ParserHelper.RemoveTrailingComma(sb));
            //
            //
            sb.Append("nope");
            Assert.AreEqual(",nope", ParserHelper.RemoveTrailingComma(sb));
            //
            sb.Append(",");
            Assert.AreEqual(",nope", ParserHelper.RemoveTrailingComma(sb));
        }

        [TestMethod]
        public void TestMethod_GetCountryList_Big()
        {
            string testrow = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN""
 ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"" lang=""en"">
<head>
<!-- rmosher 2010 - 2014 -->
<meta http-equiv=""Content-type"" content=""text/html;charset=UTF-8"" />
<script src=""/javascripts/jquery/jquery-1.3.2.js?1235084520"" type=""text/javascript""></script>
<script src=""/javascripts/jquery/jquery.history.js?1364589087"" type=""text/javascript""></script>
<script src=""/javascripts/jquery/jquery-ui.js?1269850573"" type=""text/javascript""></script>
<script src=""/javascripts/jquery/jrails.js?1269850578"" type=""text/javascript""></script>
<script src=""/javascripts/bgp.js?1260526324"" type=""text/javascript""></script>
<link href=""/stylesheets/bgp.css?1444893633"" media=""screen"" rel=""stylesheet"" type=""text/css"" />


<script src=""/javascripts/tabs.js?1364588335"" type=""text/javascript""></script>
<script src=""/javascripts/sorttable.js?1283318963"" type=""text/javascript""></script>
<link href=""/stylesheets/report.css?1282275220"" media=""screen"" rel=""stylesheet"" type=""text/css"" />
<title>World Report - bgp.he.net</title>
<meta name=""description"" content=""World Report"" />


</head>

<body>
	<div id='header'>
		<a href=""http://www.he.net/""><img alt='Hurricane Electric' src='/helogo.gif'></img></a>
		<form action=""/search"" method=""get"">
			<div class='search'>
			<input id=""search_search"" name=""search[search]"" size=""15"" type=""text"" />
			<input name=""commit"" type=""submit"" value=""Search"" />
		</div>
		</form>
		
<h1><a href=""/report/dns"" rel=""bookmark"" title=""World Report"">World Report</a></h1>

		<div class='clear'></div>
		<div class='floatleft'>
			<div class='leftsidemenu'>
				<div class='menuheader'>Quick Links</div>
				<ul class='leftsidemenuitems'>
					<li><a href='http://bgp.he.net/'>BGP Toolkit Home</a></li>
					<li><a href=""/report/prefixes"">BGP Prefix Report</a></li>
					<li><a href=""/report/peers"">BGP Peer Report</a></li>
					<li><a href=""/report/bogons"">Bogon Routes</a></li>
					<li><a href=""/report/world"">World Report</a></li>
					<li><a href=""/report/multi-origin-routes"">Multi Origin Routes</a></li>
					<li><a href=""/report/dns"">DNS Report</a></li>
					<li><a href=""/report/tophosts"">Top Host Report</a></li>
					<li><a href=""/report/netstats"">Internet Statistics</a></li>
					<li><a href='http://lg.he.net/'>Looking Glass</a></li>
					<li><a href='http://networktools.he.net/'>Network Tools App</a></li>
					<li><a href='http://tunnelbroker.net/'>Free IPv6 Tunnel</a></li>
					<li><a href='http://ipv6.he.net/certification/'>IPv6 Certification</a></li>
					<li><a href='http://bgp.he.net/ipv6-progress-report.cgi'>IPv6 Progress</a></li>
					<li><a href='http://bgp.he.net/going-native.pdf'>Going Native</a></li>
					<li><a href='http://bgp.he.net/contact/'>Contact Us</a></li>
				</ul>
	
			</div>
			<div class='clear'></div>
			<div class='social'>
			
				<a href=""/r/Twitter"" title=""Hurricane Electric on Twitter""><img alt=""Hurricane Electric on Twitter"" src=""/images/twitter.png?1215539178"" /></a>
			
				<a href=""/r/Facebook"" title=""Hurricane Electric on Facebook""><img alt=""Hurricane Electric on Facebook"" src=""/images/facebook.png?1215539178"" /></a>
			
			</div>
		</div>
	</div>

	<div id='content'>
			
		

		




<ul class='tabmenu'>
    <li id='tab_countries'       class='tabmenuli'>Countries</li>
</ul>

<div class='clear'></div>

<div id='countries' class='tabdata'>
	<h2>Countries with ASNs: 238</h2>
	<table id='table_countries' class='sortable'>
		<thead><tr><th>Description</th><th>CC</th><th>ASNs</th><th>Report</th></tr></thead>
		<tbody>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""United States"" src=""/images/flags/us.gif?1282328089"" title=""United States"" /></div>
					
					<div class=""down2 floatleft"">
				
					United States		
					</div>
				</td>
				<td class='centeralign'>
					US
				</td>
				<td class=""alignright"">
					24,970
				</td>
				<td class='centeralign'>
					<a href=""/country/US"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Russian Federation"" src=""/images/flags/ru.gif?1282328083"" title=""Russian Federation"" /></div>
					
					<div class=""down2 floatleft"">
				
					Russian Federation		
					</div>
				</td>
				<td class='centeralign'>
					RU
				</td>
				<td class=""alignright"">
					6,011
				</td>
				<td class='centeralign'>
					<a href=""/country/RU"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Brazil"" src=""/images/flags/br.gif?1282328040"" title=""Brazil"" /></div>
					
					<div class=""down2 floatleft"">
				
					Brazil		
					</div>
				</td>
				<td class='centeralign'>
					BR
				</td>
				<td class=""alignright"">
					3,811
				</td>
				<td class='centeralign'>
					<a href=""/country/BR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""United Kingdom"" src=""/images/flags/gb.gif?1282328062"" title=""United Kingdom"" /></div>
					
					<div class=""down2 floatleft"">
				
					United Kingdom		
					</div>
				</td>
				<td class='centeralign'>
					GB
				</td>
				<td class=""alignright"">
					2,415
				</td>
				<td class='centeralign'>
					<a href=""/country/GB"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Poland"" src=""/images/flags/pl.gif?1282328081"" title=""Poland"" /></div>
					
					<div class=""down2 floatleft"">
				
					Poland		
					</div>
				</td>
				<td class='centeralign'>
					PL
				</td>
				<td class=""alignright"">
					2,258
				</td>
				<td class='centeralign'>
					<a href=""/country/PL"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Ukraine"" src=""/images/flags/ua.gif?1282328089"" title=""Ukraine"" /></div>
					
					<div class=""down2 floatleft"">
				
					Ukraine		
					</div>
				</td>
				<td class='centeralign'>
					UA
				</td>
				<td class=""alignright"">
					2,244
				</td>
				<td class='centeralign'>
					<a href=""/country/UA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Germany"" src=""/images/flags/de.gif?1282328059"" title=""Germany"" /></div>
					
					<div class=""down2 floatleft"">
				
					Germany		
					</div>
				</td>
				<td class='centeralign'>
					DE
				</td>
				<td class=""alignright"">
					2,084
				</td>
				<td class='centeralign'>
					<a href=""/country/DE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Australia"" src=""/images/flags/au.gif?1282328038"" title=""Australia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Australia		
					</div>
				</td>
				<td class='centeralign'>
					AU
				</td>
				<td class=""alignright"">
					1,968
				</td>
				<td class='centeralign'>
					<a href=""/country/AU"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Canada"" src=""/images/flags/ca.gif?1282328042"" title=""Canada"" /></div>
					
					<div class=""down2 floatleft"">
				
					Canada		
					</div>
				</td>
				<td class='centeralign'>
					CA
				</td>
				<td class=""alignright"">
					1,849
				</td>
				<td class='centeralign'>
					<a href=""/country/CA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Romania"" src=""/images/flags/ro.gif?1282328083"" title=""Romania"" /></div>
					
					<div class=""down2 floatleft"">
				
					Romania		
					</div>
				</td>
				<td class='centeralign'>
					RO
				</td>
				<td class=""alignright"">
					1,719
				</td>
				<td class='centeralign'>
					<a href=""/country/RO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""India"" src=""/images/flags/in.gif?1282328066"" title=""India"" /></div>
					
					<div class=""down2 floatleft"">
				
					India		
					</div>
				</td>
				<td class='centeralign'>
					IN
				</td>
				<td class=""alignright"">
					1,463
				</td>
				<td class='centeralign'>
					<a href=""/country/IN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""France"" src=""/images/flags/fr.gif?1282328062"" title=""France"" /></div>
					
					<div class=""down2 floatleft"">
				
					France		
					</div>
				</td>
				<td class='centeralign'>
					FR
				</td>
				<td class=""alignright"">
					1,268
				</td>
				<td class='centeralign'>
					<a href=""/country/FR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""China"" src=""/images/flags/cn.gif?1282328046"" title=""China"" /></div>
					
					<div class=""down2 floatleft"">
				
					China		
					</div>
				</td>
				<td class='centeralign'>
					CN
				</td>
				<td class=""alignright"">
					1,259
				</td>
				<td class='centeralign'>
					<a href=""/country/CN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Japan"" src=""/images/flags/jp.gif?1282328067"" title=""Japan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Japan		
					</div>
				</td>
				<td class='centeralign'>
					JP
				</td>
				<td class=""alignright"">
					1,123
				</td>
				<td class='centeralign'>
					<a href=""/country/JP"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Korea, Republic of"" src=""/images/flags/kr.gif?1282328068"" title=""Korea, Republic of"" /></div>
					
					<div class=""down2 floatleft"">
				
					Korea, Republic of		
					</div>
				</td>
				<td class='centeralign'>
					KR
				</td>
				<td class=""alignright"">
					1,019
				</td>
				<td class='centeralign'>
					<a href=""/country/KR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Indonesia"" src=""/images/flags/id.gif?1282328065"" title=""Indonesia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Indonesia		
					</div>
				</td>
				<td class='centeralign'>
					ID
				</td>
				<td class=""alignright"">
					1,010
				</td>
				<td class='centeralign'>
					<a href=""/country/ID"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Netherlands"" src=""/images/flags/nl.gif?1282328075"" title=""Netherlands"" /></div>
					
					<div class=""down2 floatleft"">
				
					Netherlands		
					</div>
				</td>
				<td class='centeralign'>
					NL
				</td>
				<td class=""alignright"">
					978
				</td>
				<td class='centeralign'>
					<a href=""/country/NL"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Italy"" src=""/images/flags/it.gif?1282328067"" title=""Italy"" /></div>
					
					<div class=""down2 floatleft"">
				
					Italy		
					</div>
				</td>
				<td class='centeralign'>
					IT
				</td>
				<td class=""alignright"">
					947
				</td>
				<td class='centeralign'>
					<a href=""/country/IT"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Switzerland"" src=""/images/flags/ch.gif?1282328045"" title=""Switzerland"" /></div>
					
					<div class=""down2 floatleft"">
				
					Switzerland		
					</div>
				</td>
				<td class='centeralign'>
					CH
				</td>
				<td class=""alignright"">
					795
				</td>
				<td class='centeralign'>
					<a href=""/country/CH"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Bulgaria"" src=""/images/flags/bg.gif?1282328039"" title=""Bulgaria"" /></div>
					
					<div class=""down2 floatleft"">
				
					Bulgaria		
					</div>
				</td>
				<td class='centeralign'>
					BG
				</td>
				<td class=""alignright"">
					734
				</td>
				<td class='centeralign'>
					<a href=""/country/BG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Spain"" src=""/images/flags/es.gif?1282328060"" title=""Spain"" /></div>
					
					<div class=""down2 floatleft"">
				
					Spain		
					</div>
				</td>
				<td class='centeralign'>
					ES
				</td>
				<td class=""alignright"">
					731
				</td>
				<td class='centeralign'>
					<a href=""/country/ES"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Sweden"" src=""/images/flags/se.gif?1282328084"" title=""Sweden"" /></div>
					
					<div class=""down2 floatleft"">
				
					Sweden		
					</div>
				</td>
				<td class='centeralign'>
					SE
				</td>
				<td class=""alignright"">
					729
				</td>
				<td class='centeralign'>
					<a href=""/country/SE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Hong Kong"" src=""/images/flags/hk.gif?1282328065"" title=""Hong Kong"" /></div>
					
					<div class=""down2 floatleft"">
				
					Hong Kong		
					</div>
				</td>
				<td class='centeralign'>
					HK
				</td>
				<td class=""alignright"">
					721
				</td>
				<td class='centeralign'>
					<a href=""/country/HK"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""European Union"" src=""/images/flags/eu.gif?1282328061"" title=""European Union"" /></div>
					
					<div class=""down2 floatleft"">
				
					European Union		
					</div>
				</td>
				<td class='centeralign'>
					EU
				</td>
				<td class=""alignright"">
					661
				</td>
				<td class='centeralign'>
					<a href=""/country/EU"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Czech Republic"" src=""/images/flags/cz.gif?1282328059"" title=""Czech Republic"" /></div>
					
					<div class=""down2 floatleft"">
				
					Czech Republic		
					</div>
				</td>
				<td class='centeralign'>
					CZ
				</td>
				<td class=""alignright"">
					652
				</td>
				<td class='centeralign'>
					<a href=""/country/CZ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Turkey"" src=""/images/flags/tr.gif?1282328088"" title=""Turkey"" /></div>
					
					<div class=""down2 floatleft"">
				
					Turkey		
					</div>
				</td>
				<td class='centeralign'>
					TR
				</td>
				<td class=""alignright"">
					581
				</td>
				<td class='centeralign'>
					<a href=""/country/TR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Austria"" src=""/images/flags/at.gif?1282328037"" title=""Austria"" /></div>
					
					<div class=""down2 floatleft"">
				
					Austria		
					</div>
				</td>
				<td class='centeralign'>
					AT
				</td>
				<td class=""alignright"">
					557
				</td>
				<td class='centeralign'>
					<a href=""/country/AT"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Argentina"" src=""/images/flags/ar.gif?1282328037"" title=""Argentina"" /></div>
					
					<div class=""down2 floatleft"">
				
					Argentina		
					</div>
				</td>
				<td class='centeralign'>
					AR
				</td>
				<td class=""alignright"">
					551
				</td>
				<td class='centeralign'>
					<a href=""/country/AR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""New Zealand"" src=""/images/flags/nz.gif?1282328076"" title=""New Zealand"" /></div>
					
					<div class=""down2 floatleft"">
				
					New Zealand		
					</div>
				</td>
				<td class='centeralign'>
					NZ
				</td>
				<td class=""alignright"">
					529
				</td>
				<td class='centeralign'>
					<a href=""/country/NZ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Iran, Islamic Republic of"" src=""/images/flags/ir.gif?1282328067"" title=""Iran, Islamic Republic of"" /></div>
					
					<div class=""down2 floatleft"">
				
					Iran, Islamic Republic of		
					</div>
				</td>
				<td class='centeralign'>
					IR
				</td>
				<td class=""alignright"">
					519
				</td>
				<td class='centeralign'>
					<a href=""/country/IR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Thailand"" src=""/images/flags/th.gif?1282328087"" title=""Thailand"" /></div>
					
					<div class=""down2 floatleft"">
				
					Thailand		
					</div>
				</td>
				<td class='centeralign'>
					TH
				</td>
				<td class=""alignright"">
					477
				</td>
				<td class='centeralign'>
					<a href=""/country/TH"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Singapore"" src=""/images/flags/sg.gif?1282328084"" title=""Singapore"" /></div>
					
					<div class=""down2 floatleft"">
				
					Singapore		
					</div>
				</td>
				<td class='centeralign'>
					SG
				</td>
				<td class=""alignright"">
					431
				</td>
				<td class='centeralign'>
					<a href=""/country/SG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Philippines"" src=""/images/flags/ph.gif?1282328077"" title=""Philippines"" /></div>
					
					<div class=""down2 floatleft"">
				
					Philippines		
					</div>
				</td>
				<td class='centeralign'>
					PH
				</td>
				<td class=""alignright"">
					386
				</td>
				<td class='centeralign'>
					<a href=""/country/PH"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Bangladesh"" src=""/images/flags/bd.gif?1282328039"" title=""Bangladesh"" /></div>
					
					<div class=""down2 floatleft"">
				
					Bangladesh		
					</div>
				</td>
				<td class='centeralign'>
					BD
				</td>
				<td class=""alignright"">
					380
				</td>
				<td class='centeralign'>
					<a href=""/country/BD"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""South Africa"" src=""/images/flags/za.gif?1282328093"" title=""South Africa"" /></div>
					
					<div class=""down2 floatleft"">
				
					South Africa		
					</div>
				</td>
				<td class='centeralign'>
					ZA
				</td>
				<td class=""alignright"">
					371
				</td>
				<td class='centeralign'>
					<a href=""/country/ZA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Taiwan"" src=""/images/flags/tw.gif?1282328088"" title=""Taiwan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Taiwan		
					</div>
				</td>
				<td class='centeralign'>
					TW
				</td>
				<td class=""alignright"">
					347
				</td>
				<td class='centeralign'>
					<a href=""/country/TW"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Mexico"" src=""/images/flags/mx.gif?1282328074"" title=""Mexico"" /></div>
					
					<div class=""down2 floatleft"">
				
					Mexico		
					</div>
				</td>
				<td class='centeralign'>
					MX
				</td>
				<td class=""alignright"">
					333
				</td>
				<td class='centeralign'>
					<a href=""/country/MX"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Denmark"" src=""/images/flags/dk.gif?1282328059"" title=""Denmark"" /></div>
					
					<div class=""down2 floatleft"">
				
					Denmark		
					</div>
				</td>
				<td class='centeralign'>
					DK
				</td>
				<td class=""alignright"">
					318
				</td>
				<td class='centeralign'>
					<a href=""/country/DK"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Latvia"" src=""/images/flags/lv.gif?1282328070"" title=""Latvia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Latvia		
					</div>
				</td>
				<td class='centeralign'>
					LV
				</td>
				<td class=""alignright"">
					313
				</td>
				<td class='centeralign'>
					<a href=""/country/LV"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Norway"" src=""/images/flags/no.gif?1282328076"" title=""Norway"" /></div>
					
					<div class=""down2 floatleft"">
				
					Norway		
					</div>
				</td>
				<td class='centeralign'>
					NO
				</td>
				<td class=""alignright"">
					308
				</td>
				<td class='centeralign'>
					<a href=""/country/NO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Slovenia"" src=""/images/flags/si.gif?1282328084"" title=""Slovenia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Slovenia		
					</div>
				</td>
				<td class='centeralign'>
					SI
				</td>
				<td class=""alignright"">
					287
				</td>
				<td class='centeralign'>
					<a href=""/country/SI"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Finland"" src=""/images/flags/fi.gif?1282328061"" title=""Finland"" /></div>
					
					<div class=""down2 floatleft"">
				
					Finland		
					</div>
				</td>
				<td class='centeralign'>
					FI
				</td>
				<td class=""alignright"">
					282
				</td>
				<td class='centeralign'>
					<a href=""/country/FI"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Israel"" src=""/images/flags/il.gif?1282328066"" title=""Israel"" /></div>
					
					<div class=""down2 floatleft"">
				
					Israel		
					</div>
				</td>
				<td class='centeralign'>
					IL
				</td>
				<td class=""alignright"">
					280
				</td>
				<td class='centeralign'>
					<a href=""/country/IL"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Belgium"" src=""/images/flags/be.gif?1282328039"" title=""Belgium"" /></div>
					
					<div class=""down2 floatleft"">
				
					Belgium		
					</div>
				</td>
				<td class='centeralign'>
					BE
				</td>
				<td class=""alignright"">
					276
				</td>
				<td class='centeralign'>
					<a href=""/country/BE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Viet Nam"" src=""/images/flags/vn.gif?1282328091"" title=""Viet Nam"" /></div>
					
					<div class=""down2 floatleft"">
				
					Viet Nam		
					</div>
				</td>
				<td class='centeralign'>
					VN
				</td>
				<td class=""alignright"">
					269
				</td>
				<td class='centeralign'>
					<a href=""/country/VN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Hungary"" src=""/images/flags/hu.gif?1282328065"" title=""Hungary"" /></div>
					
					<div class=""down2 floatleft"">
				
					Hungary		
					</div>
				</td>
				<td class='centeralign'>
					HU
				</td>
				<td class=""alignright"">
					264
				</td>
				<td class='centeralign'>
					<a href=""/country/HU"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Malaysia"" src=""/images/flags/my.gif?1282328074"" title=""Malaysia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Malaysia		
					</div>
				</td>
				<td class='centeralign'>
					MY
				</td>
				<td class=""alignright"">
					226
				</td>
				<td class='centeralign'>
					<a href=""/country/MY"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Ireland"" src=""/images/flags/ie.gif?1282328066"" title=""Ireland"" /></div>
					
					<div class=""down2 floatleft"">
				
					Ireland		
					</div>
				</td>
				<td class='centeralign'>
					IE
				</td>
				<td class=""alignright"">
					218
				</td>
				<td class='centeralign'>
					<a href=""/country/IE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Chile"" src=""/images/flags/cl.gif?1282328045"" title=""Chile"" /></div>
					
					<div class=""down2 floatleft"">
				
					Chile		
					</div>
				</td>
				<td class='centeralign'>
					CL
				</td>
				<td class=""alignright"">
					206
				</td>
				<td class='centeralign'>
					<a href=""/country/CL"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Greece"" src=""/images/flags/gr.gif?1282328064"" title=""Greece"" /></div>
					
					<div class=""down2 floatleft"">
				
					Greece		
					</div>
				</td>
				<td class='centeralign'>
					GR
				</td>
				<td class=""alignright"">
					198
				</td>
				<td class='centeralign'>
					<a href=""/country/GR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Serbia"" src=""/images/flags/rs.gif?1282328083"" title=""Serbia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Serbia		
					</div>
				</td>
				<td class='centeralign'>
					RS
				</td>
				<td class=""alignright"">
					174
				</td>
				<td class='centeralign'>
					<a href=""/country/RS"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Slovakia"" src=""/images/flags/sk.gif?1282328085"" title=""Slovakia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Slovakia		
					</div>
				</td>
				<td class='centeralign'>
					SK
				</td>
				<td class=""alignright"">
					165
				</td>
				<td class='centeralign'>
					<a href=""/country/SK"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Nigeria"" src=""/images/flags/ng.gif?1282328075"" title=""Nigeria"" /></div>
					
					<div class=""down2 floatleft"">
				
					Nigeria		
					</div>
				</td>
				<td class='centeralign'>
					NG
				</td>
				<td class=""alignright"">
					161
				</td>
				<td class='centeralign'>
					<a href=""/country/NG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Colombia"" src=""/images/flags/co.gif?1282328046"" title=""Colombia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Colombia		
					</div>
				</td>
				<td class='centeralign'>
					CO
				</td>
				<td class=""alignright"">
					155
				</td>
				<td class='centeralign'>
					<a href=""/country/CO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Lithuania"" src=""/images/flags/lt.gif?1282328070"" title=""Lithuania"" /></div>
					
					<div class=""down2 floatleft"">
				
					Lithuania		
					</div>
				</td>
				<td class='centeralign'>
					LT
				</td>
				<td class=""alignright"">
					140
				</td>
				<td class='centeralign'>
					<a href=""/country/LT"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Saudi Arabia"" src=""/images/flags/sa.gif?1282328083"" title=""Saudi Arabia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Saudi Arabia		
					</div>
				</td>
				<td class='centeralign'>
					SA
				</td>
				<td class=""alignright"">
					138
				</td>
				<td class='centeralign'>
					<a href=""/country/SA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Belarus"" src=""/images/flags/by.gif?1282328041"" title=""Belarus"" /></div>
					
					<div class=""down2 floatleft"">
				
					Belarus		
					</div>
				</td>
				<td class='centeralign'>
					BY
				</td>
				<td class=""alignright"">
					128
				</td>
				<td class='centeralign'>
					<a href=""/country/BY"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Lebanon"" src=""/images/flags/lb.gif?1282328069"" title=""Lebanon"" /></div>
					
					<div class=""down2 floatleft"">
				
					Lebanon		
					</div>
				</td>
				<td class='centeralign'>
					LB
				</td>
				<td class=""alignright"">
					127
				</td>
				<td class='centeralign'>
					<a href=""/country/LB"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Kazakhstan"" src=""/images/flags/kz.gif?1282328069"" title=""Kazakhstan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Kazakhstan		
					</div>
				</td>
				<td class='centeralign'>
					KZ
				</td>
				<td class=""alignright"">
					124
				</td>
				<td class='centeralign'>
					<a href=""/country/KZ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Croatia"" src=""/images/flags/hr.gif?1282328065"" title=""Croatia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Croatia		
					</div>
				</td>
				<td class='centeralign'>
					HR
				</td>
				<td class=""alignright"">
					123
				</td>
				<td class='centeralign'>
					<a href=""/country/HR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Panama"" src=""/images/flags/pa.gif?1282328077"" title=""Panama"" /></div>
					
					<div class=""down2 floatleft"">
				
					Panama		
					</div>
				</td>
				<td class='centeralign'>
					PA
				</td>
				<td class=""alignright"">
					119
				</td>
				<td class='centeralign'>
					<a href=""/country/PA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Pakistan"" src=""/images/flags/pk.gif?1282328081"" title=""Pakistan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Pakistan		
					</div>
				</td>
				<td class='centeralign'>
					PK
				</td>
				<td class=""alignright"">
					108
				</td>
				<td class='centeralign'>
					<a href=""/country/PK"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Portugal"" src=""/images/flags/pt.gif?1282328082"" title=""Portugal"" /></div>
					
					<div class=""down2 floatleft"">
				
					Portugal		
					</div>
				</td>
				<td class='centeralign'>
					PT
				</td>
				<td class=""alignright"">
					104
				</td>
				<td class='centeralign'>
					<a href=""/country/PT"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Moldova, Republic of"" src=""/images/flags/md.gif?1282328071"" title=""Moldova, Republic of"" /></div>
					
					<div class=""down2 floatleft"">
				
					Moldova, Republic of		
					</div>
				</td>
				<td class='centeralign'>
					MD
				</td>
				<td class=""alignright"">
					103
				</td>
				<td class='centeralign'>
					<a href=""/country/MD"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Cyprus"" src=""/images/flags/cy.gif?1282328059"" title=""Cyprus"" /></div>
					
					<div class=""down2 floatleft"">
				
					Cyprus		
					</div>
				</td>
				<td class='centeralign'>
					CY
				</td>
				<td class=""alignright"">
					94
				</td>
				<td class='centeralign'>
					<a href=""/country/CY"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Kenya"" src=""/images/flags/ke.gif?1282328068"" title=""Kenya"" /></div>
					
					<div class=""down2 floatleft"">
				
					Kenya		
					</div>
				</td>
				<td class='centeralign'>
					KE
				</td>
				<td class=""alignright"">
					93
				</td>
				<td class='centeralign'>
					<a href=""/country/KE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Estonia"" src=""/images/flags/ee.gif?1282328060"" title=""Estonia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Estonia		
					</div>
				</td>
				<td class='centeralign'>
					EE
				</td>
				<td class=""alignright"">
					87
				</td>
				<td class='centeralign'>
					<a href=""/country/EE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Iraq"" src=""/images/flags/iq.gif?1282328066"" title=""Iraq"" /></div>
					
					<div class=""down2 floatleft"">
				
					Iraq		
					</div>
				</td>
				<td class='centeralign'>
					IQ
				</td>
				<td class=""alignright"">
					80
				</td>
				<td class='centeralign'>
					<a href=""/country/IQ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""United Arab Emirates"" src=""/images/flags/ae.gif?1282328036"" title=""United Arab Emirates"" /></div>
					
					<div class=""down2 floatleft"">
				
					United Arab Emirates		
					</div>
				</td>
				<td class='centeralign'>
					AE
				</td>
				<td class=""alignright"">
					80
				</td>
				<td class='centeralign'>
					<a href=""/country/AE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Venezuela, Bolivarian Republic of"" src=""/images/flags/ve.gif?1282328090"" title=""Venezuela, Bolivarian Republic of"" /></div>
					
					<div class=""down2 floatleft"">
				
					Venezuela, Bolivarian Republic of		
					</div>
				</td>
				<td class='centeralign'>
					VE
				</td>
				<td class=""alignright"">
					78
				</td>
				<td class='centeralign'>
					<a href=""/country/VE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Luxembourg"" src=""/images/flags/lu.gif?1282328070"" title=""Luxembourg"" /></div>
					
					<div class=""down2 floatleft"">
				
					Luxembourg		
					</div>
				</td>
				<td class='centeralign'>
					LU
				</td>
				<td class=""alignright"">
					77
				</td>
				<td class='centeralign'>
					<a href=""/country/LU"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Georgia"" src=""/images/flags/ge.gif?1282328062"" title=""Georgia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Georgia		
					</div>
				</td>
				<td class='centeralign'>
					GE
				</td>
				<td class=""alignright"">
					76
				</td>
				<td class='centeralign'>
					<a href=""/country/GE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Egypt"" src=""/images/flags/eg.gif?1282328060"" title=""Egypt"" /></div>
					
					<div class=""down2 floatleft"">
				
					Egypt		
					</div>
				</td>
				<td class='centeralign'>
					EG
				</td>
				<td class=""alignright"">
					76
				</td>
				<td class='centeralign'>
					<a href=""/country/EG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Armenia"" src=""/images/flags/am.gif?1282328037"" title=""Armenia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Armenia		
					</div>
				</td>
				<td class='centeralign'>
					AM
				</td>
				<td class=""alignright"">
					74
				</td>
				<td class='centeralign'>
					<a href=""/country/AM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Cambodia"" src=""/images/flags/kh.gif?1282328068"" title=""Cambodia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Cambodia		
					</div>
				</td>
				<td class='centeralign'>
					KH
				</td>
				<td class=""alignright"">
					73
				</td>
				<td class='centeralign'>
					<a href=""/country/KH"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Costa Rica"" src=""/images/flags/cr.gif?1282328058"" title=""Costa Rica"" /></div>
					
					<div class=""down2 floatleft"">
				
					Costa Rica		
					</div>
				</td>
				<td class='centeralign'>
					CR
				</td>
				<td class=""alignright"">
					70
				</td>
				<td class='centeralign'>
					<a href=""/country/CR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Tanzania, United Republic of"" src=""/images/flags/tz.gif?1282328089"" title=""Tanzania, United Republic of"" /></div>
					
					<div class=""down2 floatleft"">
				
					Tanzania, United Republic of		
					</div>
				</td>
				<td class='centeralign'>
					TZ
				</td>
				<td class=""alignright"">
					69
				</td>
				<td class='centeralign'>
					<a href=""/country/TZ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Ecuador"" src=""/images/flags/ec.gif?1282328060"" title=""Ecuador"" /></div>
					
					<div class=""down2 floatleft"">
				
					Ecuador		
					</div>
				</td>
				<td class='centeralign'>
					EC
				</td>
				<td class=""alignright"">
					67
				</td>
				<td class='centeralign'>
					<a href=""/country/EC"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Puerto Rico"" src=""/images/flags/pr.gif?1282328082"" title=""Puerto Rico"" /></div>
					
					<div class=""down2 floatleft"">
				
					Puerto Rico		
					</div>
				</td>
				<td class='centeralign'>
					PR
				</td>
				<td class=""alignright"">
					63
				</td>
				<td class='centeralign'>
					<a href=""/country/PR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Iceland"" src=""/images/flags/is.gif?1282328067"" title=""Iceland"" /></div>
					
					<div class=""down2 floatleft"">
				
					Iceland		
					</div>
				</td>
				<td class='centeralign'>
					IS
				</td>
				<td class=""alignright"">
					63
				</td>
				<td class='centeralign'>
					<a href=""/country/IS"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Ghana"" src=""/images/flags/gh.gif?1282328062"" title=""Ghana"" /></div>
					
					<div class=""down2 floatleft"">
				
					Ghana		
					</div>
				</td>
				<td class='centeralign'>
					GH
				</td>
				<td class=""alignright"">
					63
				</td>
				<td class='centeralign'>
					<a href=""/country/GH"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Kuwait"" src=""/images/flags/kw.gif?1282328069"" title=""Kuwait"" /></div>
					
					<div class=""down2 floatleft"">
				
					Kuwait		
					</div>
				</td>
				<td class='centeralign'>
					KW
				</td>
				<td class=""alignright"">
					61
				</td>
				<td class='centeralign'>
					<a href=""/country/KW"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Nepal"" src=""/images/flags/np.gif?1282328076"" title=""Nepal"" /></div>
					
					<div class=""down2 floatleft"">
				
					Nepal		
					</div>
				</td>
				<td class='centeralign'>
					NP
				</td>
				<td class=""alignright"">
					57
				</td>
				<td class='centeralign'>
					<a href=""/country/NP"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Mongolia"" src=""/images/flags/mn.gif?1282328072"" title=""Mongolia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Mongolia		
					</div>
				</td>
				<td class='centeralign'>
					MN
				</td>
				<td class=""alignright"">
					55
				</td>
				<td class='centeralign'>
					<a href=""/country/MN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Albania"" src=""/images/flags/al.gif?1282328036"" title=""Albania"" /></div>
					
					<div class=""down2 floatleft"">
				
					Albania		
					</div>
				</td>
				<td class='centeralign'>
					AL
				</td>
				<td class=""alignright"">
					50
				</td>
				<td class='centeralign'>
					<a href=""/country/AL"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Afghanistan"" src=""/images/flags/af.gif?1282328036"" title=""Afghanistan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Afghanistan		
					</div>
				</td>
				<td class='centeralign'>
					AF
				</td>
				<td class=""alignright"">
					49
				</td>
				<td class='centeralign'>
					<a href=""/country/AF"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Honduras"" src=""/images/flags/hn.gif?1282328065"" title=""Honduras"" /></div>
					
					<div class=""down2 floatleft"">
				
					Honduras		
					</div>
				</td>
				<td class='centeralign'>
					HN
				</td>
				<td class=""alignright"">
					47
				</td>
				<td class='centeralign'>
					<a href=""/country/HN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Azerbaijan"" src=""/images/flags/az.gif?1282328038"" title=""Azerbaijan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Azerbaijan		
					</div>
				</td>
				<td class='centeralign'>
					AZ
				</td>
				<td class=""alignright"">
					47
				</td>
				<td class='centeralign'>
					<a href=""/country/AZ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Uzbekistan"" src=""/images/flags/uz.gif?1282328090"" title=""Uzbekistan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Uzbekistan		
					</div>
				</td>
				<td class='centeralign'>
					UZ
				</td>
				<td class=""alignright"">
					44
				</td>
				<td class='centeralign'>
					<a href=""/country/UZ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Palestinian Territory, Occupied"" src=""/images/flags/ps.gif?1282328082"" title=""Palestinian Territory, Occupied"" /></div>
					
					<div class=""down2 floatleft"">
				
					Palestinian Territory, Occupied		
					</div>
				</td>
				<td class='centeralign'>
					PS
				</td>
				<td class=""alignright"">
					44
				</td>
				<td class='centeralign'>
					<a href=""/country/PS"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Guatemala"" src=""/images/flags/gt.gif?1282328064"" title=""Guatemala"" /></div>
					
					<div class=""down2 floatleft"">
				
					Guatemala		
					</div>
				</td>
				<td class='centeralign'>
					GT
				</td>
				<td class=""alignright"">
					41
				</td>
				<td class='centeralign'>
					<a href=""/country/GT"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Macedonia, The Former Yugoslav Republic of"" src=""/images/flags/mk.gif?1282328072"" title=""Macedonia, The Former Yugoslav Republic of"" /></div>
					
					<div class=""down2 floatleft"">
				
					Macedonia, The Former Yugoslav Republic of		
					</div>
				</td>
				<td class='centeralign'>
					MK
				</td>
				<td class=""alignright"">
					40
				</td>
				<td class='centeralign'>
					<a href=""/country/MK"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Bosnia and Herzegovina"" src=""/images/flags/ba.gif?1282328038"" title=""Bosnia and Herzegovina"" /></div>
					
					<div class=""down2 floatleft"">
				
					Bosnia and Herzegovina		
					</div>
				</td>
				<td class='centeralign'>
					BA
				</td>
				<td class=""alignright"">
					40
				</td>
				<td class='centeralign'>
					<a href=""/country/BA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Angola"" src=""/images/flags/ao.gif?1282328037"" title=""Angola"" /></div>
					
					<div class=""down2 floatleft"">
				
					Angola		
					</div>
				</td>
				<td class='centeralign'>
					AO
				</td>
				<td class=""alignright"">
					40
				</td>
				<td class='centeralign'>
					<a href=""/country/AO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Malta"" src=""/images/flags/mt.gif?1282328073"" title=""Malta"" /></div>
					
					<div class=""down2 floatleft"">
				
					Malta		
					</div>
				</td>
				<td class='centeralign'>
					MT
				</td>
				<td class=""alignright"">
					38
				</td>
				<td class='centeralign'>
					<a href=""/country/MT"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Peru"" src=""/images/flags/pe.gif?1282328077"" title=""Peru"" /></div>
					
					<div class=""down2 floatleft"">
				
					Peru		
					</div>
				</td>
				<td class='centeralign'>
					PE
				</td>
				<td class=""alignright"">
					37
				</td>
				<td class='centeralign'>
					<a href=""/country/PE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Jordan"" src=""/images/flags/jo.gif?1282328067"" title=""Jordan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Jordan		
					</div>
				</td>
				<td class='centeralign'>
					JO
				</td>
				<td class=""alignright"">
					37
				</td>
				<td class='centeralign'>
					<a href=""/country/JO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Uganda"" src=""/images/flags/ug.gif?1282328089"" title=""Uganda"" /></div>
					
					<div class=""down2 floatleft"">
				
					Uganda		
					</div>
				</td>
				<td class='centeralign'>
					UG
				</td>
				<td class=""alignright"">
					36
				</td>
				<td class='centeralign'>
					<a href=""/country/UG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Mauritius"" src=""/images/flags/mu.gif?1282328074"" title=""Mauritius"" /></div>
					
					<div class=""down2 floatleft"">
				
					Mauritius		
					</div>
				</td>
				<td class='centeralign'>
					MU
				</td>
				<td class=""alignright"">
					36
				</td>
				<td class='centeralign'>
					<a href=""/country/MU"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Kyrgyzstan"" src=""/images/flags/kg.gif?1282328068"" title=""Kyrgyzstan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Kyrgyzstan		
					</div>
				</td>
				<td class='centeralign'>
					KG
				</td>
				<td class=""alignright"">
					36
				</td>
				<td class='centeralign'>
					<a href=""/country/KG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Paraguay"" src=""/images/flags/py.gif?1282328082"" title=""Paraguay"" /></div>
					
					<div class=""down2 floatleft"">
				
					Paraguay		
					</div>
				</td>
				<td class='centeralign'>
					PY
				</td>
				<td class=""alignright"">
					32
				</td>
				<td class='centeralign'>
					<a href=""/country/PY"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Curaçao"" src=""/images/flags/cw.gif?1361307263"" title=""Curaçao"" /></div>
					
					<div class=""down2 floatleft"">
				
					Curaçao		
					</div>
				</td>
				<td class='centeralign'>
					CW
				</td>
				<td class=""alignright"">
					32
				</td>
				<td class='centeralign'>
					<a href=""/country/CW"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Dominican Republic"" src=""/images/flags/do.gif?1282328059"" title=""Dominican Republic"" /></div>
					
					<div class=""down2 floatleft"">
				
					Dominican Republic		
					</div>
				</td>
				<td class='centeralign'>
					DO
				</td>
				<td class=""alignright"">
					31
				</td>
				<td class='centeralign'>
					<a href=""/country/DO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Uruguay"" src=""/images/flags/uy.gif?1282328090"" title=""Uruguay"" /></div>
					
					<div class=""down2 floatleft"">
				
					Uruguay		
					</div>
				</td>
				<td class='centeralign'>
					UY
				</td>
				<td class=""alignright"">
					30
				</td>
				<td class='centeralign'>
					<a href=""/country/UY"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Bahrain"" src=""/images/flags/bh.gif?1282328039"" title=""Bahrain"" /></div>
					
					<div class=""down2 floatleft"">
				
					Bahrain		
					</div>
				</td>
				<td class='centeralign'>
					BH
				</td>
				<td class=""alignright"">
					30
				</td>
				<td class='centeralign'>
					<a href=""/country/BH"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Zimbabwe"" src=""/images/flags/zw.gif?1282328093"" title=""Zimbabwe"" /></div>
					
					<div class=""down2 floatleft"">
				
					Zimbabwe		
					</div>
				</td>
				<td class='centeralign'>
					ZW
				</td>
				<td class=""alignright"">
					29
				</td>
				<td class='centeralign'>
					<a href=""/country/ZW"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Mozambique"" src=""/images/flags/mz.gif?1282328074"" title=""Mozambique"" /></div>
					
					<div class=""down2 floatleft"">
				
					Mozambique		
					</div>
				</td>
				<td class='centeralign'>
					MZ
				</td>
				<td class=""alignright"">
					29
				</td>
				<td class='centeralign'>
					<a href=""/country/MZ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Nicaragua"" src=""/images/flags/ni.gif?1282328075"" title=""Nicaragua"" /></div>
					
					<div class=""down2 floatleft"">
				
					Nicaragua		
					</div>
				</td>
				<td class='centeralign'>
					NI
				</td>
				<td class=""alignright"">
					27
				</td>
				<td class='centeralign'>
					<a href=""/country/NI"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""El Salvador"" src=""/images/flags/sv.gif?1282328086"" title=""El Salvador"" /></div>
					
					<div class=""down2 floatleft"">
				
					El Salvador		
					</div>
				</td>
				<td class='centeralign'>
					SV
				</td>
				<td class=""alignright"">
					26
				</td>
				<td class='centeralign'>
					<a href=""/country/SV"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Bolivia, Plurinational State of"" src=""/images/flags/bo.gif?1282328040"" title=""Bolivia, Plurinational State of"" /></div>
					
					<div class=""down2 floatleft"">
				
					Bolivia, Plurinational State of		
					</div>
				</td>
				<td class='centeralign'>
					BO
				</td>
				<td class=""alignright"">
					26
				</td>
				<td class='centeralign'>
					<a href=""/country/BO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Sri Lanka"" src=""/images/flags/lk.gif?1282328070"" title=""Sri Lanka"" /></div>
					
					<div class=""down2 floatleft"">
				
					Sri Lanka		
					</div>
				</td>
				<td class='centeralign'>
					LK
				</td>
				<td class=""alignright"">
					23
				</td>
				<td class='centeralign'>
					<a href=""/country/LK"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Congo, The Democratic Republic of the"" src=""/images/flags/cd.gif?1282328043"" title=""Congo, The Democratic Republic of the"" /></div>
					
					<div class=""down2 floatleft"">
				
					Congo, The Democratic Republic of the		
					</div>
				</td>
				<td class='centeralign'>
					CD
				</td>
				<td class=""alignright"">
					23
				</td>
				<td class='centeralign'>
					<a href=""/country/CD"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Botswana"" src=""/images/flags/bw.gif?1282328041"" title=""Botswana"" /></div>
					
					<div class=""down2 floatleft"">
				
					Botswana		
					</div>
				</td>
				<td class='centeralign'>
					BW
				</td>
				<td class=""alignright"">
					23
				</td>
				<td class='centeralign'>
					<a href=""/country/BW"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Myanmar"" src=""/images/flags/mm.gif?1282328072"" title=""Myanmar"" /></div>
					
					<div class=""down2 floatleft"">
				
					Myanmar		
					</div>
				</td>
				<td class='centeralign'>
					MM
				</td>
				<td class=""alignright"">
					22
				</td>
				<td class='centeralign'>
					<a href=""/country/MM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Montenegro"" src=""/images/flags/me.gif?1282328071"" title=""Montenegro"" /></div>
					
					<div class=""down2 floatleft"">
				
					Montenegro		
					</div>
				</td>
				<td class='centeralign'>
					ME
				</td>
				<td class=""alignright"">
					21
				</td>
				<td class='centeralign'>
					<a href=""/country/ME"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Liechtenstein"" src=""/images/flags/li.gif?1282328070"" title=""Liechtenstein"" /></div>
					
					<div class=""down2 floatleft"">
				
					Liechtenstein		
					</div>
				</td>
				<td class='centeralign'>
					LI
				</td>
				<td class=""alignright"">
					20
				</td>
				<td class='centeralign'>
					<a href=""/country/LI"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Zambia"" src=""/images/flags/zm.gif?1282328093"" title=""Zambia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Zambia		
					</div>
				</td>
				<td class='centeralign'>
					ZM
				</td>
				<td class=""alignright"">
					18
				</td>
				<td class='centeralign'>
					<a href=""/country/ZM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Algeria"" src=""/images/flags/dz.gif?1282328059"" title=""Algeria"" /></div>
					
					<div class=""down2 floatleft"">
				
					Algeria		
					</div>
				</td>
				<td class='centeralign'>
					DZ
				</td>
				<td class=""alignright"">
					18
				</td>
				<td class='centeralign'>
					<a href=""/country/DZ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Tunisia"" src=""/images/flags/tn.gif?1282328087"" title=""Tunisia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Tunisia		
					</div>
				</td>
				<td class='centeralign'>
					TN
				</td>
				<td class=""alignright"">
					17
				</td>
				<td class='centeralign'>
					<a href=""/country/TN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Namibia"" src=""/images/flags/na.gif?1282328075"" title=""Namibia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Namibia		
					</div>
				</td>
				<td class='centeralign'>
					NA
				</td>
				<td class=""alignright"">
					17
				</td>
				<td class='centeralign'>
					<a href=""/country/NA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Lao People&#39;s Democratic Republic"" src=""/images/flags/la.gif?1282328069"" title=""Lao People&#39;s Democratic Republic"" /></div>
					
					<div class=""down2 floatleft"">
				
					Lao People&#39;s Democratic Republic		
					</div>
				</td>
				<td class='centeralign'>
					LA
				</td>
				<td class=""alignright"">
					17
				</td>
				<td class='centeralign'>
					<a href=""/country/LA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Gibraltar"" src=""/images/flags/gi.gif?1282328063"" title=""Gibraltar"" /></div>
					
					<div class=""down2 floatleft"">
				
					Gibraltar		
					</div>
				</td>
				<td class='centeralign'>
					GI
				</td>
				<td class=""alignright"">
					17
				</td>
				<td class='centeralign'>
					<a href=""/country/GI"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Cameroon"" src=""/images/flags/cm.gif?1282328046"" title=""Cameroon"" /></div>
					
					<div class=""down2 floatleft"">
				
					Cameroon		
					</div>
				</td>
				<td class='centeralign'>
					CM
				</td>
				<td class=""alignright"">
					17
				</td>
				<td class='centeralign'>
					<a href=""/country/CM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Rwanda"" src=""/images/flags/rw.gif?1282328083"" title=""Rwanda"" /></div>
					
					<div class=""down2 floatleft"">
				
					Rwanda		
					</div>
				</td>
				<td class='centeralign'>
					RW
				</td>
				<td class=""alignright"">
					16
				</td>
				<td class='centeralign'>
					<a href=""/country/RW"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Belize"" src=""/images/flags/bz.gif?1282328041"" title=""Belize"" /></div>
					
					<div class=""down2 floatleft"">
				
					Belize		
					</div>
				</td>
				<td class='centeralign'>
					BZ
				</td>
				<td class=""alignright"">
					16
				</td>
				<td class='centeralign'>
					<a href=""/country/BZ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Bermuda"" src=""/images/flags/bm.gif?1282328039"" title=""Bermuda"" /></div>
					
					<div class=""down2 floatleft"">
				
					Bermuda		
					</div>
				</td>
				<td class='centeralign'>
					BM
				</td>
				<td class=""alignright"">
					16
				</td>
				<td class='centeralign'>
					<a href=""/country/BM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Seychelles"" src=""/images/flags/sc.gif?1282328084"" title=""Seychelles"" /></div>
					
					<div class=""down2 floatleft"">
				
					Seychelles		
					</div>
				</td>
				<td class='centeralign'>
					SC
				</td>
				<td class=""alignright"">
					15
				</td>
				<td class='centeralign'>
					<a href=""/country/SC"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Malawi"" src=""/images/flags/mw.gif?1282328074"" title=""Malawi"" /></div>
					
					<div class=""down2 floatleft"">
				
					Malawi		
					</div>
				</td>
				<td class='centeralign'>
					MW
				</td>
				<td class=""alignright"">
					15
				</td>
				<td class='centeralign'>
					<a href=""/country/MW"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Virgin Islands, British"" src=""/images/flags/vg.gif?1282328090"" title=""Virgin Islands, British"" /></div>
					
					<div class=""down2 floatleft"">
				
					Virgin Islands, British		
					</div>
				</td>
				<td class='centeralign'>
					VG
				</td>
				<td class=""alignright"">
					14
				</td>
				<td class='centeralign'>
					<a href=""/country/VG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Sierra Leone"" src=""/images/flags/sl.gif?1282328085"" title=""Sierra Leone"" /></div>
					
					<div class=""down2 floatleft"">
				
					Sierra Leone		
					</div>
				</td>
				<td class='centeralign'>
					SL
				</td>
				<td class=""alignright"">
					14
				</td>
				<td class='centeralign'>
					<a href=""/country/SL"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Qatar"" src=""/images/flags/qa.gif?1282328083"" title=""Qatar"" /></div>
					
					<div class=""down2 floatleft"">
				
					Qatar		
					</div>
				</td>
				<td class='centeralign'>
					QA
				</td>
				<td class=""alignright"">
					14
				</td>
				<td class='centeralign'>
					<a href=""/country/QA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Somalia"" src=""/images/flags/so.gif?1282328085"" title=""Somalia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Somalia		
					</div>
				</td>
				<td class='centeralign'>
					SO
				</td>
				<td class=""alignright"">
					13
				</td>
				<td class='centeralign'>
					<a href=""/country/SO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Papua New Guinea"" src=""/images/flags/pg.gif?1282328077"" title=""Papua New Guinea"" /></div>
					
					<div class=""down2 floatleft"">
				
					Papua New Guinea		
					</div>
				</td>
				<td class='centeralign'>
					PG
				</td>
				<td class=""alignright"">
					13
				</td>
				<td class='centeralign'>
					<a href=""/country/PG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Cayman Islands"" src=""/images/flags/ky.gif?1282328069"" title=""Cayman Islands"" /></div>
					
					<div class=""down2 floatleft"">
				
					Cayman Islands		
					</div>
				</td>
				<td class='centeralign'>
					KY
				</td>
				<td class=""alignright"">
					13
				</td>
				<td class='centeralign'>
					<a href=""/country/KY"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Côte d&#39;Ivoire"" src=""/images/flags/ci.gif?1282328045"" title=""Côte d&#39;Ivoire"" /></div>
					
					<div class=""down2 floatleft"">
				
					Côte d&#39;Ivoire		
					</div>
				</td>
				<td class='centeralign'>
					CI
				</td>
				<td class=""alignright"">
					13
				</td>
				<td class='centeralign'>
					<a href=""/country/CI"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Congo"" src=""/images/flags/cg.gif?1282328044"" title=""Congo"" /></div>
					
					<div class=""down2 floatleft"">
				
					Congo		
					</div>
				</td>
				<td class='centeralign'>
					CG
				</td>
				<td class=""alignright"">
					13
				</td>
				<td class='centeralign'>
					<a href=""/country/CG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Trinidad and Tobago"" src=""/images/flags/tt.gif?1282328088"" title=""Trinidad and Tobago"" /></div>
					
					<div class=""down2 floatleft"">
				
					Trinidad and Tobago		
					</div>
				</td>
				<td class='centeralign'>
					TT
				</td>
				<td class=""alignright"">
					12
				</td>
				<td class='centeralign'>
					<a href=""/country/TT"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Haiti"" src=""/images/flags/ht.gif?1282328065"" title=""Haiti"" /></div>
					
					<div class=""down2 floatleft"">
				
					Haiti		
					</div>
				</td>
				<td class='centeralign'>
					HT
				</td>
				<td class=""alignright"">
					12
				</td>
				<td class='centeralign'>
					<a href=""/country/HT"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Gabon"" src=""/images/flags/ga.gif?1282328062"" title=""Gabon"" /></div>
					
					<div class=""down2 floatleft"">
				
					Gabon		
					</div>
				</td>
				<td class='centeralign'>
					GA
				</td>
				<td class=""alignright"">
					12
				</td>
				<td class='centeralign'>
					<a href=""/country/GA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Vanuatu"" src=""/images/flags/vu.gif?1282328091"" title=""Vanuatu"" /></div>
					
					<div class=""down2 floatleft"">
				
					Vanuatu		
					</div>
				</td>
				<td class='centeralign'>
					VU
				</td>
				<td class=""alignright"">
					11
				</td>
				<td class='centeralign'>
					<a href=""/country/VU"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Burundi"" src=""/images/flags/bi.gif?1282328039"" title=""Burundi"" /></div>
					
					<div class=""down2 floatleft"">
				
					Burundi		
					</div>
				</td>
				<td class='centeralign'>
					BI
				</td>
				<td class=""alignright"">
					11
				</td>
				<td class='centeralign'>
					<a href=""/country/BI"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""South Sudan"" src=""/images/flags/ss.gif?1360084805"" title=""South Sudan"" /></div>
					
					<div class=""down2 floatleft"">
				
					South Sudan		
					</div>
				</td>
				<td class='centeralign'>
					SS
				</td>
				<td class=""alignright"">
					10
				</td>
				<td class='centeralign'>
					<a href=""/country/SS"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""New Caledonia"" src=""/images/flags/nc.gif?1282328075"" title=""New Caledonia"" /></div>
					
					<div class=""down2 floatleft"">
				
					New Caledonia		
					</div>
				</td>
				<td class='centeralign'>
					NC
				</td>
				<td class=""alignright"">
					10
				</td>
				<td class='centeralign'>
					<a href=""/country/NC"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Lesotho"" src=""/images/flags/ls.gif?1282328070"" title=""Lesotho"" /></div>
					
					<div class=""down2 floatleft"">
				
					Lesotho		
					</div>
				</td>
				<td class='centeralign'>
					LS
				</td>
				<td class=""alignright"">
					10
				</td>
				<td class='centeralign'>
					<a href=""/country/LS"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Isle of Man"" src=""/images/flags/im.gif?1282328066"" title=""Isle of Man"" /></div>
					
					<div class=""down2 floatleft"">
				
					Isle of Man		
					</div>
				</td>
				<td class='centeralign'>
					IM
				</td>
				<td class=""alignright"">
					10
				</td>
				<td class='centeralign'>
					<a href=""/country/IM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Fiji"" src=""/images/flags/fj.gif?1282328061"" title=""Fiji"" /></div>
					
					<div class=""down2 floatleft"">
				
					Fiji		
					</div>
				</td>
				<td class='centeralign'>
					FJ
				</td>
				<td class=""alignright"">
					10
				</td>
				<td class='centeralign'>
					<a href=""/country/FJ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Brunei Darussalam"" src=""/images/flags/bn.gif?1282328040"" title=""Brunei Darussalam"" /></div>
					
					<div class=""down2 floatleft"">
				
					Brunei Darussalam		
					</div>
				</td>
				<td class='centeralign'>
					BN
				</td>
				<td class=""alignright"">
					10
				</td>
				<td class='centeralign'>
					<a href=""/country/BN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Barbados"" src=""/images/flags/bb.gif?1282328038"" title=""Barbados"" /></div>
					
					<div class=""down2 floatleft"">
				
					Barbados		
					</div>
				</td>
				<td class='centeralign'>
					BB
				</td>
				<td class=""alignright"">
					10
				</td>
				<td class='centeralign'>
					<a href=""/country/BB"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Samoa"" src=""/images/flags/ws.gif?1282328091"" title=""Samoa"" /></div>
					
					<div class=""down2 floatleft"">
				
					Samoa		
					</div>
				</td>
				<td class='centeralign'>
					WS
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/WS"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Virgin Islands, U.S."" src=""/images/flags/vi.gif?1282328090"" title=""Virgin Islands, U.S."" /></div>
					
					<div class=""down2 floatleft"">
				
					Virgin Islands, U.S.		
					</div>
				</td>
				<td class='centeralign'>
					VI
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/VI"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Tajikistan"" src=""/images/flags/tj.gif?1282328087"" title=""Tajikistan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Tajikistan		
					</div>
				</td>
				<td class='centeralign'>
					TJ
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/TJ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Oman"" src=""/images/flags/om.gif?1282328076"" title=""Oman"" /></div>
					
					<div class=""down2 floatleft"">
				
					Oman		
					</div>
				</td>
				<td class='centeralign'>
					OM
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/OM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Madagascar"" src=""/images/flags/mg.gif?1282328071"" title=""Madagascar"" /></div>
					
					<div class=""down2 floatleft"">
				
					Madagascar		
					</div>
				</td>
				<td class='centeralign'>
					MG
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/MG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Morocco"" src=""/images/flags/ma.gif?1282328071"" title=""Morocco"" /></div>
					
					<div class=""down2 floatleft"">
				
					Morocco		
					</div>
				</td>
				<td class='centeralign'>
					MA
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/MA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Libya"" src=""/images/flags/ly.gif?1343786277"" title=""Libya"" /></div>
					
					<div class=""down2 floatleft"">
				
					Libya		
					</div>
				</td>
				<td class='centeralign'>
					LY
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/LY"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Liberia"" src=""/images/flags/lr.gif?1282328070"" title=""Liberia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Liberia		
					</div>
				</td>
				<td class='centeralign'>
					LR
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/LR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Jamaica"" src=""/images/flags/jm.gif?1282328067"" title=""Jamaica"" /></div>
					
					<div class=""down2 floatleft"">
				
					Jamaica		
					</div>
				</td>
				<td class='centeralign'>
					JM
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/JM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Guam"" src=""/images/flags/gu.gif?1282328064"" title=""Guam"" /></div>
					
					<div class=""down2 floatleft"">
				
					Guam		
					</div>
				</td>
				<td class='centeralign'>
					GU
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/GU"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Benin"" src=""/images/flags/bj.gif?1282328039"" title=""Benin"" /></div>
					
					<div class=""down2 floatleft"">
				
					Benin		
					</div>
				</td>
				<td class='centeralign'>
					BJ
				</td>
				<td class=""alignright"">
					9
				</td>
				<td class='centeralign'>
					<a href=""/country/BJ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Swaziland"" src=""/images/flags/sz.gif?1282328086"" title=""Swaziland"" /></div>
					
					<div class=""down2 floatleft"">
				
					Swaziland		
					</div>
				</td>
				<td class='centeralign'>
					SZ
				</td>
				<td class=""alignright"">
					8
				</td>
				<td class='centeralign'>
					<a href=""/country/SZ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""San Marino"" src=""/images/flags/sm.gif?1282328085"" title=""San Marino"" /></div>
					
					<div class=""down2 floatleft"">
				
					San Marino		
					</div>
				</td>
				<td class='centeralign'>
					SM
				</td>
				<td class=""alignright"">
					8
				</td>
				<td class='centeralign'>
					<a href=""/country/SM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Sudan"" src=""/images/flags/sd.gif?1282328084"" title=""Sudan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Sudan		
					</div>
				</td>
				<td class='centeralign'>
					SD
				</td>
				<td class=""alignright"">
					8
				</td>
				<td class='centeralign'>
					<a href=""/country/SD"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Macao"" src=""/images/flags/mo.gif?1282328072"" title=""Macao"" /></div>
					
					<div class=""down2 floatleft"">
				
					Macao		
					</div>
				</td>
				<td class='centeralign'>
					MO
				</td>
				<td class=""alignright"">
					8
				</td>
				<td class='centeralign'>
					<a href=""/country/MO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Guinea"" src=""/images/flags/gn.gif?1282328063"" title=""Guinea"" /></div>
					
					<div class=""down2 floatleft"">
				
					Guinea		
					</div>
				</td>
				<td class='centeralign'>
					GN
				</td>
				<td class=""alignright"">
					8
				</td>
				<td class='centeralign'>
					<a href=""/country/GN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Gambia"" src=""/images/flags/gm.gif?1282328063"" title=""Gambia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Gambia		
					</div>
				</td>
				<td class='centeralign'>
					GM
				</td>
				<td class=""alignright"">
					8
				</td>
				<td class='centeralign'>
					<a href=""/country/GM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Bahamas"" src=""/images/flags/bs.gif?1282328040"" title=""Bahamas"" /></div>
					
					<div class=""down2 floatleft"">
				
					Bahamas		
					</div>
				</td>
				<td class='centeralign'>
					BS
				</td>
				<td class=""alignright"">
					8
				</td>
				<td class='centeralign'>
					<a href=""/country/BS"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Burkina Faso"" src=""/images/flags/bf.gif?1282328039"" title=""Burkina Faso"" /></div>
					
					<div class=""down2 floatleft"">
				
					Burkina Faso		
					</div>
				</td>
				<td class='centeralign'>
					BF
				</td>
				<td class=""alignright"">
					8
				</td>
				<td class='centeralign'>
					<a href=""/country/BF"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt="""" src=""/images/flags/ap.gif?1282328093"" title="""" /></div>
					
					<div class=""down2 floatleft"">
				
							
					</div>
				</td>
				<td class='centeralign'>
					AP
				</td>
				<td class=""alignright"">
					8
				</td>
				<td class='centeralign'>
					<a href=""/country/AP"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Syrian Arab Republic"" src=""/images/flags/sy.gif?1282328086"" title=""Syrian Arab Republic"" /></div>
					
					<div class=""down2 floatleft"">
				
					Syrian Arab Republic		
					</div>
				</td>
				<td class='centeralign'>
					SY
				</td>
				<td class=""alignright"">
					7
				</td>
				<td class='centeralign'>
					<a href=""/country/SY"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Niger"" src=""/images/flags/ne.gif?1282328075"" title=""Niger"" /></div>
					
					<div class=""down2 floatleft"">
				
					Niger		
					</div>
				</td>
				<td class='centeralign'>
					NE
				</td>
				<td class=""alignright"">
					7
				</td>
				<td class='centeralign'>
					<a href=""/country/NE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Jersey"" src=""/images/flags/je.gif?1282328067"" title=""Jersey"" /></div>
					
					<div class=""down2 floatleft"">
				
					Jersey		
					</div>
				</td>
				<td class='centeralign'>
					JE
				</td>
				<td class=""alignright"">
					7
				</td>
				<td class='centeralign'>
					<a href=""/country/JE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Bhutan"" src=""/images/flags/bt.gif?1282328040"" title=""Bhutan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Bhutan		
					</div>
				</td>
				<td class='centeralign'>
					BT
				</td>
				<td class=""alignright"">
					7
				</td>
				<td class='centeralign'>
					<a href=""/country/BT"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Chad"" src=""/images/flags/td.gif?1282328086"" title=""Chad"" /></div>
					
					<div class=""down2 floatleft"">
				
					Chad		
					</div>
				</td>
				<td class='centeralign'>
					TD
				</td>
				<td class=""alignright"">
					6
				</td>
				<td class='centeralign'>
					<a href=""/country/TD"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Solomon Islands"" src=""/images/flags/sb.gif?1282328084"" title=""Solomon Islands"" /></div>
					
					<div class=""down2 floatleft"">
				
					Solomon Islands		
					</div>
				</td>
				<td class='centeralign'>
					SB
				</td>
				<td class=""alignright"">
					6
				</td>
				<td class='centeralign'>
					<a href=""/country/SB"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Grenada"" src=""/images/flags/gd.gif?1282328062"" title=""Grenada"" /></div>
					
					<div class=""down2 floatleft"">
				
					Grenada		
					</div>
				</td>
				<td class='centeralign'>
					GD
				</td>
				<td class=""alignright"">
					6
				</td>
				<td class='centeralign'>
					<a href=""/country/GD"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Faroe Islands"" src=""/images/flags/fo.gif?1282328061"" title=""Faroe Islands"" /></div>
					
					<div class=""down2 floatleft"">
				
					Faroe Islands		
					</div>
				</td>
				<td class='centeralign'>
					FO
				</td>
				<td class=""alignright"">
					6
				</td>
				<td class='centeralign'>
					<a href=""/country/FO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Cuba"" src=""/images/flags/cu.gif?1282328058"" title=""Cuba"" /></div>
					
					<div class=""down2 floatleft"">
				
					Cuba		
					</div>
				</td>
				<td class='centeralign'>
					CU
				</td>
				<td class=""alignright"">
					6
				</td>
				<td class='centeralign'>
					<a href=""/country/CU"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Yemen"" src=""/images/flags/ye.gif?1282328092"" title=""Yemen"" /></div>
					
					<div class=""down2 floatleft"">
				
					Yemen		
					</div>
				</td>
				<td class='centeralign'>
					YE
				</td>
				<td class=""alignright"">
					5
				</td>
				<td class='centeralign'>
					<a href=""/country/YE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Turkmenistan"" src=""/images/flags/tm.gif?1282328087"" title=""Turkmenistan"" /></div>
					
					<div class=""down2 floatleft"">
				
					Turkmenistan		
					</div>
				</td>
				<td class='centeralign'>
					TM
				</td>
				<td class=""alignright"">
					5
				</td>
				<td class='centeralign'>
					<a href=""/country/TM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Suriname"" src=""/images/flags/sr.gif?1282328085"" title=""Suriname"" /></div>
					
					<div class=""down2 floatleft"">
				
					Suriname		
					</div>
				</td>
				<td class='centeralign'>
					SR
				</td>
				<td class=""alignright"">
					5
				</td>
				<td class='centeralign'>
					<a href=""/country/SR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Senegal"" src=""/images/flags/sn.gif?1282328085"" title=""Senegal"" /></div>
					
					<div class=""down2 floatleft"">
				
					Senegal		
					</div>
				</td>
				<td class='centeralign'>
					SN
				</td>
				<td class=""alignright"">
					5
				</td>
				<td class='centeralign'>
					<a href=""/country/SN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Mali"" src=""/images/flags/ml.gif?1282328072"" title=""Mali"" /></div>
					
					<div class=""down2 floatleft"">
				
					Mali		
					</div>
				</td>
				<td class='centeralign'>
					ML
				</td>
				<td class=""alignright"">
					5
				</td>
				<td class='centeralign'>
					<a href=""/country/ML"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Equatorial Guinea"" src=""/images/flags/gq.gif?1282328063"" title=""Equatorial Guinea"" /></div>
					
					<div class=""down2 floatleft"">
				
					Equatorial Guinea		
					</div>
				</td>
				<td class='centeralign'>
					GQ
				</td>
				<td class=""alignright"">
					5
				</td>
				<td class='centeralign'>
					<a href=""/country/GQ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""down2 floatleft"">
				
					Bonaire, Sint Eustatius and Saba		
					</div>
				</td>
				<td class='centeralign'>
					BQ
				</td>
				<td class=""alignright"">
					5
				</td>
				<td class='centeralign'>
					<a href=""/country/BQ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Antigua and Barbuda"" src=""/images/flags/ag.gif?1282328036"" title=""Antigua and Barbuda"" /></div>
					
					<div class=""down2 floatleft"">
				
					Antigua and Barbuda		
					</div>
				</td>
				<td class='centeralign'>
					AG
				</td>
				<td class=""alignright"">
					5
				</td>
				<td class='centeralign'>
					<a href=""/country/AG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Tonga"" src=""/images/flags/to.gif?1282328088"" title=""Tonga"" /></div>
					
					<div class=""down2 floatleft"">
				
					Tonga		
					</div>
				</td>
				<td class='centeralign'>
					TO
				</td>
				<td class=""alignright"">
					4
				</td>
				<td class='centeralign'>
					<a href=""/country/TO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Timor-Leste"" src=""/images/flags/tl.gif?1282328087"" title=""Timor-Leste"" /></div>
					
					<div class=""down2 floatleft"">
				
					Timor-Leste		
					</div>
				</td>
				<td class='centeralign'>
					TL
				</td>
				<td class=""alignright"">
					4
				</td>
				<td class='centeralign'>
					<a href=""/country/TL"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""French Polynesia"" src=""/images/flags/pf.gif?1282328077"" title=""French Polynesia"" /></div>
					
					<div class=""down2 floatleft"">
				
					French Polynesia		
					</div>
				</td>
				<td class='centeralign'>
					PF
				</td>
				<td class=""alignright"">
					4
				</td>
				<td class='centeralign'>
					<a href=""/country/PF"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Maldives"" src=""/images/flags/mv.gif?1282328074"" title=""Maldives"" /></div>
					
					<div class=""down2 floatleft"">
				
					Maldives		
					</div>
				</td>
				<td class='centeralign'>
					MV
				</td>
				<td class=""alignright"">
					4
				</td>
				<td class='centeralign'>
					<a href=""/country/MV"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Guyana"" src=""/images/flags/gy.gif?1282328065"" title=""Guyana"" /></div>
					
					<div class=""down2 floatleft"">
				
					Guyana		
					</div>
				</td>
				<td class='centeralign'>
					GY
				</td>
				<td class=""alignright"">
					4
				</td>
				<td class='centeralign'>
					<a href=""/country/GY"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Guadeloupe"" src=""/images/flags/gp.gif?1282328063"" title=""Guadeloupe"" /></div>
					
					<div class=""down2 floatleft"">
				
					Guadeloupe		
					</div>
				</td>
				<td class='centeralign'>
					GP
				</td>
				<td class=""alignright"">
					4
				</td>
				<td class='centeralign'>
					<a href=""/country/GP"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""French Guiana"" src=""/images/flags/gf.gif?1282328062"" title=""French Guiana"" /></div>
					
					<div class=""down2 floatleft"">
				
					French Guiana		
					</div>
				</td>
				<td class='centeralign'>
					GF
				</td>
				<td class=""alignright"">
					4
				</td>
				<td class='centeralign'>
					<a href=""/country/GF"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Micronesia, Federated States of"" src=""/images/flags/fm.gif?1282328061"" title=""Micronesia, Federated States of"" /></div>
					
					<div class=""down2 floatleft"">
				
					Micronesia, Federated States of		
					</div>
				</td>
				<td class='centeralign'>
					FM
				</td>
				<td class=""alignright"">
					4
				</td>
				<td class='centeralign'>
					<a href=""/country/FM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Dominica"" src=""/images/flags/dm.gif?1282328059"" title=""Dominica"" /></div>
					
					<div class=""down2 floatleft"">
				
					Dominica		
					</div>
				</td>
				<td class='centeralign'>
					DM
				</td>
				<td class=""alignright"">
					4
				</td>
				<td class='centeralign'>
					<a href=""/country/DM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Aruba"" src=""/images/flags/aw.gif?1282328038"" title=""Aruba"" /></div>
					
					<div class=""down2 floatleft"">
				
					Aruba		
					</div>
				</td>
				<td class='centeralign'>
					AW
				</td>
				<td class=""alignright"">
					4
				</td>
				<td class='centeralign'>
					<a href=""/country/AW"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Togo"" src=""/images/flags/tg.gif?1282328086"" title=""Togo"" /></div>
					
					<div class=""down2 floatleft"">
				
					Togo		
					</div>
				</td>
				<td class='centeralign'>
					TG
				</td>
				<td class=""alignright"">
					3
				</td>
				<td class='centeralign'>
					<a href=""/country/TG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Turks and Caicos Islands"" src=""/images/flags/tc.gif?1282328086"" title=""Turks and Caicos Islands"" /></div>
					
					<div class=""down2 floatleft"">
				
					Turks and Caicos Islands		
					</div>
				</td>
				<td class='centeralign'>
					TC
				</td>
				<td class=""alignright"">
					3
				</td>
				<td class='centeralign'>
					<a href=""/country/TC"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""down2 floatleft"">
				
					Sint Maarten (Dutch part)		
					</div>
				</td>
				<td class='centeralign'>
					SX
				</td>
				<td class=""alignright"">
					3
				</td>
				<td class='centeralign'>
					<a href=""/country/SX"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""RÉUNION"" src=""/images/flags/re.gif?1282328083"" title=""RÉUNION"" /></div>
					
					<div class=""down2 floatleft"">
				
					RÉUNION		
					</div>
				</td>
				<td class='centeralign'>
					RE
				</td>
				<td class=""alignright"">
					3
				</td>
				<td class='centeralign'>
					<a href=""/country/RE"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Palau"" src=""/images/flags/pw.gif?1282328082"" title=""Palau"" /></div>
					
					<div class=""down2 floatleft"">
				
					Palau		
					</div>
				</td>
				<td class='centeralign'>
					PW
				</td>
				<td class=""alignright"">
					3
				</td>
				<td class='centeralign'>
					<a href=""/country/PW"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Mauritania"" src=""/images/flags/mr.gif?1282328073"" title=""Mauritania"" /></div>
					
					<div class=""down2 floatleft"">
				
					Mauritania		
					</div>
				</td>
				<td class='centeralign'>
					MR
				</td>
				<td class=""alignright"">
					3
				</td>
				<td class='centeralign'>
					<a href=""/country/MR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""down2 floatleft"">
				
					Saint Martin (French part)		
					</div>
				</td>
				<td class='centeralign'>
					MF
				</td>
				<td class=""alignright"">
					3
				</td>
				<td class='centeralign'>
					<a href=""/country/MF"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Djibouti"" src=""/images/flags/dj.gif?1282328059"" title=""Djibouti"" /></div>
					
					<div class=""down2 floatleft"">
				
					Djibouti		
					</div>
				</td>
				<td class='centeralign'>
					DJ
				</td>
				<td class=""alignright"">
					3
				</td>
				<td class='centeralign'>
					<a href=""/country/DJ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Cape Verde"" src=""/images/flags/cv.gif?1282328058"" title=""Cape Verde"" /></div>
					
					<div class=""down2 floatleft"">
				
					Cape Verde		
					</div>
				</td>
				<td class='centeralign'>
					CV
				</td>
				<td class=""alignright"">
					3
				</td>
				<td class='centeralign'>
					<a href=""/country/CV"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Anguilla"" src=""/images/flags/ai.gif?1282328036"" title=""Anguilla"" /></div>
					
					<div class=""down2 floatleft"">
				
					Anguilla		
					</div>
				</td>
				<td class='centeralign'>
					AI
				</td>
				<td class=""alignright"">
					3
				</td>
				<td class='centeralign'>
					<a href=""/country/AI"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Saint Vincent and the Grenadines"" src=""/images/flags/vc.gif?1282328090"" title=""Saint Vincent and the Grenadines"" /></div>
					
					<div class=""down2 floatleft"">
				
					Saint Vincent and the Grenadines		
					</div>
				</td>
				<td class='centeralign'>
					VC
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/VC"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Holy See (Vatican City State)"" src=""/images/flags/va.gif?1282328090"" title=""Holy See (Vatican City State)"" /></div>
					
					<div class=""down2 floatleft"">
				
					Holy See (Vatican City State)		
					</div>
				</td>
				<td class='centeralign'>
					VA
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/VA"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Nauru"" src=""/images/flags/nr.gif?1282328076"" title=""Nauru"" /></div>
					
					<div class=""down2 floatleft"">
				
					Nauru		
					</div>
				</td>
				<td class='centeralign'>
					NR
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/NR"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Norfolk Island"" src=""/images/flags/nf.gif?1282328075"" title=""Norfolk Island"" /></div>
					
					<div class=""down2 floatleft"">
				
					Norfolk Island		
					</div>
				</td>
				<td class='centeralign'>
					NF
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/NF"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Marshall Islands"" src=""/images/flags/mh.gif?1282328072"" title=""Marshall Islands"" /></div>
					
					<div class=""down2 floatleft"">
				
					Marshall Islands		
					</div>
				</td>
				<td class='centeralign'>
					MH
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/MH"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Saint Kitts and Nevis"" src=""/images/flags/kn.gif?1282328068"" title=""Saint Kitts and Nevis"" /></div>
					
					<div class=""down2 floatleft"">
				
					Saint Kitts and Nevis		
					</div>
				</td>
				<td class='centeralign'>
					KN
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/KN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Guinea-Bissau"" src=""/images/flags/gw.gif?1282328064"" title=""Guinea-Bissau"" /></div>
					
					<div class=""down2 floatleft"">
				
					Guinea-Bissau		
					</div>
				</td>
				<td class='centeralign'>
					GW
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/GW"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Guernsey"" src=""/images/flags/gg.gif?1282328062"" title=""Guernsey"" /></div>
					
					<div class=""down2 floatleft"">
				
					Guernsey		
					</div>
				</td>
				<td class='centeralign'>
					GG
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/GG"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Ethiopia"" src=""/images/flags/et.gif?1282328060"" title=""Ethiopia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Ethiopia		
					</div>
				</td>
				<td class='centeralign'>
					ET
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/ET"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Central African Republic"" src=""/images/flags/cf.gif?1282328044"" title=""Central African Republic"" /></div>
					
					<div class=""down2 floatleft"">
				
					Central African Republic		
					</div>
				</td>
				<td class='centeralign'>
					CF
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/CF"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""down2 floatleft"">
				
					Saint Barthélemy		
					</div>
				</td>
				<td class='centeralign'>
					BL
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/BL"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""American Samoa"" src=""/images/flags/as.gif?1282328037"" title=""American Samoa"" /></div>
					
					<div class=""down2 floatleft"">
				
					American Samoa		
					</div>
				</td>
				<td class='centeralign'>
					AS
				</td>
				<td class=""alignright"">
					2
				</td>
				<td class='centeralign'>
					<a href=""/country/AS"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Mayotte"" src=""/images/flags/yt.gif?1282328092"" title=""Mayotte"" /></div>
					
					<div class=""down2 floatleft"">
				
					Mayotte		
					</div>
				</td>
				<td class='centeralign'>
					YT
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/YT"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Wallis and Futuna"" src=""/images/flags/wf.gif?1282328091"" title=""Wallis and Futuna"" /></div>
					
					<div class=""down2 floatleft"">
				
					Wallis and Futuna		
					</div>
				</td>
				<td class='centeralign'>
					WF
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/WF"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""United States Minor Outlying Islands"" src=""/images/flags/um.gif?1282329010"" title=""United States Minor Outlying Islands"" /></div>
					
					<div class=""down2 floatleft"">
				
					United States Minor Outlying Islands		
					</div>
				</td>
				<td class='centeralign'>
					UM
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/UM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Tokelau"" src=""/images/flags/tk.gif?1282328087"" title=""Tokelau"" /></div>
					
					<div class=""down2 floatleft"">
				
					Tokelau		
					</div>
				</td>
				<td class='centeralign'>
					TK
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/TK"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Sao Tome and Principe"" src=""/images/flags/st.gif?1282328085"" title=""Sao Tome and Principe"" /></div>
					
					<div class=""down2 floatleft"">
				
					Sao Tome and Principe		
					</div>
				</td>
				<td class='centeralign'>
					ST
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/ST"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Saint Pierre and Miquelon"" src=""/images/flags/pm.gif?1282328081"" title=""Saint Pierre and Miquelon"" /></div>
					
					<div class=""down2 floatleft"">
				
					Saint Pierre and Miquelon		
					</div>
				</td>
				<td class='centeralign'>
					PM
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/PM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Niue"" src=""/images/flags/nu.gif?1282328076"" title=""Niue"" /></div>
					
					<div class=""down2 floatleft"">
				
					Niue		
					</div>
				</td>
				<td class='centeralign'>
					NU
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/NU"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Martinique"" src=""/images/flags/mq.gif?1282328073"" title=""Martinique"" /></div>
					
					<div class=""down2 floatleft"">
				
					Martinique		
					</div>
				</td>
				<td class='centeralign'>
					MQ
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/MQ"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Northern Mariana Islands"" src=""/images/flags/mp.gif?1282328073"" title=""Northern Mariana Islands"" /></div>
					
					<div class=""down2 floatleft"">
				
					Northern Mariana Islands		
					</div>
				</td>
				<td class='centeralign'>
					MP
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/MP"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Monaco"" src=""/images/flags/mc.gif?1282328071"" title=""Monaco"" /></div>
					
					<div class=""down2 floatleft"">
				
					Monaco		
					</div>
				</td>
				<td class='centeralign'>
					MC
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/MC"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Saint Lucia"" src=""/images/flags/lc.gif?1282328069"" title=""Saint Lucia"" /></div>
					
					<div class=""down2 floatleft"">
				
					Saint Lucia		
					</div>
				</td>
				<td class='centeralign'>
					LC
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/LC"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Korea, Democratic People&#39;s Republic of"" src=""/images/flags/kp.gif?1282328068"" title=""Korea, Democratic People&#39;s Republic of"" /></div>
					
					<div class=""down2 floatleft"">
				
					Korea, Democratic People&#39;s Republic of		
					</div>
				</td>
				<td class='centeralign'>
					KP
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/KP"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Comoros"" src=""/images/flags/km.gif?1282328068"" title=""Comoros"" /></div>
					
					<div class=""down2 floatleft"">
				
					Comoros		
					</div>
				</td>
				<td class='centeralign'>
					KM
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/KM"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Kiribati"" src=""/images/flags/ki.gif?1282328068"" title=""Kiribati"" /></div>
					
					<div class=""down2 floatleft"">
				
					Kiribati		
					</div>
				</td>
				<td class='centeralign'>
					KI
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/KI"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""British Indian Ocean Territory"" src=""/images/flags/io.gif?1282328066"" title=""British Indian Ocean Territory"" /></div>
					
					<div class=""down2 floatleft"">
				
					British Indian Ocean Territory		
					</div>
				</td>
				<td class='centeralign'>
					IO
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/IO"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Greenland"" src=""/images/flags/gl.gif?1282328063"" title=""Greenland"" /></div>
					
					<div class=""down2 floatleft"">
				
					Greenland		
					</div>
				</td>
				<td class='centeralign'>
					GL
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/GL"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Eritrea"" src=""/images/flags/er.gif?1282328060"" title=""Eritrea"" /></div>
					
					<div class=""down2 floatleft"">
				
					Eritrea		
					</div>
				</td>
				<td class='centeralign'>
					ER
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/ER"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Cook Islands"" src=""/images/flags/ck.gif?1282328045"" title=""Cook Islands"" /></div>
					
					<div class=""down2 floatleft"">
				
					Cook Islands		
					</div>
				</td>
				<td class='centeralign'>
					CK
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/CK"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Åland Islands"" src=""/images/flags/ax.gif?1282328038"" title=""Åland Islands"" /></div>
					
					<div class=""down2 floatleft"">
				
					Åland Islands		
					</div>
				</td>
				<td class='centeralign'>
					AX
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/AX"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Netherlands Antilles"" src=""/images/flags/an.gif?1282328037"" title=""Netherlands Antilles"" /></div>
					
					<div class=""down2 floatleft"">
				
					Netherlands Antilles		
					</div>
				</td>
				<td class='centeralign'>
					AN
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/AN"">Report</a>
				</td>
			</tr>
			
			<tr>
				<td>
					
					<div class=""flag alignright floatright""><img alt=""Andorra"" src=""/images/flags/ad.gif?1282328035"" title=""Andorra"" /></div>
					
					<div class=""down2 floatleft"">
				
					Andorra		
					</div>
				</td>
				<td class='centeralign'>
					AD
				</td>
				<td class=""alignright"">
					1
				</td>
				<td class='centeralign'>
					<a href=""/country/AD"">Report</a>
				</td>
			</tr>
			
		</tbody>
	</table>
</div>
<script type=""text/javascript"">
/* <![CDATA[ */
var google_conversion_id = 1068215855;
var google_conversion_label = ""viUgCKmAuwMQr9yu_QM"";
var google_custom_params = window.google_tag_params;
var google_remarketing_only = true;
/* ]]> */
</script>
<script type=""text/javascript"" src=""//www.googleadservices.com/pagead/conversion.js"">
</script>
<noscript>
<div style=""display:inline;"">
<img height=""1"" width=""1"" style=""border-style:none;"" alt="""" src=""//googleads.g.doubleclick.net/pagead/viewthroughconversion/1068215855/?value=0&amp;label=viUgCKmAuwMQr9yu_QM&amp;guid=ON&amp;script=0""/>
</div>
</noscript>

	</div>
	
	<div id='footer'>
	Updated 30 Dec 2015 09:01 PST &copy; 2015 Hurricane Electric
	</div>
	
	<script type=""text/javascript"">
		var gaJsHost = ((""https:"" == document.location.protocol) ? ""https://ssl."" : ""http://www."");
		document.write(unescape(""%3Cscript src='"" + gaJsHost + ""google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E""));
	</script>
	<script type=""text/javascript"">
		try {
			var pageTracker = _gat._getTracker(""UA-12276073-1"");
			pageTracker._trackPageview();
		} catch(err) {}
	</script>
</body>
</html>
";

            List<CountryInfo> countries = Parser_ANSCountry.GetCountryListHTML(testrow, logger);
            Assert.IsTrue(countries.Count >= 237);
            Assert.AreEqual("United States", countries[0].CountryDescription);
            Assert.AreEqual("US", countries[0].CountryCodeCC);
            Assert.AreEqual(24970, countries[0].ASNs);
            Assert.AreEqual("/country/US", countries[0].reportLink);
        }
    }
}
