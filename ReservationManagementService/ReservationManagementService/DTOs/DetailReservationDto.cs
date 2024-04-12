using System;
using System.Collections.Generic;

namespace ReservationManagementService.DTOs
{
    public class DetailReservationDto
    {
        public IEnumerable<RoomDto> ReservetRooms { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Guests { get; set; }
    }
}