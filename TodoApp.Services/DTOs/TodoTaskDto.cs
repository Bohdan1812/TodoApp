namespace TodoApp.Services.DTOs;

public record TodoTaskDto(
    Guid Id,
    string Title,
    string? Description,
    bool IsCompleted,
    DateTime? DueDate,
    DateTime CreatedAt,
    Guid? CategoryId
);