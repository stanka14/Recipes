using Data.Models;

namespace Data.Interfaces
{
    public interface IRecipeRepository : IDisposable
    {
        Task<Recipe> GetRecipeByIdAsync(string id);
        Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task<IEnumerable<Recipe>> GetLatestRecipesAsync(int count);
        Task<IEnumerable<Recipe>> GetTopRatedRecipesAsync(int count);
        Task<IEnumerable<Recipe>> GetUserRecipesAsync(string userId);
        Task<IEnumerable<Recipe>> GetUserFavouriteRecipesAsync(string userId);
        Task AddRecipeAsync(Recipe recipe);
        Task UpdateRecipeAsync(Recipe recipe);
        Task UpdateIngredientsAsync(string recipeId, List<Ingredient> ingredients);
        Task DeleteRecipeAsync(string recipeId);
        Task AddRecipeToUserFavouritesAsync(UserFavoriteRecipe userFavoriteRecipe);
        Task RemoveRecipeFromUserFavouritesAsync(string userId, string recipeId);
    }
}
