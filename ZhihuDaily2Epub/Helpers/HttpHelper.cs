using System.IO;
using System.Net;
using System.Text;

namespace ZhihuDaily2Epub.Helpers{
    public class HttpHelper{
        public static string Get(string url) {
            var cc = new CookieContainer();
            string referer = string.Empty;
            return Get(url, ref cc, referer);
        }

        public static string Get(string url, Encoding ed) {
            var cc = new CookieContainer();
            string referer = string.Empty;
            return Get(url, ref cc, referer, ed);
        }

        public static string Get(string url, ref CookieContainer cc, Encoding ed) {
            string referer = string.Empty;
            return Get(url, ref cc, referer, ed);
        }

        public static string Get(string URL, ref CookieContainer cc, string referer) {
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(URL);
            httpWebRequest.Accept =
                    "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            httpWebRequest.Referer = referer;
            httpWebRequest.MaximumAutomaticRedirections = 10;
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.Timeout = 100000;
            httpWebRequest.ReadWriteTimeout = 100000;
            httpWebRequest.CookieContainer = cc;
            httpWebRequest.UserAgent =
                    "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
            httpWebRequest.Method = "GET";
            HttpWebResponse webResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            StreamReader sr2 = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            return sr2.ReadToEnd();
        }

        public static string Get(string URL, ref CookieContainer cc, string referer, Encoding ed) {
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(URL);
            httpWebRequest.Accept =
                    "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            httpWebRequest.Referer = referer;
            httpWebRequest.Timeout = 100000;
            httpWebRequest.MaximumAutomaticRedirections = 10;
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.ReadWriteTimeout = 100000;
            httpWebRequest.CookieContainer = cc;
            httpWebRequest.UserAgent =
                    "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.15 (KHTML, like Gecko) Chrome/10.0.612.3 Safari/534.15";
            httpWebRequest.Method = "GET";
            HttpWebResponse webResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            StreamReader sr2 = new StreamReader(webResponse.GetResponseStream(), ed);
            return sr2.ReadToEnd();
        }

        public static string Post(string URL, byte[] byteRequest, string referer, ref CookieContainer cc, string encode) {
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(URL);
            httpWebRequest.CookieContainer = cc;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Accept =
                    "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            httpWebRequest.Referer = referer;
            httpWebRequest.UserAgent =
                    "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.15 (KHTML, like Gecko) Chrome/10.0.612.3 Safari/534.15";
            httpWebRequest.Method = "Post";
            httpWebRequest.MaximumAutomaticRedirections = 10;
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.ContentLength = byteRequest.Length;
            Stream stream = httpWebRequest.GetRequestStream();
            stream.Write(byteRequest, 0, byteRequest.Length);
            stream.Close();
            HttpWebResponse webResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            StreamReader sr2 = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding(encode));
            return sr2.ReadToEnd();
        }
    }
}