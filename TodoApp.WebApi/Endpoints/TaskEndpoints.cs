using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Services.DTOs;
using TodoApp.Services.Interfaces;

namespace TodoApp.WebApi.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("tasks").WithTags("Tasks")
        .RequireAuthorization();;

        group.MapGet("/", async (ClaimsPrincipal user, ITaskService taskService) =>
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var tasks = await taskService.GetAllAsync(userId);
            return Results.Ok(tasks);
        });

        group.MapGet("/{id:guid}", async (Guid id, ClaimsPrincipal user, ITaskService taskService) =>
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var task = await taskService.GetByIdAsync(id, userId);
            return Results.Ok(task);
        }).WithName("GetTaskById");

        group.MapPost("/", async ([FromBody] CreateTodoTaskDto dto, ClaimsPrincipal user, ITaskService taskService) =>
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            if (string.IsNullOrEmpty(dto.Title))
                return Results.BadRequest(new {message = "Назва завдання не може бути попрожнім"});

            var createdTask = await taskService.CreateAsync(dto, userId);

            return Results.CreatedAtRoute("GetTaskById", new {id = createdTask.Id, userId}, createdTask);

        });
        
        group.MapPut("/{id:guid}", async (Guid id, [FromBody] CreateTodoTaskDto dto, ClaimsPrincipal user, ITaskService taskService) =>
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var updated = await taskService.UpdateAsync(id, dto, userId);
            return updated ? Results.NoContent() : Results.NotFound(new {message = "Завдання не знайдено або доступ заборонено"});
        });

        group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal user, ITaskService taskService) =>
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

           var deleted =  await taskService.DeleteAsync(id, userId);
           return deleted ? Results.NoContent() : Results.NotFound(new { message = "Завдання не знайдено або доступ заборонено"}); 
        });

        group.MapPatch("/{id:guid}/toggle", async (Guid id, ClaimsPrincipal user, ITaskService taskService) =>
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var updated = await taskService.ToggleCompleteAsync(id, userId);
            return updated ? Results.NoContent() : Results.NotFound(new {message = "Завдання не знайдено або доступ заборонено"});
        });

    }
}