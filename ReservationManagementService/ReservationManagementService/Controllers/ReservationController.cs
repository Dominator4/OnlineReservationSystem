using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<RoomDto>>> CheckAvailability([FromBody] AvailabilityRequest request)
        {
            var availableRooms = await _reservationService.CheckAvailability(request.CheckInDate, request.CheckOutDate);
            return Ok(availableRooms);
        }

        // Metoda do tworzenia nowej rezerwacji
        [HttpPost("make-reservation")]
        public async Task<ActionResult<ReservationDto>> MakeReservation([FromBody] CreateReservationDto createReservationDto)
        {
            try
            {
                var reservation = await _reservationService.MakeReservation(createReservationDto);
                return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
            }
            catch (Exception ex)
            {
                // Obsługa błędów, np. gdy pokój nie jest dostępny
                return BadRequest(ex.Message);
            }
        }

        // Metoda do pobierania szczegółów rezerwacji
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

        [HttpGet]
        public String Get()
        {
            return "testujemy reservation";
        }
        // Możesz dodać więcej metod związanych z operacjami na rezerwacjach...
    }
}