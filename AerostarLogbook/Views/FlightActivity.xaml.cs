using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SQLite;
using System.Xml.Linq;
using AerostarLogbook.Models;
using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;

namespace AerostarLogbook.Views;

[QueryProperty(nameof(UavNum), "UavNum")]
public partial class FlightActivity : ContentPage
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

    public FlightActivity(FlightActivityViewModel vm)
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
        if (BindingContext is FlightActivityViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(GeneralButton);
        _buttonService.RegisterButton(NewFlightButton);
        _buttonService.RegisterButton(UpdateDIButton);
        _formButtons.RegisterFormButtons(Form_AddButton, Form_UpdateButton, Form_DeleteButton);
        _gridService.RegisterGrid("NewFlightActivityForm", NewFlightActivityForm);
        _gridService.RegisterGrid("DIForm", DIForm);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        FlightListView.SelectedItem = null;
        _buttonService.UnregisterButton(GeneralButton);
        _buttonService.UnregisterButton(NewFlightButton);
        _buttonService.UnregisterButton(UpdateDIButton);
        _gridService.UnregisterGrid("NewFlightActivityForm");
        _gridService.UnregisterGrid("DIForm");

    }

    private void FlightActivityGeneral_Clicked(object sender, EventArgs e)
    {

        FlightListView.SelectedItem = null;
        var button = sender as Button;
        if (button != null)
        {
            _gridService.HideAll();
        }
        _formButtons.Form_LocalNoneIsVisible();
    }
    private void OpenNewFlightActivity_Clicked(object sender, EventArgs e)
    {
       
        FlightListView.SelectedItem = null;
        _formButtons.Form_LocalAddIsVisible();
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("NewFlightActivityForm");
        }
    }
    private void UpdateDIClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("DIForm");
        }
        _formButtons.Form_LocalNoneIsVisible();
    }

    private void ShowOrHideButton_Clicked(object sender, EventArgs e)
    {
        Scroll.IsVisible = !Scroll.IsVisible;
        //DIList.IsVisible = !DIList.IsVisible;
    }

    private void FlightListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _formButtons.Form_LocalUpdateDeleteIsVisible();
        if (sender != null)
        {
            _gridService.ShowOnly("NewFlightActivityForm");
        }
        Form_UpdateButton.Focus();
        if (BindingContext is FlightActivityViewModel viewModel)
        {
            if (viewModel.UpdateFormCommand.CanExecute(null))
            {
                viewModel.UpdateFormCommand.Execute(null);
            }
        }
       // _buttonService.NoneButtonClicked();


        mainScrollView.ScrollToAsync(0, 0, true);
    }
  


    private void AddInspection_Clicked(object sender, EventArgs e)
    {
        DI.IsVisible = true;
        DINotValid.IsVisible = false;
    }


}