using Backend.Common.database;
using Backend.modules.user;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

Env.Load("../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<UserService>();

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

var app = builder.Build();

//app.UseHttpsRedirection();
app.MapControllers();

app.Run();