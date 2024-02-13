using System.Runtime.Serialization;

namespace Data.Enums
{
    public enum RecipeCategoryEnum
    {
        [EnumMember(Value = "MainCourse")]
        MainCourse,
        [EnumMember(Value = "Dessert")]
        Dessert,
        [EnumMember(Value = "Snack")]
        Snack,
        [EnumMember(Value = "Salad")]
        Salad
    }
}
