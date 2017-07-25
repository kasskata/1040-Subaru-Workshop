namespace CarService.Models
{
    using System;
    using System.Collections.Generic;

    public class PostFbViewModel
    {
        public string Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Message { get; set; }

        public int CommentsCount { get; set; }

        public IList<string> Images { get; set; }
    }
}