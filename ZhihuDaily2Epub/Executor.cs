using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ZhihuDaily2Epub
{
    public class Executor
    {  
        public string Start()
        {
            var date =   DownLoadHtml.Start();
          var dir = WorkContext.Config.TempDir + date + "/";
 
          var epub = new Epub.Document(); 
          // set metadata
          epub.AddAuthor("xiangwan");
        
          epub.AddTitle("zhihu-daily-"+date);
          epub.AddLanguage("zh");

          // Add image files 
            var images = Directory.GetFiles(dir + "image");
            foreach (var image in images)
            {
                epub.AddImageFile(image, "image\\"+Path.GetFileName(image));
            } 

          // add chapters' xhtml and setup TOC entries
          int navCounter = 1;
            var htmls = Directory.GetFiles(dir, "*.html");
            foreach (var html in htmls)
            {
                var name = Path.GetFileName(html);
                epub.AddXhtmlFile(html, name);
                string text = File.ReadAllText(html);
                string title = Regex.Match(text, "<title>\\s*.*\\s*</title>").Value;
                title = Regex.Match(title, ">\\s*.*\\s*<").Value.Trim('>', '<', ' ', '\t', '\n');

                epub.AddNavPoint(title, name, navCounter++);
            }
         
          var saveDir = WorkContext.Config.SaveDir;
          if (!Directory.Exists(saveDir))
          {
              Directory.CreateDirectory(saveDir);
          }
          // Generate resulting epub file
          epub.Generate(saveDir + "zhihu-daily-" + date + ".epub"); 

           return "ok";
        }
    }
}