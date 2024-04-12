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
        public async Task<IActionResult> UserProfile()
        {
            var user = await _reservationServiceClient.GetUser();
            if (user != null)
            {
                // Jeśli są dane, to wyświetl je w widoku UserProfile
                return View(user); // Przekazuje model do widoku
            }
            else
            {
                // Nie ma danych, więc przekierowuje do akcji z wypełnianiem danych i wysyłaniem ich na serwer
                return RedirectToAction("CompleteProfile");
            }
        }
        /*
                [HttpGet]
                public IActionResult CompleteProfile()
                {
                    return View(new UserViewModel());
                }
        */

        [HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            //            if (userId.HasValue)

            var user = await _reservationServiceClient.GetUser();
                if (user != null)
                {
                    return View(user);
                }
            
            return View(new UserViewModel()); // Nowy profil, jeśli nie ma userId lub użytkownika nie znaleziono
        }

        [HttpPost]
        public async Task<IActionResult> CompleteProfile(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Wywołanie metody UpdateUser z ReservationServiceClient
            var updateSuccess = await _reservationServiceClient.UpdateUser(model);

            if (updateSuccess)
            {
                // Po pomyślnym zaktualizowaniu danych, przekieruj do UserProfile
                return RedirectToAction("UserProfile");
            }
            else
            {
                // W przypadku niepowodzenia, ponownie wyświetl formularz z komunikatem o błędzie
                //return RedirectToAction("CheckAvailability");
                //ModelState.AddModelError("", "Nie udało się zaktualizować profilu. Spróbuj ponownie.");
                return View(model);
            }
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
                return RedirectToAction("UserProfile");
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
            try
            {
                var reservations = await _reservationServiceClient.GetUserReservations();
                return View(reservations);
            } catch {
                return RedirectToAction("UserProfile");
            }
            //            return View("UserReservations");//
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetReservationDetails(int id)
        {
                    
            var reservationDetails = await _reservationServiceClient.GetReservationDetails(id);
            if (reservationDetails == null)
            {
                return NotFound();
            }
            return View(reservationDetails);
        }

        [HttpPost]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var result = await _reservationServiceClient.CancelReservation(id);
            if (result)
            {
                // Przekieruj do widoku z listą rezerwacji
                return RedirectToAction("UserReservations");
            }
            else
            {
                // Obsłuż błąd
                return View("UserProfile");
        //        return View("Error");
            }
        }



    }
}