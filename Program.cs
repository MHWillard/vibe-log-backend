using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using vibe_backend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<UserDb>(options => options.UseInMemoryDatabase("users"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VibeLog API",
        Description = "",
        Version = "v1"
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VibeLog API V1");
    });
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/users", async (UserDb db) => await db.Users.ToListAsync());

app.MapPost("/user", async (UserDb db, User user) =>
{
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return Results.Created($"/user/{user.Id}", user);
});

app.MapGet("/user/{id}", async (UserDb db, int id) => await db.Users.FindAsync(id));

app.MapPut("/user/{id}", async (UserDb db, User updateuser, int id) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();
    user.Name = updateuser.Name;
    user.Description = updateuser.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/user/{id}", async (UserDb db, int id) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null)
    {
        return Results.NotFound();
    }
    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
