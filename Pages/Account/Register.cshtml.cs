using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FoodOrderingApp.Data;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly FoodOrderingApp.Data.FoodOrderingAppContext _context;

        public RegisterModel(FoodOrderingApp.Data.FoodOrderingAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;
        public string SuccessMessage { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                if (Request.Form["confirmPassword"] == User.Password)
                {
                    var role = _context.Role.Where(r => r.Name == "ROLE_RESTAURANT").FirstOrDefault();
                    if (role != null)
                    {
                        User.RoleId = role.Id;
                        User.Restaurant.Email = User.Email;
                        User.Restaurant.Phone = User.Phone;
                        _context.User.Add(User);
                        await _context.SaveChangesAsync();
                        SuccessMessage = "Account registered successfully.";
                        return Page();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Role not found.");
                    }
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
