using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReservationManagementService.DTOs;
using ReservationManagementService.Models;
//Add-Migration AddReservationRoomRelationship
namespace ReservationManagementService.Services
{
    public class ReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task EnsureCustomerExists( int userId, string email)
        {
            // Sprawdź, czy klient o danym userId już istnieje
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId); // Zakładam, że dodałeś pole UserId do modelu Customer

            if (existingCustomer == null)
            {
                // Jeśli nie istnieje, utwórz nowego klienta
                var newCustomer = new Customer
                {
                    UserId = userId,
                    Email = email
                    // Możesz dodać więcej pól, jeśli są dostępne i wymagane
                };

                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RoomDto>> CheckAvailability(DateTime checkInDate, DateTime checkOutDate)
        {
            var availableRooms = await _context.Rooms
                .Where(room => !room.ReservationRooms.Any(reservationRoom =>
                                reservationRoom.Reservation.CheckInDate < checkOutDate &&
                                reservationRoom.Reservation.CheckOutDate > checkInDate))
                .Select(room => new RoomDto
                {
                    Id = room.Id,
                    Number = room.Number,
                    Floor = room.Floor,
                    Type = room.Type,
                    Amenities = room.Amenities,
                    Price = room.Price,
                    IsAvailable = true
                })
                .ToListAsync();

            return availableRooms;
        }


        public async Task<ReservationDto> MakeReservation(CreateReservationDto createReservationDto)
        {
            // Walidacja dat
            
            if (createReservationDto.CheckInDate >= createReservationDto.CheckOutDate)
            {
                throw new ArgumentException("Data zameldowania musi być wcześniejsza niż data wymeldowania.");
            }


            // Tworzenie instancji rezerwacji
            
            var reservation = new Reservation
            {
                CustomerId = createReservationDto.CustomerId,
                CheckInDate = createReservationDto.CheckInDate,
                CheckOutDate = createReservationDto.CheckOutDate,
                Guests = createReservationDto.Guests,
                Status = "Confirmed"
            };


            _context.Reservations.Add(reservation);
                        await _context.SaveChangesAsync();
            

            // Przypisanie pokoi do rezerwacji
            foreach (var roomId in createReservationDto.SelectedRoomIds)
            {
                var reservationRoom = new ReservationRoom
                {
                    ReservationId = reservation.Id,
                    RoomId = roomId
                };

                _context.ReservationRooms.Add(reservationRoom);
            }

            await _context.SaveChangesAsync();

            // Przykładowe przekształcenie Reservation na ReservationDto (możesz potrzebować dostosować)
            return new ReservationDto
            {
                Id = reservation.Id,
                RoomIds = createReservationDto.SelectedRoomIds,
                CustomerId = reservation.CustomerId,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                Guests = reservation.Guests,
                Status = reservation.Status
            };
        }


        public async Task<Reservation> GetReservationById(int id)
        {
            return await _context.Reservations
                .Include(reservation => reservation.ReservationRooms)
                    .ThenInclude(reservationRoom => reservationRoom.Room)
                .Include(reservation => reservation.Customer)
                .FirstOrDefaultAsync(reservation => reservation.Id == id);
        }

public async Task<IEnumerable<ReservationDto>> GetUserReservations(int userId)
{
    // Pobierz wszystkie rezerwacje dla danego userId
    var reservations = await _context.Reservations
        .Where(reservation => reservation.Customer.UserId == userId)
        .Select(reservation => new ReservationDto
        {
            // Przypisz właściwości z Reservation do ReservationDto
            // Zakładam, że klasa ReservationDto została odpowiednio zdefiniowana
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            Guests = reservation.Guests,
            Status = reservation.Status,
            // Przykład, jak można przekształcić powiązane pokoje, jeśli to potrzebne
            RoomIds = reservation.ReservationRooms.Select(rr => rr.RoomId).ToList()
        })
        .ToListAsync();

    return reservations;
}


    }
}
