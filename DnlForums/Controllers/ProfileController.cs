using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnlForumsData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DnlForums.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileController(UserManager<ApplicationUser> userManager,
                                 IAplicationUser userService,
                                 IUpload uploadService)
        {
            this.userManager = userManager;
        }

        public IActionResult Detail(string id)
        {
            return View();
        }
    }
}