using Data.Interfaces;
using Data.Models;
using Dtos.Dtos;
using Moq;
using Service.Services;

namespace Tests.ServiceTests
{
    [TestFixture]
    public class ShoppingListServiceTests
    {
        private Mock<IShoppingListRepository> _shoppingListRepositoryMock;
        private ShoppingListService _shoppingListService;

        [SetUp]
        public void Setup()
        {
            _shoppingListRepositoryMock = new Mock<IShoppingListRepository>();
            _shoppingListService = new ShoppingListService(_shoppingListRepositoryMock.Object);
        }

        [Test]
        public async Task AddIngredientAsync_ValidData_CallsRepositoryAddIngredientAsync()
        {
            // Arrange
            var userId = "userId";
            var ingredientDto = new IngredientDto { Name = "TestIngredient", Quantity = 2 };

            // Act
            await _shoppingListService.AddIngredientAsync(userId, ingredientDto);

            // Assert
            _shoppingListRepositoryMock.Verify(repo => repo.AddIngredientAsync(userId, It.IsAny<Ingredient>()), Times.Once);
        }

        [Test]
        public async Task RemoveIngredientAsync_ValidData_CallsRepositoryRemoveIngredientAsync()
        {
            // Arrange
            var userId = "userId";
            var ingredientId = "ingredientId";

            // Act
            await _shoppingListService.RemoveIngredientAsync(userId, ingredientId);

            // Assert
            _shoppingListRepositoryMock.Verify(repo => repo.RemoveIngredientAsync(userId, ingredientId), Times.Once);
        }

        [Test]
        public async Task GetUserShoppingListAsync_ValidUserId_ReturnsShoppingListDto()
        {
            // Arrange
            var userId = "userId";
            var shoppingList = new ShoppingList
            {
                Id = "shoppingListId",
                UserId = userId,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = "ingredient1Id", Name = "Ingredient1", Quantity = 2 },
                    new Ingredient { Id = "ingredient2Id", Name = "Ingredient2", Quantity = 3 }
                }
            };

            _shoppingListRepositoryMock.Setup(repo => repo.GetUserShoppingListAsync(userId))
                .ReturnsAsync(shoppingList);

            // Act
            var result = await _shoppingListService.GetUserShoppingListAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(shoppingList.Id));
            Assert.Multiple(() =>
            {
                Assert.That(result.UserId, Is.EqualTo(shoppingList.UserId));

                Assert.That(result.Ingredients, Is.Not.Null);
            });
            Assert.AreEqual(shoppingList.Ingredients.Count, result.Ingredients.Count);

            var ingredients = shoppingList?.Ingredients.ToList();

            if (ingredients != null)
            {
                for (int i = 0; i < ingredients.Count; i++)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(result.Ingredients[i].Id, Is.EqualTo(ingredients[i].Id));
                        Assert.That(result.Ingredients[i].Name, Is.EqualTo(ingredients[i].Name));
                        Assert.That(result.Ingredients[i].Quantity, Is.EqualTo(ingredients[i].Quantity));
                    });
                }
            }
        }
    }
}
