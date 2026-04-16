using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace ToDoList.Pages;

public static class ToDoService
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private const string BaseUrl = "https://todo-list.dcism.org";

    public static ObservableCollection<ToDoClass> Items { get; } = new();
    public static ObservableCollection<CompletedTask> CompletedItems { get; } = new();

    public static async Task FetchTasksAsync(int userId, string statusType = "active")
    {
        try
        {
            var tasks = await GetTasksFromApi(userId, statusType);
            MainThread.BeginInvokeOnMainThread(() => {
                Items.Clear();
                foreach (var task in tasks)
                {
                    Items.Add(task);
                }
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("API Error", "Could not fetch tasks: " + ex.Message, "OK");
        }
    }

    public static async Task FetchCompletedTasksAsync(int userId, string statusType = "inactive")
    {
        try
        {
            var tasks = await GetTasksFromApi(userId, statusType);
            MainThread.BeginInvokeOnMainThread(() => {
                CompletedItems.Clear();
                foreach (var task in tasks)
                {
                    CompletedItems.Add(new CompletedTask { Item = task, CompletedAt = DateTime.Now });
                }
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("API Error", "Could not fetch completed tasks: " + ex.Message, "OK");
        }
    }

    private static async Task<List<ToDoClass>> GetTasksFromApi(int userId, string statusType)
    {
        var url = $"{BaseUrl}/getItems_action.php?status={Uri.EscapeDataString(statusType)}&user_id={userId}";
        Debug.WriteLine($"ToDoService.GetTasksFromApi url={url}");
        var response = await _httpClient.GetAsync(url);
        Debug.WriteLine($"ToDoService.GetTasksFromApi status={response.StatusCode}");

        if (!response.IsSuccessStatusCode)
        {
            Debug.WriteLine($"ToDoService.GetTasksFromApi failed: {response.ReasonPhrase}");
            return new List<ToDoClass>();
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        Debug.WriteLine($"ToDoService.GetTasksFromApi body={jsonString}");
        var tasks = ParseTasks(jsonString);
        Debug.WriteLine($"ToDoService.GetTasksFromApi parsed={tasks.Count}");
        return tasks;
    }

    private static List<ToDoClass> ParseTasks(string jsonString)
    {
        var tasks = new List<ToDoClass>();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        using var jsonDoc = JsonDocument.Parse(jsonString);
        if (!jsonDoc.RootElement.TryGetProperty("data", out var dataElement))
        {
            return tasks;
        }

        if (dataElement.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in dataElement.EnumerateObject())
            {
                var task = JsonSerializer.Deserialize<ToDoClass>(property.Value.GetRawText(), options);
                if (task != null) tasks.Add(task);
            }
        }
        else if (dataElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var element in dataElement.EnumerateArray())
            {
                var task = JsonSerializer.Deserialize<ToDoClass>(element.GetRawText(), options);
                if (task != null) tasks.Add(task);
            }
        }

        return tasks;
    }

    public static void Add(string name, string description) 
    {
        // Local add for immediate UI feedback, usually followed by an API Post
        Items.Add(new ToDoClass { item_name = name, item_description = description });
    }

    public static void Update(ToDoClass item, string name, string description)
    {
        item.item_name = name;
        item.item_description = description;
    }

    public static void Complete(ToDoClass item)
    {
        Items.Remove(item);
        CompletedItems.Add(new CompletedTask { Item = item, CompletedAt = DateTime.Now });
    }

    public static void PurgeExpired()
    {
        var expired = CompletedItems.Where(x => (DateTime.Now - x.CompletedAt).TotalDays >= 7).ToList();
        foreach (var x in expired) CompletedItems.Remove(x);
    }
}