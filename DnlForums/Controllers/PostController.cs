using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnlForums.Models.Post;
using DnlForums.Models.Reply;
using DnlForumsData;
using DnlForumsData.Models;
using Microsoft.AspNetCore.Mvc;

namespace DnlForums.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost postService;

        public PostController(IPost postService)
        {
            this.postService = postService;
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
    }
}