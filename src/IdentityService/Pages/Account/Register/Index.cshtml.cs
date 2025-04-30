using System.Security.Claims;
using Duende.IdentityModel;
using IdentityService.Models;
using IdentityService.Pages.Account.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Register
{
    [SecurityHeaders]
    [AllowAnonymous]

    public class Index : PageModel
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public Index(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        [BindProperty]
        public RegisterViewModel Input { get; set; }
        [BindProperty]
        public bool RegisterSuccess { get; set; }

        

        public IActionResult OnGet(string ReturnUrl)
        {
            Input = new RegisterViewModel
            {
                ReturnUrl = ReturnUrl
            };
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (Input.Button != "register") return Redirect("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    EmailConfirmed = true
                };
                var result = await UserManager.CreateAsync(user,Input.Password);

                if (result.Succeeded)
                {
                    await UserManager.AddClaimsAsync(user,new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name,Input.FullName)
                    });
                    RegisterSuccess = true;
                }
            }

            return Page();

        }
    }
}
