using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementService.DTOs
{
/// <summary>
/// DTO for creating a reservation. It includes all necessary details to make a reservation.
/// </summary>
public class CreateReservationDto
{
    /// <summary>
    /// List of selected room IDs for the reservation.
    /// </summary>
    public List<int> SelectedRoomIds { get; set; } = new List<int>();

    /// <summary>
    /// Customer ID making the reservation.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Check-in date for the reservation.
    /// </summary>
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Check-out date for the reservation.
    /// </summary>
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// Number of guests included in the reservation.
    /// </summary>
    public int Guests { get; set; }
}

}
