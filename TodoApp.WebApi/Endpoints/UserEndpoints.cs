using Microsoft.AspNetCore.Mvc;
using TodoApp.Services.Interfaces;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").WithTags("Users");


        group.MapGet("/{id:guid}", async (Guid id, IUserService userService) =>
        {
            var user = await userService.GetByIdAsync(id);
            return Results.Ok(user);
        }).WithName("GetUserById");

        group.MapGet("/", async (IUserService userService) =>
        {
            var users = await userService.GetAllAsync();
            return Results.Ok(users);
        });

        group.MapPost("/", async (string userName, IUserService userService) =>
        {
            if (string.IsNullOrEmpty(userName))
                return Results.BadRequest(new {message = "Ім'я користувача не може бути попрожнім"});

            var newUser = await userService.CreateAsync(userName);

            return Results.CreatedAtRoute("GetUserById", new {newUser.Id}, newUser);

        });

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] string username, IUserService userService) =>
        {
            var result = await userService.UpdateAsync(id, username);
            return result ? Results.NoContent() : Results.NotFound(new {message = "Користувача не знайдено або доступ заборонено"});
        });

        group.MapDelete("/{id:guid}", async (Guid id, IUserService userService) =>
        {
            var result = await userService.DeleteAsync(id);
            return result ? Results.NoContent() : Results.NotFound(new {message = "Користувача не знайдено або доступ заборонено"});
        });

    }
}