using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<UserFavoriteRecipe> FavouriteRecipes { get; set; }
        [JsonIgnore]
        public ICollection<RecipeRating> Ratings { get; set; }
        [JsonIgnore]
        public ICollection<RecipeComment> Comments { get; set; }
        public ICollection<Notification> Notifications { get; set; }

    }
}
