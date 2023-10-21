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
using Castle.Core.Resource;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            Customer? customerInfo = await _propertyRepository.Customer(userId);
            List<Property>? properties = await _propertyRepository.GetAllByUserId(userId) as List<Property>;
            var itemListViewModel = new PropertyListViewModel(properties, "List", customerInfo);
            return View(itemListViewModel);
        }

    }
}

