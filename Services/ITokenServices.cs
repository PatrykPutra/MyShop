namespace MyShop.Services
{
    public interface ITokenServices
    {
        string Generate(int id);
        int GetId(string token);
    }
}