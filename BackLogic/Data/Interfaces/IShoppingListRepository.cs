using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IShoppingListRepository
    {
        Task<ShoppingList> GetUserShoppingListAsync(string userId);
        Task AddIngredientAsync(string userId, Ingredient ingredient);
        Task RemoveIngredientAsync(string userId, string ingredientId);
    }
}
