namespace MyShop.Models
{
    public class CreateItemCategoryDto
    {
        public required string Name { get; init; } // Mało kto używa required. Dużo częsciej wprwadza sie wymaganie explicit nullable na całym projekcie
    }
}
