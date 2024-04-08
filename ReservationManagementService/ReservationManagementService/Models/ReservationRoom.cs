using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ReservationManagementService.Models
{
public class ReservationRoom
{
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; }

    public int RoomId { get; set; }
    public Room Room { get; set; }
}
}