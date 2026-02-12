namespace RandevuYonetimSistemi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string PhoneNum { get; set; }

        public bool IsVerified { get; set; }
        public string VerificationCode { get; set; }
        public DateTime? VerificationExpire { get; set; }

        public bool IsSetupCompleted { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}
