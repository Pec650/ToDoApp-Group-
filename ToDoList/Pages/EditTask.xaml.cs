namespace ToDoList.Pages;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class EditTask : ContentPage
{
    private ToDoClass? _item;

    public string ItemId
    {
        set
        {
            _item = ToDoService.Items.FirstOrDefault(x => x.item_id.ToString() == value);
            if (_item != null)
            {
                titleEntry.Text = _item.item_name;
                detailsEditor.Text = _item.item_description;
            }
        }
    }

    public EditTask()
    {
        InitializeComponent();
    }

    private async void SaveEdit(object sender, EventArgs e)
    {
        if (_item == null || string.IsNullOrWhiteSpace(titleEntry.Text)) return;

        ToDoService.Update(_item, titleEntry.Text, detailsEditor.Text ?? string.Empty);

        await Shell.Current.GoToAsync("..");
    }

    private async void CancelEdit(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}