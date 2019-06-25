using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DnlForums.Models;
using DnlForums.Models.Home;
using DnlForumsData;
using DnlForums.Models.Post;
using DnlForums.Models.Forum;
using DnlForumsData.Models;

namespace DnlForums.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPost postService;

        public HomeController(IPost postService)
        {
            this.postService = postService;
        }

        public IActionResult Index()
        {
            var model = this.BuildHomeIndexModel();
            return View(model);
        }

        private HomeIndexModel BuildHomeIndexModel()
        {
            var latestPosts = this.postService.GetLatestPost(10);
            var posts = latestPosts.Select(post => new PostListingModel()
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = this.GetForumListingForPost(post)
            });

            return new HomeIndexModel()
            {
                LatestPosts = posts,
                SearchQuery = ""
            };
        }

        private ForumListingModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;
            return new ForumListingModel()
            {
                Id = forum.Id,
                Title = forum.Title,
                ImageUrl = forum.ImageUrl
            };
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
