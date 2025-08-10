using MyShop.Data;

namespace MyShop.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly MyShopDbContext _dbContext;
        public TokenServices(MyShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string Generate(int id)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            
            var stringChars = new char[20];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var token = new String(stringChars);
            token = token + id.ToString();
            return token;
        }
        public int GetId(string token)
        {
            if (int.TryParse(token.Substring(20, token.Length-20), out int id)) return id;
            throw new ArgumentException($"Unable to read token");
        }
    }
}
