namespace CarService.Models
{
    using System.Collections.Generic;

    public class FeedViewModel
    {
        public FeedViewModel()
        {
            this.Posts = new List<PostFbViewModel>(20);
        }

        public ICollection<PostFbViewModel> Posts { get; set; }
    }
}