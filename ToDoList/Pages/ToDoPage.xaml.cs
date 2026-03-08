namespace ToDoList.Pages;

using System.Collections.ObjectModel;
using ToDoClass;

public partial class ToDoPage : ContentPage
{
    public ObservableCollection<ToDoClass> ToDoItems { get; set; }
    
    private ToDoClass selectedItem;

    public ToDoPage()
    {
        InitializeComponent();
        ToDoItems = new ObservableCollection<ToDoClass>();
        todoLV.ItemsSource = ToDoItems;
        updateTitle();
    }

    private void AddToDoItem(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(titleEntry.Text)) return;

        var newItem = new ToDoClass
        {
            id = ToDoItems.Count + 1,
            title = titleEntry.Text,
            detail = detailsEditor.Text
        };

        ToDoItems.Add(newItem);
        ClearForm();
        updateTitle();
    }

    private void DeleteToDoItem(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var itemToDelete = ToDoItems.FirstOrDefault(x => x.id.ToString() == button.ClassId);

        if (itemToDelete != null)
        {
            if (selectedItem == itemToDelete)
            {
                CancelEdit(sender, e);
            }
            
            ToDoItems.Remove(itemToDelete);
            updateTitle();
        }
    }

    private void EditToDoItem(object sender, EventArgs e)
    {
        if (selectedItem != null)
        {
            selectedItem.title = titleEntry.Text;
            selectedItem.detail = detailsEditor.Text;

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
    }

    private void TodoLV_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;

        selectedItem = (ToDoClass)e.SelectedItem;

        titleEntry.Text = selectedItem.title;
        detailsEditor.Text = selectedItem.detail;

        addBtn.IsVisible = false;
        editBtn.IsVisible = true;
        cancelBtn.IsVisible = true;
    }

    private void todoLV_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        ((ListView)sender).SelectedItem = null;
    }

    private void updateTitle()
    {
        if (ToDoItems.Count == 0)
        {
            listTitle.Text = "To-Do List is Empty...";
        }
        else
        {
            listTitle.Text = "To-Do List";
        }
    }

    private void ClearForm()
    {
        titleEntry.Text = string.Empty;
        detailsEditor.Text = string.Empty;
        titleEntry.Unfocus();
        detailsEditor.Unfocus();
    }
}