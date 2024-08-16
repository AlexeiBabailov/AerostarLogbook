using AerostarLogbook.ViewModels;
using AerostarLogbook.Services;


namespace AerostarLogbook.Views;

public partial class MainPage : ContentPage
{
    public Action<string> OnPasswordEntered { get; private set; }

    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

    }


    private async void OnGoToMaintananceTracking_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(MaintenanceTracking)}");
    }

    private async void OnGoToFlightActivity_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(FlightActivity)}");
    }

    private async void OnGoToPermits_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(Permits)}");
    }

    private async void OnGoToWeightConfigurations_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(WeightConfigurations)}");
    }

    private async void OnGoToFrequencies_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(Frequencies)}");
    }

    private async void OnGoToUnusualEvent_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(UnusualEvent)}");
    }

    private async void OnGotoServiceBulletin_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(ServiceBulletin)}");
    }

    private async Task OnGoToManagment_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AdministratorView)}");
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var passwordPopup = new PasswordPopup(OnPasswordEntered);
        await Navigation.PushModalAsync(passwordPopup);
    }

    private async void OnGoToBiWeekly_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(BiWeekly)}");
    }

    private async void ONGOTOSoftwareConfiguration_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(SoftwareConfiguration)}");
    }

    private async void ONGOTOHardwareConfiguration_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(HardwareConfiguration)}");
    }

    private async void OnGoToFsm_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(FuselageScheduledMaintenance)}");
    }

    private async void OnGoToCSM_Clicked(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(ComponentSM)}");
    }

    private async void OnGoTo_BatterySM(object sender, EventArgs e)
    {
        await GoToPageAsync($"{nameof(BatteryScheduledMaintenance)}");
    }


    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var viewModel = BindingContext as MainPageViewModel;
        if (viewModel?.SelectionChangedCommand.CanExecute(null) ?? false)
        {
            viewModel.SelectionChangedCommand.Execute(null);
        }
    }


    private async Task GoToPageAsync(string pageName)
    {
        if (UavPicker.SelectedItem != null)
        {
            string? uavNum = UavPicker.SelectedItem.ToString();
            await Shell.Current.GoToAsync($"{pageName}?UavNum={uavNum}");
        }
        else
        {
            MissingUavNum();
        }
    }

    private void MissingUavNum()
    {
        _ = DisplayAlert("Missing Information", "Please choose UAV tail number", "OK");
    }

}
