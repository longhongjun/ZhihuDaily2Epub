using System;
using System.Collections.Generic;

namespace ZhihuDaily2Epub.Model
{
    public class New
    {
        public string body { get; set; }
        public string image_source { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string share_url { get; set; }
        public List<object> js { get; set; }
        public string thumbnail { get; set; }
        public string ga_prefix { get; set; }
        public int id { get; set; }
        public List<string> css { get; set; }
    }
    public class News
    {
        public string title { get; set; }
        public string url { get; set; }
        public string image { get; set; }
        public string share_url { get; set; }
        public string thumbnail { get; set; }
        public string ga_prefix { get; set; }
        public int id { get; set; }
    }
     

    public class Daily
    {
        public string date { get; set; }
        public List<News> news { get; set; }
        public bool is_today { get; set; }
        public List<News> top_stories { get; set; }
        public string display_date { get; set; }
    }
}