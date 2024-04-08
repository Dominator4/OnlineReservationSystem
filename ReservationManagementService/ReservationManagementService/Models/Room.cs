using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationManagementService.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; } // Unikalny identyfikator pokoju

        [Required]
        public string Number { get; set; } // Numer pokoju

        public int Floor { get; set; } // Piętro, na którym znajduje się pokój

        [Required]
        public string Type { get; set; } // Typ pokoju (np. pojedynczy, podwójny, apartament)

        public string Amenities { get; set; } // Lista udogodnień w pokoju

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Cena za noc

        public bool IsAvailable { get; set; } // Informacja, czy pokój jest dostępny

        // Właściwość nawigacyjna wskazująca na rezerwacje tego pokoju
//        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<ReservationRoom> ReservationRooms { get; set; }
    }
}
