using Dtos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.ErrorHandling;
using Service.Interfaces;

namespace Recipes.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    [Authorize]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IRatingsAndComments _ratingsAndComments;
        private readonly Service.Logger.ILogger _logger;

        public RecipeController(IRecipeService recipeService, Service.Logger.ILogger logger, IRatingsAndComments ratingsAndComments)
        {
            _recipeService = recipeService;
            _ratingsAndComments = ratingsAndComments;
            _logger = logger;

            _recipeService.RecipeAdded += _logger.OnRecipeAdded;
            _recipeService.RecipeUpdated += _logger.OnRecipeUpdated;
            _recipeService.RecipeDeleted += _logger.OnRecipeDeleted;
            _ratingsAndComments.CommentAdded += _logger.OnCommentAdded;
            _ratingsAndComments.RatingAdded += _logger.OnRateAdded;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAllRecipes(string userId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var recipes = await _recipeService.GetAllRecipesAsync(userId);
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetLatestRecipes(int count)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var recipes = await _recipeService.GetLatestRecipesAsync(count);
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("toprated")]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetTopRatedRecipes(int count)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var recipes = await _recipeService.GetTopRatedRecipesAsync(count);
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("userrecepies")]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAllUserRecipes(string userId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var recipes = await _recipeService.GetAllUserRecipesAsync(userId);
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("userfavouriterecepies")]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAllUserFavouriteRecipes(string userId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var recipes = await _recipeService.GetAllUserFavouriteRecipesAsync(userId);
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("getrecipe")]
        public async Task<ActionResult<RecipeDto>> GetRecipeById(string id)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var recipe = await _recipeService.GetRecipeByIdAsync(id);
                if (recipe == null)
                {
                    return NotFound();
                }
                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult> CreateRecipe([FromBody] RecipeDto model)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                if (model.ImageUrl != null)
                {
                    string[] parts = model.ImageUrl.Split("\\");
                    string fileName = parts[parts.Length - 1];
                    model.ImageUrl = fileName;
                }

                await _recipeService.CreateRecipeAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateRecipe([FromBody] RecipeDto model)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                if (model.ImageUrl != null)
                {
                    string[] parts = model.ImageUrl.Split("/");
                    string fileName = parts[parts.Length - 1];
                    model.ImageUrl = fileName;
                }

                await _recipeService.UpdateRecipeAsync(model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("addtofavourites/{userId}/{recipeId}")]
        public async Task<ActionResult> AddRecipeToUserFavourites(string userId, string recipeId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var recipe = await _recipeService.AddRecipeToUserFavouritesAsync(userId, recipeId);
                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("removefromfavourites/{userId}/{recipeId}")]
        public async Task<ActionResult> RemoveFromUserFavourites(string userId, string recipeId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var recipe = await _recipeService.RemoveRecipeFromUserFavouritesAsync(userId, recipeId);
                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecipe(string id)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                await _recipeService.DeleteRecipeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ratings")]
        public async Task<ActionResult> AddNewRating(RatingDto ratingDto)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                await _ratingsAndComments.AddNewRating(ratingDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("comments")]
        public async Task<ActionResult> AddNewComment(CommentDto commentDto)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                await _ratingsAndComments.AddNewComment(commentDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("ratings/{loggedInUser}/{recipeId}")]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetAllRatings(string loggedInUser, string recipeId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var ratings = await _ratingsAndComments.GetAllRecipeRatingsAsync(loggedInUser, recipeId);
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("comments/{loggedInUser}/{recipeId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAllComments(string loggedInUser, string recipeId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var comments = await _ratingsAndComments.GetAllRecipeCommentsAsync(loggedInUser, recipeId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("ratings/{ratingId}")]
        public async Task<ActionResult> DeleteRating(string ratingId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                await _ratingsAndComments.DeleteRatingAsync(ratingId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("comments/{commentId}")]
        public async Task<ActionResult> DeleteComment(string commentId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                await _ratingsAndComments.DeleteCommentAsync(commentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private ErrorResponseDto ValidateModelState()
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            if (!errors.Any())
                return new ErrorResponseDto() { Success = true };

            var errorMessage = string.Join(" ", errors);
            return new ErrorResponseDto { Message = errorMessage, Success = false };
        }
    }
}
