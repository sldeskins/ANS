using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Walker
{
    public class Walker
    {
       public static bool Get(Uri uri, out HttpWebResponse response)
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
        public static string GetResponseAsText(HttpWebResponse response)
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

    }
}
