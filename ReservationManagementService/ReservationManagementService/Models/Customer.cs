using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ReservationManagementService.Models
{
/// <summary>
/// Represents a customer in the reservation management system.
/// </summary>
public class Customer
{
    /// <summary>
    /// Unique identifier for the customer.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// First name of the customer.
    /// </summary>
    [StringLength(50)]
    public string FirstName { get; set; }

    /// <summary>
    /// Last name of the customer.
    /// </summary>
    [StringLength(50)]
    public string LastName { get; set; }

    /// <summary>
    /// Email address of the customer.
    /// </summary>
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Phone number of the customer.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// List of reservations made by the customer.
    /// </summary>
    public List<Reservation> Reservations { get; set; }
}

}
