using System;
using System.Windows.Controls;
using RandevuYonetimSistemi.ViewModels.AppointmentVM;
using RandevuYonetimSistemi.Models;

namespace RandevuYonetimSistemi.Views.Components
{
    public partial class ListView : UserControl
    {
        public AppointmentListViewModel ViewModel { get; set; }

        public event Action<AppointmentModel> AppointmentSelected;

        public ListView()
        {
            InitializeComponent();

            ViewModel = new AppointmentListViewModel();
            DataContext = ViewModel;
        }

        public async void LoadAppointments()
        {
            await ViewModel.LoadAppointmentsAsync();
        }

        private void AppointmentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppointmentsListBox.SelectedItem is AppointmentModel selected)
            {
                AppointmentSelected?.Invoke(selected); 
            }

            AppointmentsListBox.SelectedItem = null;
        }
    }
}
