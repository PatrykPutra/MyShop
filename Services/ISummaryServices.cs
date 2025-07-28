using MyShop.Models;

namespace MyShop.Services
{
    public interface ISummaryServices
    {
        Task<Summary> GetAsync();
    }
}