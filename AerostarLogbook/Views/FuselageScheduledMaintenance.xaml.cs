using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;

namespace AerostarLogbook.Views;

[QueryProperty(nameof(UavNum), "UavNum")]

public partial class FuselageScheduledMaintenance : ContentPage
{
    private string selctedUAV;
    private ViewButtonService _buttonService;
    private GridService _gridService;
    private FormCRUDButtonService _formButtons;


    public string UavNum
    {
        get => selctedUAV;
        set
        {
            selctedUAV = value;
        }
    }

    public FuselageScheduledMaintenance(FuselageScheduledMaintenanceViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        _buttonService = new ViewButtonService();
        _gridService = new GridService();
        _formButtons = new FormCRUDButtonService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is FuselageScheduledMaintenanceViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(MainViewButton);
        _buttonService.RegisterButton(AddItemButton);
        _formButtons.RegisterFormButtons(AddFSM, UpdateFSM, DeleteFSM);
        _gridService.RegisterGrid("FSMForm", FSMForm);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        FSMListView.SelectedItem = null;
        _gridService.HideAll();
        _formButtons.Form_LocalNoneIsVisible();
        _formButtons.UnregisterFromButtons();
        _buttonService.UnregisterButton(MainViewButton);
        _buttonService.UnregisterButton(AddItemButton);
        _gridService.UnregisterGrid("FSMForm");
    }
    private void MainViewButton_Clicked(object sender, EventArgs e)
    {
        FSMListView.SelectedItem = null;
        var button = sender as Button;
        if (button != null)
        {
            _gridService.HideAll();
        }
        _formButtons.Form_LocalNoneIsVisible();
    }

    private void AddItemButton_Clicked(object sender, EventArgs e)
    {
        _formButtons.Form_LocalAddIsVisible();
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("FSMForm");
        }
    }
    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _buttonService.NoneButtonClicked();
        _formButtons.Form_LocalUpdateDeleteIsVisible();
        if (sender != null)
        {
            _gridService.ShowOnly("FSMForm");
        }
        FSMListView.Focus();
        if (BindingContext is FuselageScheduledMaintenanceViewModel viewModel)
            if (viewModel.UpdateFsmFormCommand.CanExecute(null))
            {
                viewModel.UpdateFsmFormCommand.Execute(null);
            }
    }

    private void Add_Button_Clicked(object sender, EventArgs e)
    {
        var missingFields = new List<string>();
        if (BindingContext is FuselageScheduledMaintenanceViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ValidUavHr))
                missingFields.Add("Valid UAV Hours");

            if (string.IsNullOrEmpty(viewModel.DoneAtUavHr))
                missingFields.Add("Done At UAV Hours");

            if (string.IsNullOrEmpty(viewModel.Technician))
                missingFields.Add("Technician");

            if (string.IsNullOrEmpty(viewModel.ChiefTechnician))
                missingFields.Add("ChiefTechnician");

            if (string.IsNullOrEmpty(viewModel.Type))
                missingFields.Add("Maintenance Type");

            if (missingFields.Count != 0)
            {
                string message = "Please fill in the following fields: " + string.Join(", ", missingFields);
                DisplayAlert("Missing Information", message, "OK");
            }
            else
            {
                // Assuming you have a command in your ViewModel to handle the assembly
                if (viewModel.AddFsmCommand.CanExecute(null))
                {
                    viewModel.AddFsmCommand.Execute(null);
                }
            }
        }
    }


}