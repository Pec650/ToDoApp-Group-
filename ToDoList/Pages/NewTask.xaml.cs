namespace ToDoList.Pages;

public partial class NewTask : ContentPage
{
    public NewTask()
    {
        InitializeComponent();
    }

    private async void AddTask(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(titleEntry.Text)) return;

        ToDoService.Add(titleEntry.Text, detailsEditor.Text ?? string.Empty);

        titleEntry.Text = string.Empty;
        detailsEditor.Text = string.Empty;

        await Shell.Current.GoToAsync("..");
    }

    private async void CancelNewTask(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}