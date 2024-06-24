// Controllers/AccountController.cs
using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using FoodOrderingApp.Session;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MvcAdoExample.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;

        public AccountController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = model.Password, // TODO Note: Hash passwords in a real app!
                };

                var result = await _userRepository.RegisterUserAsync(user);
                if (result)
                {
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "Failed to register user.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("Basket");

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.LoginUserAsync(model.Email, model.Password);
                if (user != null)
                {
                    // Authenticate user (this is just an example, implement proper authentication)
                    HttpContext.Session.SetObject<User>("User", user);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return RedirectToAction("Dish", "Index");

        }
    }
}
