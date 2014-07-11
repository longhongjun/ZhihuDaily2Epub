using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NLog;
using ZhihuDaily2Epub.Helpers;
using ZhihuDaily2Epub.Model;

namespace ZhihuDaily2Epub
{
    public class DownLoadHtml
    {
        private   const string HtmlTemplate = @"<!DOCTYPE html> 
<html lang='zh' xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta charset='utf-8' />
    <title>**Title**</title>
</head>
<body>
<h1>**Title**</h1>
**Body**
</body>
</html>";
        public static string Start()
        {
            var rawList = HttpHelper.Get(WorkContext.Config.ApiUrl);
            Daily daily = JsonConvert.DeserializeObject<Daily>(rawList);
            DownLoadNews(daily.top_stories,daily.date);
            DownLoadNews(daily.news, daily.date);
            return daily.date;
        }

        private static void DownLoadNews(List<News> news,string date)
        {
            var regex = new Regex(@"(?<=\<img.+?src=\"")(.+?\/)([^/]+?)(?=\"".*?\>)",
                RegexOptions.Compiled | RegexOptions.Multiline);
            var dir = WorkContext.Config.TempDir + date+"/";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var imageDir = dir + "image/";
            if (!Directory.Exists(imageDir))
            {
                Directory.CreateDirectory(imageDir);
            }
            using (var httpClient = new WebClient())
            { 
                foreach (var @new in news)
                {
                    var raw = HttpHelper.Get(@new.url);
                    New new1 = JsonConvert.DeserializeObject<New>(raw);
                    var htmlBody = new1.body;
                    var matchs = regex.Matches(htmlBody);
                    if (matchs.Count>0)
                    {
                        foreach (Match match in matchs)
                        {
                            var url = match.Groups[0].Value;
                            var name = match.Groups[2].Value;
                            try
                            {
                                httpClient.DownloadFile(url, imageDir + name);
                            }
                            catch (Exception e)
                            { 
                                LogManager.GetCurrentClassLogger().Error(e);
                            }
                          
                        }
                        htmlBody = regex.Replace(htmlBody, "image/$2");
                    }
                    var html = HtmlTemplate.Replace("**Title**", @new.title).Replace("**Body**", htmlBody);
                    using (FileStream  file=File.Create(dir+@new.id+".html"))
                    {
                        var b = Encoding.UTF8.GetBytes(html);
                        file.Write(b, 0, b.Length);
                    }
                } 
            }
        }
    }
}