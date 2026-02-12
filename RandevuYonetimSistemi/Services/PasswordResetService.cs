using System;
using System.Data.SqlClient;
using RandevuYonetimSistemi.Services;

namespace RandevuYonetimSistemi.Services
{
    public class PasswordResetService
    {
        private readonly string connectionString =
            @"Server=localhost;Database=AppointmentTrackingSystem;Trusted_Connection=True;";

        public bool UserExists(string email)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE Email=@Email", conn);
            cmd.Parameters.AddWithValue("@Email", email);

            return (int)cmd.ExecuteScalar() > 0;
        }

        public void SaveVerificationCode(string email, string code)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                UPDATE Users 
                SET VerificationCode=@Code,
                    VerificationExpire=@Expire
                WHERE Email=@Email", conn);

            cmd.Parameters.AddWithValue("@Code", code);
            cmd.Parameters.AddWithValue("@Expire", DateTime.Now.AddMinutes(3));
            cmd.Parameters.AddWithValue("@Email", email);

            cmd.ExecuteNonQuery();
        }

        public bool VerifyCode(string email, string code)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                SELECT VerificationExpire 
                FROM Users 
                WHERE Email=@Email AND VerificationCode=@Code", conn);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Code", code);

            var expire = cmd.ExecuteScalar();

            if (expire == null) return false;

            return DateTime.Now <= (DateTime)expire;
        }

        public void ResetPassword(string email, string newPassword)
        {
            var hash = UserHelper.ComputeSha256Hash(newPassword);

            using var conn = new SqlConnection(connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                UPDATE Users 
                SET PasswordHash=@Hash,
                    VerificationCode=NULL,
                    VerificationExpire=NULL
                WHERE Email=@Email", conn);

            cmd.Parameters.AddWithValue("@Hash", hash);
            cmd.Parameters.AddWithValue("@Email", email);

            cmd.ExecuteNonQuery();
        }
    }
}
