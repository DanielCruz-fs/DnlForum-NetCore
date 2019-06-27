using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnlForums.Models.Forum;
using DnlForums.Models.Post;
using DnlForums.Models.Search;
using DnlForumsData;
using DnlForumsData.Models;
using Microsoft.AspNetCore.Mvc;

namespace DnlForums.Controllers
{
    public class SearchController : Controller
    {
        private readonly IPost postService;

        public SearchController(IPost postService)
        {
            this.postService = postService;
        }

        public IActionResult Results(string searchQuery)
        {
            var posts = this.postService.GetFilteredPosts(searchQuery);
            var areNoResults = (!String.IsNullOrEmpty(searchQuery) && !posts.Any());
            var postListing = posts.Select(post => new PostListingModel()
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = this.BuildForumListing(post)
            });
            var model = new SearchResultModel()
            {
                Posts = postListing,
                SearchQuery = searchQuery,
                EmptySearchResults = areNoResults
            };

            return View(model);
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;

            return new ForumListingModel()
            {
                Id = forum.Id,
                ImageUrl = forum.ImageUrl,
                Title = forum.Title,
                Description = forum.Description
            };
        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Results", new { searchQuery });
        }
    }
}