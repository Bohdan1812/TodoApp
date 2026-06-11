using TodoApp.Services.DTOs;

namespace TodoApp.Services.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TodoTaskDto>> GetAllAsync(Guid userId);
    Task<TodoTaskDto?> GetByIdAsync(Guid id, Guid userId);
    Task<TodoTaskDto> CreateAsync(CreateTodoTaskDto dto, Guid userId);
    Task<bool> UpdateAsync(Guid id, CreateTodoTaskDto dto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
    Task<bool> ToggleCompleteAsync(Guid id, Guid userId);
}