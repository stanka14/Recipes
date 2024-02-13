using Data.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dtos.Dtos
{
    public class NotificationDto
    {
        public NotificationDto() { }
        public string Id { get; set; }
        public string Message { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public DateTime SendAt { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationType NotificationType { get; set; }
        public bool IsRead { get; set; }
        public string RecipeId { get; set; }
        public string RelatedObjectId { get; set; }
    }
}
