using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOrderingApp.Areas.Orders.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FoodOrderingApp.Data.FoodOrderingAppContext _context;

        public IndexModel(FoodOrderingApp.Data.FoodOrderingAppContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;
        public IEnumerable<OrderState> OrderStates { get; set; } = default!;

        public async Task OnGetAsync(OrderState? stateFilter = null)
        {

            Order = await _context.Order
                .Where(o => o.state == stateFilter)
                .Include(o => o.User).ToListAsync();
            OrderStates = Enum.GetValues(typeof(OrderState)).Cast<OrderState>();
            ViewData["stateFilter"] = stateFilter.GetValueOrDefault().ToString();

        }
    }
}
