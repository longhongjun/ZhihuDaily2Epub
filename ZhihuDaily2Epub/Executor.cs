using System;
using System.IO;
using SharpEpub;

namespace ZhihuDaily2Epub
{
    public class Executor
    {  
        public string Start()
        {
            var date =  DownLoadHtml.Start();
          var dir = WorkContext.Config.TempDir + date + "/";
          Epub epub = new Epub(dir, TocOptions.ByTitleTag);
          epub.Metadata.Creator = "向晚";
          epub.Metadata.Publisher = "知乎";
          epub.Metadata.Language = "zh-CN";
          epub.Metadata.Date = DateTime.Now.ToString("yyyy-MM-dd");
          epub.Metadata.Title = "知乎日报-"+date;
          epub.Structure.Directories.ImageFolder = "image"; 
          epub.DirectorySearchOption = SearchOption.AllDirectories;
            var saveDir = WorkContext.Config.SaveDir;
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }
            epub.BuildToFile(saveDir +"zhihu-daily-"+ date + ".epub");
           return "ok";
        }
    }
}