using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;
namespace AerostarLogbook.Views;
[QueryProperty(nameof(UavNum), "UavNum")]
public partial class UnusualEvent : ContentPage
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
    public UnusualEvent(UnusualEventViewModel vm)
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
        if (BindingContext is UnusualEventViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(MainViewButton);
        _buttonService.RegisterButton(AddItemButton);
        _formButtons.RegisterFormButtons(AddUnusualEvent, UpdateUnusualEvent, DeleteUnusualEvent);
        _gridService.RegisterGrid("UnusualEventForm", UnusualEventForm);


    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        EventsListView.SelectedItem = null;
        _gridService.HideAll();
        _formButtons.Form_LocalNoneIsVisible();
        _formButtons.UnregisterFromButtons();
        _buttonService.UnregisterButton(MainViewButton);
        _buttonService.UnregisterButton(AddItemButton);
        _gridService.UnregisterGrid("UnusualEventForm");
    }
    private void MainViewButton_Clicked(object sender, EventArgs e)
    {
        EventsListView.SelectedItem = null;
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
            _gridService.ShowOnly("UnusualEventForm");
        }
    }

    private void EventsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _buttonService.NoneButtonClicked();
        _formButtons.Form_LocalUpdateDeleteIsVisible();
        if (sender != null)
        {
            _gridService.ShowOnly("UnusualEventForm");
        }
        EventsListView.Focus();
        if (BindingContext is UnusualEventViewModel viewModel)
            if (viewModel.UpdateEventFormCommand.CanExecute(null))
            {
                viewModel.UpdateEventFormCommand.Execute(null);
            }
    }
}