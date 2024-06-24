using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodOrderingApp.Areas.Categories.Pages
{
    public class CreateModel : PageModel
    {
        private readonly FoodOrderingAppContext _context;

        public CreateModel(FoodOrderingAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = new();
        public string ErrorMessage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var item = new Category();
            if (await TryUpdateModelAsync(item, "Category", f => f.Name))
            {
                _context.Category.Add(item);
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
