using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Domain;

namespace Portal.Web.Areas.User.Pages.Profile
{
    public class ConfirmPhoneNumberModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmPhoneNumberModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public String Code { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            //var smsSender = new SmsSender();
            //await smsSender.SendSmsAsync(user.UserName, $"کد تایید سیبسان : {code}");

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            currentUser.PhoneNumberConfirmed = await _userManager.VerifyChangePhoneNumberTokenAsync(currentUser, Code, currentUser.PhoneNumber);
            await _userManager.UpdateAsync(currentUser);

            return RedirectToPage("/profile/index", new { area = "user" });

        }
    }
}
