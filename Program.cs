using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using vibe_backend.models;
using vibe_backend.services;
using System.Reflection.Metadata.Ecma335;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

//var connectionString = "Host=localhost;Port=5432;Database=vibe_db;Username=postgres;Password=dat45586";
var connectionString = "Host=postgres.railway.internal;Port=5432;Database=railway;Username=postgres;Password=YHDbYyQeHrnnhJEtoZCngCUuenoSfbsC";

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
                          policy.WithOrigins("https://vibe-log-frontend.pages.dev","http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
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

app.MapGet("/api/protected/user-id", () => 1234567890);

app.MapGet("/posts", async (FeedContext db) =>
{
    return Results.Ok(await db.posts.ToListAsync());
});


app.MapGet("/feed/{user_id}", async (FeedContext db) =>
{
    return Results.Ok(await db.feeds.ToListAsync());
});

app.MapPost("/new-post", async (FeedContext db, HttpContext ctx) =>
{
    var postData = await ctx.Request.ReadFromJsonAsync<PostData>();
    if (postData != null)
    {
        // Process postData.Content and postData.UserId
        Post post = new Post();
        post.post_id = 5678;
        post.post_date = DateTime.UtcNow;
        post.user_id = 5555;
        post.content = postData.Content;

        await db.posts.AddAsync(post);
        await db.SaveChangesAsync();
        return Results.Ok(new { message = "Post created successfully" });
    }
    return Results.BadRequest(new { error = "Invalid data" });

    //return Results.Ok(new { success = true, message = "Post received successfully" });

    /*get data from frontend as json/object
    //turn it into a Post post
    //then add
    */

    //this will probably get the post information, build a post object here, and then add it async accordingly here on the backend
    /*

    */
});

app.MapGet("/post/{post_id}", async (FeedContext db, int post_id) => await db.posts.FindAsync(post_id));

app.MapGet("/test-post", () => new
{
        post_table_id = 1,
        user_id = 1234,
        content = "Returning a test post",
        post_id = 5678,
        post_date = DateTime.UtcNow
});

app.MapGet("/test-posts", () => new[]
{
        new { post_table_id = 18, user_id = 1234, content = "happy", post_id = 5678, post_date = "2024-10-20T14:00:00Z" },
        new { post_table_id = 19, user_id = 1234, content = "sad", post_id = 6789, post_date = "2024-10-20T18:30:00Z" },
        new { post_table_id = 20, user_id = 1234, content = "excited", post_id = 7890, post_date = "2024-10-20T22:45:00Z" },
        new { post_table_id = 21, user_id = 1234, content = "angry", post_id = 8901, post_date = "2024-10-21T13:15:00Z" },
        new { post_table_id = 22, user_id = 1234, content = "calm", post_id = 9012, post_date = "2024-10-21T20:00:00Z" }
});

app.Run();
