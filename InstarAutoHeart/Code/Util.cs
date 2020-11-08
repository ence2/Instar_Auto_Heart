using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Threading;

namespace InstarAutoHeart
{
    public class Util
    {
        public static string MakeKey(string appId, string deviceToken)
        {
            return appId + "_" + deviceToken;
        }

        static public Uri GetFindLocalIP(String url)
        {
            var urlFormat = url.Split(new string[] { "://", ":" }, StringSplitOptions.RemoveEmptyEntries);
            String host = urlFormat[1];
            String schema = urlFormat[0];
            String port = urlFormat[2];

            String combine = "";
            System.Net.IPHostEntry host2;

            if (urlFormat[1] != "localhost")
            {
                String[] ipClasses = host.Split(new char[] { '.' });
                if (ipClasses.Length < 4)
                {
                    host2 = System.Net.Dns.GetHostEntry(host);
                }
                else
                {
                    if (ipClasses[2] == "*")
                        combine = String.Format("{0}.{1}", ipClasses[0], ipClasses[1]);
                    else if (ipClasses[3] == "*")
                        combine = String.Format("{0}.{1}.{2}", ipClasses[0], ipClasses[1], ipClasses[2]);
                    else
                    {
                        UriBuilder uri = new UriBuilder(url);
                        return uri.Uri;
                    }
                    host2 = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                }
            }
            else
            {
                host2 = System.Net.Dns.GetHostEntry(host);
            }

            string clientIP = string.Empty;
            for (int i = 0; i < host2.AddressList.Length; i++)
            {
                if (host2.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    clientIP = host2.AddressList[i].ToString();

                    if (clientIP.StartsWith(combine))
                    {
                        UriBuilder uri = new UriBuilder(schema, clientIP, Int32.Parse(port));
                        return uri.Uri;
                    }
                }
            }

            throw new Exception("Not found host");
        }

        public static string FindLocalIP(string url)
        {
            UriBuilder builder = new UriBuilder(url);

            if (builder.Host == "localhost")
            {
                builder.Host = "127.0.0.1";
            }

            string[] ipClasses = builder.Host.Split(new char[] { '.' });
            string combine = "";
            if (ipClasses[2] == "*")
                combine = string.Format("{0}.{1}", ipClasses[0], ipClasses[1], ipClasses[2]);
            else if (ipClasses[3] == "*")
                combine = string.Format("{0}.{1}.{2}", ipClasses[0], ipClasses[1], ipClasses[2]);
            else
                return builder.ToString();

            System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            string clientIP = string.Empty;
            for (int i = 0; i < hostEntry.AddressList.Length; i++)
            {
                if (hostEntry.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    clientIP = hostEntry.AddressList[i].ToString();

                    if (clientIP.StartsWith(combine))
                    {
                        builder.Host = clientIP;
                        return builder.ToString();
                    }
                }
            }

            throw new Exception("Not found host");
        }

        public static object ToObject(string json, Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);
        }

        public static T ToObject<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.None, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        public static DateTime Next(DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }

        public static int RoundToInt(float f)
        {
            return (int)Math.Round((double)f);
        }
    }
}
