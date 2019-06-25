using DnlForums.Models.Post;
using System.Collections.Generic;

namespace DnlForums.Models.Home
{
    public class HomeIndexModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<PostListingModel> LatestPosts { get; set; }
    }
}
