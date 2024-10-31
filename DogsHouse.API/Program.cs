using DogsHouse.API.Configuration;
using DogsHouse.API.Middleware;
using DogsHouse.Core.Interfaces;
using DogsHouse.Core.Mappings;
using DogsHouse.Core.Services;
using DogsHouse.Infrastructure.Data;
using DogsHouse.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Bind rate limiting settings from configuration
var rateLimitingSettings = builder.Configuration
    .GetSection("RateLimiting")
    .Get<RateLimitingSettings>();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<DogsContext>(options =>
{
    var provider = builder.Configuration["DatabaseProvider"];
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (provider == "PostgreSQL")
        options.UseNpgsql(connectionString);
    else
        options.UseSqlServer(connectionString);
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IDogRepository, DogRepository>();
builder.Services.AddScoped<IDogService, DogService>();

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = rateLimitingSettings!.PermitLimit,
                QueueLimit = rateLimitingSettings.QueueLimit,
                Window = TimeSpan.FromSeconds(rateLimitingSettings.Window)
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Too many requests. Please try again later.",
        }, cancellationToken: cancellationToken);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
