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
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AirMet.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly ILogger<PropertyController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public PropertyController(IPropertyRepository propertyRepository, ILogger<PropertyController> logger, UserManager<IdentityUser> userManager)
        {
            _propertyRepository = propertyRepository;
            _logger = logger;
            _userManager = userManager;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Details(int id)
        {
            var property = await _propertyRepository.GetItemById(id);
            if (property == null)
            {
                _logger.LogError("[HomeController] property not found for the PropertyId {PropertyId:0000}", id);
                return NotFound("Property not found for the PropertyId");
            }
            return View(property);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
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
                property.UserId = _userManager.GetUserId(User);

                bool returnOk = await _propertyRepository.Create(property);
                if (returnOk)
                    return RedirectToAction("List", "Home");
            }
            _logger.LogWarning("[HomeController] Property creation failed {@property}", property);
            return View(property);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var property = await _propertyRepository.GetItemById(id);
            if (property == null)
            {
                _logger.LogWarning("[HomeController] Property not found when updating the PropertyId {PropertyId:0000}", id);
                return BadRequest("Property not found for the PropertyId");
            }
            return View(property);
        }

        [HttpPost]
        [Authorize]
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

                return RedirectToAction("List", "Home");
            }
            _logger.LogWarning("[HomeController] Property update failed {@updatedProperty}", updatedProperty);
            return View(updatedProperty);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteImage(int id)
        {
            int result = await _propertyRepository.DeleteImage(id);
            if (result == -1)
            {
                _logger.LogError("[HomeController] Image not found when deleting the PropertyId {PropertyId:0000}", id);
                return BadRequest("Property not found for the PropertyId");
            }

            // Redirect back to the Update view
            // Note: You might need to find a way to get the PropertyId if it's needed for the redirect.
            return RedirectToAction("Update", new { id = result });
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _propertyRepository.GetItemById(id);
            if (property == null)
            {
                _logger.LogError("[HomeController] Property not found for the PropertyId {PropertyId:0000}", id);
                return BadRequest("Property not found for the PropertyId");

            }
            return View(property);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool returnOk = await _propertyRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[HomeController] Property deletion failed for the PropertyId {PropertyId:0000}", id);
                return BadRequest("Property deletion failed");
            }
            return RedirectToAction("List","Home");
        }
    }
}

