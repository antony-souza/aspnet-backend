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
builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(Program).Assembly)
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
    .AsSelfWithInterfaces()
    .WithScopedLifetime()
);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();