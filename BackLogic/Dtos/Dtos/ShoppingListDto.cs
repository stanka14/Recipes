namespace Dtos.Dtos
{
    public class ShoppingListDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
    }
}
