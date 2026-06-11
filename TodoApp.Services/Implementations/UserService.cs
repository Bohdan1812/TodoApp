using Microsoft.EntityFrameworkCore;
using TodoApp.DataAccess;
using TodoApp.DataAccess.Entities;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services.Implementations;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }
    public async Task<User> CreateAsync(string userName)
    {
        var user = new User
        {
            Username = userName
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<bool> DeleteAsync(Guid userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return false;
        
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<bool> UpdateAsync(Guid userId, string userName)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user is null)
            return false;
        
        user.Username = userName;
        
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
}