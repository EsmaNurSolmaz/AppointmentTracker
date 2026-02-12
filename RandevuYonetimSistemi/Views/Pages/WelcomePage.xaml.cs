using RandevuYonetimSistemi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;

namespace RandevuYonetimSistemi.Views.Pages
{
    public partial class Welcome : Page
    {
        private const string PlaceholderText = "Healthcare, beauty services etc.";
        private int _sectorTextBoxCount = 1;

        private readonly SectorsService _sectorsService;
        private readonly UserSession _userSession;

        private readonly string _connectionString =
            @"Server=localhost;Database=AppointmentTrackingSystem;Trusted_Connection=True;";

        public Welcome()
        {
            InitializeComponent();

            var services = ((App)Application.Current).Services;
            _userSession = services.GetRequiredService<UserSession>();

            _sectorsService = new SectorsService(_connectionString);

            NextButton.Click += async (s, e) => await NextButton_Click();
        }

        private void SectorTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == PlaceholderText)
            {
                tb.Text = string.Empty;
                tb.Foreground = Brushes.Black;
            }
        }

        private void SectorTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = PlaceholderText;
                tb.Foreground = Brushes.Gray;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newTextBox = new TextBox
            {
                Width = 300,
                Height = 36,
                FontSize = 14,
                Margin = new Thickness(0, 5, 0, 0),
                Foreground = Brushes.Gray,
                Text = PlaceholderText
            };

            newTextBox.GotFocus += SectorTextBox_GotFocus;
            newTextBox.LostFocus += SectorTextBox_LostFocus;

            SectorsPanel.Children.Add(newTextBox);
        }

        private async Task NextButton_Click()
        {
            if (!_userSession.IsLoggedIn)
            {
                MessageBox.Show("User not found.");
                return;
            }

            int userId = _userSession.CurrentUser.UserId;

            List<string> sectors = new();

            foreach (var child in SectorsPanel.Children)
            {
                if (child is TextBox tb &&
                    !string.IsNullOrWhiteSpace(tb.Text) &&
                    tb.Text != PlaceholderText)
                {
                    sectors.Add(tb.Text.Trim());
                }
            }

            if (sectors.Count == 0)
            {
                MessageBox.Show("Please enter at least one service.");
                return;
            }

            await _sectorsService.AddSectorsAsync(userId, sectors);

            var frame = Window.GetWindow(this).FindName("MainFrame") as Frame;
            frame?.Navigate(new Offerings());
        }

    }
}
