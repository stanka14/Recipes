using Data.DbContext;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingList> GetUserShoppingListAsync(string userId)
        {
            try
            {
                return await _context.ShoppingLists
                    .Include(sl => sl.Ingredients)
                    .FirstOrDefaultAsync(sl => sl.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching user's shopping list.", ex);
            }
        }

        public async Task AddIngredientAsync(string userId, Ingredient ingredient)
        {
            try
            {
                var shoppingList = await _context.ShoppingLists
                    .Include(sl => sl.Ingredients)
                    .FirstOrDefaultAsync(sl => sl.UserId == userId);

                if (shoppingList == null)
                {
                    shoppingList = new ShoppingList
                    {
                        UserId = userId,
                        Ingredients = new List<Ingredient> { ingredient }                    
                    };

                    await _context.ShoppingLists.AddAsync(shoppingList);
                }

                shoppingList.Ingredients.Add(ingredient);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding ingredient to the shopping list.", ex);
            }
        }

        public async Task RemoveIngredientAsync(string userId, string ingredientId)
        {
            try
            {
                var shoppingList = await _context.ShoppingLists
                    .Include(sl => sl.Ingredients)
                    .FirstOrDefaultAsync(sl => sl.UserId == userId);

                if (shoppingList == null)
                {
                    throw new Exception("Shopping list not found.");
                }

                var ingredientToRemove = shoppingList.Ingredients.FirstOrDefault(i => i.Id == ingredientId);

                if (ingredientToRemove != null)
                {
                    shoppingList.Ingredients.Remove(ingredientToRemove);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Ingredient not found in the shopping list.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while removing ingredient from the shopping list.", ex);
            }
        }
    }
}
