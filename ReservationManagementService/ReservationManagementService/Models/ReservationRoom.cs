namespace ReservationManagementService.Models
{
    /// <summary>
    /// Represents a link between a reservation and a room, detailing which rooms are involved in specific reservations.
    /// </summary>
    public class ReservationRoom
    {
        /// <summary>
        /// Identifier for the reservation.
        /// </summary>
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        /// <summary>
        /// Identifier for the room.
        /// </summary>
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}