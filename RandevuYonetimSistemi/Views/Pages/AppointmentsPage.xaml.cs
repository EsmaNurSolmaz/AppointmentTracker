using Microsoft.Extensions.DependencyInjection;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RandevuYonetimSistemi.Views.Pages
{
    public partial class AppointmentsPage : Page
    {
        private readonly CustomerService _customerService;
        private readonly AppointmentService _appointmentService;
        private readonly UserSession _userSession;

        public AppointmentsPage(
            CustomerService customerService,
            AppointmentService appointmentService,
            UserSession userSession)
        {
            InitializeComponent();

            _customerService = customerService;
            _appointmentService = appointmentService;
            _userSession = userSession;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_userSession.IsLoggedIn)
                return;

            CalendarComponent?.RefreshAsync();
        }

        private void ViewToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (CalendarComponent == null || ListComponent == null)
                return;

            CalendarComponent.Visibility =
                CalendarToggle.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;

            ListComponent.Visibility =
                ListToggle.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;

            if (ListToggle.IsChecked == true)
            {
                ListComponent.LoadAppointments();
            }
        }

        private void OnAppointmentSelected(AppointmentModel appointment)
        {
            if (appointment == null)
                return;

            DetailComponent.DeleteRequested -= OnDeleteRequested;
            DetailComponent.DeleteRequested += OnDeleteRequested;

            DetailComponent.Appointment = appointment;
            DetailOverlay.Visibility = Visibility.Visible;
        }

        private async void OnDeleteRequested(AppointmentModel appointment)
        {
            if (appointment == null)
                return;

            var result = MessageBox.Show(
                "Do you want to delete this appointment?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            await _appointmentService.DeleteAppointmentAsync(appointment.AppointmentId);

            DetailOverlay.Visibility = Visibility.Collapsed;

            CalendarComponent?.RefreshAsync();

            if (ListToggle.IsChecked == true)
            {
                ListComponent.LoadAppointments();
            }
        }

        private void DetailOverlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DetailOverlay.Visibility = Visibility.Collapsed;
        }

        private void DetailPopup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void NewCustomer_Click(object sender, RoutedEventArgs e)
        {
            var services = ((App)Application.Current).Services;
            var page = services.GetRequiredService<AddCustomerPage>();

            NavigationService?.Navigate(page);
        }

        private void NewAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (!_userSession.IsLoggedIn)
                return;

            var services = ((App)Application.Current).Services;
            var page = services.GetRequiredService<AddAppointmentPage>();

            NavigationService?.Navigate(page);
        }
    }
}
