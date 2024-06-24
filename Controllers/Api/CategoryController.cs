using FoodOrderingApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _repository;

        public CategoryController(CategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber = 1, string searchTerm = null)
        {
            var categories = await _repository.GetCategoryesAsync(pageNumber, 10, searchTerm);
            
            return Ok(categories);
        }

     
    }
}
