using DogsHouse.Core.Interfaces;
using DogsHouse.Core.Services;
using DogsHouse.Infrastructure.Data;
using DogsHouse.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DogsContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Host=localhost;Port=5433;Database=Queue;Username=postgres;Password=developer2023")));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IDogRepository, DogRepository>();
builder.Services.AddScoped<IDogService, DogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
