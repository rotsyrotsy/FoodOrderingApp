using FoodOrderingApp.Data;
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
        private IWebHostEnvironment _environment;

        public CreateModel(FoodOrderingApp.Data.FoodOrderingAppContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Dish Dish { get; set; } = new();
        public string ErrorMessage { get; set; }
        public IFormFile Upload { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostFormDishesAsync()
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
        public async Task<IActionResult> OnPostFormUploadAsync()
        {
            try
            {
                var file = Path.Combine(_environment.ContentRootPath, "wwwroot/uploads", Upload.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }
                List<Dish> dishes;
                if (Path.GetExtension(file).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    var csvReader = new DishesCsvReader(_context);
                    dishes = csvReader.ReadCsvFile(file);
                }
                else
                {
                    throw new Exception("Unsupported file format");
                }
                // Insert projects into the database
                _context.Dish.AddRange(dishes);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return Page();
            }

        }
    }
}

