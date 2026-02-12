using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;

namespace RandevuYonetimSistemi.Views.Pages
{
    public partial class AddCustomerPage : Page
    {
        private readonly CustomerService _customerService;

        public AddCustomerPage(CustomerService customerService)
        {
            InitializeComponent();
            _customerService = customerService;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }


        private async void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullNameBox.Text))
                {
                    await ShowError("Name cannot be empty.");
                    return;
                }

                var customer = new CustomerModel
                {
                    FullName = FullNameBox.Text,
                    PhoneNum = PhoneBox.Text,
                    Email = EmailBox.Text
                };

                await _customerService.AddCustomerAsync(customer);

                FeedbackText.Text = "Customer saved successfully ✔";
                FeedbackText.Foreground = Brushes.LightGreen;
                FeedbackText.Visibility = Visibility.Visible;

                await Task.Delay(1000);

                var services =
                    ((App)Application.Current).Services;

                var appointmentsPage =
                    services.GetRequiredService<AppointmentsPage>();

                ((MainWindow)Application.Current.MainWindow)
                    .MainFrame.Navigate(appointmentsPage);
            }
            catch (Exception)
            {
                await ShowError("En error occured.");
            }
        }

        private async Task ShowError(string message)
        {
            FeedbackText.Text = message;
            FeedbackText.Foreground = Brushes.IndianRed;
            FeedbackText.Visibility = Visibility.Visible;

            await Task.Delay(2000);
            FeedbackText.Visibility = Visibility.Collapsed;
        }
    }
}
