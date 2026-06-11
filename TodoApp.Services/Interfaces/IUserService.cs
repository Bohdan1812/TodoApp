using TodoApp.DataAccess.Entities;

namespace TodoApp.Services.Interfaces;

public interface IUserService
{
   Task<User> CreateAsync (string userName);
   Task<bool> UpdateAsync (Guid userId, string userName);
   Task<IEnumerable<User>> GetAllAsync();
   Task<User?> GetByIdAsync(Guid userId);
   Task<bool> DeleteAsync(Guid userId);
    
}