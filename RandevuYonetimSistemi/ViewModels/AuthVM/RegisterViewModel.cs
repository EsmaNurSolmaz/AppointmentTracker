using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Services;
using RandevuYonetimSistemi.Views.Auth;

namespace RandevuYonetimSistemi.ViewModels.AuthVM
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly RegisterService _registerService = new RegisterService();

        private string? _fullName;
        private string? _username;
        private string? _email;
        private string? _phoneNum;
        private string? _password;
        private string? _message;
        private Brush _messageColor = Brushes.Red;

        public string? FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        public string? Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string? Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string? PhoneNum
        {
            get => _phoneNum;
            set { _phoneNum = value; OnPropertyChanged(); }
        }

        public string? Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string? Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        public Brush MessageColor
        {
            get => _messageColor;
            set { _messageColor = value; OnPropertyChanged(); }
        }

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
        }

        private void Register()
        {
            var user = new User
            {
                FullName = FullName?.Trim(),
                UserName = Username?.Trim(),
                Email = Email?.Trim(),
                PhoneNum = PhoneNum?.Trim(),
                PasswordHash = Password?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.Now,
                IsSetupCompleted = false
            };

            string result = _registerService.StartVerification(user);

            if (result == "VERIFY")
            {
                Message = "Verification code sent to your email.";
                MessageColor = Brushes.DarkBlue;

                var popup = new VerificationPopup
                {
                    Owner = Application.Current.MainWindow
                };

                var popupVm = new VerificationPopupViewModel(user);
                popup.DataContext = popupVm;

                popupVm.CloseRequested += success =>
                {
                    popup.DialogResult = success;
                };

                bool? verified = popup.ShowDialog();

                if (verified == true)
                {
                    Message = "Registration completed successfully!";
                    MessageColor = Brushes.Green;
                    NavigateToLogin();
                }
                else
                {
                    Message = "Verification failed or cancelled.";
                    MessageColor = Brushes.Red;
                }
            }
            else
            {
                Message = result;
                MessageColor = Brushes.Red;
            }
        }

        private void NavigateToLogin()
        {
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFrame.Navigate(new LoginView());
            };

            timer.Start();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
