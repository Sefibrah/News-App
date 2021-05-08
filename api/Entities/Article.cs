using System;

namespace api.Entities {
    public class Article {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
    }
}