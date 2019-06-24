using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnlForums.Models.Forum;
using DnlForums.Models.Post;
using DnlForumsData;
using DnlForumsData.Models;
using Microsoft.AspNetCore.Mvc;

namespace DnlForums.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum forumService;
        private readonly IPost postService;

        public ForumController(IForum forumService, IPost postService)
        {
            this.forumService = forumService;
            this.postService = postService;
        }
        public IActionResult Index()
        {
            var forums = this.forumService.GetAll().Select(forum => new ForumListingModel()
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description
            });

            var model = new ForumIndexModel()
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id)
        {
            var forum = this.forumService.GetById(id);
            var posts = forum.Posts;

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

            var model = new ForumTopicModel()
            {
                Posts = postListing,
                Forum = this.BuildForumListing(forum)
            };

            return View(model);
        }
        
        //this private method has an overload to accept two different entities
        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return this.BuildForumListing(forum);
        }

        private ForumListingModel BuildForumListing(Forum forum)
        {
            return new ForumListingModel()
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }
    }
}