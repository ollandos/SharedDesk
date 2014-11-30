using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.IO;
using System.Reflection;

namespace SharedDesk
{
    public class APIService
    {
        private string url;

        public APIService()
        {
            this.url = "http://service.maximize-it.eu";
        }
        
        public Boolean register(string name, string email, string password)
        {
            Uri address = new Uri(this.url + "/register");

            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST  
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //TODO: GENERAL: Security improvement: data encryption
            StringBuilder data = new StringBuilder();
            data.Append("name=" + HttpUtility.UrlEncode(name));
            data.Append("&email=" + HttpUtility.UrlEncode(email));
            data.Append("&password=" + HttpUtility.UrlEncode(password));

            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

            // Set the content length in the request headers  
            request.ContentLength = byteData.Length;

            // Write data  
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
                postStream.Close();
            }

            try
            {
                // Get response  
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    var json = JObject.Parse(rawJson);  //Turns your raw string into a key value lookup
                    string error = json["error"].ToObject<string>();
                    string message = json["message"].ToObject<string>();

                    if (error == "True")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (WebException webEx)
            {
                Console.Out.WriteLine(webEx.StackTrace);
                return false;
            }
        }

        public String login(string email, string password)
        {
            Uri address = new Uri(this.url + "/login");

            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST  
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //TODO: GENERAL: Security improvement: data encryption
            StringBuilder data = new StringBuilder();
            data.Append("email=" + HttpUtility.UrlEncode(email));
            data.Append("&password=" + HttpUtility.UrlEncode(password));

            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

            // Set the content length in the request headers  
            request.ContentLength = byteData.Length;
            request.AllowAutoRedirect = false;

            // Write data  
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
                postStream.Close();
            }

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    var json = JObject.Parse(rawJson);  //Turns your raw string into a key value lookup
                    string error = json["error"].ToObject<string>();

                    if (error == "False")
                    {
                        string apiKey = json["apiKey"].ToObject<string>();
                        return apiKey;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (WebException webEx)
            {
                Console.Out.WriteLine(webEx.StackTrace);
                return null;
            }
        }

        public Boolean addPeer(string apiKey, string uuid, string ip, string port)
        {
            Uri address = new Uri(this.url + "/peers");

            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST  
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            StringBuilder data = new StringBuilder();
            data.Append("uuid=" + HttpUtility.UrlEncode(uuid));
            data.Append("&ip=" + HttpUtility.UrlEncode(ip));
            data.Append("&port=" + HttpUtility.UrlEncode(port));

            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.AllowAutoRedirect = false;

            request.Headers["Authorization"] = apiKey;

            // Set the content length in the request headers  
            request.ContentLength = byteData.Length;

            // Write data  
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
                postStream.Close();
            }

            try
            {
                // Get response  
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    var json = JObject.Parse(rawJson);  //Turns your raw string into a key value lookup
                    
                    string error = json["error"].ToObject<string>();
                    string message = json["message"].ToObject<string>();

                    if (error == "True")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (WebException webEx)
            {
                Console.Out.WriteLine(webEx.StackTrace);
                return false;
            }
        }

        public Boolean updatePeer(string apiKey, string uuid, string ip, string port)
        {
            Uri address = new Uri(this.url + "/peers/" + uuid);

            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST  
            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";

            StringBuilder data = new StringBuilder();
            data.Append("ip=" + HttpUtility.UrlEncode(ip));
            data.Append("&port=" + HttpUtility.UrlEncode(port));

            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

            request.Headers.Add("Authorization", apiKey);

            // Set the content length in the request headers  
            request.ContentLength = byteData.Length;

            // Write data  
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
                postStream.Close();
            }

            try
            {
                // Get response  
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    var json = JObject.Parse(rawJson);  //Turns your raw string into a key value lookup

                    string error = json["error"].ToObject<string>();
                    string message = json["message"].ToObject<string>();

                    if (error == "True")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (WebException webEx)
            {
                Console.Out.WriteLine(webEx.StackTrace);
                return false;
            }
        }

        public Boolean deletePeer(string apiKey, string uuid)
        {
            Uri address = new Uri(this.url + "/peers/" + uuid);

            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST  
            request.Method = "DELETE";
            request.ContentType = "application/x-www-form-urlencoded";

            StringBuilder data = new StringBuilder();
            data.Append("uuid=" + HttpUtility.UrlEncode(uuid));

            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

            request.Headers.Add("Authorization", apiKey);

            // Set the content length in the request headers  
            request.ContentLength = byteData.Length;

            // Write data  
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
                postStream.Close();
            }

            try
            {
                // Get response  
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    var json = JObject.Parse(rawJson);  //Turns your raw string into a key value lookup

                    string error = json["error"].ToObject<string>();
                    string message = json["message"].ToObject<string>();

                    if (error == "True")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (WebException webEx)
            {
                Console.Out.WriteLine(webEx.StackTrace);
                return false;
            }
        }

        public List<PeerInfo> getPeers(string apiKey)
        {
            Uri address = new Uri(this.url + "/peers");

            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            request.Method = "GET";
            request.ContentType = "application/json";

            byte[] authBytes = Encoding.UTF8.GetBytes(apiKey.ToCharArray());
            request.Headers.Add("Authorization", apiKey);

            try
            {
                // Get response  
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var json = JObject.Parse(rawJson);  //Turns your raw string into a key value lookup
                    string error = json["error"].ToObject<string>();

                    if (error == "False")
                    {

                        var peers = JObject.Parse(rawJson).SelectToken("peers").ToObject<List<PeerInfo>>();

                        List<PeerInfo> temp = new List<PeerInfo>();

                        foreach (var obj in peers)
                        {
                            temp.Add(obj);
                        }

                        return temp;

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (WebException webEx)
            {
                Console.Out.WriteLine(webEx.StackTrace);
                return null;
            }
        }
    }
}
