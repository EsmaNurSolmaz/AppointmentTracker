using System.Windows.Controls;
using RandevuYonetimSistemi.ViewModels.AuthVM;

namespace RandevuYonetimSistemi.Views.Auth
{
    public partial class RegisterView : Page
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
