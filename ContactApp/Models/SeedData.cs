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
                Password = BCrypt.Net.BCrypt.HashPassword("batuberk")
            },
            new User
            {
                Id = new Guid(),
                Username = "test_user",
                Password = BCrypt.Net.BCrypt.HashPassword("test_password")
            },
            new User
            {
                Id = new Guid(),
                Username = "berksahin",
                Password = BCrypt.Net.BCrypt.HashPassword("berksahin")
            }
        );
        
        context.SaveChanges();
    }
}