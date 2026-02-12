using RandevuYonetimSistemi.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace RandevuYonetimSistemi.ViewModels.AuthVM
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private readonly PasswordResetService _resetService;
        private readonly BrevoMailService _mailService;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action PasswordResetCompleted;


        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }

        private string _message;
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        public Brush MessageColor { get; set; } = Brushes.Red;

        public ICommand SendCodeCommand { get; }
        public ICommand VerifyAndResetCommand { get; }

        public ForgotPasswordViewModel()
        {
            _resetService = new PasswordResetService();
            _mailService = new BrevoMailService();

            SendCodeCommand = new RelayCommand(SendCode);
            VerifyAndResetCommand = new RelayCommand(VerifyAndReset);
        }

        private async void SendCode()
        {
            if (!_resetService.UserExists(Email))
            {
                Message = "Email not found.";
                return;
            }

            var code = new Random().Next(100000, 999999).ToString();

            _resetService.SaveVerificationCode(Email, code);
            await _mailService.SendVerificationCode(Email, code);

            MessageColor = Brushes.Green;
            Message = "Verification code sent.";
        }
        private void VerifyAndReset()
        {
            if (!_resetService.VerifyCode(Email, Code))
            {
                MessageColor = Brushes.Red;
                Message = "Invalid or expired code.";
                return;
            }

            _resetService.ResetPassword(Email, NewPassword);

            MessageColor = Brushes.Green;
            Message = "Password reset successful.";

            // 🔁 1 saniye sonra Login'e dön
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                PasswordResetCompleted?.Invoke();
            };

            timer.Start();
        }


        void OnPropertyChanged([CallerMemberName] string p = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }
}
