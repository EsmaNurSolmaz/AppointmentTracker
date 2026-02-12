using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace RandevuYonetimSistemi.Services
{
    public class TokenService
    {
        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(JwtSettings.Key);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
