using Microsoft.EntityFrameworkCore;
using TodoApp.DataAccess;
using TodoApp.DataAccess.Entities;
using TodoApp.Services.Implementations;
using TodoApp.Services.Interfaces;
using TodoApp.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() {Title = "TodoApp", Version = "v1"});
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp API v1");
    });
}

app.UseHttpsRedirection();

app.MapTaskEndpoints();
app.MapUserEndpoints();
app.MapCategoryEndpoints();

app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> sources) =>
{
    return sources.SelectMany(s => s.Endpoints).Select(e => e.DisplayName);
});

app.Run();

