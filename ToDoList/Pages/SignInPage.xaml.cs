namespace ToDoList.Pages;

public partial class SignInPage
{
    public SignInPage()
    {
        InitializeComponent();
    }

    private void SubmitSignIn(object? sender, EventArgs e)
    {
        if (IsEmptyInput(UsernameInput.Text) && IsEmptyInput(PasswordInput.Text))
        {
            ShowError("Please enter a username and password");
            return;
        }

        if (IsEmptyInput(UsernameInput.Text))
        {
            ShowError("Please enter a username");
            return;
        }

        if (IsEmptyInput(PasswordInput.Text))
        {
            ShowError("Please enter a password");
            return;
        }
        
        RemoveError();
        GoToMain();
        return;
        
        ShowError("Incorrect username or password");
        return;
    }
    
    private async void GoToMain()
    {
        await Shell.Current.GoToAsync("//main");
    }

    private void GoToSignUp(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new SignUpPage());
    }

    private bool IsEmptyInput(String input)
    {
        return String.IsNullOrWhiteSpace(input);
    }

    private void ShowError(String errorMsg)
    {
        InputError.Text = errorMsg;
        InputError.IsVisible = true;
    }

    private void RemoveError()
    {
        InputError.IsVisible = false;
    }
}