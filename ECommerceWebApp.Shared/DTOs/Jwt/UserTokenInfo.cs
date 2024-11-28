namespace ECommerceWebApp.Shared.DTOs.Jwt
{
    public class UserTokenInfo
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string SecurityStamp { get; set; }

        public required string Email { get; set; }
    }
}
