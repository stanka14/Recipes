using Dtos.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Recipes.Controllers;
using Service.Interfaces;

namespace Tests.ControllerTests
{
    [TestFixture]
    public class RecipeControllerTests
    {
        private Mock<IRecipeService> _recipeServiceMock;
        private Mock<IRatingsAndComments> _ratingsAndCommentsMock;
        private Mock<Service.Logger.ILogger> _loggerMock;
        private RecipeController _recipeController;

        [SetUp]
        public void Setup()
        {
            _recipeServiceMock = new Mock<IRecipeService>();
            _ratingsAndCommentsMock = new Mock<IRatingsAndComments>();
            _loggerMock = new Mock<Service.Logger.ILogger>();
            _recipeController = new RecipeController(_recipeServiceMock.Object, _loggerMock.Object, _ratingsAndCommentsMock.Object);
        }

        [Test]
        public async Task GetAllRecipes_ValidUserId_ReturnsOkResultWithRecipes()
        {
            // Arrange
            string userId = "userId";
            var recipe1 = new RecipeDto() { Id = "recipe1Id", UserId = userId };
            var recipe2 = new RecipeDto() { Id = "recipe2Id", UserId = userId };
            var recipes = new List<RecipeDto> { recipe1, recipe2 };

            _recipeServiceMock.Setup(service => service.GetAllRecipesAsync(userId)).ReturnsAsync(recipes);

            // Act
            var result = await _recipeController.GetAllRecipes(userId);

            var transformedResult = result.Result as OkObjectResult;

            // Assert
            Assert.That(transformedResult, Is.Not.Null);
            Assert.That(transformedResult.StatusCode, Is.EqualTo(200));
            Assert.That(transformedResult.Value, Is.EqualTo(recipes));
        }

        [Test]
        public async Task GetAllRecipes_InvalidUserId_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            string invalidUserId = "invalidUserId";
            _recipeController.ModelState.AddModelError("UserId", "Invalid UserId");

            // Act
            var result = await _recipeController.GetAllRecipes(invalidUserId);

            var transformedResult = result.Result as BadRequestObjectResult;

            // Assert
            Assert.That(transformedResult, Is.Not.Null);
            Assert.That(transformedResult.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task CreateRecipe_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var recipeDto = new RecipeDto() { Id = "recipe1Id", UserId = "userId" };
            _recipeController.ModelState.Clear();

            // Act
            var result = await _recipeController.CreateRecipe(recipeDto);

            var transformerResult = result as OkResult;

            // Assert
            Assert.That(transformerResult, Is.Not.Null);
            Assert.That(transformerResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task CreateRecipe_InvalidModel_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            var invalidRecipeDto = new RecipeDto { Id = "recipeId" };
            _recipeController.ModelState.AddModelError("userId", "Invalid Property");

            // Act
            var result = await _recipeController.CreateRecipe(invalidRecipeDto) as BadRequestObjectResult;

            var transformerResult = result as BadRequestObjectResult;

            // Assert
            Assert.That(transformerResult, Is.Not.Null);
            Assert.That(transformerResult.StatusCode, Is.EqualTo(400));
        }
        [Test]
        public async Task GetLatestRecipes_ValidCount_ReturnsOkResultWithRecipes()
        {
            // Arrange
            int count = 2;
            var userId = "userId";
            var recipe1 = new RecipeDto() { Id = "recipe1Id", UserId = userId };
            var recipe2 = new RecipeDto() { Id = "recipe2Id", UserId = userId };
            var recipe3 = new RecipeDto() { Id = "recipe3Id", UserId = userId };
            var recipe4 = new RecipeDto() { Id = "recipe4Id", UserId = userId };
            var recipe5 = new RecipeDto() { Id = "recipe5Id", UserId = userId };

            var recipes = new List<RecipeDto> { recipe1, recipe2, recipe3, recipe4, recipe5 };

            _recipeServiceMock.Setup(service => service.GetLatestRecipesAsync(count)).ReturnsAsync(recipes);

            // Act
            var result = await _recipeController.GetLatestRecipes(count);

            var transformedResult = result.Result as OkObjectResult;

            // Assert
            Assert.That(transformedResult, Is.Not.Null);
            Assert.That(transformedResult.StatusCode, Is.EqualTo(200));
            Assert.That(transformedResult.Value, Is.EqualTo(recipes));
        }

        [Test]
        public async Task GetTopRatedRecipes_ValidCount_ReturnsOkResultWithRecipes()
        {
            // Arrange
            int count = 2;
            var userId = "userId";
            var recipe1 = new RecipeDto() { Id = "recipe1Id", UserId = userId };
            var recipe2 = new RecipeDto() { Id = "recipe2Id", UserId = userId };
            var recipe3 = new RecipeDto() { Id = "recipe3Id", UserId = userId };
            var recipe4 = new RecipeDto() { Id = "recipe4Id", UserId = userId };
            var recipe5 = new RecipeDto() { Id = "recipe5Id", UserId = userId };

            var recipes = new List<RecipeDto> { recipe1, recipe2, recipe3, recipe4, recipe5 };

            _recipeServiceMock.Setup(service => service.GetTopRatedRecipesAsync(count)).ReturnsAsync(recipes);

            // Act
            var result = await _recipeController.GetTopRatedRecipes(count);

            var transformedResult = result.Result as OkObjectResult;

            // Assert
            Assert.That(transformedResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(transformedResult.StatusCode, Is.EqualTo(200));
                Assert.That(transformedResult.Value, Is.EqualTo(recipes));
            });
        }

