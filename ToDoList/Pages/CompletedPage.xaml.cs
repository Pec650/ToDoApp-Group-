namespace ToDoList.Pages;

using System.Collections.ObjectModel;
using System.Linq;
using MauiApp1;

public partial class CompletedPage : ContentPage
{
    public ObservableCollection<ToDoClass> ToDoItems { get; set; }

    private static int _nextId = 1;
    private ToDoClass? selectedItem;  // ← nullable to fix CS8618

    public CompletedPage()
    {
        InitializeComponent();
        ToDoItems = new ObservableCollection<ToDoClass>();
    }
}