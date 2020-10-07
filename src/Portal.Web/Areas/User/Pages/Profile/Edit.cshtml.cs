using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Domain;
using Portal.Web.Common.Extentions;

namespace Portal.Web.Areas.User.Pages.Profile
{
    public class EditModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;


        public EditModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }

        public class UserProfileModel
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        [BindProperty]
        public UserProfileModel Input { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var user = (await _userManager.FindByIdAsync(User.GetUserId()));
            Input = new UserProfileModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id
            };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Input.Id = User.GetUserId();

            var user = await _userManager.FindByIdAsync(Input.Id);
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            await _userManager.UpdateAsync(user);
            return RedirectToPage("index");
        }
    }
}