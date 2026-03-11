using Backend.Common.database;
using Backend.modules.user;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

DotNetEnv.Env.Load("../.env");

var builder = WebApplication.CreateBuilder(args);

var connectionString = DotNetEnv.Env.GetString("DATABASE_URL");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

//app.UseHttpsRedirection();
app.MapControllers();

app.Run();