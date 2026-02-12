using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RandevuYonetimSistemi.ViewModels.AppointmentVM
{
    public class AppointmentCalendarViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<AppointmentModel> Appointments { get; set; }
        private readonly AppointmentService _service;

        private DateTime _weekStartDate;
        public DateTime WeekStartDate
        {
            get => _weekStartDate;
            set
            {
                _weekStartDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WeekDates));
            }
        }

        public string[] WeekDates =>
            new string[7]
            {
                WeekStartDate.ToString("dd MMM"),
                WeekStartDate.AddDays(1).ToString("dd MMM"),
                WeekStartDate.AddDays(2).ToString("dd MMM"),
                WeekStartDate.AddDays(3).ToString("dd MMM"),
                WeekStartDate.AddDays(4).ToString("dd MMM"),
                WeekStartDate.AddDays(5).ToString("dd MMM"),
                WeekStartDate.AddDays(6).ToString("dd MMM")
            };

        public AppointmentCalendarViewModel()
        {
            Appointments = new ObservableCollection<AppointmentModel>();
            _service = new AppointmentService();

            WeekStartDate = DateTime.Today.StartOfWeek(DayOfWeek.Monday);
        }

        public async Task LoadAppointmentsAsync()
        {
            Appointments.Clear();

            var list = await _service.GetAllAppointmentsAsync();

            foreach (var item in list)
                Appointments.Add(item);
        }

        public void NextWeek()
        {
            WeekStartDate = WeekStartDate.AddDays(7);
        }

        public void PreviousWeek()
        {
            WeekStartDate = WeekStartDate.AddDays(-7);
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public static class DateExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-diff).Date;
        }
    }
}
