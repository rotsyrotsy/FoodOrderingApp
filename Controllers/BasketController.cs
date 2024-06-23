using FoodOrderingApp.Models;
using FoodOrderingApp.Session;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingApp.Controllers
{
    public class BasketController : Controller
    {
        public async Task<IActionResult> Index()
        {
            // Example: Verify if session user is present
            var user = HttpContext.Session.GetObject<User>("User");

            if (user == null)
            {
                // If user is not logged in, redirect to login page
                return RedirectToAction("Login", "Account");
            }

            List<Basket> baskets = HttpContext.Session.GetObject<List<Basket>>("Basket") ?? new List<Basket>();
            decimal total = 0;
            foreach (var basket in baskets)
            {
                total += basket.Dish.Price;
            }

            ViewBag.TotalPrice = total;

            return View(baskets);
        }
    }
}
