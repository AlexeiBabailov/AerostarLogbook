using AerostarLogbook.Views;
using AerostarLogbook.Services;

using AerostarLogbook.Models;


namespace AerostarLogbook
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(UnusualEvent), typeof(UnusualEvent));
            Routing.RegisterRoute(nameof(BiWeekly), typeof(BiWeekly));
            Routing.RegisterRoute(nameof(Frequencies), typeof(Frequencies));
            Routing.RegisterRoute(nameof(WeightConfigurations), typeof(WeightConfigurations));
            Routing.RegisterRoute(nameof(BatteryScheduledMaintenance), typeof(BatteryScheduledMaintenance));
            Routing.RegisterRoute(nameof(FuselageScheduledMaintenance), typeof(FuselageScheduledMaintenance));
            Routing.RegisterRoute(nameof(HardwareConfiguration), typeof(HardwareConfiguration));
            Routing.RegisterRoute(nameof(FlightActivity), typeof(FlightActivity));
            Routing.RegisterRoute(nameof(Permits), typeof(Permits));
            Routing.RegisterRoute(nameof(ServiceBulletin), typeof(ServiceBulletin));
            Routing.RegisterRoute(nameof(SoftwareConfiguration), typeof(SoftwareConfiguration));
            Routing.RegisterRoute(nameof(MaintenanceTracking), typeof(MaintenanceTracking));
            Routing.RegisterRoute(nameof(ComponentSM), typeof(ComponentSM));
            Routing.RegisterRoute(nameof(AdministratorView), typeof(AdministratorView));

        }
    }
}
