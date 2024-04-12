using System;

namespace BookingClientApp.Models
{
    public class Room
    {
        public int Id { get; set; } // Unique identifier for the room
        public string Number { get; set; } // Room number
        public int Floor { get; set; }
        public string Type { get; set; } // Type of the room (e.g., single, double, suite)
        public string Amenities { get; set; } // Comma-separated list of amenities
        public decimal Price { get; set; } // Price per night
        public bool IsAvailable { get; set; }
    }
}