using AerostarLogbook.ViewModels;
using AerostarLogbook.Views.ViewServices;

namespace AerostarLogbook.Views;

[QueryProperty(nameof(UavNum), "UavNum")]

public partial class Frequencies : ContentPage
{
    private string selctedUAV;
    private FormCRUDButtonService _formCbandButtons;
    private FormCRUDButtonService _formUhfButtons;


    public string UavNum
    {
        get => selctedUAV;
        set
        {
            selctedUAV = value;
        }
    }

    public Frequencies(FrequenciesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        _formCbandButtons = new FormCRUDButtonService();
        _formUhfButtons = new FormCRUDButtonService();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is FrequenciesViewModel viewModel)
        {
            if (viewModel.RefreshDataCommand.CanExecute(selctedUAV))
            {
                viewModel.RefreshDataCommand.Execute(selctedUAV);
            }
        }
        _formCbandButtons.RegisterFormButtons(CbandAddButton, CbandUpdateutton, CbandDeleteButton);
        _formUhfButtons.RegisterFormButtons(UhfAddButton, UhfUpdateButton, UhfDeleteButton);

    }

    private void FormCband_Button_Clicked(object sender, EventArgs e)
    {
        CbandList.SelectedItem = null;
        _formCbandButtons.Form_LocalAddIsVisible();
    }

    private void FormUhf_Button_Clicked(object sender, EventArgs e)
    {
        UhfList.SelectedItem = null;
        _formUhfButtons.Form_LocalAddIsVisible();
    }
    private void CbandList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _formCbandButtons.Form_LocalAllIsVisible();
        if (BindingContext is FrequenciesViewModel viewModel)
        {
            if (viewModel.UpdateCbandFrequenciesFormCommand.CanExecute(null))
            {
                viewModel.UpdateCbandFrequenciesFormCommand.Execute(null);
            }
        }

    }

    private void UhfList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _formUhfButtons.Form_LocalAllIsVisible();
        if (BindingContext is FrequenciesViewModel viewModel)
        {
            if (viewModel.UpdateUhfFrequenciesFormCommand.CanExecute(null))
            {
                viewModel.UpdateUhfFrequenciesFormCommand.Execute(null);
            }
        }
    }


}