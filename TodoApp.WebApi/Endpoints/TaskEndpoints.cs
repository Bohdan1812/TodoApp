using Microsoft.AspNetCore.Mvc;
using TodoApp.Services.DTOs;
using TodoApp.Services.Interfaces;

namespace TodoApp.WebApi.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("tasks").WithTags("Tasks");

        group.MapGet("/", async (Guid userId, ITaskService taskService) =>
        {
            var tasks = await taskService.GetAllAsync(userId);
            return Results.Ok(tasks);
        });

        group.MapGet("/{id:guid}", async (Guid id, Guid userId, ITaskService taskService) =>
        {
            var task = await taskService.GetByIdAsync(id, userId);
            return Results.Ok(task);
        });

        group.MapPost("/", async ([FromBody] CreateTodoTaskDto dto, Guid userId, ITaskService taskService) =>
        {
            if (string.IsNullOrEmpty(dto.Title))
                return Results.BadRequest(new {message = "Назва завдання не може бути попрожнім"});

            var createdTask = await taskService.CreateAsync(dto, userId);

            return Results.CreatedAtRoute("GetTaskById", new {id = createdTask, userId}, createdTask);

        }).WithName("GetTaskById");
        
        group.MapPut("/{id:guid}", async (Guid id, [FromBody] CreateTodoTaskDto dto, Guid userId, ITaskService taskService) =>
        {
            var updated = await taskService.UpdateAsync(id, dto, userId);
            return updated ? Results.NoContent() : Results.NotFound(new {message = "Завдання не знайдено або доступ заборонено"});
        });

        group.MapDelete("/{id:guid}", async (Guid id, Guid userId, ITaskService taskService) =>
        {
           var deleted =  await taskService.DeleteAsync(id, userId);
           return deleted ? Results.NoContent() : Results.NotFound(new { message = "Завдання не знайдено або доступ заборонено"}); 
        });

        group.MapPatch("/{id:guid}/toggle", async (Guid id, Guid userId, ITaskService taskService) =>
        {
            var updated = await taskService.ToggleCompleteAsync(id, userId);
            return updated ? Results.NoContent() : Results.NotFound(new {message = "Завдання не знайдено або доступ заборонено"});
        });

    }
}