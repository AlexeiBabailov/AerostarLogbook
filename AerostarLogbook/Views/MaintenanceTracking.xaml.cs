using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;
namespace AerostarLogbook.Views;

[QueryProperty(nameof(UavNum), "UavNum")]
public partial class MaintenanceTracking : ContentPage
{
    private string selctedUAV;
    private ViewButtonService _buttonService;
    private GridService _gridService;

    public string UavNum
    {
        get => selctedUAV;
        set
        {
            selctedUAV = value;
        }
    }
    public MaintenanceTracking(MaintenanceTrackingViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _buttonService = new ViewButtonService();
        _gridService = new GridService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MaintenanceTrackingViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(GeneralButton);
        _buttonService.RegisterButton(OpenButton);
        _buttonService.RegisterButton(CloseButton);
        _gridService.RegisterGrid("Open", Open);
        _gridService.RegisterGrid("Close", Close);
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MaintenanceTrackingListView.SelectedItem = null;
        _buttonService.UnregisterButton(GeneralButton);
        _buttonService.UnregisterButton(OpenButton);
        _buttonService.UnregisterButton(CloseButton);
        _gridService.UnregisterGrid("Open");
        _gridService.UnregisterGrid("Close");
        
    }


    private void MaintenanceGeneral_Clicked(object sender, EventArgs e)
    {
        MaintenanceTrackingListView.SelectedItem = null;
        var button = sender as Button;
        if (button != null)
        {
            _gridService.HideAll();
        }
    }

    private void OpenNewMaintenance_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("Open");
        }
    }

    private void CloseChosenMaintenance_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("Close");
        }
    }
}