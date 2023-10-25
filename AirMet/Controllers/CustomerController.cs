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
        private readonly IPropertyRepository _propertyRepository;
        private readonly ILogger<PropertyController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public CustomerController(IPropertyRepository propertyRepository, ILogger<PropertyController> logger, UserManager<IdentityUser> userManager)
        {
            _propertyRepository = propertyRepository;
            _logger = logger;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                _logger.LogWarning("[CustomerController] User Not found!");
                return NotFound("User not found!");// Handle null userId
            }
            Customer? customerInfo = await _propertyRepository.Customer(userId);
            List<Property>? properties = (List<Property>?)await _propertyRepository.GetAllByUserId(userId);
            if (properties == null)
            {
                _logger.LogWarning("[CustomerController] property list not found while executing _propertyRepository.GetAllByUserId()");
                
            }
            var itemListViewModel = new PropertyListViewModel(properties, "List", customerInfo);
            return View(itemListViewModel);
        }

    }
}

