using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Exceptions;
using MyShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static MyShop.AuthentitactionSettings;

namespace MyShop.Services
{
    public interface ILoginServices
    {
        string GenerateJwt(CredentialsDto credentials);
        
    }

    public class LoginServices : ILoginServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public LoginServices(MyShopDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        
        public string GenerateJwt(CredentialsDto credentials)
        {
            User? user = _dbContext.Users.FirstOrDefault(user => user.UserName == credentials.Username);
            if (user == null) throw new ArgumentException("Login or password not correct.");
            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, credentials.Password) == PasswordVerificationResult.Failed) 
                throw new ArgumentException("Login or password not correct.");
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.IsAdmin == true ? "Admin" : "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer, 
                claims, 
                expires: expires,
                signingCredentials:signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        
    }
}
