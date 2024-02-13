namespace Data.Models
{
    public class UserFavoriteRecipe
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
