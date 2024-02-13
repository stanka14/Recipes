using Data.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ApplicationUser User { get; set; }
        public RecipeCategoryEnum Category { get; set; }
        [JsonIgnore]
        public ICollection<RecipeRating> Ratings { get; set; }
        [JsonIgnore]
        public ICollection<RecipeComment> Comments { get; set; }
        [JsonIgnore]
        public ICollection<Notification> Notifications { get; set; }
        [JsonIgnore]
        public ICollection<Ingredient> Ingredients { get; set; }

    }
}
