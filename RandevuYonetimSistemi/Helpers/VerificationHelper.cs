using System;

namespace RandevuYonetimSistemi.Helpers
{
    public static class VerificationHelper
    {
        public static string GenerateCode()
        {
            var rnd = new Random();
            return rnd.Next(100000, 999999).ToString(); 
        }
    }
}
