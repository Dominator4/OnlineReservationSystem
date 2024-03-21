using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementService.DTOs
{
    public class CreateReservationDto
    {
        public int RoomId { get; set; } // Identyfikator pokoju, który ma zostać zarezerwowany
        public int CustomerId { get; set; } // Identyfikator klienta dokonującego rezerwacji
        public DateTime CheckInDate { get; set; } // Data zameldowania
        public DateTime CheckOutDate { get; set; } // Data wymeldowania
    }

}
