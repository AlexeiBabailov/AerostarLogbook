using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;

namespace AerostarLogbook.Views;

[QueryProperty(nameof(UavNum), "UavNum")]

public partial class Permits : ContentPage
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

    public Permits(PermitsViewModel vm)
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
        if (BindingContext is PermitsViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(GeneralButton);
        _buttonService.RegisterButton(OpenButton);
        _buttonService.RegisterButton(CloseButton);
        _formButtons.RegisterFormButtons(Form_AddButton, Form_UpdateButton, OpenForm_DeleteButton);
        _gridService.RegisterGrid("OpenForm", OpenForm);
        _gridService.RegisterGrid("CloseForm", CloseForm);
        _formButtons.Form_LocalAddIsVisible();

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        PermitsListView.SelectedItem= null;
        _buttonService.UnregisterButton(GeneralButton);
        _buttonService.UnregisterButton(OpenButton);
        _buttonService.UnregisterButton(CloseButton);
        _gridService.UnregisterGrid("OpenForm");
        _gridService.UnregisterGrid("CloseForm");

    }


    private void PermitsGeneralClicked(object sender, EventArgs e)
    {
        PermitsListView.SelectedItem = null;
        var button = sender as Button;
        if (button != null)
        {
            _gridService.HideAll();
        }
    }

    private void OpenPermitClicked(object sender, EventArgs e)
    {
        PermitsListView.SelectedItem = null;
        _formButtons.Form_LocalAddIsVisible();
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("OpenForm");
        }
    }

    private void ClosePermitClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("CloseForm");
        }
    }

    private void PermitsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _buttonService.NoneButtonClicked();
        _formButtons.Form_LocalUpdateDeleteIsVisible();
        if (sender != null & CloseForm.IsVisible == false)
        {
            _gridService.ShowOnly("OpenForm");
        }
        if (BindingContext is PermitsViewModel viewModel)
            if (viewModel.UpdatePermitFormCommand.CanExecute(null))
            {
                viewModel.UpdatePermitFormCommand.Execute(null);
            }

    }

    private void Form_Button_Clicked(object sender, EventArgs e)
    {
        PermitsListView.SelectedItem = null;
        _formButtons.Form_LocalAddIsVisible();
    }
}