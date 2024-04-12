using System;
using System.Collections.Generic;

namespace BookingClientApp.Models
{
    public class DetailReservationViewModel
    {
        public IEnumerable<Room> ReservetRooms { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Guests { get; set; }
    }
}