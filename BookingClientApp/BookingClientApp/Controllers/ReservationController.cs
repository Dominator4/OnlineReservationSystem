using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingClientApp.Models;
using BookingClientApp.Services;

namespace BookingClientApp.Controllers
{
public class ReservationController : Controller
{
    private readonly ReservationServiceClient _reservationServiceClient;

    public ReservationController(ReservationServiceClient reservationServiceClient)
    {
        _reservationServiceClient = reservationServiceClient;
    }

    [HttpGet]
    public IActionResult CheckAvailability()
    {
        return View(new AvailableRoomsViewModel());
    }

    [HttpPost]
   public async Task<IActionResult> CheckAvailability(AvailableRoomsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var availableRooms = await _reservationServiceClient.GetAvailableRooms(model.CheckInDate, model.CheckOutDate);
        model.AvailableRooms = availableRooms;
        return View(model);
    }

/*    [HttpGet]
    public IActionResult MakeReservation(int roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        var model = new ReservationViewModel
        {
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate
        };
        return View(model);
    }
*/

    [HttpPost]
    public async Task<IActionResult> MakeReservation(ReservationViewModel model)
    {
            if (!ModelState.IsValid)
        {
                //return View(model);
                ViewData["Message"] = "To mój komunikat!";
                return View("err");
            }

        var response = await _reservationServiceClient.MakeReservation(model);
            int statusCode = (int)response.StatusCode;
            if (statusCode != 400)
        {
                return View("UserProfile");
//            return RedirectToAction("ReservationConfirmation");
        }
            
//            ModelState.AddModelError("", "Nie udało się dokonać rezerwacji.");
            //return View(model);
            var responseContent = await response.Content.ReadAsStringAsync();
            ViewData["Message"] = responseContent;
            return View("err");
        }

[HttpGet]
public async Task<IActionResult> UserReservations()
{
    var reservations = await _reservationServiceClient.GetUserReservations();
    return View(reservations);
}

[HttpGet("details/{id}")]
public async Task<IActionResult> GetReservationDetails(int id)
{
    var reservationDetails = await _reservationServiceClient.GetReservationDetails(id);
    if (reservationDetails == null)
    {
        return NotFound();
    }
    return View(reservationDetails); // Widok ze szczegółami rezerwacji
}


}
}