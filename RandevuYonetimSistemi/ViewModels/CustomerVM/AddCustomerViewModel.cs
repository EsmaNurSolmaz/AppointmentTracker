using RandevuYonetimSistemi.Data;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RandevuYonetimSistemi.ViewModels.CustomerVM
{
    public class AddCustomerViewModel : INotifyPropertyChanged
    {
        public string FullName { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }

        private string _feedbackMessage;
        public string FeedbackMessage
        {
            get => _feedbackMessage;
            set { _feedbackMessage = value; OnPropertyChanged(); }
        }

        private Brush _feedbackColor;
        public Brush FeedbackColor
        {
            get => _feedbackColor;
            set { _feedbackColor = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }

        public AddCustomerViewModel()
        {
            SaveCommand = new RelayCommand(async () => await SaveCustomerAsync());
        }

        private async Task SaveCustomerAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullName))
                {
                    ShowError("Name cannot be empty");
                    return;
                }

                using var db = new AppDbContext();

                var customer = new CustomerModel
                {
                    FullName = FullName,
                    PhoneNum = PhoneNum,
                    Email = Email,
                    CreatedAt = DateTime.Now
                };

                db.Customers.Add(customer);
                await db.SaveChangesAsync();

                FeedbackColor = Brushes.LightGreen;
                FeedbackMessage = "Customer saved successfully";

                await Task.Delay(1000);

                var nav = Application.Current.MainWindow as System.Windows.Navigation.NavigationWindow;
                nav?.GoBack();
            }
            catch (Exception ex)
            {
                ShowError("Registration failed.");
            }
        }

        private async void ShowError(string message)
        {
            FeedbackColor = Brushes.IndianRed;
            FeedbackMessage = message;

            await Task.Delay(2000);

            FeedbackMessage = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
