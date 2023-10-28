using Microsoft.AspNetCore.Mvc;
using AirMet.Models;
using AirMet.ViewModels;
using AirMet.DAL;
using Microsoft.AspNetCore.Identity;

namespace AirMet.Controllers {

    public class HomeController : Controller
    {
        // Dependency injection repository, logger, and uaser manager
        // _propertyRepository: to interact with property-related data
        private readonly IPropertyRepository _propertyRepository;

        // _logger: to log information, warnings and errors
        private readonly ILogger<HomeController> _logger;

        // _userManager: to manage user identity, and check the user
        private readonly UserManager<IdentityUser> _userManager;


        // Constructor receives dependencies and initializes fields
        public HomeController(IPropertyRepository propertyRepository, ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _propertyRepository = propertyRepository;
            _logger = logger;
            _userManager = userManager;
        }

        // Action for the home page
        // Asynchronously fetches and displays a list of all properties
        public async Task<IActionResult> Index()
        {
            // Fetch all properties from the repository
            List<Property>? properties = (List<Property>?)await _propertyRepository.GetAll();

            // If properties couldn't be fetched, log an error and return a NotFound result
            if (properties == null)
            {
                _logger.LogError("[HomeController] property list not found while executing _propertyRepository.GetAll()");
                return NotFound("Properties list not found!");
            }

            // Initialize customer information as null, incase there is no one logged in
            Customer? customerInfo = null;
            // Try to get the current user's ID
            var userId = _userManager.GetUserId(User);
            // If a user is logged in, fetch their customer information
            if (userId != null)
            {
                customerInfo = await _propertyRepository.Customer(userId);  // Fetch customer info if user is logged in
            }

            // Create and return a view with the list of properties and customer info
            var itemListViewModel = new PropertyListViewModel(properties, "Index", customerInfo);
            return View(itemListViewModel);
        }

        // Action to get properties by type
        // Asynchronously fetches and displays a list of properties filtered by typeId
        public async Task<IActionResult> PropertyTypes(int typeId)
        {
            // Fetch properties by typeId from the repository
            List<Property>? properties = (List<Property>?)await _propertyRepository.GetAllByTypeId(typeId);
            if (properties == null)
            {
                _logger.LogError("[HomeController] property list not found while executing _propertyRepository.GetAll()");
                return NotFound("Properties list not found!");
            }
            Customer? customerInfo = null;
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                customerInfo = await _propertyRepository.Customer(userId);  // Fetch customer info if user is logged in
            }
            var itemListViewModel = new PropertyListViewModel(properties, "Index", customerInfo);
            return View(itemListViewModel);
        }

        // Displays the "About Us" page
        public IActionResult Aboutus()
        {
            return View();
        }

    }
}

