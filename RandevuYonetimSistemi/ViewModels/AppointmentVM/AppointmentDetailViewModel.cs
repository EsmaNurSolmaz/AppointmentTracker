using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Services;

namespace RandevuYonetimSistemi.ViewModels
{
    public class AppointmentDetailViewModel : INotifyPropertyChanged
    {
        private readonly AppointmentService _service = new();

        public AppointmentModel Appointment { get; }

        public event Action<AppointmentModel>? DeleteRequested;

        public bool IsEditMode
        {
            get => _isEditMode;
            set { _isEditMode = value; OnPropertyChanged(); }
        }
        private bool _isEditMode;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set { _selectedDate = value; OnPropertyChanged(); }
        }
        private DateTime _selectedDate;

        public ObservableCollection<string> Hours { get; } = new();

        public string SelectedHour
        {
            get => _selectedHour;
            set { _selectedHour = value; OnPropertyChanged(); }
        }
        private string _selectedHour = "09:00";

        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }

        private AppointmentModel? _backup;

        public AppointmentDetailViewModel(AppointmentModel appointment)
        {
            Appointment = appointment;

            SelectedDate = appointment.AppointmentDate.Date;
            SelectedHour = appointment.AppointmentDate.ToString("HH:mm");

            for (int h = 8; h <= 20; h++)
            {
                Hours.Add($"{h:00}:00");
                Hours.Add($"{h:00}:30");
            }

            EditCommand = new RelayCommand(Edit);
            SaveCommand = new RelayCommand(async () => await Save());
            CancelCommand = new RelayCommand(Cancel);
            DeleteCommand = new RelayCommand(Delete);
        }

        private void Edit()
        {
            _backup = new AppointmentModel
            {
                Service = Appointment.Service,
                Notes = Appointment.Notes,
                Status = Appointment.Status,
                AppointmentDate = Appointment.AppointmentDate
            };

            IsEditMode = true;
        }

        private async Task Save()
        {
            try
            {
                ApplyDateTime();
                await _service.UpdateAppointmentAsync(Appointment);
                IsEditMode = false;
                MessageBox.Show("✅ Appointment updated");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Cancel()
        {
            if (_backup != null)
            {
                Appointment.Service = _backup.Service;
                Appointment.Notes = _backup.Notes;
                Appointment.Status = _backup.Status;
                Appointment.AppointmentDate = _backup.AppointmentDate;
            }

            IsEditMode = false;
        }

        private void Delete()
        {
            DeleteRequested?.Invoke(Appointment);
        }

        private void ApplyDateTime()
        {
            var time = TimeSpan.Parse(SelectedHour);
            Appointment.AppointmentDate = SelectedDate.Date + time;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public RelayCommand(Action execute) => _execute = execute;

        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged;
    }
}
