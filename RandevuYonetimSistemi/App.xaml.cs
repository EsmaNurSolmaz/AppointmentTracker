using Microsoft.Extensions.DependencyInjection;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Services;
using RandevuYonetimSistemi.ViewModels.AppointmentVM;
using RandevuYonetimSistemi.Views.Pages;
using System;
using System.Globalization;
using System.Windows;

namespace RandevuYonetimSistemi
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            base.OnStartup(e);

            var services = new ServiceCollection();

            services.AddSingleton<UserSession>();

            services.AddSingleton<CustomerService>();
            services.AddSingleton<LoginService>();
            services.AddSingleton<AppointmentService>();

            services.AddTransient<AddAppointmentViewModel>();

            services.AddTransient<AppointmentsPage>();
            services.AddTransient<AddCustomerPage>();
            services.AddTransient<AddAppointmentPage>();

            services.AddSingleton<PasswordResetService>();
            services.AddSingleton<BrevoMailService>();


            Services = services.BuildServiceProvider();

            if (Settings.Default.IsLoggedIn &&
                Settings.Default.UserId > 0)
            {
                var userSession = Services.GetRequiredService<UserSession>();

                userSession.Login(new Models.User
                {
                    UserId = Settings.Default.UserId,
                    Email = Settings.Default.Email
                });
            }
        }
    }
}
