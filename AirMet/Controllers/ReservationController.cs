using Microsoft.AspNetCore.Mvc;
using AirMet.Models;
using AirMet.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AirMet.Controllers
{
    // Controller for handling reservations
    public class ReservationController : Controller
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ILogger<ReservationController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        
        // Constructor
        public ReservationController(IReservationRepository reservationRepository, ILogger<ReservationController> logger, UserManager<IdentityUser> userManager)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
            _userManager = userManager;
        }

        // List reservations for a property
        [Authorize]
        public async Task<IActionResult> ListReservations(int id)
        {
            // Get reservations based on Property id
            List<Reservation>? reservations = (List<Reservation>?)await _reservationRepository.GetReservationsByPropertyId(id);
            if (reservations == null)
            {
                _logger.LogError("[HomeController] property list not found while executing _propertyRepository.GetAll()");
            }

            // If there is no reservation, it will return empty table
            Customer? customerInfo = null;
            var viewModel = new ReservationsListViewModel(reservations, "ListReservations", customerInfo);

            // Fetch customer information and populate ViewModel if reservations exists
            if (reservations != null)
            {
                foreach (var reservation in reservations)
                {
                    customerInfo = await _reservationRepository.GetCustomerByReservationId(reservation.ReservationId);  // Fetch customer info if user is logged in
                }
                viewModel = new ReservationsListViewModel(reservations, "ListReservations", customerInfo);
            }
            
            return View(viewModel);
        }

        // View details of a reservation
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _reservationRepository.GetReservationById(id);
            if (reservation == null)
            {
                _logger.LogWarning("[ReservationController] reservation not found while executing _reservationRepository.GetReservationById()", id);
                return NotFound("Reservation not found!");
            }
            return View(reservation);
        }

        // Initiate the reservation process
        [HttpGet]
        [Authorize]
        public IActionResult Reserve(int propertyId, DateTime reservationDate, int numberOfGuests)
        {
            return View(new { PropertyId = propertyId, StartDate = reservationDate, NumberOfGuests = numberOfGuests });
        }

        // Complete the reservation (POST)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Reserve(int propertyId, DateTime reservationDate, int numberOfGuests, DateTime endReservationDate)
        {
            try
            {
                // Validate and prepare reservation data
                var startDate = reservationDate;
                var endDate = endReservationDate; // Initialize the end date

                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    _logger.LogWarning("[ReservationController] User Not found!");
                    return NotFound("User not found!");// Handle null userId
                }
                Customer? customer = await _reservationRepository.Customer(userId);
                if (customer == null)
                {
                    _logger.LogWarning("[PropertyController] User Not found!");
                    return NotFound("User not found!");// Handle null userId
                }

                var property = await _reservationRepository.GetPropertyById(propertyId);
                if (property == null)
                {
                    _logger.LogError("[ReservationController] property not found for the PropertyId {PropertyId:0000}", propertyId);
                    return NotFound("Property not found for the PropertyId");
                }


                if (numberOfGuests > property.Guest)
                {
                    TempData["GuestMessage"] = "The number of guests is more than available!";
                    return RedirectToAction("Details", "Property", new { id = propertyId });
                }

                var existingReservations = await _reservationRepository.GetReservationsByPropertyId(propertyId);
                if (existingReservations != null)
                {
                    foreach (var res in existingReservations)
                    {
                        if ((startDate <= res.EndDate && endDate >= res.StartDate))
                        {
                            TempData["ReservationMessage"] = "The chosen date is unavailable for this property, please choose another date!";
                            return RedirectToAction("Details", "Property", new { id = propertyId });
                        }
                    }
                }

                // Create and save a reservation
                var reservation = new Reservation
                {
                    UserId = userId,
                    Customer = customer,
                    PropertyId = propertyId,
                    StartDate = reservationDate,
                    EndDate = endReservationDate,
                    NumberOfGuests = numberOfGuests,
                };

                var days = (reservation.EndDate - reservation.StartDate).Days;
                reservation.TotalDays = days;
                reservation.Property = property;
                reservation.TotalPrice = property.Price * days;


                // Save the reservation to database
                _ = _reservationRepository.Add(reservation); // Implement _reservationRepository accordingly

                return RedirectToAction("Reservation"); // Redirect to the reservation list page after a successful reservation.
            }
            catch
            {
                var errors = ModelState.SelectMany(x => x.Value?.Errors?.Select(p => p.ErrorMessage) ?? Enumerable.Empty<string>()).ToList();
                _logger.LogWarning("[ReservationController] Model State is not valid. Errors: {@errors}", errors);
                return BadRequest("Reservation creation failed.");
            }
        }

        
        // View user's own reservations
        [Authorize]
        public async Task<IActionResult> Reservation()
        {
            var userId = _userManager.GetUserId(User);
            Customer? customer = await _reservationRepository.Customer(userId);
            if (customer == null)
            {
                _logger.LogWarning("[ReservationController] Customer info not found");
                return NotFound("Customer Not found");
            }
            // Retrieve the user's reservations
            var reservations = await _reservationRepository.GetReservationsByUserId(userId);

            // Create a view model and populate it
            var viewModel = new ReservationsListViewModel((List<Reservation>?)reservations, "Reservstion", customer);

            return View(viewModel);
        }

        // Get unavailable dates for a property
        [HttpGet]
        public async Task<JsonResult> GetUnavailableDates(int propertyId)
        {
            var reservations = await _reservationRepository.GetReservationsByPropertyId(propertyId);
            if (reservations != null)
            {
                var unavailableDates = reservations.SelectMany(r => Enumerable.Range(0, (r.EndDate - r.StartDate).Days + 1).Select(offset => r.StartDate.AddDays(offset).ToString("yyyy-MM-dd"))).ToList();
                return Json(new { UnavailableDates = unavailableDates });
            }
            else
            {
                return Json(new { UnavailableDates = Array.Empty<string>() });
            }
        }

        // Update a reservation (GET)
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var reservation = await _reservationRepository.GetReservationById(id);
            if (reservation == null)
            {
                _logger.LogWarning("[ReservationController] reservation not found while executing _reservationRepository.GetReservationById()", id);
                return NotFound("Reservation not found!");
            }
            return View(reservation);
        }

        // Update a reservation (POST)
        [HttpPost]
        public async Task<IActionResult> Update(Reservation reservation, int id)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    _logger.LogWarning("[ReservationController] User Not found!");
                    return NotFound("User not found!");// Handle null userId
                }

                var property = await _reservationRepository.GetPropertyById(id);
                if (property == null)
                {
                    _logger.LogError("[ReservationController] property not found for the PropertyId {PropertyId:0000}", id);
                    return NotFound("Property not found for the PropertyId");
                }

                // Fetch the reservation from the database using the user id and property id
                var reservationFromDb = await _reservationRepository.GetReservationByUserIdAndPropertyId(userId, property.PropertyId);
                if (reservationFromDb == null)
                {
                    _logger.LogWarning("[ReservationController] reservation not found while executing _reservationRepository.GetReservationByUserIdAndPropertyId()", property.PropertyId);
                    return NotFound("Reservation not found!");
                }

                if (reservation.NumberOfGuests > property.Guest)
                {
                    TempData["GuestMessage"] = "The number of guests is more than available!";
                    return RedirectToAction("Update", "Reservation", new { id = reservationFromDb.ReservationId });
                }

                var existingReservations = await _reservationRepository.GetReservationsByPropertyId(reservation.PropertyId);
                if (existingReservations != null)
                {
                    foreach (var res in existingReservations)
                    {
                        if ((reservation.StartDate <= res.EndDate && reservation.EndDate >= res.StartDate))
                        {
                            TempData["ReservationMessage"] = "The chosen date is unavailable for this property, please choose another date!";
                            return RedirectToAction("Update", "Reservation", new { id = reservation.ReservationId });
                        }
                    }
                }

                // Update the reservation fields
                reservationFromDb.StartDate = reservation.StartDate;
                reservationFromDb.EndDate = reservation.EndDate;
                reservationFromDb.NumberOfGuests = reservation.NumberOfGuests;

                // Calculate the total days and price for the reservation
                var days = (reservationFromDb.EndDate - reservationFromDb.StartDate).Days;
                reservationFromDb.TotalDays = days;
                reservationFromDb.Property = property;
                reservationFromDb.TotalPrice = property.Price * days;


                var result = await _reservationRepository.Update(reservationFromDb);

                if (result)
                {
                    return RedirectToAction(nameof(Reservation));
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

        // Delete a reservation (GET)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationRepository.GetReservationById(id);
            if (reservation == null)
            {
                _logger.LogError("[ReservationController] Reservation not found for the ReservationId {ReservationId:0000}", id);
                return BadRequest("Reservation not found for the ReservationId");

            }
            return View(reservation);
        }

        // Delete a reservation (POST)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool returnOk = await _reservationRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[ReservationController] Reservation deletion failed for the ReservationId {ReservationId:0000}", id);
                return BadRequest("Reservation deletion failed");
            }
            return RedirectToAction(nameof(Reservation));
        }
    }
}

