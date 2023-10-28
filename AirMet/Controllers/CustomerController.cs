using Microsoft.AspNetCore.Mvc;
using AirMet.Models;
using AirMet.ViewModels;
using AirMet.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace AirMet.Controllers
{
    public class CustomerController : Controller
    {
        // Dependency injection for repository, logger and userManager
        private readonly IPropertyRepository _propertyRepository;
        private readonly ILogger<PropertyController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor
        public CustomerController(IPropertyRepository propertyRepository, ILogger<PropertyController> logger, UserManager<IdentityUser> userManager)
        {
            _propertyRepository = propertyRepository;
            _logger = logger;
            _userManager = userManager;
        }

        // Authorizes the user before displaying the list
        [Authorize]
        public async Task<IActionResult> List()
        {
            // Retrive the id of the currently authenticated user
            var userId = _userManager.GetUserId(User);

            // Check if the user id exists
            if (userId == null)
            {
                _logger.LogWarning("[CustomerController] User Not found!");
                return NotFound("User not found!");// Handle null userId
            }

            // Retrieve customer information using user id
            Customer? customerInfo = await _propertyRepository.Customer(userId);
            List<Property>? properties = (List<Property>?)await _propertyRepository.GetAllByUserId(userId);
            if (properties == null)
            {
                _logger.LogWarning("[CustomerController] property list not found while executing _propertyRepository.GetAllByUserId()");
            }

            // Create the ViewModel for the View
            var itemListViewModel = new PropertyListViewModel(properties, "List", customerInfo);
            return View(itemListViewModel);
        }

    }
}

