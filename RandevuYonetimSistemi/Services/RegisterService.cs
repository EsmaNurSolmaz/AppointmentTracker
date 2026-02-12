using RandevuYonetimSistemi.Helpers;
using RandevuYonetimSistemi.Models;
using System;
using System.Data.SqlClient;

namespace RandevuYonetimSistemi.Services
{
    public class RegisterService
    {
        private readonly string connectionString = ConnectionStringConfig.ConnectionString;
        private readonly BrevoMailService _mailService = new BrevoMailService();

        public string StartVerification(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FullName) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.PhoneNum) ||
                string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return "Please fill in all required fields.";
            }

            if (!UserHelper.IsValidEmail(user.Email))
                return "Please enter a valid email address.";

            if (user.PasswordHash.Length < 8)
                return "Password must be at least 8 characters long.";

            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            using SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
            checkCmd.Parameters.AddWithValue("@Email", user.Email);

            if ((int)checkCmd.ExecuteScalar() > 0)
                return "This email is already registered.";

            string code = VerificationHelper.GenerateCode();

            VerificationStore.Email = user.Email;
            VerificationStore.Code = code;
            VerificationStore.Expire = DateTime.Now.AddMinutes(3);

            _mailService.SendVerificationCode(user.Email, code);

            return "VERIFY";
        }

        public bool CompleteRegistration(User user)
        {
            string hashedPassword = UserHelper.ComputeSha256Hash(user.PasswordHash);

            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string insertQuery = @"
                INSERT INTO Users
                (Username, Email, PasswordHash, FullName, PhoneNum, IsActive, CreatedAt, IsSetupCompleted)
                VALUES
                (@Username, @Email, @PasswordHash, @FullName, @PhoneNum, 1, @CreatedAt, 0)";

            using SqlCommand cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@Username", user.UserName ?? "");
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@PhoneNum", user.PhoneNum);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
