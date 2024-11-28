namespace ECommerceWebApp.Shared.DTOs.Jwt
{
    public class JwtTokenResultDto
    {
        public string Token { get; set; }
        public long ExpirationDateDuration { get; set; }
        public DateTimeOffset ExpirationDateOffset { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
