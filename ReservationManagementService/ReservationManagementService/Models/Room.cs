using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ReservationManagementService.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; } // Unique identifier for the room

        [Required]
        public string Number { get; set; } // Room number

        public int Floor { get; set; } // Floor where the room is located

        [Required]
        public string Type { get; set; } // Type of the room (e.g., single, double, suite)

        public string Amenities { get; set; } // Comma-separated list of amenities

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Price per night

        public bool IsAvailable { get; set; } // Availability status
    }
}
