using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using News.API.Middleware;
using News.Business.Services;
using News.Core.Entities;
using News.Core.Interfaces;
using News.Infrastructure.Database;
using News.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("NewsDb");

builder.Services.AddDbContext<NewsDbContext>(options =>
    options.UseNpgsql(connectionString));

// For Identity
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NewsDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddTransient<NewsDbSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<NewsDbSeeder>();

    await seeder.CreateDb();
    await seeder.Seed();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

app.Run();