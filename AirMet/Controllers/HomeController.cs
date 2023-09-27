using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AirMet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirMet.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AirMet.Controllers {

    public class HomeController : Controller
    {

        private readonly PropertyDbContext _propertyDbContext;

        public HomeController(PropertyDbContext propertyDbContext)
        {
            _propertyDbContext = propertyDbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Property> properties = _propertyDbContext.Properties.ToList();
            var itemListViewModel = new PropertyListViewModel(properties, "Index");
            return View(itemListViewModel);
        }
        public IActionResult Aboutus()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            //List<Item> items = await _itemDbContext.Items.ToListAsync();
            var item =  _propertyDbContext.Properties.FirstOrDefault(i => i.PropertyId == id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Property property)
        {
            if (ModelState.IsValid)
            {
                _propertyDbContext.Properties.Add(property);
                _propertyDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(property);
        }

    }
}

