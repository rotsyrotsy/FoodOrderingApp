﻿using Microsoft.AspNetCore.Mvc;
using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FoodOrderingApp.Session;
using NuGet.ContentModel;

namespace FoodOrderingApp.Controllers
{
    public class DishController : Controller
    {
        private readonly DishRepository _dishRepo;

        public DishController(DishRepository dishRepository)
        {
            _dishRepo = dishRepository;
        }
        public async Task<IActionResult> Index(int pageNumber = 1,string searchTerm = null, int? categoryId = null)
        {
            var dishes = await _dishRepo.GetDishesAsync(pageNumber, 10, searchTerm, categoryId);
            var totalItems = await _dishRepo.GetTotalDishesCountAsync(searchTerm, categoryId);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = 10;
            ViewBag.TotalItems = totalItems;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;

            return View(dishes);
        }
        public async Task<IActionResult> AddToCart(int dishId)
        {
            // Example: Verify if session user is present
            var user = HttpContext.Session.GetObject<User>("User");
            

            if (user == null)
            {
                // If user is not logged in, redirect to login page
                return RedirectToAction("Login", "Account");
            }

            Dish dish = await _dishRepo.GetDishByIdAsync(dishId);


            List<Basket> baskets = HttpContext.Session.GetObject<List<Basket>>("Basket") ?? new List<Basket>();
            // Verify if Dish is already inserted into the Basket and Increment quantity if that : 
            Boolean alreadyIn = false;
            foreach (var basket in baskets)
            {
                if(basket.DishId == dishId)
                {
                    basket.Quantity += 1;
                    basket.BasketPrice = basket.Dish.Price * basket.Quantity;
                    alreadyIn = true;
                }
            }
            if (!alreadyIn)
            {
                Basket bask = new Basket();
                bask.DishId = dish.Id;
                bask.Dish = dish;
                bask.Quantity = 1;
                bask.BasketPrice = bask.Dish.Price;
                baskets.Add(bask);
            }
           
            HttpContext.Session.SetObject("Basket", baskets);

            // Redirect to a page confirming the addition to cart or another appropriate action
            return RedirectToAction("Index", "Basket"); // Adjust as per your application flow

        }

    }
}
