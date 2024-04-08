using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ReservationManagementService.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; } // Unique identifier for the reservation

        [Required]
        public int CustomerId { get; set; } // Reference to the customer who made the reservation

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } // Navigation property for the customer

        [Required]
        public DateTime CheckInDate { get; set; } // Check-in date

        [Required]
        public DateTime CheckOutDate { get; set; } // Check-out date

        public int Guests { get; set; } // Number of guests

        [Required]
        public string Status { get; set; } // Reservation status (e.g., pending, confirmed, cancelled)

    public virtual ICollection<ReservationRoom> ReservationRooms { get; set; }
    }
}
