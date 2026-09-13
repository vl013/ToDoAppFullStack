using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDoApp.Interfaces.DTOs;
using ToDoApp.Interfaces.Entities;
using ToDoApp.Interfaces.Repositories;
using ToDoApp.Interfaces.Services;

namespace ToDoApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(UserRegisterDto dto)
        {
            var existingUsers = await _userRepository.FindAsync(u => u.Username == dto.Username);
            if (existingUsers.Any()) throw new Exception("Користувас існує");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)

            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return "Реєстрація успішна";

        }

        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            var users = await _userRepository.FindAsync(u => u.Username == dto.Username);
            var user = users.FirstOrDefault() ?? throw new Exception("Дані невірні");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Дані невірні");

            return GenerateJWTToken(user);
        }

        private string GenerateJWTToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}