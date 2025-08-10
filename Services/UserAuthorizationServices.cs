using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Services
{
    public class UserAuthorizationServices : IUserAuthorizationServices
    {
        private MyShopDbContext _dbContext;
        public UserAuthorizationServices(MyShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsAuthorized(string token, int userId)
        {
            User? user = await _dbContext.Users.Include(user => user.Token).FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null) return false;
            if (user.Token == null || !user.Token.Body.Equals(token)) return false;
            return true;
        }
    }
}
