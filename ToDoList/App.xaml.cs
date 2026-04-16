namespace ToDoList;

using ToDoList.Pages; // This now works because User is in this namespace

public partial class App : Application
{
    // Static instance so it's globally accessible
    public static User CurrentUser { get; } = new User();
    
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    public static void resetUser()
    {
        CurrentUser.emptyUser(); // Uses your method from User.cs
    }

    public static bool isLoggedIn()
    {
        return CurrentUser.userExists(); // Uses your method from User.cs
    }
}