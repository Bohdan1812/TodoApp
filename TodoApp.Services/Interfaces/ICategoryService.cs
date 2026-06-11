using TodoApp.DataAccess.Entities;

namespace TodoApp.Services.Interfaces;

public interface ICategoryService
{
    Task <IEnumerable<Category>> GetAllAsync(Guid userId);
    Task<Category?> GetByIdAsync(Guid id, Guid userId);
    Task<Category> AddAsync(string name, Guid userId);
    Task<bool> UpdateAsync(Guid id, string name, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}