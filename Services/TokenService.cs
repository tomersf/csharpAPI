using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace api.Services
{
    public class TokenService
    {
        private readonly string _secret;
        private readonly IMongoCollection<User> _userCollection;

        public TokenService(ApplicationDBContext dbContext)
        {
            _secret = Environment.GetEnvironmentVariable("JWT_KEY");
            _userCollection = dbContext.UserCollection;
        }

        public async Task<string> RegisterAsync(string username, string password)
        {
            var existingUser = await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (existingUser != null)
                throw new Exception("User already exists");

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password)
            };

            await _userCollection.InsertOneAsync(user);
            return GenerateToken(user.Id);
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            return GenerateToken(user.Id);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

        public string GenerateToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userId) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}