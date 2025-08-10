namespace MyShop.Exceptions
{
    public class UnauthorizedRequestException :Exception
    {
        public UnauthorizedRequestException(string message) :base(message) 
        {
            
        }
    }
}
