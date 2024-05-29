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
    /// <summary>
    /// Controller handling reservation operations such as creating, updating, checking, and cancelling reservations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        /// <summary>
        /// Initializes a new instance of the ReservationController class.
        /// </summary>
        /// <param name="reservationService">The reservation service to be used by this controller.</param>
        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        /// <summary>
        /// Checks if the user exists in the database based on their identifier.
        /// </summary>
        /// <returns>Returns the user details if found; otherwise returns NotFound.</returns>
        [HttpGet("Check-User")]
        [Authorize]
        public async Task<ActionResult<UserDto>> CheckUser()
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
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

        /// <summary>
        /// Updates or creates a user based on the provided user data.
        /// </summary>
        /// <param name="userDto">The user data transfer object containing the user details.</param>
        /// <returns>Returns NoContent on success; BadRequest if the update fails.</returns>
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
                return NoContent(); // Successful update or creation
            }
            else
            {
                return BadRequest(); // Unexpected error
            }
        }

        /// <summary>
        /// Checks room availability based on the provided date range.
        /// </summary>
        /// <param name="request">The availability request containing check-in and check-out dates.</param>
        /// <returns>A list of available rooms.</returns>
        [HttpPost("check-availability")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RoomDto>>> CheckAvailability([FromBody] AvailabilityRequest request)
        {
            var availableRooms = await _reservationService.CheckAvailability(request.CheckInDate, request.CheckOutDate);
            return Ok(availableRooms);
        }

        /// <summary>
        /// Creates a reservation based on the provided reservation details.
        /// </summary>
        /// <param name="createReservationDto">The reservation details to create a new reservation.</param>
        /// <returns>A newly created reservation with its details.</returns>
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
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve.</param>
        /// <returns>The reservation details if found; otherwise, NotFound.</returns>
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

        /// <summary>
        /// Retrieves all reservations made by a specific user.
        /// </summary>
        /// <returns>A list of user's reservations if any; otherwise, NotFound.</returns>
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

        /// <summary>
        /// Retrieves detailed information about a specific reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve detailed information for.</param>
        /// <returns>Detailed information about the reservation if found; otherwise, NotFound.</returns>
        [HttpGet("details/{id}")]
        public async Task<ActionResult<DetailReservationDto>> GetReservationDetails(int id)
        {
            var reservationDetails = await _reservationService.GetReservationDetails(id);
            if (reservationDetails == null)
            {
                return NotFound();
            }
            return Ok(reservationDetails);
        }

        /// <summary>
        /// Cancels a reservation if it exists and belongs to the user making the request.
        /// </summary>
        /// <param name="id">The ID of the reservation to cancel.</param>
        /// <returns>Ok if the reservation was successfully canceled; otherwise, NotFound.</returns>
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

        /// <summary>
        /// A simple method to check if the ReservationService is running.
        /// </summary>
        /// <returns>A string indicating that the service is started.</returns>
        [HttpGet]
        public String Get()
        {
            return "ReservationService started";
        }
    }
}
