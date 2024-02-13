using Data.Models;
using Dtos.Dtos;

namespace Service.Interfaces
{
    public interface IShoppingListService
    {
        Task<ShoppingListDto> GetUserShoppingListAsync(string userId);
        Task AddIngredientAsync(string userId, IngredientDto ingredient);
        Task RemoveIngredientAsync(string userId, string ingredientId);
    }

}
