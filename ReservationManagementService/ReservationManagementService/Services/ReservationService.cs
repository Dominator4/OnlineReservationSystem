using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationManagementService.DTOs;
using ReservationManagementService.Models;


namespace ReservationManagementService.Services
{
    public class ReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> CheckAvailability(DateTime checkInDate, DateTime checkOutDate)
        {
            // Przykład zapytania sprawdzającego dostępność pokoi
            // Zakłada, że pokój jest dostępny, jeśli nie ma żadnych rezerwacji na te same daty
            return await _context.Rooms
                .Where(room => !room.Reservations.Any(reservation =>
                               reservation.CheckInDate < checkOutDate &&
                               reservation.CheckOutDate > checkInDate))
                .ToListAsync();
        }

        public async Task<Reservation> MakeReservation(CreateReservationDto createReservationDto)
        {
            var reservation = new Reservation
            {
                RoomId = createReservationDto.RoomId,
                CustomerId = createReservationDto.CustomerId,
                CheckInDate = createReservationDto.CheckInDate,
                CheckOutDate = createReservationDto.CheckOutDate,
                //Guests = createReservationDto.Guests,
                Status = "Confirmed" // Przykład ustawienia domyślnego statusu
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task<Reservation> GetReservationById(int id)
        {
            return await _context.Reservations
                .Include(reservation => reservation.Room)
                .Include(reservation => reservation.Customer)
                .FirstOrDefaultAsync(reservation => reservation.Id == id);
        }


    public (string UserId, string Email) ExtractTokenInfo(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
            throw new ArgumentException("Invalid JWT Token");

        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

        if (userIdClaim == null || emailClaim == null)
            throw new InvalidOperationException("Required claims not found in the token");

        return (userIdClaim, emailClaim);
    }
        // Możesz dodać więcej metod, np. do aktualizacji lub anulowania rezerwacji
    }
}