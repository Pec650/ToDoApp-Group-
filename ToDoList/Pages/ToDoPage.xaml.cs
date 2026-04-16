using System.Diagnostics;

namespace ToDoList.Pages;

public partial class ToDoPage : ContentPage
{
    public ToDoPage()
    {
        InitializeComponent();
        todoCV.ItemsSource = ToDoService.Items;
        Debug.WriteLine($"ToDoPage ctor loggedIn={App.isLoggedIn()} userId={App.CurrentUser.id} items={ToDoService.Items.Count}");

        if (App.isLoggedIn())
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _ = ToDoService.FetchTasksAsync(App.CurrentUser.id, "active");
            });
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Debug.WriteLine($"ToDoPage.OnAppearing loggedIn={App.isLoggedIn()} userId={App.CurrentUser.id}");
        if (App.isLoggedIn())
        {
            int userId = App.CurrentUser.id; // Use .id, not .item_id
            await ToDoService.FetchTasksAsync(userId, "active");
            Debug.WriteLine($"ToDoPage.OnAppearing after fetch items={ToDoService.Items.Count}");
        }
    }

    private async void GoToNewTask(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(NewTask));

    private void CompleteToDoItem(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = ToDoService.Items.FirstOrDefault(x => x.item_id.ToString() == button.ClassId);
        if (item != null) ToDoService.Complete(item);
    }
}