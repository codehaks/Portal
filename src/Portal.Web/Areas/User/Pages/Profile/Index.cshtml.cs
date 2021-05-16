using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Data.Models;
using Portal.Web.Common.Extentions;

namespace Portal.Web.Areas.User.Pages.Profile
{
    public class IndexModel : PageModel
    {
        public ApplicationUser UserProfile { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;


        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }

        public async Task<IActionResult> OnGet()
        {
            UserProfile = await _userManager.FindByIdAsync(User.GetUserId());
            return Page();
        }
    }
}