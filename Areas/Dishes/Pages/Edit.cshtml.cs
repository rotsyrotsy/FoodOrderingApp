using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodOrderingApp.Data;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Areas.Dishes.Pages
{
    public class EditModel : PageModel
    {
        private readonly FoodOrderingApp.Data.FoodOrderingAppContext _context;

        public EditModel(FoodOrderingApp.Data.FoodOrderingAppContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Dish Dish { get; set; } = default!;
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish =  await _context.Dish.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            Dish = dish;
           ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var item = await _context.Dish.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync(item, "Dish"))
            {
                item.DateUpdate = DateTime.Now;
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
