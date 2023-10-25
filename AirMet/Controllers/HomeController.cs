using Microsoft.AspNetCore.Mvc;
using AirMet.Models;
using AirMet.ViewModels;
using AirMet.DAL;
using Microsoft.AspNetCore.Identity;

namespace AirMet.Controllers {

    public class HomeController : Controller
    {

        private readonly IPropertyRepository _propertyRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(IPropertyRepository propertyRepository, ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _propertyRepository = propertyRepository;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<Property>? properties = (List<Property>?)await _propertyRepository.GetAll();
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
        public async Task<IActionResult> PropertyTypes(int typeId)
        {
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
        public IActionResult Aboutus()
        {
            return View();
        }

       


    }
}

