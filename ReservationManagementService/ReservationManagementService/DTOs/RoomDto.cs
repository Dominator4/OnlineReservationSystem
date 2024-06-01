namespace ReservationManagementService.DTOs
{
    /// <summary>
    /// DTO representing a room in the reservation system.
    /// </summary>
    public class RoomDto
    {
        /// <summary>
        /// Unique identifier for the room.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Room number assigned to the room.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Floor where the room is located.
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// Type of the room (e.g., single, double, suite).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// List of amenities available in the room (e.g., WiFi, TV).
        /// </summary>
        public string Amenities { get; set; }

        /// <summary>
        /// Price per night for the room.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Indicates whether the room is currently available.
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}
