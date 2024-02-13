using Data.Interfaces;
using Data.Models;
using Dtos.Dtos;
using Moq;
using Service.Services;

namespace Tests.ServiceTests
{
    [TestFixture]
    public class RecipeServiceTests
    {
        private Mock<IRecipeRepository> _recipeRepositoryMock;
        private RecipeService _recipeService;

        [SetUp]
        public void Setup()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _recipeService = new RecipeService(_recipeRepositoryMock.Object);
        }

        [Test]
        public async Task AddRecipeToUserFavouritesAsync_ValidData_ReturnsRecipeDtoWithFavouriteTrue()
        {
            // Arrange
            var userId = "userId";
            var recipeId = "recipeId";

            _recipeRepositoryMock.Setup(repo => repo.GetRecipeByIdAsync(recipeId))
                .ReturnsAsync(new Recipe { ID = recipeId, UserId = userId });

            _recipeRepositoryMock.Setup(repo => repo.AddRecipeToUserFavouritesAsync(It.IsAny<UserFavoriteRecipe>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _recipeService.AddRecipeToUserFavouritesAsync(userId, recipeId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Favourite, Is.True);
        }

        [Test]
        public async Task RemoveRecipeFromUserFavouritesAsync_RecipeNotInFavourites_ThrowsException()
        {
            // Arrange
            var userId = "userId";
            var recipeId = "recipeId";

            _recipeRepositoryMock.Setup(repo => repo.GetUserFavouriteRecipesAsync(userId))
                .ReturnsAsync(new List<Recipe>());

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _recipeService.RemoveRecipeFromUserFavouritesAsync(userId, recipeId));
        }

        [Test]
        public void GetLatestRecipesAsync_ValidCount_ReturnsListOfRecipeDto()
        {
            // Arrange
            var count = 5;
            var recipes = new List<Recipe> { new Recipe() { ID = "recipe1Id" }, new Recipe() { ID = "recipe2Id" } };

            _recipeRepositoryMock.Setup(repo => repo.GetLatestRecipesAsync(count))
                .ReturnsAsync(recipes);

            // Act
            var result = _recipeService.GetLatestRecipesAsync(count).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(recipes.Count));
        }

        [Test]
        public async Task RemoveRecipeFromUserFavouritesAsync_ValidData_ReturnsRecipeDto_HavingFavouriteFlagFalse()
        {
            // Arrange
            var userId = "userId";
            var recipe1 = new Recipe() { ID = "recipe1Id", UserId = userId };
            var recipe2 = new Recipe() { ID = "recipe2Id", UserId = userId };
            var recipes = new List<Recipe> { recipe1, recipe2 };

            _recipeRepositoryMock.Setup(repo => repo.GetUserFavouriteRecipesAsync(userId))
                .ReturnsAsync(recipes);

            _recipeRepositoryMock.Setup(repo => repo.GetRecipeByIdAsync(recipe1.ID))
                .ReturnsAsync(recipe1);

            // Act
            var result = await _recipeService.RemoveRecipeFromUserFavouritesAsync(userId, recipe1.ID);

            // Assert
            _recipeRepositoryMock.Verify(repo => repo.RemoveRecipeFromUserFavouritesAsync(userId, recipe1.ID), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Favourite, Is.False);
        }

