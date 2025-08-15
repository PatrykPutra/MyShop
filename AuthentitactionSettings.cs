namespace MyShop
{
    public class AuthentitactionSettings
    {
        public class AuthenticationSettings
        {
            public string JwtKey { get; set; }
            public int JwtExpireDays { get; set; }
            public string JwtIssuer { get; set; }
        }
    }
}
