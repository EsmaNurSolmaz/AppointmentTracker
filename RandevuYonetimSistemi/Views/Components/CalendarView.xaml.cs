using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.ViewModels.AppointmentVM;

namespace RandevuYonetimSistemi.Views.Components
{
    public partial class CalendarView : UserControl
    {
        private AppointmentCalendarViewModel vm => DataContext as AppointmentCalendarViewModel;

        public event Action<AppointmentModel> AppointmentSelected;

        public CalendarView()
        {
            InitializeComponent();

            DataContext = new AppointmentCalendarViewModel();

            Loaded += CalendarView_Loaded;
        }

        private async void CalendarView_Loaded(object sender, RoutedEventArgs e)
        {
            await vm.LoadAppointmentsAsync();

            vm.Appointments.CollectionChanged += Appointments_CollectionChanged;

            DrawCalendar();
        }

        public async void RefreshAsync()
        {
            await vm.LoadAppointmentsAsync();
            DrawCalendar();
        }

        private void Appointments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DrawCalendar();
        }

        private void DrawCalendar()
        {
            CalendarGrid.Children.Clear();

            AddDayHeaders();
            AddHourLabels();
            AddAppointments();

            WeekLabel.Text =
                $"{vm.WeekStartDate:dd MMM yyyy} - {vm.WeekStartDate.AddDays(6):dd MMM yyyy}";
        }


        private void AddDayHeaders()
        {
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

            for (int i = 0; i < 7; i++)
            {
                var header = new TextBlock
                {
                    Text = $"{days[i]}\n{vm.WeekStartDate.AddDays(i):dd MMM}",
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Grid.SetColumn(header, i + 1);
                Grid.SetRow(header, 0);

                CalendarGrid.Children.Add(header);
            }
        }

        private void AddHourLabels()
        {
            for (int i = 0; i < 12; i++)
            {
                int hour = 8 + i;

                var tb = new TextBlock
                {
                    Text = $"{hour}:00",
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5)
                };

                Grid.SetColumn(tb, 0);
                Grid.SetRow(tb, i + 1);

                CalendarGrid.Children.Add(tb);
            }
        }

        private void AddAppointments()
        {
            foreach (var appt in vm.Appointments)
            {
                var date = appt.AppointmentDate;

                int col = (date.Date - vm.WeekStartDate).Days + 1;
                if (col < 1 || col > 7) continue;

                int row = (date.Hour - 8) + 1;
                if (row < 1 || row > 12) continue;

                var box = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(20, 120, 240)),
                    CornerRadius = new CornerRadius(6),
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = appt
                };

                box.Child = new TextBlock
                {
                    Text = $"{appt.Customer?.FullName}\n{appt.Service}",
                    Foreground = Brushes.White
                };

                box.MouseLeftButtonUp += Appointment_Clicked;

                Grid.SetColumn(box, col);
                Grid.SetRow(box, row);

                CalendarGrid.Children.Add(box);
            }
        }

        private void Appointment_Clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is AppointmentModel model)
            {
                AppointmentSelected?.Invoke(model);
            }
        }

        private void PrevWeek_Click(object sender, RoutedEventArgs e)
        {
            vm.WeekStartDate = vm.WeekStartDate.AddDays(-7);
            DrawCalendar();
        }

        private void NextWeek_Click(object sender, RoutedEventArgs e)
        {
            vm.WeekStartDate = vm.WeekStartDate.AddDays(7);
            DrawCalendar();
        }
    }
}
