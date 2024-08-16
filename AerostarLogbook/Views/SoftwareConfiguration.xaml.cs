using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;

namespace AerostarLogbook.Views;
[QueryProperty(nameof(UavNum), "UavNum")]

public partial class SoftwareConfiguration : ContentPage
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

    public SoftwareConfiguration(SoftwareConfigurationViewModel vm)
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
        if (BindingContext is SoftwareConfigurationViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(MainViewButton);
        _buttonService.RegisterButton(AddItemButton);
        _formButtons.RegisterFormButtons(AddSoftwareConf, UpdateSoftwareConf, DeleteSoftwareConf);
        _gridService.RegisterGrid("SoftwareConfForm", SoftwareConfForm);

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        SoftwareConfListView.SelectedItem = null;
        _gridService.HideAll();
        _formButtons.Form_LocalNoneIsVisible();
        _formButtons.UnregisterFromButtons();
        _buttonService.UnregisterButton(MainViewButton);
        _buttonService.UnregisterButton(AddItemButton);
        _gridService.UnregisterGrid("SoftwareConfForm");
    }

    private void MainViewButton_Clicked(object sender, EventArgs e)
    {
        SoftwareConfListView.SelectedItem = null;
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
            _gridService.ShowOnly("SoftwareConfForm");
        }
    }

    private void SoftwareConfListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _buttonService.NoneButtonClicked();
        _formButtons.Form_LocalUpdateDeleteIsVisible();
        if (sender != null)
        {
            _gridService.ShowOnly("SoftwareConfForm");
        }
        SoftwareConfListView.Focus();
        if (BindingContext is SoftwareConfigurationViewModel viewModel)
            if (viewModel.UpdateSoftwareConfigurationFormCommand.CanExecute(null))
            {
                viewModel.UpdateSoftwareConfigurationFormCommand.Execute(null);
            }

    }
}