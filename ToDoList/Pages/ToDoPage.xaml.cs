namespace ToDoList.Pages;

using System.Linq;

public partial class ToDoPage : ContentPage
{
    public ToDoPage()
    {
        InitializeComponent();
        todoCV.ItemsSource = ToDoService.Items;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateTitle();
    }

    private async void GoToNewTask(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NewTask));
    }

    private async void EditListTitle(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync(
            "Rename List", "Enter a new title:",
            initialValue: listTitle.Text,
            maxLength: 30);

        if (!string.IsNullOrWhiteSpace(result))
            listTitle.Text = result;
    }

    private void CompleteToDoItem(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = ToDoService.Items.FirstOrDefault(x => x.item_id.ToString() == button.ClassId);
        if (item != null)
            ToDoService.Complete(item);
    }

    private async void EditToDoItem(object sender, EventArgs e)
    {
        var button = (Button)sender;
        await Shell.Current.GoToAsync($"{nameof(EditTask)}?itemId={button.ClassId}");
    }

    private void TodoCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        todoCV.SelectedItem = null;
    }

    private void UpdateTitle()
    {
        listTitle.Text = ToDoService.Items.Count == 0 ? "Your To-Do List" : "Your To-Do List";
    }
}