using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Services.DTOs;
using TodoApp.Services.Interfaces;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("user").WithTags("User Profile")
        .RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, IUserService userService) =>
        {
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!Guid.TryParse(userIdString, out Guid userId))
                return Results.Unauthorized();
            
            var userInfo = await userService.GetInfo(userId);

            return Results.Ok(userInfo);
        });

        group.MapPut("/", async (ClaimsPrincipal user, [FromBody] UpdateUserDto updateDto, IUserService userService) =>
        {
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!Guid.TryParse(userIdString, out Guid userId))
                return Results.Unauthorized();

            var result = await userService.UpdateAsync(userId, updateDto);

            return result ? Results.NoContent() : Results.BadRequest();//Потрібно прописати повідомлення помилки
             
        });

        group.MapDelete("/", async (ClaimsPrincipal user, [FromBody]string password, IUserService userService)=>
        {
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!Guid.TryParse(userIdString, out Guid userId))
                return Results.Unauthorized();
            
            var result = await userService.RemoveAsync(userId, password);
            return result ? Results.NoContent() : Results.BadRequest();//Потрібно прописати повідомлення помилки  
        });
    }
}