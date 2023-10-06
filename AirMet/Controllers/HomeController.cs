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

namespace AirMet.Controllers {

    public class HomeController : Controller
    {

        private readonly IPropertyRepository _propertyRepository;

        public HomeController(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            List<Property> properties = (List<Property>)await _propertyRepository.GetAll();
            var itemListViewModel = new PropertyListViewModel(properties, "Index");
            return View(itemListViewModel);
        }
        public IActionResult Aboutus()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var property = await _propertyRepository.GetItemById(id);
            if (property == null)
                return NotFound();
            return View(property);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Property property)
        {
            if (ModelState.IsValid)
            {
                if (property.Files != null)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    List<PropertyImage> images = new List<PropertyImage>();

                    foreach (var file in property.Files)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploads, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        images.Add(new PropertyImage { ImageUrl = $"/images/{fileName}" });
                    }

                    property.Images = images;
                }

                await _propertyRepository.Create(property);
                return RedirectToAction(nameof(Index));
            }
            return View(property);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var property = await _propertyRepository.GetItemById(id);
            if (property == null)
            {
                return NotFound();
            }
            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Property updatedProperty)
        {
            if (ModelState.IsValid)
            {
                updatedProperty.PropertyId = id;

                // Fetch the existing property from the database
                var propertyFromDb = await _propertyRepository.GetItemById(updatedProperty.PropertyId);

                if (propertyFromDb == null)
                {
                    return NotFound();
                }

                // Update the properties of the existing property
                propertyFromDb.Description = updatedProperty.Description;
                propertyFromDb.Price = updatedProperty.Price;
                propertyFromDb.Address = updatedProperty.Address;

                // If there are new files/images uploaded
                if (updatedProperty.Files != null && updatedProperty.Files.Count > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var newImages = new List<PropertyImage>();

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    // Save the new images
                    foreach (var file in updatedProperty.Files)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploads, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        var newImage = new PropertyImage { ImageUrl = $"/images/{fileName}", PropertyId = propertyFromDb.PropertyId };
                        newImages.Add(newImage);
                    }
                    await _propertyRepository.AddNewImages(propertyFromDb.PropertyId, newImages);
                }

                await _propertyRepository.Update(propertyFromDb);

                return RedirectToAction(nameof(Index));
            }

            return View(updatedProperty);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int id)
        {
            int result = await _propertyRepository.DeleteImage(id);
            if (result == -1)
            {
                return NotFound();
            }

            // Redirect back to the Update view
            // Note: You might need to find a way to get the PropertyId if it's needed for the redirect.
            return RedirectToAction("Update", new {id = result});
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _propertyRepository.GetItemById(id);
            if (property == null)
            {
                return NotFound();
            }
            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _propertyRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        


    }
}

