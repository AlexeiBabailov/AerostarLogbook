using AerostarLogbook.ViewModels;
using AerostarLogbook.Views;
using AerostarLogbook.Services;
using Microsoft.Extensions.Logging;
using AerostarLogbook.Interfaces;


namespace AerostarLogbook
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<UnusualEventViewModel>();
            builder.Services.AddSingleton<UnusualEvent>();
            builder.Services.AddSingleton<BiWeeklyViewModel>();
            builder.Services.AddSingleton<BiWeekly>();
            builder.Services.AddSingleton<FrequenciesViewModel>();
            builder.Services.AddSingleton<Frequencies>();
            builder.Services.AddSingleton<WeightConfigurationsViewModel>();
            builder.Services.AddSingleton<WeightConfigurations>();
            builder.Services.AddSingleton<BatterySMViewModel>();
            builder.Services.AddSingleton<BatteryScheduledMaintenance>();
            builder.Services.AddSingleton<FuselageScheduledMaintenanceViewModel>();
            builder.Services.AddSingleton<FuselageScheduledMaintenance>();
            builder.Services.AddSingleton<HardwareConfigurationViewModel>();
            builder.Services.AddSingleton<HardwareConfiguration>();
            builder.Services.AddSingleton<FlightActivityViewModel>();
            builder.Services.AddSingleton<FlightActivity>();
            builder.Services.AddSingleton<PermitsViewModel>();
            builder.Services.AddSingleton<Permits>();
            builder.Services.AddSingleton<ServiceBulletinViewModel>();
            builder.Services.AddSingleton<ServiceBulletin>();
            builder.Services.AddSingleton<SoftwareConfigurationViewModel>();
            builder.Services.AddSingleton<SoftwareConfiguration>();
            builder.Services.AddSingleton<MaintenanceTrackingViewModel>();
            builder.Services.AddSingleton<MaintenanceTracking>();
            builder.Services.AddSingleton<ComponentSMViewModel>();
            builder.Services.AddSingleton<ComponentSM>();
            builder.Services.AddSingleton<AdministratorViewModel>();
            builder.Services.AddSingleton<AdministratorView>();
            builder.Services.AddSingleton<IMessagingService, MessagingService>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
