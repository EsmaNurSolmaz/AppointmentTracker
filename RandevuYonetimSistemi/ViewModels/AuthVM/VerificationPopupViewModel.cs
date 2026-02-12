using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RandevuYonetimSistemi.ViewModels.AuthVM
{
    public class VerificationPopupViewModel : INotifyPropertyChanged
    {
        private readonly User _user;
        private readonly RegisterService _registerService = new RegisterService();

        private string _verificationCode;
        public string VerificationCode
        {
            get => _verificationCode;
            set
            {
                _verificationCode = value;
                OnPropertyChanged();
            }
        }

        public ICommand VerifyCommand { get; }

        public event Action<bool> CloseRequested;
        public event PropertyChangedEventHandler PropertyChanged;

        public VerificationPopupViewModel(User user)
        {
            _user = user;
            VerifyCommand = new RelayCommand(Verify);
        }
        private Brush _messageColor = Brushes.Red;


        private void Verify()
        {
            if (string.IsNullOrWhiteSpace(VerificationCode))
            {
                MessageBox.Show("Please enter the verification code.");
                return;
            }

            if (!VerificationStore.IsValid(_user.Email, VerificationCode))
            {
                MessageBox.Show("Invalid or expired verification code.");
                return;
            }

            bool success = _registerService.CompleteRegistration(_user);

            if (success)
            {
                VerificationStore.Clear();
                MessageBox.Show("Registration completed successfully.");
                CloseRequested?.Invoke(true);
            }
            else
            {
                MessageBox.Show("Registration failed.");

            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
