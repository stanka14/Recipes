using Data.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Dtos
{
    public class RecipeDto
    {
        public RecipeDto() { }
        public string Id { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }
        public bool Favourite { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RecipeCategoryEnum Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<IngredientDto>? Ingredients { get; set; }   
    }
}
