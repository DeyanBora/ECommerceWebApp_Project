using ECommerceWebApp.Api.Authenticate.Interfaces;
using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.DataAccess.Data;
using ECommerceWebApp.Entities.Entities.Users;
using ECommerceWebApp.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerceWebApp.Api.Authenticate.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly ECommerceContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            ITokenGeneratorService tokenGeneratorService,
            ECommerceContext context,
            IPasswordHasher<User> passwordHasher,
            ILogger<AuthenticationService> logger)
        {
            _tokenGeneratorService = tokenGeneratorService;
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<UserLoginResultDto> AuthenticateAsync(UserLoginPostDto userLoginDto)
        {
            if (string.IsNullOrWhiteSpace(userLoginDto.Email))
                throw new ArgumentException("The email address is required.");

            if (string.IsNullOrWhiteSpace(userLoginDto.Password) || userLoginDto.Password.Length < 6)
                throw new ArgumentException("The password must be at least 6 characters long.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == userLoginDto.Email.ToLower());

            if (user == null)
            {
                _logger.LogWarning("Authentication failed: User with email {Email} not found.", userLoginDto.Email);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, userLoginDto.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Authentication failed: Invalid password for user {Email}.", user.Email);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (string.IsNullOrEmpty(user.SecurityStamp.ToString()))
            {
                user.SecurityStamp = Guid.NewGuid();
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            var roles = new List<string>(); /*await GetUserRolesAsync(user.Id);*/
            var token = _tokenGeneratorService.GenerateToken(user.Email);
            var tokenExpiration = DateTime.UtcNow.AddHours(1);

            return new UserLoginResultDto(
                Email: user.Email,
                Username: user.UserName,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Token: token.Token,
                TokenExpiration: tokenExpiration,
                TokenExpirationDuration: token.ExpirationDateDuration,
                TokenExpirationOffset: token.ExpirationDateOffset
            );
        }

        //TODO: Implement this method and fix
        //private async Task<List<string>> GetUserRolesAsync(int userId)
        //{
        //    return await _context.Users
        //        .Where(ur => ur.RoleId == userId)
        //        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
        //        .ToListAsync();
        //}
    }
}