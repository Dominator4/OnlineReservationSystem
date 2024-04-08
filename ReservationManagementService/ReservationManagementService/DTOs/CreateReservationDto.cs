using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementService.DTOs
{
    public class CreateReservationDto
    {
    public List<int> SelectedRoomIds { get; set; } = new List<int>();
        public int CustomerId { get; set; } // Identyfikator klienta dokonującego rezerwacji
        public DateTime CheckInDate { get; set; } // Data zameldowania
        public DateTime CheckOutDate { get; set; } // Data wymeldowania
        public int Guests { get; set; }
    }

}
