using Microsoft.AspNetCore.Identity;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenServices _tokenServices;
        public LoginServices(MyShopDbContext dbContext, IPasswordHasher<User> passwordHasher, ITokenServices tokenServices)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _tokenServices = tokenServices;
        }
        public async Task<string> Login(CredentialsDto credentials)
        {
            User? user = _dbContext.Users.FirstOrDefault(user => user.UserName == credentials.Username);
            if (user == null) throw new ArgumentException();
            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, credentials.Password) == PasswordVerificationResult.Success)
            {
                string tokenBody = _tokenServices.Generate(user.Id);
                Token? token = _dbContext.Tokens.FirstOrDefault(token => token.UserId == user.Id);
                if (token == null)
                {
                    Token newToken = new Token()
                    {
                        Body = tokenBody,
                        User = user,
                        UserId = user.Id,
                    };
                    await _dbContext.Tokens.AddAsync(newToken);
                }
                else
                {
                    token.Body = tokenBody;
                }
                _dbContext.SaveChanges();
                return tokenBody;
            }
            throw new ArgumentException();

        }
        
    }
}
