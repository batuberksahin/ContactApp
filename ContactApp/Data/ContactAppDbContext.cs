using ContactApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Data
{
    public class ContactAppDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public ContactAppDbContext(DbContextOptions<ContactAppDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasKey(u => u.Id);
        }
    }
}