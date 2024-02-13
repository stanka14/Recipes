using System.Runtime.Serialization;

namespace Data.Enums
{
    public enum NotificationType
    {
        [EnumMember(Value = "Message")]
        Message,
        [EnumMember(Value = "Rate")]
        Rate,
        [EnumMember(Value = "Comment")]
        Comment
    }
}
