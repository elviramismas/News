using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using News.Core.Entities;

namespace News.Infrastructure.Database;

public class NewsDbSeeder
{
    private readonly NewsDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;

    public NewsDbSeeder(NewsDbContext context,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public async Task CreateDb()
    {
       await _context.Database.EnsureCreatedAsync(); 
    }
    
    public async Task Seed()
    {
        if(await _roleManager.RoleExistsAsync("Author") == false)
            await _roleManager.CreateAsync(new IdentityRole("Author"));

        if(await _roleManager.RoleExistsAsync("Reader") == false)
            await _roleManager.CreateAsync(new IdentityRole("Reader"));

        if (!_context.Users.Any())
        {
            var password = new PasswordHasher<ApplicationUser>();

            IEnumerable<ApplicationUser> users =
            [
                new ApplicationUser()
                {
                    UserName = "reader@email.com",
                    NormalizedUserName = "READER@EMAIL.COM",
                    Email = "reader@email.com",
                    NormalizedEmail = "READER@EMAIL.COM",
                    LockoutEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser()
                {
                    UserName = "author@email.com",
                    NormalizedUserName = "AUTHOR@EMAIL.COM",
                    Email = "author@email.com",
                    NormalizedEmail = "AUTHOR@EMAIL.COM",
                    LockoutEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            ];

            users.First().PasswordHash = password.HashPassword(users.First(), "Reader123!");
            users.Last().PasswordHash = password.HashPassword(users.Last(), "Author123!");
            
            var userStore = new UserStore<ApplicationUser>(_context);
            await userStore.CreateAsync(users.First());
            await userStore.AddToRoleAsync(users.First(), "READER");
            await userStore.CreateAsync(users.Last());
            await userStore.AddToRoleAsync(users.Last(), "AUTHOR");
        }

        await _context.SaveChangesAsync();
    }
}