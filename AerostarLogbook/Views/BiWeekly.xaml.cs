using Microsoft.Maui.Platform;
using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;

namespace AerostarLogbook.Views;

[QueryProperty(nameof(UavNum), "UavNum")]
public partial class BiWeekly : ContentPage
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
    public BiWeekly(BiWeeklyViewModel vm)
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
        if (BindingContext is BiWeeklyViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(MainViewButton);
        _buttonService.RegisterButton(AddItemButton);
        _formButtons.RegisterFormButtons(Form_AddButton, Form_UpdateButton, Form_DeleteButton);
        _gridService.RegisterGrid("BiweeklyForm", BiweeklyForm);

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        BiWeeklyListView.SelectedItem = null;
        _buttonService.NoneButtonClicked();
        _gridService.HideAll();
        _formButtons.Form_LocalNoneIsVisible();
        _formButtons.UnregisterFromButtons();
        _buttonService.UnregisterButton(MainViewButton);
        _buttonService.UnregisterButton(AddItemButton);
        _gridService.UnregisterGrid("BiweeklyForm");
    }


    private void MainViewButton_Clicked(object sender, EventArgs e)
    {
        BiWeeklyListView.SelectedItem = null;
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
            _gridService.ShowOnly("BiweeklyForm");
        }
    }


    private void BiWeeklyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _buttonService.NoneButtonClicked();
        _formButtons.Form_LocalUpdateDeleteIsVisible();
        if (sender != null)
        {
            _gridService.ShowOnly("BiweeklyForm");
        }
        BiweeklyForm.Focus();
        if (BindingContext is BiWeeklyViewModel viewModel)
            if (viewModel.UpdateFormCommand.CanExecute(null))
            {
                viewModel.UpdateFormCommand.Execute(null);
            }
    }
}