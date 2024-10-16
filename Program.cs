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
    var requestBody = await new StreamReader(ctx.Request.Body).ReadToEndAsync();
    Console.WriteLine($"Received post content: {requestBody}");

    Post post = new Post();
    post.userid = 4;
    post.content = requestBody;

    await db.posts.AddAsync(post);
    await db.SaveChangesAsync();
    return Results.Created($"/new-post/{post.post_id}", post);

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

app.Run();
