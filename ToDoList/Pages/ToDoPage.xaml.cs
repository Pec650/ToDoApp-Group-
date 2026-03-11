namespace ToDoList.Pages;

using System.Collections.ObjectModel;
using System.Linq;
using MauiApp1;

public partial class ToDoPage : ContentPage
{
    public ObservableCollection<ToDoClass> ToDoItems { get; set; }

    private static int _nextId = 1;
    private ToDoClass? selectedItem;  // ← nullable to fix CS8618

    public ToDoPage()
    {
        InitializeComponent();
        ToDoItems = new ObservableCollection<ToDoClass>();
        todoCV.ItemsSource = ToDoItems;
        updateTitle();
    }

    private void EditListTitle(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(listTitle.Text)) return;
        listTitle.Text = titleEntry.Text;
    }

    private void AddToDoItem(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(titleEntry.Text)) return;

        var newItem = new ToDoClass
        {
            item_id = _nextId++,
            item_name = titleEntry.Text,
            item_description = detailsEditor.Text ?? string.Empty,
            status = string.Empty,
            user_id = 0
        };

        ToDoItems.Add(newItem);
        ClearForm();
        updateTitle();
    }

    private void DeleteToDoItem(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var itemToDelete = ToDoItems.FirstOrDefault(x => x.item_id.ToString() == button.ClassId);

        if (itemToDelete != null)
        {
            if (selectedItem == itemToDelete)
                CancelEdit(sender, e);

            ToDoItems.Remove(itemToDelete);
            updateTitle();
        }
    }

    private void EditToDoItem(object sender, EventArgs e)
    {
        if (selectedItem != null)
        {
            selectedItem.item_name = titleEntry.Text ?? string.Empty;
            selectedItem.item_description = detailsEditor.Text ?? string.Empty;
            CancelEdit(sender, e);
        }
    }

    private void CancelEdit(object sender, EventArgs e)
    {
        selectedItem = null;
        ClearForm();

        addBtn.IsVisible = true;
        editBtn.IsVisible = false;
        cancelBtn.IsVisible = false;

        todoCV.SelectedItem = null;  // ← fixes CS8625 (no null literal on non-nullable)
    }

    private void TodoCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not ToDoClass item) return;

        selectedItem = item;
        titleEntry.Text = selectedItem.item_name;
        detailsEditor.Text = selectedItem.item_description;

        addBtn.IsVisible = false;
        editBtn.IsVisible = true;
        cancelBtn.IsVisible = true;
    }

    private void updateTitle()
    {
        listTitle.Text = ToDoItems.Count == 0 ? "To-Do List is Empty..." : "To-Do List";
    }

    private void ClearForm()
    {
        titleEntry.Text = string.Empty;
        detailsEditor.Text = string.Empty;
        titleEntry.Unfocus();
        detailsEditor.Unfocus();
    }
}