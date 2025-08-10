namespace MyShop.Models
{
    public class CreateItemCategoryDto
    {
        public required string Name { get; init; }
        public required string Token { get; set; }
    }
}
