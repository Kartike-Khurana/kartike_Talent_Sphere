using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TalentSphere.Enums;
using TalentSphere.Models;

namespace TalentSphere.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration config, ILogger<TokenService> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Creates a JWT token for the authenticated user.
        /// Validates that the user is active before token creation.
        /// </summary>
        /// <param name="user">The user object</param>
        /// <param name="role">The user's role</param>
        /// <returns>JWT token string</returns>
        /// <exception cref="InvalidOperationException">Thrown if user is not active or if configuration is missing</exception>
        public string CreateToken(User user, RoleName role)
        {
            // Validate user is not null
            if (user == null)
                throw new InvalidOperationException("User cannot be null.");

            // Validate user is not deleted
            if (user.IsDeleted)
            {
                _logger.LogWarning($"Token creation denied: User {user.UserID} is deleted.");
                throw new InvalidOperationException("Cannot create token for deleted user.");
            }

            // Validate user is active
            if (user.Status != UserStatus.Active)
            {
                _logger.LogWarning($"Token creation denied: User {user.UserID} status is {user.Status}.");
                throw new InvalidOperationException($"Cannot create token for {user.Status} user. User must be active.");
            }

            // Validate role
            if (role == null)
                throw new InvalidOperationException("Role cannot be null.");

            _logger.LogInformation($"Creating token for active user {user.UserID} with role {role}.");

            // Create claims using the existing User property names.
            // Use UserID (as provided in the type signatures) instead of non-existent Id.
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var keyString = _config["TokenKey"] ?? _config["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("Token signing key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            _logger.LogInformation($"Token successfully created for user {user.UserID}.");
            return tokenHandler.WriteToken(token);
        }
    }
}

