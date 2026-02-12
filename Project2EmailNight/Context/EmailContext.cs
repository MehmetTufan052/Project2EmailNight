using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Context
{
    public class EmailContext:IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-MO1FN9D;Initial catalog=Project2EmailNightDb;integrated security=true;trust server certificate=true");
        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<EmailCategory> EmailCategories { get; set; }
    }
}
