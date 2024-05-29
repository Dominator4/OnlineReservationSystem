using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ReservationManagementService.Models
{
/// <summary>
/// Represents a reservation made by a customer.
/// </summary>
public class Reservation
{
    /// <summary>
    /// Unique identifier for the reservation.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the customer who made the reservation.
    /// </summary>
    [Required]
    public int CustomerId { get; set; }

    /// <summary>
    /// Navigation property for the customer.
    /// </summary>
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }

    /// <summary>
    /// Check-in date for the reservation.
    /// </summary>
    [Required]
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Check-out date for the reservation.
    /// </summary>
    [Required]
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// Number of guests included in the reservation.
    /// </summary>
    public int Guests { get; set; }

    /// <summary>
    /// Current status of the reservation (e.g., pending, confirmed, cancelled).
    /// </summary>
    [Required]
    public string Status { get; set; }

    /// <summary>
    /// Collection of ReservationRoom links detailing the rooms involved in the reservation.
    /// </summary>
    public virtual ICollection<ReservationRoom> ReservationRooms { get; set; }
}

}
