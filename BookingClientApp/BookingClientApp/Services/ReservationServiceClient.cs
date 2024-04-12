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


    public class ReservationServiceClient
    {
        private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReservationServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

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

    public async Task< UserViewModel > GetUser()
{
        var token = GetBearerToken();
        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

    var response = await _httpClient.GetAsync($"reservation/Check-User");
    if (!response.IsSuccessStatusCode)
    {
        return null;
    }
    var json = await response.Content.ReadAsStringAsync();
    var user = JsonConvert.DeserializeObject< UserViewModel >(json);
    return user;
}


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

        // Sprawdzenie, czy operacja zakończyła się sukcesem
        return response.IsSuccessStatusCode;
    }

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