using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;

namespace AerostarLogbook.Views;

[QueryProperty(nameof(UavNum), "UavNum")]

public partial class ServiceBulletin : ContentPage
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

    public ServiceBulletin(ServiceBulletinViewModel vm)
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
        if (BindingContext is ServiceBulletinViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(MainViewButton);
        _buttonService.RegisterButton(AddItemButton);
        _formButtons.RegisterFormButtons(AddServiceBulletin, UpdateServiceBulletin, DeleteServiceBulletin);
        _gridService.RegisterGrid("ServiceBulletinForm", ServiceBulletinForm);

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ServiceBulletin_ListView.SelectedItem = null;
        _gridService.HideAll();
        _formButtons.Form_LocalNoneIsVisible();
        _formButtons.UnregisterFromButtons();
        _buttonService.UnregisterButton(MainViewButton);
        _buttonService.UnregisterButton(AddItemButton);
        _gridService.UnregisterGrid("ServiceBulletinForm");
    }

    private void MainViewButton_Clicked(object sender, EventArgs e)
    {
        ServiceBulletin_ListView.SelectedItem = null;
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
            _gridService.ShowOnly("ServiceBulletinForm");
        }
    }



    private void ServiceBulletin_ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _buttonService.NoneButtonClicked();
        _formButtons.Form_LocalUpdateDeleteIsVisible();
        if (sender != null)
        {
            _gridService.ShowOnly("ServiceBulletinForm");
        }
        ServiceBulletin_ListView.Focus();
        if (BindingContext is ServiceBulletinViewModel viewModel)
            if (viewModel.UpdateServiceBulletinFormCommand.CanExecute(null))
            {
                viewModel.UpdateServiceBulletinFormCommand.Execute(null);
            }

    }
}