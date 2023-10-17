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
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AirMet.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly ILogger<ReservationController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private DateTime startDate;
        private DateTime endDate;

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
            startDate = reservationDate; // Initialize the start date
            return View(new { PropertyId = propertyId, StartDate = reservationDate, NumberOfGuests = numberOfGuests });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Reserve(int propertyId, DateTime reservationDate, int numberOfGuests, DateTime endReservationDate)
        {
            startDate = reservationDate;
            endDate = endReservationDate; // Initialize the end date
            var userId = _userManager.GetUserId(User);

            var existingReservations = _propertyRepository.GetReservationsByPropertyId(propertyId);
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
                StartDate = startDate,
                EndDate = endDate,
                NumberOfGuests = numberOfGuests
            };

            // Save the reservation to your data store (e.g., a database)
            _propertyRepository.Add(reservation); // Implement _reservationRepository accordingly

            return RedirectToAction("Reservation"); // Redirect to the property list page after a successful reservation.
        }

        [Authorize]
        public async Task<IActionResult> Reservation()
        {
            var userId = _userManager.GetUserId(User);

            // Retrieve the user's reservations
            var reservations = await _propertyRepository.GetReservationsByUserId(userId);

            // Create a view model and populate it
            var viewModel = new ReservationsListViewModel(reservations)
            {
                Reservations = reservations
            };

            return View(viewModel);
        }

        [HttpGet]
        public JsonResult GetUnavailableDates(int propertyId)
        {
            var reservations = _propertyRepository.GetReservationsByPropertyId(propertyId);
            var unavailableDates = reservations.SelectMany(r => Enumerable.Range(0, (r.EndDate - r.StartDate).Days + 1).Select(offset => r.StartDate.AddDays(offset).ToString("yyyy-MM-dd"))).ToList();
            return Json(new { UnavailableDates = unavailableDates });
        }



    }
}

