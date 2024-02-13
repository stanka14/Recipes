using Data.Interfaces;
using Data.Models;
using Dtos.Dtos;
using Service.Interfaces;

namespace Service.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IShoppingListRepository _shoppingListRepository;

        public ShoppingListService(IShoppingListRepository shoppingListRepository)
        {
            _shoppingListRepository = shoppingListRepository;
        }

        public async Task AddIngredientAsync(string userId, IngredientDto ingredient)
        {
            try
            {
                await _shoppingListRepository.AddIngredientAsync(userId, new Ingredient()
                {
                    Quantity = ingredient.Quantity,
                    Name = ingredient.Name,
                });

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while adding ingredient to the shopping list.", ex);
            }
        }

        public async Task RemoveIngredientAsync(string userId, string ingredientId)
        {
            try
            {
                var shoppingList = await _shoppingListRepository.GetUserShoppingListAsync(userId);
                await _shoppingListRepository.RemoveIngredientAsync(userId, ingredientId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while adding ingredient to the shopping list.", ex);
            }
        }

        public async Task<ShoppingListDto> GetUserShoppingListAsync(string userId)
        {
            try
            {
                var shoppingList = await _shoppingListRepository.GetUserShoppingListAsync(userId);

                if (shoppingList == null)
                    return new ShoppingListDto();

                return new ShoppingListDto()
                {
                    Id = shoppingList.Id,
                    UserId = shoppingList.UserId,
                    Ingredients = shoppingList?.Ingredients?.Select(MapIngredientToDto).ToList() ?? new List<IngredientDto>()
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting user shopping list.", ex);
            }
        }

        private IngredientDto MapIngredientToDto(Ingredient ingredient)
        {
            return new IngredientDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Quantity = ingredient.Quantity,
            };
        }
    }
}
