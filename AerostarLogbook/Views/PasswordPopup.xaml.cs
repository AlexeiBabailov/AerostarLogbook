namespace AerostarLogbook.Views;

public partial class PasswordPopup : ContentPage
{
    public PasswordPopup(Action<string> onPasswordEntered)
    {
        InitializeComponent();
        btnSubmit.Clicked += (s, e) => OnPasswordEntered(txtPassword.Text);

    }

    private async void OnPasswordEntered(string password)
    {
        const string correctPassword = "a"; // Set your password here
        if (password == correctPassword)
        {
            // Pop the password pop-up
            await Navigation.PopModalAsync();

            // Navigate to the protected page
            await Shell.Current.GoToAsync($"{nameof(AdministratorView)}");
        }
        else
        {
            // Show an error message or handle incorrect password
            await DisplayAlert("Error", "Incorrect password", "OK");
            txtPassword.Text = string.Empty;
        }
    }

    private async void BtnBack_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}