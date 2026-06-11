using Microsoft.EntityFrameworkCore;
using TodoApp.DataAccess;
using TodoApp.DataAccess.Entities;
using TodoApp.Services.DTOs;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services.Implementations;

public class TaskService:ITaskService
{
    private readonly AppDbContext _context;
    public TaskService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<TodoTaskDto>> GetAllAsync(Guid userId)
    {
        return await _context.Tasks.Where(t => t.UserId == userId)
        .Select(t => new TodoTaskDto(
            t.Id,
            t.Title,
            t.Description,
            t.IsCompleted,
            t.DueDate,
            t.CreatedAt,
            t.CategoryId)
        )
        .ToListAsync();
    }

    public async Task<TodoTaskDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task is null) return null;

        return new TodoTaskDto(
            task.Id,
            task.Title,
            task.Description,
            task.IsCompleted,
            task.DueDate,
            task.CreatedAt,
            task.CategoryId
        );
    }

    public async Task<TodoTaskDto> CreateAsync(CreateTodoTaskDto dto, Guid userId)
    {
        var newTask = new TodoTask
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            CategoryId = dto.CategoryId,
            UserId = userId
        };

        _context.Tasks.Add(newTask);

        await _context.SaveChangesAsync();
        return new TodoTaskDto
        (
            newTask.Id,
            newTask.Title,
            newTask.Description,
            newTask.IsCompleted,
            newTask.DueDate,
            newTask.CreatedAt,
            newTask.CategoryId
        );
    }

    public async Task<bool> UpdateAsync(Guid id, CreateTodoTaskDto dto, Guid userId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task is null)
            return false;
        
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;
        task.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task is null)
            return false;

        _context.Tasks.Remove(task);
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleCompleteAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task == null) return false;

        task.IsCompleted = !task.IsCompleted;
        await _context.SaveChangesAsync();
        return true;
    }
}