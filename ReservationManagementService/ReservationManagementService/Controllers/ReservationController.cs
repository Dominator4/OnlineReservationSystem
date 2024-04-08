using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ReservationManagementService.DTOs;
using ReservationManagementService.Services;
using ReservationManagementService.Models;

namespace ReservationManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // Metoda do sprawdzenia dostępności pokoi
        [HttpPost("check-availability")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RoomDto>>> CheckAvailability([FromBody] AvailabilityRequest request)
        {
            var availableRooms = await _reservationService.CheckAvailability(request.CheckInDate, request.CheckOutDate);
            return Ok(availableRooms);
        }

        // Metoda do tworzenia nowej rezerwacji
        [HttpPost("make-reservation")]
        [Authorize]
        public async Task<ActionResult<ReservationDto>> MakeReservation([FromBody] CreateReservationDto createReservationDto)
        {
            try
            {

        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        string userEmail = User.FindFirst(ClaimTypes.Email)?.Value; // Asumpcja

        // Upewnij się, że klient istnieje w bazie danych przed utworzeniem rezerwacji.
        await _reservationService.EnsureCustomerExists(userId, userEmail);
                createReservationDto.CustomerId = userId;
                var reservation = await _reservationService.MakeReservation(createReservationDto);
                return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
            }
            catch (Exception ex)
            {
                // Obsługa błędów, np. gdy pokój nie jest dostępny
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetReservationById(int id)
        {
            var reservation = await _reservationService.GetReservationById(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }

        [HttpGet("user-reservations")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetUserReservations()
{
        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    var reservations = await _reservationService.GetUserReservations(userId);
    if (reservations == null || !reservations.Any())
    {
        return NotFound();
    }
    return Ok(reservations);
}

        [HttpGet]
        public String Get()
        {
            return "testujemy reservation";
        }
        // Możesz dodać więcej metod związanych z operacjami na rezerwacjach...
    }
}