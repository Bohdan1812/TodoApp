using Microsoft.AspNetCore.Identity;
using TodoApp.DataAccess.Entities;
using TodoApp.Services.DTOs;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDto?> GetInfo(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if(user is null)
            return null;

        return new UserDto(id, user.Email, user.DisplayName);
    }

    public async Task<bool> RemoveAsync(Guid id, string password)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
            return false;
        
        var passCheck = await _userManager.CheckPasswordAsync(user, password);
        
        if (!passCheck)
            return false;

        var result = await  _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
            return false;
        
        user.DisplayName = updateUserDto.DisplayName;
        
        if (!string.Equals(user.Email, updateUserDto.Email, StringComparison.OrdinalIgnoreCase))
        {
            // Оновлюємо Email та його нормалізовану версію
            user.Email = updateUserDto.Email;
            user.NormalizedEmail = _userManager.NormalizeEmail(updateUserDto.Email);

            // Оскільки стандартний /login прив'язаний до UserName, 
            // синхронізуємо UserName із новим Email
            user.UserName = updateUserDto.Email;
            user.NormalizedUserName = _userManager.NormalizeName(updateUserDto.Email);

            // Якщо у майбутньому увімкнеш верифікацію — скидаємо підтвердження
            user.EmailConfirmed = false; 
        }

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}