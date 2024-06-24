using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly DishRepository _repository;

        public DishController(DishRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber = 1, string searchTerm = null, int? categoryId = null)
        {
            var dishes = await _repository.GetDishesAsync(pageNumber, 10, searchTerm, categoryId);
            var totalItems = await _repository.GetTotalDishesCountAsync(searchTerm, categoryId);
            return Ok(dishes);
        }

        // Additional action methods for CRUD operations can be added here
    }
}
