using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementService.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; } // Unikalny identyfikator pokoju
        public string Number { get; set; } // Numer pokoju
        public int Floor { get; set; } // Piętro, na którym znajduje się pokój
        public string Type { get; set; } // Typ pokoju (np. pojedynczy, podwójny, apartament)
        public string Amenities { get; set; } // Lista udogodnień w pokoju (np. WiFi, TV)
        public decimal Price { get; set; } // Cena za noc
        public bool IsAvailable { get; set; } // Informacja, czy pokój jest dostępny
    }

}
