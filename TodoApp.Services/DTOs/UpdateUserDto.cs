namespace TodoApp.Services.DTOs;

public record UpdateUserDto(
    string Email,
    string DisplayName
);