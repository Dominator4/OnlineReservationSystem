using Microsoft.EntityFrameworkCore;
using System;

namespace ReservationManagementService.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; } // Dodanie DbSets dla pokoi
        public DbSet<Reservation> Reservations { get; set; } // Dodanie DbSets dla rezerwacji

        // Dodatkowe DbSets dla innych encji
    }
}
