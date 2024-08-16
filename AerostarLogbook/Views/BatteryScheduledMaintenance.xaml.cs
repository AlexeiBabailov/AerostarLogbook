using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;

namespace AerostarLogbook.Views;

[QueryProperty(nameof(UavNum), "UavNum")]

public partial class BatteryScheduledMaintenance : ContentPage
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

    public BatteryScheduledMaintenance(BatterySMViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _buttonService = new ViewButtonService();
        _gridService = new GridService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BatterySMViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(GeneralButton);
        _buttonService.RegisterButton(AddBatteryButton);
        _gridService.RegisterGrid("BatteryForm", BatteryForm);
        
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        BatteryListView.SelectedItem = null;
        _buttonService.UnregisterButton(GeneralButton);
        _buttonService.UnregisterButton(AddBatteryButton);
        _gridService.UnregisterGrid("BatteryForm");
    }



    private void BatteryListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _buttonService.NoneButtonClicked();
        BatteryForm.IsVisible = true;
        UpdateCurrentBatt.IsVisible = true;
        DeleteCurrentBatt.IsVisible = true;
        AddNewBatt.IsVisible = false;
        BatteryForm.Focus();
        if (BindingContext is BatterySMViewModel viewModel)
            if (viewModel.UpdateFormCommand.CanExecute(null))
            {
                viewModel.UpdateFormCommand.Execute(null);
            }
        mainScrollView.ScrollToAsync(0, 0, true);
    }

    private void General_View(object sender, EventArgs e)
    {
        BatteryListView.SelectedItem = null;
        var button = sender as Button;
        if (button != null)
        {
            _gridService.HideAll();
        }
    }

    private void Open_New_Battery_Form(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("BatteryForm");
        }
        BatteryListView.SelectedItem = null;
        UpdateCurrentBatt.IsVisible = false;
        DeleteCurrentBatt.IsVisible = false;
        AddNewBatt.IsVisible = true;
    }

    private void Add_New_Battery_Button_Clicked(object sender, EventArgs e)
    {
        var missingFields = new List<string>();
        if (BindingContext is BatterySMViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.BatteryHrs))
                missingFields.Add("Battery Hours");

            if (string.IsNullOrEmpty(viewModel.InitialActivation))
                missingFields.Add("Initial Activation");

            if (string.IsNullOrEmpty(viewModel.BatteryOfficialSN))
                missingFields.Add("Battery Serial Number");

            if (missingFields.Count != 0)
            {
                string message = "Please fill in the following fields: " + string.Join(", ", missingFields);
                DisplayAlert("Missing Information", message, "OK");
            }
            else
            {
                if (viewModel.AddBatteryCommand.CanExecute(null))
                {
                    viewModel.AddBatteryCommand.Execute(null);
                }
            }
        }
    }
}