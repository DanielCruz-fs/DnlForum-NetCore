using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnlForums.Models.Post;
using DnlForums.Models.Reply;
using DnlForumsData;
using DnlForumsData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DnlForums.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost postService;
        private readonly IForum forumService;
        private readonly UserManager<ApplicationUser> userManager;

        public PostController(IPost postService, IForum forumService, UserManager<ApplicationUser> userManager) 
        {
            this.postService = postService;
            this.forumService = forumService;
            this.userManager = userManager;
        }
        public IActionResult Index(int id)
        {
            var post = this.postService.GetById(id);
            var replies = this.BuildPostReplies(post.Replies);
            var model = new PostIndexModel()
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileUrl,
                AuthorRating = post.User.Rating,
                Created = post.Created,
                PostContent = post.Content,
                Replies = replies
            };
            return View(model);
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel()
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.ProfileUrl,
                AuthorRating = reply.User.Rating,
                Created = reply.Created,
                ReplyContent = reply.Content
            });
        }

        public IActionResult Create(int id)
        {
            var forum = this.forumService.GetById(id);
            var model = new NewPostModel()
            {
                ForumId = forum.Id,
                ForumName = forum.Title,
                ForumImageUrl = forum.ImageUrl,
                AuthorName = User.Identity.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            var userId = this.userManager.GetUserId(User);
            var user = await this.userManager.FindByIdAsync(userId);

            var post = this.BuildPost(model, user);
            await this.postService.Add(post);

            return RedirectToAction("Index", "Post", new { id = post.Id });
        }

        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            var forum = this.forumService.GetById(model.ForumId);
            return new Post()
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                Forum = forum
            };
        }
    }
}