        [Test]
        public void GetTopRatedRecipesAsync_ValidCount_ReturnsListOfRecipeDto()
        {
            // Arrange
            var count = 5;
            var recipes = new List<Recipe> { new Recipe() { ID = "recipe1Id" }, new Recipe() { ID = "recipe2Id" } };

            _recipeRepositoryMock.Setup(repo => repo.GetTopRatedRecipesAsync(count))
                .ReturnsAsync(recipes);

            // Act
            var result = _recipeService.GetTopRatedRecipesAsync(count).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(recipes.Count));
        }

        [Test]
        public void GetAllRecipesAsync_ValidUserId_ReturnsListOfRecipeDto()
        {
            // Arrange
            var userId = "userId";
            var recipes = new List<Recipe> { new Recipe() { ID = "recipe1Id" }, new Recipe() { ID = "recipe2Id" } };

            _recipeRepositoryMock.Setup(repo => repo.GetRecipesAsync())
                .ReturnsAsync(recipes);

            _recipeRepositoryMock.Setup(repo => repo.GetUserFavouriteRecipesAsync(userId))
                .ReturnsAsync(new List<Recipe>());

            // Act
            var result = _recipeService.GetAllRecipesAsync(userId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(recipes.Count));
        }

        [Test]
        public void GetRecipeByIdAsync_ValidRecipeId_ReturnsRecipeDto()
        {
            // Arrange
            var recipeId = "recipe1Id";
            var recipe = new Recipe { ID = recipeId };

            _recipeRepositoryMock.Setup(repo => repo.GetRecipeByIdAsync(recipeId))
                .ReturnsAsync(recipe);

            // Act
            var result = _recipeService.GetRecipeByIdAsync(recipeId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(recipeId));
        }


        [Test]
        public void GetAllUserRecipesAsync_ValidUserId_ReturnsListOfRecipeDto()
        {
            // Arrange
            var userId = "userId";
            var recipes = new List<Recipe> { new Recipe() { ID = "recipe1Id" }, new Recipe() { ID = "recipe2Id" } };

            _recipeRepositoryMock.Setup(repo => repo.GetUserRecipesAsync(userId))
                .ReturnsAsync(recipes);

            _recipeRepositoryMock.Setup(repo => repo.GetUserFavouriteRecipesAsync(userId))
                .ReturnsAsync(new List<Recipe>());

            // Act
            var result = _recipeService.GetAllUserRecipesAsync(userId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(recipes.Count));
        }

        [Test]
        public void GetAllUserFavouriteRecipesAsync_ValidUserId_ReturnsListOfRecipeDto()
        {
            // Arrange
            var userId = "userId";
            var recipes = new List<Recipe> { new Recipe() { ID = "recipe1Id" }, new Recipe() { ID = "recipe2Id" } };

            _recipeRepositoryMock.Setup(repo => repo.GetUserFavouriteRecipesAsync(userId))
                .ReturnsAsync(recipes);

            // Act
            var result = _recipeService.GetAllUserFavouriteRecipesAsync(userId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(recipes.Count));
        }

        [Test]
        public async Task UpdateRecipeWithIngredientsAsync_ValidData_ReturnsUpdatedRecipeDto()
        {
            // Arrange
            var recipeDto = new RecipeDto
            {
                Id = "recipe1Id",
                Ingredients = new List<IngredientDto>() { new IngredientDto() { Name = "in1", Quantity = 1 } }
            };

            var existingRecipe = new Recipe
            {
                ID = recipeDto.Id,
                Ingredients = new List<Ingredient> { new Ingredient { Name = "in1", Quantity = 1 } }
            };

            _recipeRepositoryMock.Setup(repo => repo.GetRecipeByIdAsync(recipeDto.Id))
                .ReturnsAsync(existingRecipe);

            _recipeRepositoryMock.Setup(repo => repo.UpdateRecipeAsync(It.IsAny<Recipe>()))
                .Returns(Task.CompletedTask);

            _recipeRepositoryMock.Setup(repo => repo.UpdateIngredientsAsync(It.IsAny<string>(), It.IsAny<List<Ingredient>>()))
                .Returns(Task.CompletedTask);

            // Act
            await _recipeService.UpdateRecipeAsync(recipeDto);

            // Assert
            _recipeRepositoryMock.Verify(repo => repo.UpdateIngredientsAsync(recipeDto.Id, It.IsAny<List<Ingredient>>()), Times.Once);

            var updatedRecipe = await _recipeRepositoryMock.Object.GetRecipeByIdAsync(recipeDto.Id);

            Assert.That(updatedRecipe, Is.Not.Null);
            var updatedIngredient = updatedRecipe.Ingredients.FirstOrDefault(i => i.Name == "in1");
            Assert.Multiple(() =>
            {
                Assert.That(updatedIngredient, Is.Not.Null);
                Assert.That(updatedIngredient?.Name, Is.EqualTo("in1"));
                Assert.That(updatedIngredient.Quantity, Is.EqualTo(1));
            });
        }
    }
}