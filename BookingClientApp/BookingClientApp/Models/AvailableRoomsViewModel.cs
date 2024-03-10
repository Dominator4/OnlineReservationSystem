using System;
using System.Collections.Generic;

namespace BookingClientApp.Models
{
    public class AvailableRoomsViewModel
    {
        public IEnumerable<Room> AvailableRooms { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}