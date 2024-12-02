namespace ECommerceWebApp.Api.JWT
{
    public interface ITokenGenerator
    {
        public string GenerateToken(string email);
    }
}
