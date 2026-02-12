using System.Security.Cryptography;

namespace RandevuYonetimSistemi.Services
{
    public static class JwtSettings
    {
        public static readonly byte[] Key;

        static JwtSettings()
        {
            Key = new byte[32];
            RandomNumberGenerator.Fill(Key);
        }
    }
}
