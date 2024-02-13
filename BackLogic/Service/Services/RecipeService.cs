using Data.Interfaces;
using Data.Models;
using Dtos.Dtos;
using Service.Events;
using Service.Interfaces;

namespace Service.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        public event RecipeEventHandler? RecipeAdded;
        public event RecipeEventHandler? RecipeUpdated;
        public event RecipeEventHandler? RecipeDeleted;

        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeDto> AddRecipeToUserFavouritesAsync(string userId, string recipeId)
        {
            try
            {
                if (userId == null)
                    throw new Exception("User ID is required for adding a recipe to favourites.");

                if (recipeId == null)
                    throw new Exception("Recipe ID is required for adding a recipe to favourites.");

                var favoriteRecipe = new UserFavoriteRecipe
                {
                    UserId = userId,
                    RecipeId = recipeId
                };

                await _recipeRepository.AddRecipeToUserFavouritesAsync(favoriteRecipe);

                var recipe = await _recipeRepository.GetRecipeByIdAsync(recipeId);

                return MapRecipeToDto(recipe, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<RecipeDto>> GetLatestRecipesAsync(int count)
        {
            try
            {
                var recipes = await _recipeRepository.GetLatestRecipesAsync(count);
                return recipes.Select(recipe => MapRecipeToDto(recipe)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<RecipeDto>> GetTopRatedRecipesAsync(int count)
        {
            try
            {
                var recipes = await _recipeRepository.GetTopRatedRecipesAsync(count);
                return recipes.Select(recipe => MapRecipeToDto(recipe)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RecipeDto> RemoveRecipeFromUserFavouritesAsync(string userId, string recipeId)
        {
            try
            {
                var userFavoriteRecipes = await _recipeRepository.GetUserFavouriteRecipesAsync(userId);
                var userFavoriteRecipe = userFavoriteRecipes.FirstOrDefault(fr => fr.ID == recipeId);

                if (userFavoriteRecipe == null)
                {
                    throw new Exception("Recipe is not in user favourites.");
                }

                await _recipeRepository.RemoveRecipeFromUserFavouritesAsync(userId, recipeId);
                var recipe = await _recipeRepository.GetRecipeByIdAsync(recipeId);

                return MapRecipeToDto(recipe, false);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateRecipeAsync(RecipeDto recipe)
        {
            try
            {
                var recipeEntity = new Recipe()
                {
                    CreatedAt = DateTime.Now,
                    Category = recipe.Category,
                    Name = recipe.Name,
                    Description = recipe.Description,
                    ImageName = recipe.ImageUrl,
                    UserId = recipe.UserId,
                };

                if (recipe.Ingredients != null && recipe.Ingredients.Any())
                {
                    var ingredients = MapDtoToIngredients(recipe.Ingredients, recipe.Id);
                    recipeEntity.Ingredients = ingredients;
                }

                await _recipeRepository.AddRecipeAsync(recipeEntity);

                var recipeEventArgs = new RecipeEventArgs(recipe);
                RecipeAdded?.Invoke(this, recipeEventArgs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteRecipeAsync(string id)
        {
            try
            {
                var recipe = await FindRecipeByIdAsync(id);
                if (recipe == null)
                    throw new Exception("Couldn't find that recipe.");

                await _recipeRepository.DeleteRecipeAsync(recipe.ID);

                var recipeEventArgs = new RecipeEventArgs(MapRecipeToDto(recipe));
                RecipeDeleted?.Invoke(this, recipeEventArgs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync(string userId)
        {
            try
            {
                var recipes = await _recipeRepository.GetRecipesAsync();

                var recipeDtos = new List<RecipeDto>();

                foreach (var recipe in recipes)
                {
                    var userFavouriteRecipes = await _recipeRepository.GetUserFavouriteRecipesAsync(userId);
                    bool isFavourite = userFavouriteRecipes.Any(fr => fr.ID == recipe.ID);

                    recipeDtos.Add(MapRecipeToDto(recipe, isFavourite));
                }

                return recipeDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<RecipeDto>> GetAllUserRecipesAsync(string userId)
        {
            try
            {
                var recipes = await _recipeRepository.GetUserRecipesAsync(userId);

                var recipeDtos = new List<RecipeDto>();

                foreach (var recipe in recipes)
                {
                    var userFavouriteRecipes = await _recipeRepository.GetUserFavouriteRecipesAsync(userId);
                    bool isFavourite = userFavouriteRecipes.Any(fr => fr.ID == recipe.ID);

                    recipeDtos.Add(MapRecipeToDto(recipe, isFavourite));
                }

                return recipeDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<RecipeDto>> GetAllUserFavouriteRecipesAsync(string userId)
        {
            try
            {
                var recipes = await _recipeRepository.GetUserFavouriteRecipesAsync(userId);
                return recipes.Select(recipe => MapRecipeToDto(recipe, true)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RecipeDto> GetRecipeByIdAsync(string id)
        {
            try
            {
                var recipe = await FindRecipeByIdAsync(id);
                if (recipe == null)
                    throw new Exception("Couldn't find that recipe.");

                return MapRecipeToDto(recipe);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateRecipeAsync(RecipeDto recipeDto)
        {
            try
            {
                var recipe = MapDtoToRecipe(recipeDto);
                await _recipeRepository.UpdateRecipeAsync(recipe);

                if (recipeDto.Ingredients != null)
                {
                    await _recipeRepository.UpdateIngredientsAsync(recipe.ID, MapDtoToIngredients(recipeDto.Ingredients, recipe.ID).ToList());
                }

                var recipeEventArgs = new RecipeEventArgs(MapRecipeToDto(recipe));
                RecipeUpdated?.Invoke(this, recipeEventArgs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<Recipe> FindRecipeByIdAsync(string id)
        {
            try
            {
                return await _recipeRepository.GetRecipeByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private RecipeDto MapRecipeToDto(Recipe recipe, bool isInUserFavourites = false)
        {
            return new RecipeDto()
            {
                Id = recipe.ID,
                Name = recipe.Name,
                Category = recipe.Category,
                Description = recipe.Description,
                ImageUrl = recipe.ImageName,
                UserId = recipe.UserId,
                CreatedAt = recipe.CreatedAt,
                Favourite = isInUserFavourites,
                Ingredients = recipe.Ingredients?.Select(i => new IngredientDto { Id = i.Id, Name = i.Name, Quantity = i.Quantity }).ToList()
            };
        }

        private Ingredient MapDtoToIngredient(IngredientDto ingredientDto)
        {
            return new Ingredient()
            {
                Id = ingredientDto.Id,
                Name = ingredientDto.Name,
                Quantity = ingredientDto.Quantity,
            };
        }

        private List<Ingredient> MapDtoToIngredients(IEnumerable<IngredientDto> ingredientDtos, string recipeId)
        {
            return ingredientDtos.Select(dto => new Ingredient
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                RecipeId = recipeId
            }).ToList();
        }

        private Recipe MapDtoToRecipe(RecipeDto recipeDto)
        {
            return new Recipe()
            {
                ID = recipeDto.Id,
                Name = recipeDto.Name,
                Category = recipeDto.Category,
                Description = recipeDto.Description,
                ImageName = recipeDto.ImageUrl,
                UserId = recipeDto.UserId
            };
        }
    }
}
