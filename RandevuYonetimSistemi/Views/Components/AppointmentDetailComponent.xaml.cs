using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.ViewModels;

namespace RandevuYonetimSistemi.Views.Components
{
    public partial class AppointmentDetailComponent : UserControl
    {
        public AppointmentDetailComponent()
        {
            InitializeComponent();
        }

        public event Action<AppointmentModel>? DeleteRequested;

        public AppointmentModel Appointment
        {
            get => (AppointmentModel)GetValue(AppointmentProperty);
            set => SetValue(AppointmentProperty, value);
        }

        public static readonly DependencyProperty AppointmentProperty =
            DependencyProperty.Register(
                nameof(Appointment),
                typeof(AppointmentModel),
                typeof(AppointmentDetailComponent),
                new PropertyMetadata(null, OnAppointmentChanged));

        private static void OnAppointmentChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (AppointmentDetailComponent)d;

            if (e.NewValue is AppointmentModel appt)
            {
                var vm = new AppointmentDetailViewModel(appt);

                vm.DeleteRequested += appointment =>
                {
                    control.DeleteRequested?.Invoke(appointment);
                };

                control.DataContext = vm;
            }
        }
    }
}
