using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;

namespace AerostarLogbook.Views;

[QueryProperty(nameof(UavNum), "UavNum")]

public partial class HardwareConfiguration : ContentPage
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

    public HardwareConfiguration(HardwareConfigurationViewModel vm)
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
        if (BindingContext is HardwareConfigurationViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _buttonService.RegisterButton(GeneralButton);
        _buttonService.RegisterButton(AssemblyButton);
        _buttonService.RegisterButton(DisassemblyButton);
        _formButtons.RegisterFormButtons(AddHardwareConf, UpdateHardwareConf, DeleteHardwareConf);
        _gridService.RegisterGrid("AssemblyForm", AssemblyForm);
        _gridService.RegisterGrid("DisassemblyForm", DisassemblyForm);

    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        HardwareConf_ListView.SelectedItem = null;
        _gridService.HideAll();
        _formButtons.Form_LocalNoneIsVisible();
        _formButtons.UnregisterFromButtons();
        _buttonService.UnregisterButton(GeneralButton);
        _buttonService.UnregisterButton(AssemblyButton);
        _buttonService.UnregisterButton(DisassemblyButton);
        _gridService.UnregisterGrid("AssemblyForm");
        _gridService.UnregisterGrid("DisassemblyForm");

    }

    private void HWGeneralClicked(object sender, EventArgs e)
    {
        HardwareConf_ListView.SelectedItem = null;
        var button = sender as Button;
        if (button != null)
        {
            _gridService.HideAll();
        }
        _formButtons.Form_LocalNoneIsVisible();
    }

    private void OpenHWClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("AssemblyForm");
        }
        _formButtons.Form_LocalAddIsVisible();
    }

    private void CloseHWClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _gridService.ShowOnly("DisassemblyForm");
        }
    }

    private void HardwareConf_ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _buttonService.NoneButtonClicked();
        _formButtons.Form_LocalUpdateDeleteIsVisible();
        HardwareConf_ListView.Focus();
        if (BindingContext is HardwareConfigurationViewModel viewModel)
            if (viewModel.UpdateHWConfFormCommand.CanExecute(null))
            {
                viewModel.UpdateHWConfFormCommand.Execute(null);
            }

    }
}