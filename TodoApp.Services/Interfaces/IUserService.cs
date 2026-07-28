using TodoApp.Services.DTOs;

namespace TodoApp.Services.Interfaces;

public interface IUserService
{
   Task<UserDto?> GetInfo(Guid id);
   Task<bool> RemoveAsync(Guid id, string password);
   Task<bool> UpdateAsync(Guid id, UpdateUserDto updateUserDto);

}