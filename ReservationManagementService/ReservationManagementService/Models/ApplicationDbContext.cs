using Microsoft.EntityFrameworkCore;

namespace ReservationManagementService.Models
{
    /// <summary>
    /// Database context for the reservation management system, encapsulating all data access configurations.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Collection of rooms available in the system.
        /// </summary>
        public DbSet<Room> Rooms { get; set; }

        /// <summary>
        /// Collection of reservations made in the system.
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; }

        /// <summary>
        /// Collection of links between reservations and rooms.
        /// </summary>
        public DbSet<ReservationRoom> ReservationRooms { get; set; }

        /// <summary>
        /// Collection of customers in the system.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key definition for ReservationRoom
            modelBuilder.Entity<ReservationRoom>()
                .HasKey(rr => new { rr.ReservationId, rr.RoomId });

            // Relationship configurations, if necessary
            modelBuilder.Entity<ReservationRoom>()
                .HasOne(rr => rr.Reservation)
                .WithMany(r => r.ReservationRooms)
                .HasForeignKey(rr => rr.ReservationId);

            modelBuilder.Entity<ReservationRoom>()
                .HasOne(rr => rr.Room)
                .WithMany(r => r.ReservationRooms)
                .HasForeignKey(rr => rr.RoomId);

            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id); // Confirmation that Id is the primary key

            modelBuilder.Entity<Customer>()
                .Property(c => c.Id)
                .ValueGeneratedNever(); // Configuration to ensure EF does not auto-generate the Id value
        }
    }
}
