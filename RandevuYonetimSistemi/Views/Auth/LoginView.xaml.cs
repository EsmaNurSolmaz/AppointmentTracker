using System.Windows;
using System.Windows.Controls;

namespace RandevuYonetimSistemi.Views.Auth
{
    public partial class LoginView : Page
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.AuthVM.LoginViewModel vm)
                vm.Password = PasswordBox.Password;
        }
        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ForgotPasswordView());
        }

    }
}
