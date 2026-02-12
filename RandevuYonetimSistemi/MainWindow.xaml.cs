using Microsoft.Extensions.DependencyInjection;
using RandevuYonetimSistemi.Services;
using RandevuYonetimSistemi.Views;
using RandevuYonetimSistemi.Views.Auth;
using RandevuYonetimSistemi.Views.Pages;
using System.Windows;

namespace RandevuYonetimSistemi
{
    public partial class MainWindow : Window
    {
        private readonly UserSession _userSession;

        public MainWindow()
        {
            InitializeComponent();

            _userSession = ((App)Application.Current)
                .Services
                .GetRequiredService<UserSession>();

            UpdateAuthUI();

            if (_userSession.IsLoggedIn)
            {
                var appointmentsPage = ((App)Application.Current)
                    .Services
                    .GetRequiredService<AppointmentsPage>();

                MainFrame.Navigate(appointmentsPage);
            }
            else
            {
                MainFrame.Navigate(new Home());
            }
        }

        public void UpdateAuthUI()
        {
            LogoutButton.Visibility =
                _userSession.IsLoggedIn
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userSession.IsLoggedIn)
            {
                var appointmentsPage = ((App)Application.Current)
                    .Services
                    .GetRequiredService<AppointmentsPage>();

                MainFrame.Navigate(appointmentsPage);
            }
            else
            {
                MainFrame.Navigate(new Home());
            }
        }


        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _userSession.Logout();

            Settings.Default.UserId = 0;
            Settings.Default.Email = "";
            Settings.Default.IsLoggedIn = false;
            Settings.Default.Save();

            UpdateAuthUI();
            MainFrame.Navigate(new Home());
        }

    }
}