        [Test]
        public async Task GetAllUserRecipes_ValidUserId_ReturnsOkResultWithRecipes()
        {
            // Arrange
            var userId = "userId";

            var recipe1 = new RecipeDto() { Id = "recipe1Id", UserId = userId };
            var recipe2 = new RecipeDto() { Id = "recipe2Id", UserId = userId };

            var recipes = new List<RecipeDto> { recipe1, recipe2 };
            _recipeServiceMock.Setup(service => service.GetAllUserRecipesAsync(userId)).ReturnsAsync(recipes);

            // Act
            var result = await _recipeController.GetAllUserRecipes(userId);

            var transformedResult = result.Result as OkObjectResult;

            // Assert
            Assert.That(transformedResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(transformedResult.StatusCode, Is.EqualTo(200));
                Assert.That(transformedResult.Value, Is.EqualTo(recipes));
            });
        }

        [Test]
        public async Task GetAllUserFavouriteRecipes_ValidUserId_ReturnsOkResultWithRecipes()
        {
            // Arrange
            string userId = "userId";
            var recipe1 = new RecipeDto() { Id = "recipe1Id", UserId = userId };
            var recipe2 = new RecipeDto() { Id = "recipe2Id", UserId = userId };

            var recipes = new List<RecipeDto> { recipe1, recipe2 };

            _recipeServiceMock.Setup(service => service.GetAllUserFavouriteRecipesAsync(userId)).ReturnsAsync(recipes);

            // Act
            var result = await _recipeController.GetAllUserFavouriteRecipes(userId);

            var transformedResult = result.Result as OkObjectResult;

            // Assert
            Assert.That(transformedResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(transformedResult.StatusCode, Is.EqualTo(200));
                Assert.That(transformedResult.Value, Is.EqualTo(recipes));
            });
        }

        [Test]
        public async Task GetRecipeById_ValidRecipeId_ReturnsOkResultWithRecipe()
        {
            // Arrange
            var recipeId = "recipeId";
            var userId = "userId";
            var recipeDto = new RecipeDto() { Id = recipeId, UserId = userId };

            _recipeServiceMock.Setup(service => service.GetRecipeByIdAsync(recipeId)).ReturnsAsync(recipeDto);

            // Act
            var result = await _recipeController.GetRecipeById(recipeId);

            var transformedResult = result.Result as OkObjectResult;

            // Assert
            Assert.That(transformedResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(transformedResult.StatusCode, Is.EqualTo(200));
                Assert.That(transformedResult.Value, Is.EqualTo(recipeDto));
            });
        }
    }
}
