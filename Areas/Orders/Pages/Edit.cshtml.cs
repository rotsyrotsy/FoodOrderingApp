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

namespace FoodOrderingApp.Areas.Orders.Pages
{
    public class EditModel : PageModel
    {
        private readonly FoodOrderingApp.Data.FoodOrderingAppContext _context;

        public EditModel(FoodOrderingApp.Data.FoodOrderingAppContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order =  await _context.Order
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            Order = order;
            ViewData["OrderStates"] = Enum.GetValues(typeof(OrderState)).Cast<OrderState>();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var item = await _context.Order.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            try
            {
                item.state = Order.state;
                Order = item;
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }

    }
}
