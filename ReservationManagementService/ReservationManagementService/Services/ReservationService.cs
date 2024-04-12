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

public async Task<UserDto> GetUser(int userId)
{
    // Sprawdź, czy klient o danym userId już istnieje
    var existingCustomer = await _context.Customers
                                         .FirstOrDefaultAsync(c => c.Id == userId);
    if (existingCustomer != null)
    {
        return new UserDto
        {
            FirstName = existingCustomer.FirstName,
            LastName = existingCustomer.LastName,
            PhoneNumber = existingCustomer.PhoneNumber
        };
    }

    return null;
}
            
public async Task<bool> UpdateOrCreateUserAsync(int userId, UserDto userDto)
{
    var user = await _context.Customers.FirstOrDefaultAsync(u => u.Id == userId);

    if (user == null)
    {
        // Użytkownik nie został znaleziony, tworzymy nowego
        user = new Customer
        {
            Id = userId,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            PhoneNumber = userDto.PhoneNumber,
            Email = "dupka@onet.pl",
        };
        _context.Customers.Add(user);
    }
    else
    {
        // Aktualizacja istniejącego użytkownika
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.PhoneNumber = userDto.PhoneNumber;
    }

    await _context.SaveChangesAsync();
    return true;
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


            return new ReservationDto
            {
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
                .Where(reservation => reservation.CustomerId == userId)
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
                    //RoomIds = reservation.ReservationRooms.Select(rr => rr.RoomId).ToList()
                })
                .ToListAsync();

            return reservations;
        }

public async Task<DetailReservationDto> GetReservationDetails(int reservationId)
{
    var reservation = await _context.Reservations
        .Include(r => r.ReservationRooms)
            .ThenInclude(rr => rr.Room)
        .FirstOrDefaultAsync(r => r.Id == reservationId);

    if (reservation == null)
    {
        return null; // Lub rzuć wyjątek, jeśli to preferujesz
    }

    var detailReservationDto = new DetailReservationDto
    {
        CheckInDate = reservation.CheckInDate,
        CheckOutDate = reservation.CheckOutDate,
        Guests = reservation.Guests,
        ReservetRooms = reservation.ReservationRooms.Select(rr => new RoomDto
        {
            Id = rr.Room.Id,
            Number = rr.Room.Number,
            Floor = rr.Room.Floor,
            Type = rr.Room.Type,
            Amenities = rr.Room.Amenities, // Upewnij się, że Twoja klasa Room posiada te właściwości
            Price = rr.Room.Price, // Zakładając, że masz cenę pokoi w modelu
            IsAvailable = true // Może wymagać dodatkowej logiki do określenia dostępności
        }).ToList()
    };

    return detailReservationDto;
}

public async Task<bool> CancelReservation(int reservationId, int userId)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        if (reservation == null || reservation.CustomerId != userId)
        {
            return false; // Rezerwacja nie istnieje lub nie należy do użytkownika
        }

        // Usuwanie powiązanych pokoi z rezerwacją
        var reservationRooms = _context.ReservationRooms.Where(rr => rr.ReservationId == reservationId);
        _context.ReservationRooms.RemoveRange(reservationRooms);

        await _context.SaveChangesAsync();

        // Usuwanie rezerwacji
        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
        return true;
    }
    catch (Exception)
    {
        // Tutaj możesz obsłużyć wyjątek, jeśli to konieczne
        await transaction.RollbackAsync();
        return false;
    }
}



    }
}
