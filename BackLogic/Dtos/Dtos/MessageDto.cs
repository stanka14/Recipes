using System.ComponentModel.DataAnnotations;

namespace Dtos.Dtos
{
    public class MessageDto
    {
        public string Id { get; set; }
        [Required]
        public string Text { get; set; }
        public string ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public string SenderId { get; set; }
        public string? SenderName { get; set; }
        public DateTime SendAt { get; set; }
    }
}
