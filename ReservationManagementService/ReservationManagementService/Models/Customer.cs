using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ReservationManagementService.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; } // Unique identifier for the customer

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } // Customer's first name

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } // Customer's last name

        [EmailAddress]
        public string Email { get; set; } // Customer's email address

        public string PhoneNumber { get; set; } // Customer's phone number

        public List<Reservation> Reservations { get; set; } // List of reservations made by the customer
    }
}
