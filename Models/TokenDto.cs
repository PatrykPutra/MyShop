namespace MyShop.Models
{
    public class TokenDto
    {
        public string Token { get; init; }
        public TokenDto(string token)
        {
            Token = token;
        }
    }
}
