namespace MyShop.Entities
{
    public class ItemCategory
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<ShopItem> ShopItems { get; set; } = new List<ShopItem>();

    }
}
