using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingApp.Pages.Account
{
    public class RegisterModelTest : PageModel
    {
        private readonly FoodOrderingAppContext _context;

        public RegisterModelTest(FoodOrderingAppContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public string ConfirmPassword { get; set; }
        public User NewUser { get; set; } = new();
        public Restaurant NewRestaurant { get; set; }
        public string SuccessMessage { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var test = ConfirmPassword;
                var user = NewUser;
                var resto = NewRestaurant;
                if (ConfirmPassword == NewUser.Password)
                {
                    NewRestaurant.Phone = NewUser.Phone;
                    NewRestaurant.Email = NewUser.Email;
                    await _context.SaveChangesAsync();
                    SuccessMessage = "Account registered successfully.";
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Password and confirm password do not match.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return Page();
        }

    }
}
