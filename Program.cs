using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using vibe_backend.models;
using vibe_backend.services;
using System.Reflection.Metadata.Ecma335;
using FluentAssertions.Common;
using AspNetCoreRateLimit;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING");

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

builder.Services.AddOptions();

builder.Services.AddMemoryCache();

var ip_rate_limits = builder.Configuration.GetSection("IpRateLimiting");
var ip_rate_policies = builder.Configuration.GetSection("IpRateLimitPolicies");

builder.Services.Configure<IpRateLimitOptions>(ip_rate_limits);

builder.Services.Configure<IpRateLimitPolicies>(ip_rate_policies);

builder.Services.AddInMemoryRateLimiting();
//builder.Services.AddDistributedRateLimiting<AsyncKeyLockProcessingStrategy>();
//builder.Services.AddDistributedRateLimiting<RedisProcessingStrategy>();
//builder.Services.AddRedisRateLimiting();

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VibeLog API V1");
    });
//}

app.UseIpRateLimiting();

app.UseMvc();

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
});

app.MapGet("/post/{post_id}", async (FeedContext db, int post_id) => await db.posts.FindAsync(post_id));

app.Run();
