using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContactApp.Data;
using System;
using System.Linq;

namespace ContactApp.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ContactAppDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ContactAppDbContext>>());
        
        if (context.Users.Any())
        {
            return;
        }
            
        context.Users.AddRange(
            new User
            {
                Id = new Guid(),
                Username = "batuberk",
                Password = BCrypt.Net.BCrypt.HashPassword("testpassword")
            },
            new User
            {
                Id = new Guid(),
                Username = "testuser",
                Password = BCrypt.Net.BCrypt.HashPassword("test")
            },
            new User
            {
                Id = new Guid(),
                Username = "johndoe",
                Password = BCrypt.Net.BCrypt.HashPassword("foobar")
            }
        );
        
        context.SaveChanges();
    }
}