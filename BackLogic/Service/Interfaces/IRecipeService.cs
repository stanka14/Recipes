using Dtos.Dtos;
using Service.Events;

namespace Service.Interfaces
{
    public interface IRecipeService
    {
        Task<RecipeDto> GetRecipeByIdAsync(string id);
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync(string userId);
        Task<IEnumerable<RecipeDto>> GetLatestRecipesAsync(int count);
        Task<IEnumerable<RecipeDto>> GetTopRatedRecipesAsync(int count);
        Task<IEnumerable<RecipeDto>> GetAllUserRecipesAsync(string userId);
        Task<IEnumerable<RecipeDto>> GetAllUserFavouriteRecipesAsync(string userId);
        Task CreateRecipeAsync(RecipeDto recipe);
        Task UpdateRecipeAsync(RecipeDto recipe);
        Task<RecipeDto> AddRecipeToUserFavouritesAsync(string userId, string recipeId);
        Task<RecipeDto> RemoveRecipeFromUserFavouritesAsync(string userId, string recipeId);
        Task DeleteRecipeAsync(string id);
        event RecipeEventHandler? RecipeAdded;
        event RecipeEventHandler? RecipeUpdated;
        event RecipeEventHandler? RecipeDeleted;
    }
}
