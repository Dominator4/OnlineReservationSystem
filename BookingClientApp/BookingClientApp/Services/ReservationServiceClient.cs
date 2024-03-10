using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookingClientApp.Models;

namespace BookingClientApp.Services
{
/*    public interface IReservationServiceClient
    {
        Task<IEnumerable<Room>> GetAvailableRooms(DateTime checkIn, DateTime checkOut);
        Task<bool> MakeReservation(ReservationViewModel model);
    }
*/

    public class ReservationServiceClient
    {
        private readonly HttpClient _httpClient;

        public ReservationServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Room>> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            return null;// Wywołanie do ReservationManagementService aby pobrać dostępne pokoje
        }

        public async Task<bool> MakeReservation(ReservationViewModel model)
        {
            return true;// Wywołanie do ReservationManagementService aby dokonać rezerwacji
        }
    }
}