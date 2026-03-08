namespace ToDoClass;

public class User
{
    private int _id { get; set; }
    private String _username { get; set; }
    private String _email { get; set; }

    public int id
    {
        get { return _id; }
        set { _id = value; }
    }
    
    public String username
    {
        get { return _username; }
        set { _username = value; }
    }

    public String email
    {
        get { return _email; }
        set { _email = value; }
    }
}