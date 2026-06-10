namespace TodoApp.DataAccess.Entities;

public class User
{   public Guid Id {get; set;}
    public string Username {get; set;} = string.Empty;
    public ICollection<TodoTask> Tasks {get;set;} = new List<TodoTask>();
    public ICollection<Category> Categories {get;set;} = new List<Category>();

}