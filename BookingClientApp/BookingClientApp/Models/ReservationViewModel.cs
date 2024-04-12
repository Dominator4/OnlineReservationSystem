using System;
using System.Collections.Generic;

namespace BookingClientApp.Models
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
    public List<int> SelectedRoomIds { get; set; } = new List<int>();
    public int CustomerId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; }
        public int Guests { get; set; }
    }
}
