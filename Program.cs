using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using vibe_backend.models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

var connectionString = "Host=localhost;Port=5432;Database=vibe_db;Username=postgres;Password=dat45586";

builder.Services.AddDbContextPool<FeedContext>(opt =>
    opt.UseNpgsql(connectionString));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VibeLog API",
        Description = "",
        Version = "v1"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://vibe-log-frontend.pages.dev").AllowAnyHeader().AllowAnyMethod();
                      });
});

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

/*
 set up psql as if running locally
change out envs to match internet ones
test routes
 */

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VibeLog API V1");
    });
//}

app.MapGet("/",() => "Hello World");


app.MapGet("/posts", async (FeedContext db) =>
{
    //await using var conn = new NpgsqlConnection(connectionString);
    //await conn.OpenAsync();
    return Results.Ok(await db.posts.ToListAsync());

    //NEXT: => completely destroy and rebuild Posts.cs and try then
});


app.MapGet("/feeds", async (FeedContext db) =>
{
    //await using var conn = new NpgsqlConnection(connectionString);
    //await conn.OpenAsync();
    return Results.Ok(await db.feeds.ToListAsync());

    //try normal Psql query to test
});

/*
app.MapGet("/posts", async (PostDb db) => await db.Posts.ToListAsync());

app.MapPost("/post", async (PostDb db, Post post) =>
{
    await db.Posts.AddAsync(post);
    await db.SaveChangesAsync();
    return Results.Created($"/post/{post.Id}", post);
});

app.MapGet("/post/{id}", async (PostDb db, int id) => await db.Posts.FindAsync(id));
*/
app.Run();
