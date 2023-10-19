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
<<<<<<< HEAD
=======
using Microsoft.EntityFrameworkCore.Metadata.Internal;
>>>>>>> 86b410a596466e0daea38b2558ff038226c5088f
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
<<<<<<< HEAD

            var existingReservations = _propertyRepository.GetReservationsByPropertyId(propertyId);
=======
            if (numberOfGuests > property.Guest)
            {
                TempData["GuestMessage"] = "The number of guests is more than available!";
                return RedirectToAction("Details", "Property", new { id = propertyId });
            }
            var existingReservations = await _propertyRepository.GetReservationsByPropertyId(propertyId);
>>>>>>> 86b410a596466e0daea38b2558ff038226c5088f
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
          
<<<<<<< HEAD


=======
>>>>>>> 86b410a596466e0daea38b2558ff038226c5088f
            var days = (reservation.EndDate - reservation.StartDate).Days;
            reservation.TotalDays = days;
            reservation.Property = property;
            reservation.TotalPrice = property.Price * days;
<<<<<<< HEAD
=======
            
>>>>>>> 86b410a596466e0daea38b2558ff038226c5088f

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
<<<<<<< HEAD
            var viewModel = new ReservationsListViewModel(reservations)
            {
                Reservations = reservations
            };
=======
            var viewModel = new ReservationsListViewModel(reservations, "Reservstion");
>>>>>>> 86b410a596466e0daea38b2558ff038226c5088f

            return View(viewModel);
        }

        [HttpGet]
<<<<<<< HEAD
        public JsonResult GetUnavailableDates(int propertyId)
        {
            var reservations = _propertyRepository.GetReservationsByPropertyId(propertyId);
=======
        public async Task<JsonResult> GetUnavailableDates(int propertyId)
        {
            var reservations = await _propertyRepository.GetReservationsByPropertyId(propertyId);
>>>>>>> 86b410a596466e0daea38b2558ff038226c5088f
            var unavailableDates = reservations.SelectMany(r => Enumerable.Range(0, (r.EndDate - r.StartDate).Days + 1).Select(offset => r.StartDate.AddDays(offset).ToString("yyyy-MM-dd"))).ToList();
            return Json(new { UnavailableDates = unavailableDates });
        }

<<<<<<< HEAD


=======
>>>>>>> 86b410a596466e0daea38b2558ff038226c5088f
    }
}

