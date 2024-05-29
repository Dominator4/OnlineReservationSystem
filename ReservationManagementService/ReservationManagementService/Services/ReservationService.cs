using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReservationManagementService.DTOs;
using ReservationManagementService.Models;

namespace ReservationManagementService.Services
{
    /// <summary>
    /// Provides functionality for managing reservations, users, and checking room availability.
    /// </summary>
    public class ReservationService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the ReservationService with the specified database context.
        /// </summary>
        /// <param name="context">The database context to be used by this service.</param>
        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a user DTO from the database based on the provided user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>A UserDto object if found; otherwise, null.</returns>
        public async Task<UserDto> GetUser(int userId)
        {
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

        /// <summary>
        /// Updates an existing user or creates a new user if the user ID does not exist.
        /// </summary>
        /// <param name="userId">The ID of the user to update or create.</param>
        /// <param name="userDto">The user data transfer object containing the user details.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        public async Task<bool> UpdateOrCreateUserAsync(int userId, UserDto userDto)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                user = new Customer
                {
                    Id = userId,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    PhoneNumber = userDto.PhoneNumber,
                    Email = "example@example.com", // Placeholder email; consider updating based on requirements
                };
                _context.Customers.Add(user);
            }
            else
            {
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.PhoneNumber = userDto.PhoneNumber;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Checks the availability of rooms for a given date range.
        /// </summary>
        /// <param name="checkInDate">The check-in date to consider for availability.</param>
        /// <param name="checkOutDate">The check-out date to consider for availability.</param>
        /// <returns>A list of available rooms as RoomDto objects.</returns>
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

        /// <summary>
        /// Creates a reservation based on the provided reservation details.
        /// </summary>
        /// <param name="createReservationDto">The details of the reservation to create.</param>
        /// <returns>A ReservationDto object representing the newly created reservation.</returns>
        public async Task<ReservationDto> MakeReservation(CreateReservationDto createReservationDto)
        {
            if (createReservationDto.CheckInDate >= createReservationDto.CheckOutDate)
            {
                throw new ArgumentException("Check-in date must be earlier than check-out date.");
            }

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

        /// <summary>
        /// Retrieves a reservation by its ID, including related data.
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve.</param>
        /// <returns>A Reservation object if found; otherwise, null.</returns>
        public async Task<Reservation> GetReservationById(int id)
        {
            return await _context.Reservations
                .Include(reservation => reservation.ReservationRooms)
                .ThenInclude(reservationRoom => reservationRoom.Room)
                .Include(reservation => reservation.Customer)
                .FirstOrDefaultAsync(reservation => reservation.Id == id);
        }

        /// <summary>
        /// Retrieves all reservations made by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose reservations are to be retrieved.</param>
        /// <returns>A list of ReservationDto objects representing the user's reservations.</returns>
        public async Task<IEnumerable<ReservationDto>> GetUserReservations(int userId)
        {
            var reservations = await _context.Reservations
                .Where(reservation => reservation.CustomerId == userId)
                .Select(reservation => new ReservationDto
                {
                    Id = reservation.Id,
                    CustomerId = reservation.CustomerId,
                    CheckInDate = reservation.CheckInDate,
                    CheckOutDate = reservation.CheckOutDate,
                    Guests = reservation.Guests,
                    Status = reservation.Status,
                })
                .ToListAsync();

            return reservations;
        }

        /// <summary>
        /// Retrieves detailed information about a specific reservation.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation to retrieve details for.</param>
        /// <returns>A DetailReservationDto object if found; otherwise, null.</returns>
        public async Task<DetailReservationDto> GetReservationDetails(int reservationId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.ReservationRooms)
                    .ThenInclude(rr => rr.Room)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
            {
                return null; // Or throw an exception if preferred
            }

            var detailReservationDto = new DetailReservationDto
            {
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                Guests = reservation.Guests,
                ReservedRooms = reservation.ReservationRooms.Select(rr => new RoomDto
                {
                    Id = rr.Room.Id,
                    Number = rr.Room.Number,
                    Floor = rr.Room.Floor,
                    Type = rr.Room.Type,
                    Amenities = rr.Room.Amenities,
                    Price = rr.Room.Price,
                    IsAvailable = true
                }).ToList()
            };

            return detailReservationDto;
        }

        /// <summary>
        /// Cancels a reservation if it exists and belongs to the specified user.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation to cancel.</param>
        /// <param name="userId">The ID of the user attempting to cancel the reservation.</param>
        /// <returns>True if the reservation is successfully canceled; otherwise, false.</returns>
        public async Task<bool> CancelReservation(int reservationId, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var reservation = await _context.Reservations.FindAsync(reservationId);
                if (reservation == null || reservation.CustomerId != userId)
                {
                    return false; // Reservation does not exist or does not belong to the user
                }

                var reservationRooms = _context.ReservationRooms.Where(rr => rr.ReservationId == reservationId);
                _context.ReservationRooms.RemoveRange(reservationRooms);

                await _context.SaveChangesAsync();

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
