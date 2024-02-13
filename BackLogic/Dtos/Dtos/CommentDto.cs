using System.ComponentModel.DataAnnotations;

namespace Dtos.Dtos
{
    public class CommentDto
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "RecipeId is required.")]
        public string RecipeId { get; set; }
        public string UserName { get; set; }

    }
}
