using System.Windows.Controls;
using RandevuYonetimSistemi.ViewModels.AuthVM;

namespace RandevuYonetimSistemi.Views.Auth
{
    public partial class ForgotPasswordView : Page
    {
        public ForgotPasswordView()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                if (DataContext is ForgotPasswordViewModel vm)
                {
                    vm.PasswordResetCompleted += () =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            NavigationService.Navigate(new LoginView());
                        });
                    };
                }
            };
        }

        private void NewPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ForgotPasswordViewModel vm)
                vm.NewPassword = NewPasswordBox.Password;
        }
    }
}
