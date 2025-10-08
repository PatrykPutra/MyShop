using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Exceptions;
using MyShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyShop.Services
{
    public class LogoutServices : ILogoutServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IUserServices _userServices;
        private readonly IUserContextService _userContextService;

        public LogoutServices(MyShopDbContext dbContext, AuthenticationSettings authenticationSettings, IUserServices userServices, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _authenticationSettings = authenticationSettings;
            _userServices = userServices;
            _userContextService = userContextService;
        }

        public async Task<string> GenerateJwt()
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);
            if (user == null) throw new NotFoundException("Could not find user");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.IsAdmin == true ? "Admin" : "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(-1);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
