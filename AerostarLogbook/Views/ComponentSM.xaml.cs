using AerostarLogbook.Models;
using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;

namespace AerostarLogbook.Views;

[QueryProperty(nameof(UavNum), "UavNum")]
public partial class ComponentSM : ContentPage
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

    public ComponentSM(ComponentSMViewModel vm)
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
        if (BindingContext is ComponentSMViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(GeneralButton);
        _buttonService.RegisterButton(AssembleButton);
        _buttonService.RegisterButton(DisassembleButton);
        _gridService.RegisterGrid("AssembleComponentForm", AssembleComponentForm);
        _gridService.RegisterGrid("DisassembleComponentForm", DisassembleComponentForm);

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CurrentComponentList.SelectedItem = null;
        _buttonService.UnregisterButton(GeneralButton);
        _buttonService.UnregisterButton(AssembleButton);
        _buttonService.UnregisterButton(DisassembleButton);
        _gridService.UnregisterGrid("AssembleComponentForm");
        _gridService.UnregisterGrid("DisassembleComponentForm");

    }

    private void General_Clicked(object sender, EventArgs e)
    {
        CurrentComponentList.SelectedItem = null;
        var button = sender as Button;
        if (button != null)
        {
            _gridService.HideAll();
        }
    }
    private void Assemble_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("AssembleComponentForm");
        }
    }

    private void Disassemble_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("DisassembleComponentForm");
        }
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        if (picker.SelectedIndex == -1)
            return;
        // Retrieve the selected item from the picker
        string selectedItem = picker.SelectedItem as string;

        if (BindingContext is ComponentSMViewModel viewModel)
            if (viewModel.UpdateComponentSNCommand.CanExecute(selectedItem))
            {
                viewModel.UpdateComponentSNCommand.Execute(selectedItem);
            }
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        if (picker.SelectedIndex == -1)
            return;

        var selectedItem = picker.SelectedItem as ComponentModel;
        if (selectedItem != null)
        {
            if (!selectedItem.IsFreeForPicker)
            {
                DisplayAlert("Selection Unavailable", "The selected component is not available for selection.", "OK");

                picker.SelectedItem = null;
            }
        }
    }

    private void On_Assemble_Component_Clicked(object sender, EventArgs e)
    {
        var missingFields = new List<string>();
        if (BindingContext is ComponentSMViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ComponentType))
                missingFields.Add("Component Type");

            if (viewModel.PickerSelectedComponent == null)
                missingFields.Add("Serial Number");

            if (string.IsNullOrEmpty(viewModel.ValidUavHours))
                missingFields.Add("Valid UAV Hours");

            if (string.IsNullOrEmpty(viewModel.AssemblyTechnician))
                missingFields.Add("Technician");

            if (string.IsNullOrEmpty(viewModel.AssemblyChiefTechnician))
                missingFields.Add("Chief Technician");

            if (missingFields.Count != 0)
            {
                string message = "Please fill in the following fields: " + string.Join(", ", missingFields);
                DisplayAlert("Missing Information", message, "OK");
            }
            else
            {
                // Assuming you have a command in your ViewModel to handle the assembly
                if (viewModel.AssembleComponentCommand.CanExecute(null))
                {
                    viewModel.AssembleComponentCommand.Execute(null);
                }
            }
        }
    }
}