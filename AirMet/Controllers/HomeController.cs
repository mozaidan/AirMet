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

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            List<Property>? properties = await _propertyRepository.GetAll() as List<Property>;
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
            List<Property>? properties = await _propertyRepository.GetAllByTypeId(typeId) as List<Property>;
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

        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = _userManager.GetUserId(User);
            Customer? customerInfo = await _propertyRepository.Customer(userId);
            List<Property>? properties = await _propertyRepository.GetAllByUserId(userId) as List<Property>;
            var itemListViewModel = new PropertyListViewModel(properties, "List", customerInfo);
            return View(itemListViewModel);
        }


    }
}

