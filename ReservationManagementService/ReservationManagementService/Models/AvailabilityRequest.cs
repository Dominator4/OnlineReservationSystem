using System;
namespace ReservationManagementService.Models
{
    public class AvailabilityRequest
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
