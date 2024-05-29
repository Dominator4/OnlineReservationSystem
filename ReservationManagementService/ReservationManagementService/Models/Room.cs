using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationManagementService.Models
{
/// <summary>
/// Represents a room available for reservation within the system.
/// </summary>
public class Room
{
    /// <summary>
    /// Unique identifier for the room.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Room number.
    /// </summary>
    [Required]
    public string Number { get; set; }

    /// <summary>
    /// Floor number where the room is located.
    /// </summary>
    public int Floor { get; set; }

    /// <summary>
    /// Type of room (e.g., single, double, suite).
    /// </summary>
    [Required]
    public string Type { get; set; }

    /// <summary>
    /// List of amenities provided in the room.
    /// </summary>
    public string Amenities { get; set; }

    /// <summary>
    /// Nightly price rate for the room.
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// Indicates whether the room is currently available for booking.
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Navigation property indicating the reservations involving this room.
    /// </summary>
    public virtual ICollection<ReservationRoom> ReservationRooms { get; set; }
}

}
