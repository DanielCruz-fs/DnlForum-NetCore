using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnlForums.Models.Reply;
using DnlForumsData;
using DnlForumsData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DnlForums.Controllers
{
    public class ReplyController : Controller
    {
        private readonly IPost postService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReplyController(IPost postService, UserManager<ApplicationUser> userManager)
        {
            this.postService = postService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create(int id)
        {
            var post = this.postService.GetById(id);
            var user = await this.userManager.FindByNameAsync(User.Identity.Name);

            var model = new PostReplyModel()
            {
                PostId = post.Id,
                PostContent = post.Content,
                PostTitle = post.Title,

                AuthorId = user.Id,
                AuthorName = User.Identity.Name,
                AuthorImageUrl = user.ProfileUrl,
                AuthorRating = user.Rating,
                IsAuthorAdmin = User.IsInRole("Admin"),

                ForumId = post.Forum.Id,
                ForumImageUrl = post.Forum.ImageUrl,
                ForumName = post.Forum.Title,

                Created = DateTime.Now
            };

            return View(model);
        }

        public async Task<IActionResult> AddReply(PostReplyModel model)
        {
            var userId = this.userManager.GetUserId(User);
            var user = await this.userManager.FindByIdAsync(userId);

            var reply = this.BuilderReply(model, user);
            await this.postService.AddReply(reply);

            return RedirectToAction("Index", "Post", new { id = model.PostId });
        }

        private PostReply BuilderReply(PostReplyModel model, ApplicationUser user)
        {
            var post = this.postService.GetById(model.PostId);

            return new PostReply()
            {
                Post = post,
                Content = model.ReplyContent,
                Created = DateTime.Now,
                User = user
            };

        }
    }
}