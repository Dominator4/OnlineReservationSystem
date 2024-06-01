using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Models
{
    /// <summary>
    /// Database context for the authentication service, encapsulating all data access configurations.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext with the specified options.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Collection of users in the system.
        /// </summary>
        public DbSet<User> Users { get; set; }

        // Additional DbSets for other entities
    }
}
