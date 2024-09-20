using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using vibe_backend.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Vibes") ?? "Data Source=Vibe.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<PostDb>(connectionString);
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
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VibeLog API V1");
    });
//}

app.MapGet("/posts", async (PostDb db) => await db.Posts.ToListAsync());

app.MapPost("/post", async (PostDb db, Post post) =>
{
    await db.Posts.AddAsync(post);
    await db.SaveChangesAsync();
    return Results.Created($"/post/{post.Id}", post);
});

app.MapGet("/post/{id}", async (PostDb db, int id) => await db.Posts.FindAsync(id));

app.Run();
