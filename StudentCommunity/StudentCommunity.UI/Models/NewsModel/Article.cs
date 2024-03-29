﻿namespace StudentCommunity.UI.Models.NewsModel
{
    public class Article
    {
        public string article_id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public List<string> keywords { get; set; }
        public List<string> creator { get; set; }
        public string description { get; set; }
        public string content { get; set; }
        public string pubDate { get; set; }
        public string image_url { get; set; }
        public string source_id { get; set; }
        public List<string> category { get; set; }
        public string language { get; set; }
    }
}
