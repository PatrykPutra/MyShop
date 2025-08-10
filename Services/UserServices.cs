using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Exceptions;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
    public class UserServices : IUserServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserAuthorizationServices _userAuthorizationServices;
        private readonly ITokenServices _tokenServices;

        public UserServices(MyShopDbContext dbContext,  IUserAuthorizationServices userAuthorizationServices, IPasswordHasher<User> passwordHasher, ITokenServices tokenServices)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _userAuthorizationServices = userAuthorizationServices;
            _tokenServices = tokenServices;
        }

        public async Task CreateAsync(CreateUserDto userDto)
        {
            if (userDto == null) throw new ArgumentException("Invalid request parameter");
            if (_dbContext.Users.Any(user => user.UserName == userDto.UserName)) throw new ArgumentException($"User name: {userDto.UserName} already exists.");
            
            ShoppingCart newCart = new ShoppingCart();
            _dbContext.ShoppingCarts.Add(newCart);

            User newUser = new User()
            {
                UserName = userDto.UserName,
                IsAdmin = userDto.IsAdmin,
                Email = userDto.Email,
                Cart = newCart,
                CartId = newCart.Id,
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, userDto.Password);
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<User> GetAsync(string token)
        {
            int userId = _tokenServices.GetId(token);
            if (!await _userAuthorizationServices.IsAuthorized(token, userId)) throw new UnauthorizedRequestException("Unauthorized request");
            User? user = await _dbContext.Users
               .Include(user => user.Cart)
               .FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null) throw new NotFoundException("User not found.");

            return user;
        }
    }
}
