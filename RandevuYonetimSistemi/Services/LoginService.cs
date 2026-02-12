using RandevuYonetimSistemi.Models;
using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace RandevuYonetimSistemi.Services
{
    public class LoginService
    {
        private readonly string connectionString =
            @"Server=localhost;Database=AppointmentTrackingSystem;Trusted_Connection=True;";

        public LoginResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return new LoginResult { ErrorMessage = "Please fill in all fields." };

            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                string query = @"SELECT UserId, PasswordHash, IsActive, IsSetupCompleted 
                                 FROM Users 
                                 WHERE Email = @Email";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return new LoginResult { ErrorMessage = "User not found." };

                bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                if (!isActive)
                    return new LoginResult { ErrorMessage = "This account is inactive." };

                string storedHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                string inputHash = UserHelper.ComputeSha256Hash(password);

                if (storedHash != inputHash)
                    return new LoginResult { ErrorMessage = "Incorrect password." };

                int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                bool setupCompleted = reader.GetBoolean(reader.GetOrdinal("IsSetupCompleted"));

                string token = GenerateJwtToken(email);

                return new LoginResult
                {
                    Token = token,
                    UserId = userId,
                    IsSetupCompleted = setupCompleted
                };
            }
            catch (Exception ex)
            {
                return new LoginResult { ErrorMessage = $"Unexpected error: {ex.Message}" };
            }
        }

        private string GenerateJwtToken(string email)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(JwtSettings.Key);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("role", "User")
                }),

                Expires = DateTime.UtcNow.AddHours(12),

                SigningCredentials = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature)
            };

            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }

    public class LoginResult
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public bool IsSetupCompleted { get; set; }
        public string ErrorMessage { get; set; }
    }
}
