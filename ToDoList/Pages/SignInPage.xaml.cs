namespace ToDoList.Pages;

using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class SignInPage
{
    public class UserProfile
    {
        public int id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public string timemodified { get; set; }
    }
    
    public class SignInResponse
    {
        public int status { get; set; }
        public UserProfile data { get; set; }
        public string message { get; set; }
    }
    
    public SignInPage()
    {
        InitializeComponent();
    }

    private async void SubmitSignIn(object? sender, EventArgs e)
    {
        if (IsEmptyInput(EmailInput.Text) || IsEmptyInput(PasswordInput.Text))
        {
            ShowError("Please enter a username and password");
            return;
        }

        Color SubmitBtnBGColor = SubmitBtn.BackgroundColor;
        Color SignUpBtnBGColor = SignUpBtn.BackgroundColor;
        
        SubmitBtn.IsEnabled = false;
        SubmitBtn.BackgroundColor = Colors.Grey;
        SignUpBtn.IsEnabled = false;
        SignUpBtn.BackgroundColor = Colors.Grey;
        LoadingIndicator.IsRunning = true;
        RemoveError();

        bool success = await AttemptSignIn(EmailInput.Text, PasswordInput.Text);

        if (success)
        {
            await GoToMain();
        }
        else
        {
            SubmitBtn.IsEnabled = true;
            SubmitBtn.BackgroundColor = SubmitBtnBGColor;
            SignUpBtn.IsEnabled = true;
            SignUpBtn.BackgroundColor = SignUpBtnBGColor;
            LoadingIndicator.IsRunning = false;
            ShowError("Incorrect username or password");
        }
    }

    private async Task<bool> AttemptSignIn(string email, string password)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string cleanEmail = Uri.EscapeDataString(email);
                string cleanPassword = Uri.EscapeDataString(password);
                
                var response = await client.GetAsync($"https://todo-list.dcism.org/signin_action.php?email={cleanEmail}&password={cleanPassword}");

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var root = JsonConvert.DeserializeObject<SignInResponse>(jsonString);

                    if (root != null && root.status == 200)
                    {
                        return true; 
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e.Message}");
            }
            return false;
        }
    }
    
    private async Task GoToMain()
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

    private void ShowPassword(object? sender, CheckedChangedEventArgs e)
    {
        PasswordInput.IsPassword = !ShowPass.IsChecked;
    }
}