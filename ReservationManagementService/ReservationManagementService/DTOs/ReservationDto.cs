using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationManagementService.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public List<int> RoomIds { get; set; } = new List<int>();
        public int CustomerId { get; set; } // Identyfikator klienta dokonującego rezerwacji
        public DateTime CheckInDate { get; set; } // Data zameldowania
        public DateTime CheckOutDate { get; set; } // Data wymeldowania
        public string Status { get; set; } // Status rezerwacji (np. potwierdzona, oczekująca, anulowana)
        public int Guests { get; set; }
    }

}
