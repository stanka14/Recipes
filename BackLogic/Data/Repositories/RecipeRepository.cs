using Data.DbContext;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class RecipeRepository : IRecipeRepository, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        public RecipeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            try
            {
                if (recipe.Ingredients != null && recipe.Ingredients.Any())
                {
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        _dbContext.Ingredients.Add(ingredient);
                    }
                }

                await _dbContext.Recipes.AddAsync(recipe);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating a recipe.", ex);
            }
        }

        public async Task DeleteRecipeAsync(string recipeId)
        {
            try
            {
                var recipe = await _dbContext.Recipes.FindAsync(recipeId);

                if (recipe == null)
                {
                    throw new Exception("Recipe not found.");
                }

                _dbContext.Recipes.Remove(recipe);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting the recipe.", ex);
            }
        }

        public async Task<Recipe> GetRecipeByIdAsync(string id)
        {
            try
            {
                var recipe = await _dbContext.Recipes.Include(x => x.Ingredients)
                                                     .FirstOrDefaultAsync(r => r.ID == id);

                if (recipe == null)
                {
                    throw new Exception("Recipe not found.");
                }

                return recipe;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting recipe by id.", ex);
            }
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            try
            {
                return await _dbContext.Recipes.Include(x => x.Ingredients).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting all recipes.", ex);
            }
        }

        public async Task<IEnumerable<Recipe>> GetLatestRecipesAsync(int count)
        {
            return await _dbContext.Recipes.Include(x => x.Ingredients)
                .OrderByDescending(recipe => recipe.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> GetTopRatedRecipesAsync(int count)
        {
            return await _dbContext.Recipes.Include(x => x.Ingredients)
                .OrderByDescending(r => r.Ratings.Average(rating => rating.Rating))
                .Take(count)
                .ToListAsync();
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            try
            {
                if (recipe == null)
                {
                    throw new Exception("Recipe can not be null.");
                }

                _dbContext.Entry(recipe).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating the recipe.", ex);
            }
        }

        public async Task UpdateIngredientsAsync(string recipeId, List<Ingredient> newIngredients)
        {
            var existingRecipe = await _dbContext.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.ID == recipeId);

            if (existingRecipe == null)
            {
                throw new Exception("Recipe not found.");
            }

            var existingIngredients = existingRecipe.Ingredients.ToList();

            foreach (var newIngredient in newIngredients)
            {
                var existingIngredient = existingIngredients.FirstOrDefault(i => i.Id == newIngredient.Id);

                if (existingIngredient != null)
                {
                    existingIngredient.Name = newIngredient.Name;
                    existingIngredient.Quantity = newIngredient.Quantity;
                }
                else
                {
                    _dbContext.Ingredients.Add(newIngredient);
                    existingRecipe.Ingredients.Add(newIngredient);
                }
            }

            var ingredientIdsToRemove = existingIngredients.Select(i => i.Id).Except(newIngredients.Select(i => i.Id)).ToList();
            var ingredientsToRemove = existingIngredients.Where(i => ingredientIdsToRemove.Contains(i.Id)).ToList();
            _dbContext.Ingredients.RemoveRange(ingredientsToRemove);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating recipe ingredients.", ex);
            }
        }


        public async Task<IEnumerable<Recipe>> GetUserRecipesAsync(string userId)
        {
            try
            {
                return await _dbContext.Recipes.Include(x => x.Ingredients).Where(x => x.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting user recipes.", ex);
            }
        }

        public async Task<IEnumerable<Recipe>> GetUserFavouriteRecipesAsync(string userId)
        {
            try
            {
                var user = await _dbContext.Users.Include(u => u.FavouriteRecipes)
                                .ThenInclude(fr => fr.Recipe).ThenInclude(x => x.Ingredients)
                                .FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    return user.FavouriteRecipes.Select(favorite => favorite.Recipe).ToList();
                }

                return new List<Recipe>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting user recipes.", ex);
            }
        }

        public async Task AddRecipeToUserFavouritesAsync(UserFavoriteRecipe userFavoriteRecipe)
        {
            try
            {
                var existingFavorite = await _dbContext.UserFavoriteRecipes
                    .FirstOrDefaultAsync(fr =>
                        fr.UserId == userFavoriteRecipe.UserId &&
                        fr.RecipeId == userFavoriteRecipe.RecipeId);

                if (existingFavorite != null)
                {
                    throw new Exception("Recipe is already in user favourites.");
                }

                _dbContext.UserFavoriteRecipes.Add(userFavoriteRecipe);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding recipe to user favourites.", ex);
            }
        }

        public async Task RemoveRecipeFromUserFavouritesAsync(string userId, string recipeId)
        {
            var userFavoriteRecipe = await _dbContext.UserFavoriteRecipes.FirstOrDefaultAsync(x => x.RecipeId == recipeId && x.UserId == userId);

            if (userFavoriteRecipe != null)
            {
                _dbContext.UserFavoriteRecipes.Remove(userFavoriteRecipe);
                await _dbContext.SaveChangesAsync();
            }
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
