using System;

namespace RandevuYonetimSistemi.Services
{
    public static class VerificationStore
    {
        public static string Code { get; set; }
        public static DateTime Expire { get; set; }
        public static string Email { get; set; }

        public static bool IsValid(string email, string code)
        {
            return Email == email
                   && Code == code
                   && DateTime.Now <= Expire;
        }

        public static void Clear()
        {
            Code = null;
            Email = null;
            Expire = DateTime.MinValue;
        }
    }
}
