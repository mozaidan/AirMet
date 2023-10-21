using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AirMet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirMet.ViewModels;
using Microsoft.EntityFrameworkCore;
using AirMet.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AirMet.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly ILogger<ReservationController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        

        public ReservationController(IPropertyRepository propertyRepository, ILogger<ReservationController> logger, UserManager<IdentityUser> userManager
            )
        {

            _propertyRepository = propertyRepository;
            _logger = logger;
            _userManager = userManager;
        }
        // GET: /<controller>/
        [HttpGet]
        [Authorize]
        public IActionResult Reserve(int propertyId, DateTime reservationDate, int numberOfGuests)
        {
            var startDate = reservationDate; // Initialize the start date
            return View(new { PropertyId = propertyId, StartDate = reservationDate, NumberOfGuests = numberOfGuests });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Reserve(int propertyId, DateTime reservationDate, int numberOfGuests, DateTime endReservationDate)
        {
            var startDate = reservationDate;
            var endDate = endReservationDate; // Initialize the end date
            var userId = _userManager.GetUserId(User);
            var property = await _propertyRepository.GetItemById(propertyId);
            if (property == null)
            {
                // Handle error: property not found
                return RedirectToAction("ErrorPage"); // Replace with your actual error page
            }
            if (numberOfGuests > property.Guest)
            {
                TempData["GuestMessage"] = "The number of guests is more than available!";
                return RedirectToAction("Details", "Property", new { id = propertyId });
            }
            var existingReservations = await _propertyRepository.GetReservationsByPropertyId(propertyId);
            foreach (var res in existingReservations)
            {
                if ((startDate <= res.EndDate && endDate >= res.StartDate))
                {
                    TempData["ReservationMessage"] = "The chosen date is unavailable for this property, please choose another date!";
                    return RedirectToAction("Details", "Property", new { id = propertyId });
                }
            }
            // Create and save a reservation
            var reservation = new Reservation
            {
                UserId = userId,
                PropertyId = propertyId,
                StartDate = reservationDate,
                EndDate = endReservationDate,
                NumberOfGuests = numberOfGuests,
            };
          
            var days = (reservation.EndDate - reservation.StartDate).Days;
            reservation.TotalDays = days;
            reservation.Property = property;
            reservation.TotalPrice = property.Price * days;
            

            // Save the reservation to your data store (e.g., a database)
            _ = _propertyRepository.Add(reservation); // Implement _reservationRepository accordingly

            return RedirectToAction("Reservation"); // Redirect to the reservation list page after a successful reservation.
        }

        
        


        [Authorize]
        public async Task<IActionResult> Reservation()
        {
            var userId = _userManager.GetUserId(User);

            // Retrieve the user's reservations
            var reservations = await _propertyRepository.GetReservationsByUserId(userId);

            // Create a view model and populate it
            var viewModel = new ReservationsListViewModel(reservations, "Reservstion");

            return View(viewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetUnavailableDates(int propertyId)
        {
            var reservations = await _propertyRepository.GetReservationsByPropertyId(propertyId);
            var unavailableDates = reservations.SelectMany(r => Enumerable.Range(0, (r.EndDate - r.StartDate).Days + 1).Select(offset => r.StartDate.AddDays(offset).ToString("yyyy-MM-dd"))).ToList();
            return Json(new { UnavailableDates = unavailableDates });
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var reservation = await _propertyRepository.GetReservationById(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Reservation reservation, int id)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("ErrorPage");// Handle null userId
                }
                var property = await _propertyRepository.GetItemById(id);
                if (property == null)
                {
                    // Handle error: property not found
                    return RedirectToAction("ErrorPage"); // Replace with your actual error page
                }
                var reservationFromDb = await _propertyRepository.GetReservationByUserIdAndPropertyId(userId, property.PropertyId);
                if (reservationFromDb == null)
                {
                    // Handle error: reservation not found
                    return RedirectToAction("ErrorPage");
                }
                if (reservation.NumberOfGuests > property.Guest)
                {
                    TempData["GuestMessage"] = "The number of guests is more than available!";
                    return RedirectToAction("Update", "Reservation", new { id = reservationFromDb.ReservationId });
                }
                var existingReservations = await _propertyRepository.GetReservationsByPropertyId(reservation.PropertyId);
                foreach (var res in existingReservations)
                {
                    if ((reservation.StartDate <= res.EndDate && reservation.EndDate >= res.StartDate))
                    {
                        TempData["ReservationMessage"] = "The chosen date is unavailable for this property, please choose another date!";
                        return RedirectToAction("Update", "Reservation", new { id = reservation.ReservationId });
                    }
                }
                reservationFromDb.StartDate = reservation.StartDate;
                reservationFromDb.EndDate = reservation.EndDate;
                reservationFromDb.NumberOfGuests = reservation.NumberOfGuests;

                var days = (reservationFromDb.EndDate - reservationFromDb.StartDate).Days;
                reservationFromDb.TotalDays = days;
                reservationFromDb.Property = property;
                reservationFromDb.TotalPrice = property.Price * days;
                var result = await _propertyRepository.UpdateReservation(reservationFromDb);

                if (result)
                {
                    return RedirectToAction("Reservation");
                }
                else
                {
                    _logger.LogWarning("[ReservationController] Reservation update failed {@reservation}", reservationFromDb);
                    return View(reservationFromDb);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationController] Error updating reservation: {Error}", e.Message);
                return BadRequest("Failed to update reservation.");
            }

        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _propertyRepository.GetReservationById(id);
            if (reservation == null)
            {
                _logger.LogError("[ReservationController] Reservation not found for the ReservationId {ReservationId:0000}", id);
                return BadRequest("Reservation not found for the ReservationId");

            }
            return View(reservation);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool returnOk = await _propertyRepository.DeleteReservation(id);
            if (!returnOk)
            {
                _logger.LogError("[HomeController] Property deletion failed for the PropertyId {PropertyId:0000}", id);
                return BadRequest("Property deletion failed");
            }
            return RedirectToAction(nameof(Reservation));
        }
    }
}

