using Microsoft.EntityFrameworkCore;
using TodoApp.DataAccess;
using TodoApp.DataAccess.Entities;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services.Implementations;

public class CategoryService:ICategoryService
{
    private readonly AppDbContext _dbcontext;
    public CategoryService(AppDbContext dbContext)
    {
        _dbcontext = dbContext;
    }

    public async Task<Category> AddAsync(string name, Guid userId)
    {
        var newCategory = new Category{Name = name, UserId = userId};
       
        _dbcontext.Categories.Add(newCategory);
        await _dbcontext.SaveChangesAsync();

        return newCategory;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var category = await _dbcontext.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        
        if(category is null)
            return false;

        _dbcontext.Categories.Remove(category);
        await _dbcontext.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Category>> GetAllAsync(Guid userId)
    {
        return await _dbcontext.Categories.Where(c => c.UserId == userId).ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _dbcontext.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }

    public async Task<bool> UpdateAsync(Guid id, string name, Guid userId)
    {
        var category = await _dbcontext.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        
        if (category is null)
            return false;

        category.Name = name;

        _dbcontext.Categories.Update(category);
        await _dbcontext.SaveChangesAsync();
        
        return true;
    }
}