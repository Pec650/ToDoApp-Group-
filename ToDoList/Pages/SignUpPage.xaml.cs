namespace ToDoList.Pages;

using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class SignUpPage
{
    public class SignupResponse
    {
        public int status { get; set; }
        public string message { get; set; }
    }
    
    public SignUpPage()
    {
        InitializeComponent();
    }

    private async void SubmitSignUp(object? sender, EventArgs e)
    {
        if (IsEmptyInput(FNameInput.Text) || IsEmptyInput(LNameInput.Text) || IsEmptyInput(EmailInput.Text) ||
            IsEmptyInput(PasswordInput.Text) || IsEmptyInput(ConfirmPassInput.Text))
        {
            ShowError("Please input all fields");
            return;
        }

        if (!IsValidEmail(EmailInput.Text))
        {
            ShowError("Invalid email address");
            return;
        }

        if (!IsValidPassword(PasswordInput.Text))
        {
            return;
        }

        if (!String.Equals(PasswordInput.Text, ConfirmPassInput.Text))
        {
            ShowError("Passwords do not match");
            return;
        }
        
        Color SubmitBtnBGColor = SubmitBtn.BackgroundColor;
        Color SignInBtnBGColor = SignInBtn.BackgroundColor;
        
        SubmitBtn.IsEnabled = false;
        SubmitBtn.BackgroundColor = Colors.Grey;
        SignInBtn.IsEnabled = false;
        SignInBtn.BackgroundColor = Colors.Grey;
        LoadingIndicator.IsRunning = true;
        RemoveError();
        
        bool success = await AttemptSignUp(FNameInput.Text, LNameInput.Text, EmailInput.Text, PasswordInput.Text, ConfirmPassInput.Text);

        if (success)
        {
            await GoToMain();
        }
        
        SubmitBtn.IsEnabled = true;
        SubmitBtn.BackgroundColor = SubmitBtnBGColor;
        SignInBtn.IsEnabled = true;
        SignInBtn.BackgroundColor = SignInBtnBGColor;
        LoadingIndicator.IsRunning = false;
    }

    private async Task<bool> AttemptSignUp(string fname, string lname, string email, string password, string confirmPass)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string cleanFName = Uri.EscapeDataString(fname);
                string cleanLName = Uri.EscapeDataString(lname);
                string cleanEmail = Uri.EscapeDataString(email);
                string cleanPassword = Uri.EscapeDataString(password);
                
                var signupData = new Dictionary<string, string>
                {
                    { "first_name", cleanFName },
                    { "last_name", cleanLName },
                    { "email", cleanEmail },
                    { "password", cleanPassword },
                    { "confirm_password", cleanPassword }
                };

                var content = new FormUrlEncodedContent(signupData);

                var response = await client.PostAsync("https://todo-list.dcism.org/signup_action.php", content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<SignupResponse>(jsonResponse);

                    if (result != null && result.status == 200)
                    {
                        return true;
                    }
                    else
                    {
                        ShowError(result?.message ?? "Signup failed");
                    }
                }
                else
                {
                    ShowError("Server communication error.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                ShowError("Please check your internet connection.");
            }

            return false;
        }
    }

    private async Task GoToMain()
    {
        await Shell.Current.GoToAsync("//main");
    }

    private void GoToSignIn(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new SignInPage());
    }
    
    private bool IsEmptyInput(String input)
    {
        return String.IsNullOrWhiteSpace(input);
    }

    private bool IsValidEmail(String email)
    {
        return Regex.IsMatch(email,
            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
            RegexOptions.IgnoreCase);
    }

    private bool IsValidPassword(String password)
    {
        if (password.Length < 8 || password.Length > 16)
        {
            ShowError("Password must be 8-16 letters");
            return false;
        }

        if (!password.Any(char.IsLetterOrDigit))
        {
            ShowError("Password must have a digit [0-9]");
            return false;
        }

        if (!password.Any(char.IsLower))
        {
            ShowError("Password must have a lowercase [a-z]");
            return false;
        }
        
        if (!password.Any(char.IsUpper))
        {
            ShowError("Password must have a uppercase [A-Z]");
            return false;
        }

        return true;
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

    private void ShowPassword(object? sender, CheckedChangedEventArgs e)
    {
        PasswordInput.IsPassword = !ShowPass.IsChecked;
    }

    private void ShowConfirmPassword(object? sender, CheckedChangedEventArgs e)
    {
        ConfirmPassInput.IsPassword = !ShowConfirmPass.IsChecked;
    }
}