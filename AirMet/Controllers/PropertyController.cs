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
            //ViewBag.ReservationMessage = TempData["ReservationMessage"]?.ToString();
            return View(property);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var PTypes = await _propertyRepository.GetAllTypes() ?? new List<PType>();
            var Amenities = await _propertyRepository.GetAllAmenities() ?? new List<Amenity>();
            var createPropertyViewModel = new CreatePropertyViewModel
            {
                Property = new Property(),
                PTypeSelectList = PTypes.Select(PType => new SelectListItem
                {
                    Value = PType.PTypeId.ToString(),
                    Text = PType.PTypeId.ToString() + ": " + PType.PTypeName
                }).ToList(),
                Amenities = Amenities
            };
            return View(createPropertyViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Property property, List<Amenity> Amenities)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                Customer? customer = await _propertyRepository.Customer(userId);
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
                var selectedAmenities = Amenities.Where(a => a.IsChecked).ToList();

                var newProperty = new Property
                {
                    UserId = userId,
                    Customer = customer,
                    Title = property.Title,
                    Price = property.Price,
                    Address = property.Address,
                    Description = property.Description,
                    Guest = property.Guest,
                    Bed = property.Bed,
                    BedRooms = property.BedRooms,
                    BathRooms = property.BathRooms,
                    PTypeId = property.PTypeId,
                    Images = property.Images

                };
                newProperty.PType = await _propertyRepository.GetPType(newProperty.PTypeId) ?? new PType();

                bool returnOk = await _propertyRepository.Create(newProperty);
                if (returnOk)
                {
                    bool amenityReturnOk = await _propertyRepository.AddAmenitiesToProperty(newProperty.PropertyId, selectedAmenities);
                    if (!amenityReturnOk)
                    {
                        _logger.LogWarning("[HomeController] Failed to add amenities to property.");
                    }
                    return RedirectToAction("List", "Customer");
                }
                else
                {
                    _logger.LogWarning("[HomeController] Property creation failed {@property}", newProperty);
                    return View(property); // Return to the same view with the model
                }
            }
            catch (Exception)
            {
                var errors = ModelState.SelectMany(x => x.Value?.Errors?.Select(p => p.ErrorMessage) ?? Enumerable.Empty<string>()).ToList();
                _logger.LogWarning("[HomeController] Model State is not valid. Errors: {@errors}", errors);
                return BadRequest("OrderItem creation failed.");
            }
            
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var property = await _propertyRepository.GetItemById(id);
            var Amenities = await _propertyRepository.GetAllAmenities() ?? new List<Amenity>();
            if (property == null)
            {
                return NotFound();
            }

            var PTypes = await _propertyRepository.GetAllTypes() ?? new List<PType>();
            var updatePropertyViewModel = new CreatePropertyViewModel
            {
                Property = property,
                PTypeSelectList = PTypes.Select(PType => new SelectListItem
                {
                    Value = PType.PTypeId.ToString(),
                    Text = PType.PTypeId.ToString() + ": " + PType.PTypeName
                }).ToList(),
                Amenities = Amenities
            };

            return View(updatePropertyViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int id, CreatePropertyViewModel updatePropertyViewModel)
        {
            try
            {
                var property = await _propertyRepository.GetItemById(id);
                if (property == null)
                {
                    _logger.LogError("[PropertyController] Property not found for the PropertyId {PropertyId:0000}", id);
                    return NotFound("Property not found for the PropertyId");
                }
                property.Title = updatePropertyViewModel.Property.Title;
                property.Price = updatePropertyViewModel.Property.Price;
                property.Address = updatePropertyViewModel.Property.Address;
                property.Description = updatePropertyViewModel.Property.Description;
                property.Guest = updatePropertyViewModel.Property.Guest;
                property.Bed = updatePropertyViewModel.Property.Bed;
                property.BedRooms = updatePropertyViewModel.Property.BedRooms;
                property.BathRooms = updatePropertyViewModel.Property.BathRooms;
                property.PTypeId = updatePropertyViewModel.Property.PTypeId;

                // If there are new files/images uploaded
                if (updatePropertyViewModel.Property.Files != null && updatePropertyViewModel.Property.Files.Count > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var newImages = new List<PropertyImage>();

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    // Save the new images
                    foreach (var file in updatePropertyViewModel.Property.Files)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploads, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        var newImage = new PropertyImage { ImageUrl = $"/images/{fileName}", PropertyId = property.PropertyId };
                        newImages.Add(newImage);
                    }
                    await _propertyRepository.AddNewImages(property.PropertyId, newImages);
                }
                // Manage Amenities
                // First, remove all existing property-amenity relationships for this property.
                await _propertyRepository.RemoveAmenitiesForProperty(property.PropertyId);
                // Then, re-add the checked amenities.
                var checkedAmenities = updatePropertyViewModel.Amenities.Where(a => a.IsChecked).ToList();
                await _propertyRepository.AddAmenitiesToProperty(property.PropertyId, checkedAmenities);

                var result = await _propertyRepository.Update(property);

                if (result)
                {
                    return RedirectToAction("List", "Customer");
                }
                else
                {
                    _logger.LogWarning("[HomeController] Property update failed {@property}", updatePropertyViewModel);
                    return View(updatePropertyViewModel);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("[HomeController] Error updating property: {Error}", e.Message);
                return BadRequest("Failed to update property.");
            }
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
            return RedirectToAction("List","Customer");
        }
    }
}

