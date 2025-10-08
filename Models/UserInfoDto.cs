namespace MyShop.Models
{
    public class UserInfoDto
    {
        public required string UserName { get; init; }
        public required bool IsAdmin { get; set; }
        public required string Email { get; set; }
    }
}
