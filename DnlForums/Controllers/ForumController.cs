using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnlForums.Models.Forum;
using DnlForumsData;
using Microsoft.AspNetCore.Mvc;

namespace DnlForums.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum forumService;

        public ForumController(IForum forumService)
        {
            this.forumService = forumService;
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
    }
}