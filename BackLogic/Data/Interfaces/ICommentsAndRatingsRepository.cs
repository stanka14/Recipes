using Data.Models;

namespace Data.Interfaces
{
    public interface ICommentsAndRatingsRepository : IDisposable
    {
        Task AddCommentAsync(RecipeComment comment);
        Task DeleteCommentAsync(string commentId);
        Task<RecipeComment> GetCommentByIdAsync(string commentId);
        Task<IEnumerable<RecipeComment>> GetCommentsByRecipeIdAsync(string recipeId);
        Task<IEnumerable<RecipeComment>> GetCommentsByUserIdAsync(string userId);

        Task AddRatingAsync(RecipeRating rating);
        Task DeleteRatingAsync(string ratingId);
        Task<RecipeRating> GetRatingByIdAsync(string ratingId);
        Task<IEnumerable<RecipeRating>> GetRatingsByRecipeIdAsync(string recipeId);
        Task<IEnumerable<RecipeRating>> GetRatingsByUserIdAsync(string userId);
    }
}
