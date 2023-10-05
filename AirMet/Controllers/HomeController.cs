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
        public async Task<IActionResult> Index()
        {
            List<Property> properties = await _propertyDbContext.Properties.Include(p => p.Images).ToListAsync();
            var itemListViewModel = new PropertyListViewModel(properties, "Index");
            return View(itemListViewModel);
        }
        public IActionResult Aboutus()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var property = await _propertyDbContext.Properties.Include(p => p.Images).FirstOrDefaultAsync(i => i.PropertyId == id);
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

                _propertyDbContext.Properties.Add(property);
                await _propertyDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(property);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var property = await _propertyDbContext.Properties.Include(p => p.Images).FirstOrDefaultAsync(i => i.PropertyId == id);
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
                var propertyFromDb = _propertyDbContext.Properties.Include(p => p.Images)
                                      .FirstOrDefault(i => i.PropertyId == updatedProperty.PropertyId);

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
                        _propertyDbContext.PropertyImages.Add(newImage);
                        propertyFromDb.Images.Add(newImage);
                    }
                }

                _propertyDbContext.Properties.Update(propertyFromDb);
                await _propertyDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(updatedProperty);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _propertyDbContext.PropertyImages.FirstOrDefaultAsync(i => i.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            // Delete the image file from the server
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            // Delete the image record from the database
            _propertyDbContext.PropertyImages.Remove(image);
            _propertyDbContext.SaveChanges();

            // Redirect back to the Update view
            return RedirectToAction("Update", new { id = image.PropertyId });
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _propertyDbContext.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _propertyDbContext.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            _propertyDbContext.Properties.Remove(property);
            await _propertyDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        


    }
}

