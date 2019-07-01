using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnlForums.Models.ApplicationUser;
using DnlForumsData;
using DnlForumsData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DnlForums.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApplicationUser userService;
        private readonly IUpload uploadService;

        public ProfileController(UserManager<ApplicationUser> userManager,
                                 IApplicationUser userService,
                                 IUpload uploadService)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.uploadService = uploadService;
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = this.userService.GetById(id);
            var userRoles = await this.userManager.GetRolesAsync(user);
            var model = new ProfileModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileUrl,
                MemberSince = user.MemberSince,
                IsAdmin = userRoles.Contains("Admin")
            };

            return View(model);
        }
    }
}