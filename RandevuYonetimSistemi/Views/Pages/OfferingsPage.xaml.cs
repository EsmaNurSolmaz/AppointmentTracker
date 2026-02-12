using Microsoft.Extensions.DependencyInjection;
using RandevuYonetimSistemi.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RandevuYonetimSistemi.Views.Pages
{
    public partial class Offerings : Page
    {
        private readonly ServicesService _servicesService;
        private readonly UserSession _userSession;

        public Offerings()
        {
            InitializeComponent();

            var services = ((App)Application.Current).Services;

            _userSession = services.GetRequiredService<UserSession>();

            string connectionString =
                @"Server=localhost;Database=AppointmentTrackingSystem;Trusted_Connection=True;";

            _servicesService = new ServicesService(connectionString);

            NextButton.Click += NextButton_Click;
        }

        private void ServiceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == "Enter service name")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void ServiceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "Enter service name";
                tb.Foreground = Brushes.Gray;
            }
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox newTb = new TextBox
            {
                Width = 300,
                Height = 30,
                FontSize = 14,
                Foreground = Brushes.Gray,
                Text = "Enter service name",
                Margin = new Thickness(0, 5, 0, 0)
            };

            newTb.GotFocus += ServiceTextBox_GotFocus;
            newTb.LostFocus += ServiceTextBox_LostFocus;

            ServicesPanel.Children.Add(newTb);
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_userSession.IsLoggedIn)
            {
                MessageBox.Show("User not found.");
                return;
            }

            int userId = _userSession.CurrentUser.UserId;

            List<string> services = new();

            foreach (var child in ServicesPanel.Children)
            {
                if (child is TextBox tb &&
                    !string.IsNullOrWhiteSpace(tb.Text) &&
                    tb.Text != "Enter service name")
                {
                    services.Add(tb.Text.Trim());
                }
            }

            if (services.Count == 0)
            {
                MessageBox.Show("Please enter at least one service.");
                return;
            }

            try
            {
                await _servicesService.AddServicesAsync(userId, services);
                await _servicesService.MarkSetupCompletedAsync(userId);

                _userSession.CurrentUser.IsSetupCompleted = true;

                var frame = Window.GetWindow(this)
                    .FindName("MainFrame") as Frame;

                if (frame != null)
                {
                    var serviceProvider = ((App)Application.Current).Services;
                    var page = serviceProvider.GetRequiredService<AppointmentsPage>();
                    frame.Navigate(page);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("En error occured: " + ex.Message);
            }
        }

    }
}
