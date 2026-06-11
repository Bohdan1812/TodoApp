using Microsoft.AspNetCore.Mvc;
using TodoApp.Services.Interfaces;

namespace TodoApp.WebApi.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("categories").WithTags("Categories");

        group.MapGet("/{id:guid}", async (Guid id, Guid userId, ICategoryService categoryService) =>
        {
            var task = await categoryService.GetByIdAsync(id, userId);

            if(task is null)
                return Results.NotFound(new {message = "Категорію не знайдено або користувач немає доступу"});

            return Results.Ok(task);
        });

        group.MapGet("/", async (Guid userId, ICategoryService categoryService) =>
        {
            var tasks = await categoryService.GetAllAsync(userId);
            return Results.Ok(tasks);
        })
        .WithName("GetCategoryById");

        group.MapPost("/", async ([FromBody]string name, Guid userId, ICategoryService categoryService) =>
        {
            if (string.IsNullOrEmpty(name))
                return Results.BadRequest("Назва категорія не може бути пустою");
            
            var category = await categoryService.AddAsync(name, userId);

            return Results.CreatedAtRoute("GetCategoryById", new {id = category.Id, userId}, category);
        });

        group.MapPut("/", async (Guid id, [FromBody]string name, Guid userId, ICategoryService categoryService) =>
        {
            if (string.IsNullOrEmpty(name))
                return Results.BadRequest("Назва категорія не може бути пустою");
            
            var result = await categoryService.UpdateAsync(id, name, userId);

            return result ? Results.NoContent() : Results.NotFound("Категорію не знайдено або користувач немає доступу");
        });

        group.MapDelete("/", async (Guid id, Guid userId, ICategoryService categoryService) =>
        {
            var result = await categoryService.DeleteAsync(id, userId);

            return result ? Results.NoContent() : Results.NotFound("Категорію не знайдено або користувач немає доступу");
        });
    }
}