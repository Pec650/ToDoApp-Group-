namespace ToDoList.Pages;

using System.Linq;

public partial class CompletedPage : ContentPage
{
    public CompletedPage()
    {
        InitializeComponent();
        completedCV.ItemsSource = ToDoService.CompletedItems;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ToDoService.PurgeExpired();
    }

    private void UndoComplete(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var completed = ToDoService.CompletedItems
            .FirstOrDefault(x => x.Item.item_id.ToString() == button.ClassId);
        if (completed == null) return;

        ToDoService.Items.Add(completed.Item);
        ToDoService.CompletedItems.Remove(completed);
    }

    private void DeleteComplete(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var completed = ToDoService.CompletedItems
            .FirstOrDefault(x => x.Item.item_id.ToString() == button.ClassId);
        if (completed != null)
            ToDoService.CompletedItems.Remove(completed);
    }

    private async void DeleteAll(object sender, EventArgs e)
    {
        if (ToDoService.CompletedItems.Count == 0) return;

        bool confirm = await DisplayAlert(
            "Delete All",
            "Are you sure you want to delete all completed tasks?",
            "Delete", "Cancel");

        if (confirm)
            ToDoService.CompletedItems.Clear();
    }
}