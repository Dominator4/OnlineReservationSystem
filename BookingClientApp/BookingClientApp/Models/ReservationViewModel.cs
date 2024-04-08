using System;
using System.Collections.Generic;

namespace BookingClientApp.Models
{
    public class ReservationViewModel
    {
    public List<int> SelectedRoomIds { get; set; } = new List<int>();
    public int CustomerId { get; set; } =0;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Guests { get; set; }
    }
}
