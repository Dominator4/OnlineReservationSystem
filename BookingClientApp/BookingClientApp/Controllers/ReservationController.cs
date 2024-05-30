using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingClientApp.Models;
using BookingClientApp.Services;

namespace BookingClientApp.Controllers
{
    /// <summary>
    /// Handles reservation actions such as checking availability, making reservations, and managing user profiles.
    /// </summary>
    public class ReservationController : Controller
    {
        private readonly ReservationServiceClient _reservationServiceClient;

        /// <summary>
        /// Initializes a new instance of the ReservationController class.
        /// </summary>
        /// <param name="reservationServiceClient">The reservation service client used for interacting with the reservation service.</param>
        public ReservationController(ReservationServiceClient reservationServiceClient)
        {
            _reservationServiceClient = reservationServiceClient;
        }

        /// <summary>
        /// Displays the user profile.
        /// </summary>
        /// <returns>The user profile view if the user exists; otherwise, redirects to the CompleteProfile action.</returns>
        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var user = await _reservationServiceClient.GetUser();
            if (user != null)
            {
                return View(user); // Pass the model to the view
            }
            else
            {
                return RedirectToAction("CompleteProfile");
            }
        }

        /// <summary>
        /// Displays the complete profile view for updating user information.
        /// </summary>
        /// <returns>The complete profile view.</returns>
        [HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            var user = await _reservationServiceClient.GetUser();
            if (user != null)
            {
                return View(user);
            }
            return View(new UserViewModel()); // New profile if user not found
        }

        /// <summary>
        /// Handles the complete profile process for updating user information.
        /// </summary>
        /// <param name="model">The user view model containing user details.</param>
        /// <returns>Redirects to the UserProfile action if successful; otherwise, returns the complete profile view with an error message.</returns>
        [HttpPost]
        public async Task<IActionResult> CompleteProfile(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updateSuccess = await _reservationServiceClient.UpdateUser(model);

            if (updateSuccess)
            {
                return RedirectToAction("UserProfile");
            }
            else
            {
                ModelState.AddModelError("", "Failed to update profile. Please try again.");
                return View(model);
            }
        }

        /// <summary>
        /// Displays the check availability view.
        /// </summary>
        /// <returns>The check availability view.</returns>
        [HttpGet]
        public IActionResult CheckAvailability()
        {
            return View(new AvailableRoomsViewModel());
        }

        /// <summary>
        /// Handles the check availability process.
        /// </summary>
        /// <param name="model">The available rooms view model containing the check-in and check-out dates.</param>
        /// <returns>The check availability view with available rooms if successful.</returns>
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

        /// <summary>
        /// Handles the make reservation process.
        /// </summary>
        /// <param name="model">The reservation view model containing reservation details.</param>
        /// <returns>Redirects to the UserProfile action if successful; otherwise, returns an error view with a message.</returns>
        [HttpPost]
        public async Task<IActionResult> MakeReservation(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Validation failed.";
                return View("Error");
            }

            var response = await _reservationServiceClient.MakeReservation(model);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("UserProfile");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            ViewData["Message"] = responseContent;
            return View("Error");
        }

        /// <summary>
        /// Displays the user's reservations.
        /// </summary>
        /// <returns>The user reservations view.</returns>
        [HttpGet]
        public async Task<IActionResult> UserReservations()
        {
            try
            {
                var reservations = await _reservationServiceClient.GetUserReservations();
                return View(reservations);
            }
            catch
            {
                return RedirectToAction("UserProfile");
            }
        }

        /// <summary>
        /// Retrieves the details of a specific reservation.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>The reservation details view if found; otherwise, returns NotFound.</returns>
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

        /// <summary>
        /// Cancels a specific reservation.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>Redirects to the UserReservations action if successful; otherwise, returns the UserProfile view.</returns>
        [HttpPost]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var result = await _reservationServiceClient.CancelReservation(id);
            if (result)
            {
                return RedirectToAction("UserReservations");
            }
            else
            {
                return View("UserProfile");
            }
        }
    }
}
