using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementService.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; } // Unikalny identyfikator rezerwacji
        public int RoomId { get; set; } // Identyfikator zarezerwowanego pokoju
        public string RoomNumber { get; set; } // Numer pokoju, może być przydatny dla użytkownika
        public int CustomerId { get; set; } // Identyfikator klienta dokonującego rezerwacji
        public string CustomerName { get; set; } // Imię i nazwisko klienta, dla wygody
        public DateTime CheckInDate { get; set; } // Data zameldowania
        public DateTime CheckOutDate { get; set; } // Data wymeldowania
        public string Status { get; set; } // Status rezerwacji (np. potwierdzona, oczekująca, anulowana)
    }

}
