using RandevuYonetimSistemi.ViewModels.AppointmentVM;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RandevuYonetimSistemi.Views.Pages
{
    public partial class AddAppointmentPage : Page
    {
        private readonly AddAppointmentViewModel _vm;

        public AddAppointmentPage(AddAppointmentViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            _vm.AppointmentAddedSuccessfully += async () =>
            {
                await Task.Delay(1000);
                Dispatcher.Invoke(() =>
                {
                    NavigationService?.GoBack();
                });
            };
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await _vm.LoadCustomersAsync();

            WarningText.Visibility = Visibility.Visible;
            await Task.Delay(4000);
            WarningText.Visibility = Visibility.Collapsed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
