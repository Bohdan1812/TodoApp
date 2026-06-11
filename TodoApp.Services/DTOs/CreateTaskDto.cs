namespace TodoApp.Services.DTOs;

public record CreateTodoTaskDto(
    string Title, 
    string? Description,
    DateTime? DueDate,
    Guid? CategoryId
);
