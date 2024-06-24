using DinkToPdf;
using DinkToPdf.Contracts;
using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using FoodOrderingApp.Services;
using FoodOrderingApp.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FoodOrderingApp.Controllers
{

    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;
        private readonly BasketRepository _basketRepository;
        private readonly PdfService _pdfService;
        private readonly RazorViewToStringRenderer _razorViewToStringRenderer;



        public OrderController(OrderRepository orderRepository, BasketRepository basketRepository, PdfService pdfService, RazorViewToStringRenderer razorViewToStringRenderer)
        {
            _orderRepository = orderRepository;
            _basketRepository = basketRepository;
            _pdfService = pdfService;
            _razorViewToStringRenderer = razorViewToStringRenderer;
        }

        public async Task<IActionResult> Confirm(String address)
        {
            // Example: Verify if session user is present
            var user = HttpContext.Session.GetObject<User>("User");


            if (user == null)
            {
                // If user is not logged in, redirect to login page
                return RedirectToAction("Login", "Account");
            }

            List<Basket> baskets = HttpContext.Session.GetObject<List<Basket>>("Basket") ?? new List<Basket>();

            Order order = new Order();
            order.Address = address;
            order.state = OrderState.Placed;
            order.Date = DateTime.Now;
            order.UserId = user.Id;
            order.User = user;

            int orderId = await _orderRepository.AddOrderAsync(order);

            foreach (var basket in baskets)
            {
                basket.OrderId = orderId;
                await _basketRepository.AddBasketAsync(basket);
                
            }

            HttpContext.Session.Remove("Basket");

            return RedirectToAction("Index", "Order");



        }
        public async Task<IActionResult> Index()
        {
            // Example: Verify if session user is present
            var user = HttpContext.Session.GetObject<User>("User");

            if (user == null)
            {
                // If user is not logged in, redirect to login page
                return RedirectToAction("Login", "Account");
            }

            var orders = await _orderRepository.GetOrderByUserAsync(user.Id);

            return View(orders);
        }

        public async Task<IActionResult> Details(int orderId)
        {
            // Example: Verify if session user is present
            var user = HttpContext.Session.GetObject<User>("User");

            if (user == null)
            {
                // If user is not logged in, redirect to login page
                return RedirectToAction("Login", "Account");
            }

            var baskets = await _basketRepository.GetBasketsByOrderIdAsync(orderId);
            decimal total = 0;
            foreach (var basket in baskets)
            {
                basket.BasketPrice = basket.Quantity * basket.Price;
                total += basket.Quantity * basket.Price;
            }

            ViewBag.TotalPrice = total;
            //ViewBag.Order = await _orderRepository.GetOrderById(orderId);
            ViewBag.OrderId = orderId;


            return View(baskets);
        }

        [HttpGet]
        public async Task<IActionResult> Export(int orderId)
        {
            // Example: Verify if session user is present
            var user = HttpContext.Session.GetObject<User>("User");

            if (user == null)
            {
                // If user is not logged in, redirect to login page
                return RedirectToAction("Login", "Account");
            }

            var baskets = await _basketRepository.GetBasketsByOrderIdAsync(orderId);

            Order order = await _orderRepository.GetOrderByIdAsync(orderId);

            decimal total = 0;
            foreach (var basket in baskets)
            {
                basket.BasketPrice = basket.Quantity * basket.Price;
                total += basket.Quantity * basket.Price;
            }

            var model = new ExportInvoiceModel
            {
                TotalPrice = total,
                OrderId = orderId,
                Baskets = baskets,
                User = user,
                Date = order.Date
            };

            string htmlContent = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Pdf/ExportInvoiceToPdf.cshtml", model);
            var pdfBytes = _pdfService.CreatePdf(htmlContent);

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileName = $"ExportInvoice_{timestamp}.pdf";

            return File(pdfBytes, "application/pdf", fileName);

        }
        public async Task<IActionResult> Cancel()
        {
            // Example: Verify if session user is present
            var user = HttpContext.Session.GetObject<User>("User");

            if (user == null)
            {
                // If user is not logged in, redirect to login page
                return RedirectToAction("Login", "Account");
            }

            HttpContext.Session.Remove("Basket");
            return RedirectToAction("Index", "Dish");

        }


    }
}
