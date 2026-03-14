using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private void LogOut(object? sender, EventArgs e)
    {
        Application.Current.MainPage = new SignInPage();
    }
}