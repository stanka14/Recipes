using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class RecipeComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string RecipeId { get; set; }
        public DateTime DateCommented { get; set; }
        public ApplicationUser User { get; set; }
        public Recipe Recipe { get; set; }
    }
}
