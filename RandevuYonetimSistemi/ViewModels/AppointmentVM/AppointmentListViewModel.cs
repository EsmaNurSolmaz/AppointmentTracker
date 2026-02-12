using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Services;

namespace RandevuYonetimSistemi.ViewModels.AppointmentVM
{
    public class AppointmentListViewModel
    {
        public ObservableCollection<AppointmentModel> Appointments { get; set; }

        private readonly AppointmentService _service;

        public AppointmentListViewModel()
        {
            Appointments = new ObservableCollection<AppointmentModel>();
            _service = new AppointmentService();
        }

        public async Task LoadAppointmentsAsync()
        {
            Appointments.Clear();

            var list = await _service.GetAllAppointmentsAsync();

            foreach (var item in list)
                Appointments.Add(item);
        }
    }
}
