namespace ToDoList.Pages; // Change this from MauiApp1 to match your project

public class User
{
    // The API uses 'id', 'fname', 'lname', 'email'
    public int id { get; set; }
    public string fname { get; set; } = string.Empty;
    public string lname { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;

    public User()
    {
        emptyUser();
    }

    public void emptyUser()
    {
        id = -1;
        fname = string.Empty;
        lname = string.Empty;
        email = string.Empty;
    }

    public bool userExists()
    {
        return (id != -1);
    }

    public void setCurrentUser(int id, string fname, string lname, string email)
    {
        this.id = id;
        this.fname = fname;
        this.lname = lname;
        this.email = email;
    }
}