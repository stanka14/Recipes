using Data.DbContext;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CommentsAndRatingsRepository : ICommentsAndRatingsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentsAndRatingsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddCommentAsync(RecipeComment comment)
        {
            try
            {
                _dbContext.RecipeComments.Add(comment);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding comment.", ex);
            }
        }

        public async Task AddRatingAsync(RecipeRating rating)
        {
            try
            {
                _dbContext.RecipeRatings.Add(rating);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding rating.", ex);
            }
        }

        public async Task DeleteCommentAsync(string commentId)
        {
            try
            {
                var comment = await _dbContext.RecipeComments.FindAsync(commentId);
                if (comment != null)
                {
                    _dbContext.RecipeComments.Remove(comment);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting comment.", ex);
            }
        }

        public async Task DeleteRatingAsync(string ratingId)
        {
            try
            {
                var rating = await _dbContext.RecipeRatings.FindAsync(ratingId);
                if (rating != null)
                {
                    _dbContext.RecipeRatings.Remove(rating);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting rating.", ex);
            }
        }

        public async Task<RecipeComment> GetCommentByIdAsync(string commentId)
        {
            try
            {
                return await _dbContext.RecipeComments.FindAsync(commentId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving comment by ID.", ex);
            }
        }

        public async Task<IEnumerable<RecipeComment>> GetCommentsByRecipeIdAsync(string recipeId)
        {
            try
            {
                return await _dbContext.RecipeComments
                    .Where(comment => comment.RecipeId == recipeId)
                      .Include(c => c.User) 
                      .Where(c => c.RecipeId == recipeId)
                      .OrderByDescending(c => c.DateCommented)
                       .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving comments by recipe ID.", ex);
            }
        }

        public async Task<IEnumerable<RecipeComment>> GetCommentsByUserIdAsync(string userId)
        {
            try
            {
                return await _dbContext.RecipeComments
                    .Where(comment => comment.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving comments by user ID.", ex);
            }
        }

        public async Task<RecipeRating> GetRatingByIdAsync(string ratingId)
        {
            try
            {
                return await _dbContext.RecipeRatings.FindAsync(ratingId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving rating by ID.", ex);
            }
        }

        public async Task<IEnumerable<RecipeRating>> GetRatingsByRecipeIdAsync(string recipeId)
        {
            try
            {
                return await _dbContext.RecipeRatings
                    .Where(rating => rating.RecipeId == recipeId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving ratings by recipe ID.", ex);
            }
        }

        public async Task<IEnumerable<RecipeRating>> GetRatingsByUserIdAsync(string userId)
        {
            try
            {
                return await _dbContext.RecipeRatings
                    .Where(rating => rating.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving ratings by user ID.", ex);
            }
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
