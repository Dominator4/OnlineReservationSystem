using System;

namespace BookingClientApp.Models
{
    public class ReservationViewModel
    {
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Guests { get; set; }
    }
}
