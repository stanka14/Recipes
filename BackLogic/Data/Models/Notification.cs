using Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Message { get; set; }
        public string RecipeId { get; set; }
        public DateTime SentAt { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual ApplicationUser Receiver { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public Recipe? Recipe { get; set; }
        public string RelatedObjectId { get; set; }
        public bool IsRead { get; set; }
    }
}
