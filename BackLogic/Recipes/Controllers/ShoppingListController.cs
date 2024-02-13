using Dtos.Dtos;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Security.Claims;

namespace Recipes.Controllers
{
    [ApiController]
    [Route("api/shopping")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly IHttpContextAccessor _context;

        public ShoppingListController(IShoppingListService shoppingListService, IHttpContextAccessor context)
        {
            _shoppingListService = shoppingListService;
            _context = context;
        }

        [Authorize]
        [HttpGet("shoppinglist")]
        public async Task<IActionResult> GetUserShoppingList()
        {
            try
            {
                var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Invalid data.");

                var shoppingList = await _shoppingListService.GetUserShoppingListAsync(userId);
                return Ok(shoppingList);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error while getting user ingredients. {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("addIngredient")]
        public async Task<IActionResult> AddIngredientToShoppingList([FromBody] IngredientDto ingredient)
        {
            try
            {
                var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Invalid data.");

                await _shoppingListService.AddIngredientAsync(userId, ingredient);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error while adding ingredient to the shopping list. {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("{ingredientId}")]
        public async Task<IActionResult> RemoveIngredientFromShoppingList(string ingredientId)
        {
            try
            {
                var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Invalid data.");

                await _shoppingListService.RemoveIngredientAsync(userId, ingredientId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error while adding ingredient to the shopping list. {ex.Message}");
            }
        }
    }
}
