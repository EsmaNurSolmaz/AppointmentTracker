using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace RandevuYonetimSistemi.ViewModels.AppointmentVM
{
    public partial class AddAppointmentViewModel : ObservableObject
    {
        private readonly AppointmentService _appointmentService;
        private readonly CustomerService _customerService;
        private readonly UserSession _userSession;
        private readonly int _userId;

        public event Action? AppointmentAddedSuccessfully;

        public AddAppointmentViewModel(
            AppointmentService appointmentService,
            CustomerService customerService,
            UserSession userSession)
        {
            _appointmentService = appointmentService;
            _customerService = customerService;
            _userSession = userSession;

            if (!_userSession.IsLoggedIn)
            {
                MessageBox.Show("User not found, please log in again.");
                return;
            }

            _userId = _userSession.CurrentUser.UserId;

            Appointment = new AppointmentModel
            {
                AppointmentDate = DateTime.Now,
                CreatedByUserId = _userId,
                Status = "Pending"
            };

            StatusOptions = new ObservableCollection<string>
            {
                "Pending",
                "Completed",
                "Cancelled"
            };

            // varsayılan saat
            SelectedTime = "09:00";
        }

        [ObservableProperty]
        private AppointmentModel appointment;

        [ObservableProperty]
        private string service;

        [ObservableProperty]
        private string selectedTime;

        [ObservableProperty]
        private int selectedCustomerId;

        public ObservableCollection<CustomerModel> Customers
            => _customerService.Customers;

        public ObservableCollection<string> StatusOptions { get; }

        [RelayCommand]
        public async Task LoadCustomersAsync()
        {
            await _customerService.LoadCustomersAsync();
        }

        [RelayCommand]
        public async Task AddAppointmentAsync()
        {
            try
            {
                if (!_userSession.IsLoggedIn)
                {
                    MessageBox.Show("Session expired, please log in again.");
                    return;
                }

                if (SelectedCustomerId <= 0)
                {
                    MessageBox.Show("Please select a customer");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Service))
                {
                    MessageBox.Show("Please enter the service");
                    return;
                }

                if (!TimeSpan.TryParse(SelectedTime, out var time))
                {
                    MessageBox.Show("Please select a valid time.");
                    return;
                }

                Appointment.AppointmentDate =
                    Appointment.AppointmentDate.Date + time;

                Appointment.CustomerId = SelectedCustomerId;
                Appointment.CreatedByUserId = _userId;

                // 🔥 Hizmeti modele yaz
                Appointment.Service = Service;

                await _appointmentService.AddAppointmentAsync(Appointment);

                MessageBox.Show("Appointment created successfully ✔");

                AppointmentAddedSuccessfully?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Database error");
            }
        }
    }
}
