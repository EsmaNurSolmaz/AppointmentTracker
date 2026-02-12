using Microsoft.Extensions.DependencyInjection;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Services;
using RandevuYonetimSistemi.Views.Pages;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RandevuYonetimSistemi.ViewModels.AuthVM
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly LoginService _loginService;
        private readonly UserSession _userSession;

        private string _email;
        private string _password;
        private string _message;
        private Brush _messageColor = Brushes.Red;

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        public Brush MessageColor
        {
            get => _messageColor;
            set { _messageColor = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            var services = ((App)Application.Current).Services;

            _loginService = services.GetRequiredService<LoginService>();
            _userSession = services.GetRequiredService<UserSession>();

            LoginCommand = new RelayCommand(Login);
        }

        public void Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "Email or password cannot be empty.";
                MessageColor = Brushes.Red;
                return;
            }

            var result = _loginService.Login(Email.Trim(), Password.Trim());

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                Message = result.ErrorMessage;
                MessageColor = Brushes.Red;
                return;
            }

            _userSession.Login(new User
            {
                UserId = result.UserId,
                Email = Email,
                IsSetupCompleted = result.IsSetupCompleted
            });

            RandevuYonetimSistemi.Settings.Default.UserId = result.UserId;
            RandevuYonetimSistemi.Settings.Default.Email = Email;
            RandevuYonetimSistemi.Settings.Default.IsLoggedIn = true;
            RandevuYonetimSistemi.Settings.Default.Save();

            Message = "Login successful!";
            MessageColor = Brushes.Green;

            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += (s, e) =>
            {
                timer.Stop();

                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.UpdateAuthUI();

                    var services = ((App)Application.Current).Services;

                    if (result.IsSetupCompleted)
                        mainWindow.MainFrame.Navigate(
                            services.GetRequiredService<AppointmentsPage>());
                    else
                        mainWindow.MainFrame.Navigate(new Welcome());
                }
            };

            timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
