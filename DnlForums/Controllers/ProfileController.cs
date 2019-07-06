using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnlForums.Models.ApplicationUser;
using DnlForumsData;
using DnlForumsData.Models;
using Microsoft.AspNetCore.Http;
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
        
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = this.userManager.GetUserId(User);

            var userImageUrl = this.userService.GetById(userId).ProfileUrl;
            if (!String.IsNullOrEmpty(userImageUrl))
            {
                this.uploadService.DeleteImageFromFile(userImageUrl);
            }
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var imageUrl =  this.uploadService.UploadImageProfle(file);
            await this.userService.SetProfileImage(userId, imageUrl);

            return RedirectToAction("Detail", new { id = userId });
        }

        public IActionResult Index()
        {
            var profiles = this.userService.GetAll().OrderByDescending(user => user.Rating)
                                     .Select(user => new ProfileModel()
                                     {
                                         Email = user.Email,
                                         UserName = user.UserName,
                                         ProfileImageUrl = user.ProfileUrl,
                                         UserRating = user.Rating.ToString(),
                                         MemberSince = user.MemberSince

                                     });
            var model = new ProfileListModel()
            {
                Profiles = profiles
            };

            return View(model);
        }
    }
}