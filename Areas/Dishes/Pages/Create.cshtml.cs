using FoodOrderingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging.Signing;

namespace FoodOrderingApp.Areas.Dishes.Pages
{
    public class CreateModel : PageModel
    {
        private readonly FoodOrderingApp.Data.FoodOrderingAppContext _context;

        public CreateModel(FoodOrderingApp.Data.FoodOrderingAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Dish Dish { get; set; } = new();
        public string ErrorMessage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var newItem = new Dish();

            if (await TryUpdateModelAsync(newItem, "Dish"))
            {
                newItem.DateCreation = DateTime.Now;
                _context.Dish.Add(newItem);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            var validationErrors = ModelState.Values.Where(E => E.Errors.Count > 0)
            .SelectMany(E => E.Errors)
            .Select(E => E.ErrorMessage)
            .ToList();
            if (validationErrors.Count > 0)
            {
                foreach (var error in validationErrors)
                {
                    ErrorMessage += $"{error}\n";
                }
            }
            return Page();
        }
    }
}
