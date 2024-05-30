using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using BookingClientApp.Models;

namespace BookingClientApp.Services
{
    /// <summary>
    /// Handles communication with the ReservationService for operations such as retrieving, creating, and updating reservations.
    /// </summary>
    public class ReservationServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the ReservationServiceClient class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used for making requests.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor used for retrieving the current HTTP context.</param>
        public ReservationServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves the bearer token from the HTTP context cookies.
        /// </summary>
        /// <returns>The bearer token if found; otherwise, null.</returns>
        private string GetBearerToken()
        {
            var tokenJson = _httpContextAccessor.HttpContext.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(tokenJson))
            {
                var tokenObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenJson);
                if (tokenObj != null && tokenObj.ContainsKey("token"))
                {
                    return tokenObj["token"];
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves the user information from the reservation service.
        /// </summary>
        /// <returns>The user information if the request is successful; otherwise, null.</returns>
        public async Task<UserViewModel> GetUser()
        {
            var token = GetBearerToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync("reservation/Check-User");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(json);
            return user;
        }

        /// <summary>
        /// Updates the user information in the reservation service.
        /// </summary>
        /// <param name="model">The user information to update.</param>
        /// <returns>True if the update is successful; otherwise, false.</returns>
        public async Task<bool> UpdateUser(UserViewModel model)
        {
            var token = GetBearerToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("reservation/update-user", content);

            // Check if the operation was successful
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Retrieves a list of available rooms for the given date range.
        /// </summary>
        /// <param name="checkIn">The check-in date.</param>
        /// <param name="checkOut">The check-out date.</param>
        /// <returns>A list of available rooms.</returns>
        public async Task<IEnumerable<Room>> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            var token = GetBearerToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var requestContent = new StringContent(JsonConvert.SerializeObject(new { CheckInDate = checkIn, CheckOutDate = checkOut }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("reservation/check-availability", requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var rooms = JsonConvert.DeserializeObject<IEnumerable<Room>>(responseContent);

            return rooms;
        }

        /// <summary>
        /// Creates a new reservation with the provided reservation details.
        /// </summary>
        /// <param name="model">The reservation details.</param>
        /// <returns>The HTTP response message from the reservation service.</returns>
        public async Task<HttpResponseMessage> MakeReservation(ReservationViewModel model)
        {
            var token = GetBearerToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var requestContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("reservation/make-reservation", requestContent);

            return response;
        }

        /// <summary>
        /// Retrieves the list of reservations made by the user.
        /// </summary>
        /// <returns>A list of the user's reservations.</returns>
        public async Task<IEnumerable<ReservationViewModel>> GetUserReservations()
        {
            var token = GetBearerToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync("reservation/user-reservations");

            var responseContent = await response.Content.ReadAsStringAsync();
            var reservations = JsonConvert.DeserializeObject<IEnumerable<ReservationViewModel>>(responseContent);

            return reservations;
        }

        /// <summary>
        /// Retrieves the details of a specific reservation.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>The reservation details if found; otherwise, null.</returns>
        public async Task<DetailReservationViewModel> GetReservationDetails(int id)
        {
            var token = GetBearerToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync($"reservation/details/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await response.Content.ReadAsStringAsync();
            var reservationDetails = JsonConvert.DeserializeObject<DetailReservationViewModel>(json);
            return reservationDetails;
        }

        /// <summary>
        /// Cancels a specific reservation.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>True if the cancellation is successful; otherwise, false.</returns>
        public async Task<bool> CancelReservation(int id)
        {
            var token = GetBearerToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.DeleteAsync($"reservation/cancel/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
