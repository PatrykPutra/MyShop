using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Services
{
    public interface IUserAuthorizationServices
    {
        
    }
    public class UserAuthorizationServices : IUserAuthorizationServices
    {
        private MyShopDbContext _dbContext;
        public UserAuthorizationServices(MyShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       
    }
}
