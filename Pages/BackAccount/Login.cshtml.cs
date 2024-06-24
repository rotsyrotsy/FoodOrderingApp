using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FoodOrderingApp.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingApp.Pages.BackAccount
{
    public class LoginModel : PageModel
    {
        [TempData]
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
        [BindProperty, Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [BindProperty, Required, DataType(DataType.Password)]
        public string Password { get; set; }

        private readonly FoodOrderingAppContext _context;

        public LoginModel(FoodOrderingAppContext context)
        {
            _context = context;
        }

        public void OnGet(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("/Dishes");

            if (ModelState.IsValid)
            {
                var verificationResult = await _context.User.AnyAsync(u => u.Email == Email && u.Password == Password);

                if (verificationResult)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, Email)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return Redirect(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
