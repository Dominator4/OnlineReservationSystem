using Microsoft.EntityFrameworkCore;
using System;

namespace AuthenticationService.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        // Dodatkowe DbSets dla innych encji
    }
}
