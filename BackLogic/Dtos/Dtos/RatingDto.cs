using System.ComponentModel.DataAnnotations;

namespace Dtos.Dtos
{
    public class RatingDto
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Rating is required.")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "RecipeId is required.")]
        public string RecipeId { get; set; }
    }
}
