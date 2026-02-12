using System.Windows.Controls;

namespace RandevuYonetimSistemi.Views
{
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Auth.LoginView());
        }

        private void RegisterButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Auth.RegisterView());
        }
    }
}
