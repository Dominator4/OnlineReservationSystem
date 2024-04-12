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

[HttpGet("Check-User")]
[Authorize]
public async Task<ActionResult<UserDto>> CheckUser()
{
    int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    // string userEmail = User.FindFirst(ClaimTypes.Email)?.Value; // Jeśli potrzebujesz

    var user = await _reservationService.GetUser(userId);
    if (user != null)
    {
        return Ok(user);
    }
    else
    {
        return NotFound();
    }
}

[HttpPut("update-user")]
[Authorize]
public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    bool updateResult = await _reservationService.UpdateOrCreateUserAsync(userId, userDto);

    if (updateResult)
    {
        return NoContent(); // Pomyślna aktualizacja lub utworzenie użytkownika
    }
    else
    {
        return BadRequest(); // W przypadku nieoczekiwanego błędu
    }
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

[HttpGet("details/{id}")]
public async Task<ActionResult<DetailReservationDto>> GetReservationDetails(int id)
{
    var reservationDetails = await _reservationService.GetReservationDetails(id); // Załóżmy, że ta metoda istnieje i zwraca DetailReservationDto
    if (reservationDetails == null)
    {
        return NotFound();
    }
    return Ok(reservationDetails);
}

[HttpDelete("cancel/{id}")]
public async Task<IActionResult> CancelReservation(int id)
{
                int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    var success = await _reservationService.CancelReservation(id, userId);
    if (success)
    {
        return Ok();
    }
    else
    {
        return NotFound();
    }
}


        [HttpGet]
        public String Get()
        {
            return "testujemy reservation";
        }
        // Możesz dodać więcej metod związanych z operacjami na rezerwacjach...
    }
}