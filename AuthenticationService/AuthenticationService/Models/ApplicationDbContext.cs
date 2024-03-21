using Microsoft.EntityFrameworkCore;
using System;

namespace AuthenticationService.Models
{
    public class ApplicationDbContext       : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        // Dodatkowe DbSets dla innych encji
    }
}
