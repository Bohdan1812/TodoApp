namespace TodoApp.DataAccess.Entities;

public class TodoTask
{
    public Guid Id {get; set;}
    public string Title {get; set;} = string.Empty;
    public string? Description {get; set;}
    public bool IsCompleted {get; set;} = false;
    public DateTime? DueDate {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
    public Guid UserId {get;set;}
    public User? User{get;set;}
    public Guid? CategoryId {get;set;}
    public Category? Category {get;set;}

}