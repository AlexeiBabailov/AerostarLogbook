using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;
using System.Text.RegularExpressions;


namespace AerostarLogbook.Views;

public partial class AdministratorView : ContentPage
{
    private ViewButtonService _buttonService;
    private ViewServiceForAdministrator _stackService;
    public AdministratorView(AdministratorViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
        _buttonService = new ViewButtonService();
        _stackService = new ViewServiceForAdministrator();
        _buttonService.RegisterButton(MainButton);
        _buttonService.RegisterButton(UAVButton);
        _buttonService.RegisterButton(EngineButton);
        _buttonService.RegisterButton(ComponentsButton);
        _buttonService.RegisterButton(TechniciansButton);
        _buttonService.RegisterButton(ChiefTechniciansButton);
        _buttonService.RegisterButton(ExternalPilotsButton);
        _stackService.RegisterStack("UavsView", UavsView);
        _stackService.RegisterStack("EngineView", EngineView);
        _stackService.RegisterStack("ComponentsView", ComponentsView);
        _stackService.RegisterStack("TechniciansView", TechniciansView);
        _stackService.RegisterStack("ChiefTechniciansView", ChiefTechniciansView);
        _stackService.RegisterStack("ExternalPilotsView", ExternalPilotsView);
    }


    private void MainButton_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _stackService.ShowAll();
        }
    }


    private void UAV_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _stackService.ShowOnly("UavsView");
        }
    }

    private void Engine_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _stackService.ShowOnly("EngineView");
        }
    }

    private void Components_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _stackService.ShowOnly("ComponentsView");
        }
    }
    private void Technicians_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _stackService.ShowOnly("TechniciansView");
        }
    }

    private void ChiefTechnicians_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _stackService.ShowOnly("ChiefTechniciansView");
        }
    }

    private void ExternalPilots_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _stackService.ShowOnly("ExternalPilotsView");
        }
    }


    private class ViewServiceForAdministrator
    {
        private Dictionary<string, VerticalStackLayout> _stacks = new Dictionary<string, VerticalStackLayout>();

        public void RegisterStack(string key, VerticalStackLayout stack)
        {
            if (!_stacks.ContainsKey(key))
            {
                _stacks.Add(key, stack);
            }
        }

        public void ShowOnly(string key)
        {
            foreach (var stack in _stacks)
            {
                stack.Value.IsVisible = stack.Key == key;
            }
        }

        public void ShowAll()
        {
            foreach (var stack in _stacks.Values)
            {
                stack.IsVisible = true;
            }
        }

        public void UnregisterStack(string key)
        {
            if (_stacks.ContainsKey(key))
            {
                _stacks.Remove(key);
            }
        }
    }

    private void AddNewEngine_Clicked(object sender, EventArgs e)
    {
        if (EngineInitialHours != null && IsValidTime(EngineInitialHours.Text))
        {
            if (BindingContext is AdministratorViewModel viewModel)
            {
                if (viewModel.AddEngineCommand.CanExecute(null))
                {
                    viewModel.AddEngineCommand.Execute(null);
                }
            }
        }
        else
        {
            _ = DisplayAlert("Illegal Hours Format", "Please correct Initial Hours format to HH:mm", "OK");
        }
    }


    private void DeleteEngineButton_Clicked(object sender, EventArgs e)
    {
     
        _ = DisplayAlert("Forbidden Action", "It is not allowed to delete engine since it might cause Flight Activity records problems", "OK");
    }

    private void AddUavButton_Clicked(object sender, EventArgs e)
    {
        if (UavHours != null && IsValidTime(UavHours.Text))
        {
            if (BindingContext is AdministratorViewModel viewModel)
            {
                if (viewModel.AddUavCommand.CanExecute(null))
                {
                    viewModel.AddUavCommand.Execute(null);
                }
            }
        }
        else
        {
            _ = DisplayAlert("Illegal Hours Format", "Please correct UAV Hours format to HH:mm", "OK");
        }
    }

    private static bool IsValidTime(string time)
    {
        if (time == null)
        {
            return false;
        }
        string pattern = @"^([0-9]{1,3}):([0-5][0-9])$";
        return Regex.IsMatch(time, pattern);
    }

}