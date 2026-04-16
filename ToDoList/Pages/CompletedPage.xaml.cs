using System.Diagnostics;

namespace ToDoList.Pages;

public partial class CompletedPage : ContentPage
{
    public CompletedPage()
    {
        InitializeComponent();
        completedCV.ItemsSource = ToDoService.CompletedItems;
        Debug.WriteLine($"CompletedPage ctor loggedIn={App.isLoggedIn()} userId={App.CurrentUser.id} completedCount={ToDoService.CompletedItems.Count}");

        if (App.isLoggedIn())
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _ = ToDoService.FetchCompletedTasksAsync(App.CurrentUser.id, "inactive");
            });
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        Debug.WriteLine($"CompletedPage.OnAppearing loggedIn={App.isLoggedIn()} userId={App.CurrentUser.id}");
        int userId = App.CurrentUser.id;
        if (userId > 0)
        {
            await ToDoService.FetchCompletedTasksAsync(userId, "inactive");
            Debug.WriteLine($"CompletedPage.OnAppearing after fetch completedCount={ToDoService.CompletedItems.Count}");
            ToDoService.PurgeExpired();
        }
    }

    private void UndoComplete(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var completed = ToDoService.CompletedItems.FirstOrDefault(x => x.Item.item_id.ToString() == button.ClassId);
        if (completed != null)
        {
            ToDoService.CompletedItems.Remove(completed);
            completed.Item.status = "active";
            ToDoService.Items.Add(completed.Item);
        }
    }

    private void DeleteComplete(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var completed = ToDoService.CompletedItems.FirstOrDefault(x => x.Item.item_id.ToString() == button.ClassId);
        if (completed != null) ToDoService.CompletedItems.Remove(completed);
    }


    private async void DeleteAll(object sender, EventArgs e)
    {
        if (ToDoService.CompletedItems.Count == 0) return;
        bool confirm = await DisplayAlertAsync("Delete All", "Confirm deletion?", "Delete", "Cancel");
        if (confirm) ToDoService.CompletedItems.Clear();
    }
}