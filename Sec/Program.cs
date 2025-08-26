using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.MapGet("/users/{id}", (int id, IUserService userService) =>
{
    var user = userService.GetUserById(id);
    if (user == null)
    {
        return Results.NotFound($"Пользователь с ID {id} не найден.");
    }
    return Results.Ok(user);
});

app.Run();

public interface IUserService
{
    User? GetUserById(int id);
}

public class UserService : IUserService
{
    private readonly List<User> _users = new List<User>
    {
        new User { Id = 1, Name = "Alice" },
        new User { Id = 2, Name = "Bob" },
        new User { Id = 3, Name = "Charlie" }
    };

    public User? GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
