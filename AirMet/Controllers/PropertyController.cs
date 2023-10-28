using Microsoft.AspNetCore.Mvc;
using AirMet.Models;
using AirMet.ViewModels;
using AirMet.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace AirMet.Controllers
{
    // Controller for managing properties
    public class PropertyController : Controller
    {
        // Dependency injection of required services
        private readonly IPropertyRepository _propertyRepository;
        private readonly ILogger<PropertyController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public PropertyController(IPropertyRepository propertyRepository, ILogger<PropertyController> logger, UserManager<IdentityUser> userManager)
        {
            _propertyRepository = propertyRepository;
            _logger = logger;
            _userManager = userManager;
        }

        // Display details of a specific property
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the property details using the given id
            var property = await _propertyRepository.GetPropertyById(id);
            if (property == null)
            {
                // Log and return error if property not found
                _logger.LogError("[PropertyController] property not found for the PropertyId {PropertyId:0000}", id);
                return NotFound("Property not found for the PropertyId");
            }
            return View(property);
        }

        // GET: Create new property, only accessible for authorized users
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            // Fetch all property types and amenities
            var PTypes = await _propertyRepository.GetAllTypes() ?? new List<PType>();
            var Amenities = await _propertyRepository.GetAllAmenities() ?? new List<Amenity>();
            if (PTypes == null || Amenities == null)
            {
                // Log warning and return NotFound if any of these are not found
                _logger.LogWarning("[PropertyController] PTypes or Amenities Not found!");
                return NotFound("PTypes or Amenities Not found!");
            }
            else
            {
                // Prepare the view model for creating property
                var createPropertyViewModel = new CreatePropertyViewModel
                {
                    Property = new Property(),
                    PTypeSelectList = PTypes.Select(PType => new SelectListItem
                    {
                        Value = PType.PTypeId.ToString(),
                        Text = PType.PTypeId.ToString() + ": " + PType.PTypeName
                    }).ToList(),
                    Amenities = (List<Amenity>)Amenities
                };
                return View(createPropertyViewModel);
            }
            
        }


        // POST: Create new property, only accessible for authorized users
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Property property, List<Amenity> Amenities)
        {
            try
            {
                // Get the user id and fetch the customer data of the currently logged-in user
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    _logger.LogWarning("[PropertyController] User Not found!");
                    return NotFound("User not found!");// Handle null userId
                }
                Customer? customer = await _propertyRepository.Customer(userId);
                if (customer == null)
                {
                    _logger.LogWarning("[PropertyController] User Not found!");
                    return NotFound("User not found!");// Handle null userId
                }

                // Handle uploaded image files
                if (property.Files != null)
                {
                    // Define the path for storing uploaded images
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    // If the directory does not exist, create a new one
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    // Initialize a list to store property images URLs
                    List<PropertyImage> images = new();

                    // Save each uploaded file
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

                    // Add the saved images to the property object
                    property.Images = images;
                }

                // Initialize a new property with the collected data
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

                // Fetch the property type associated with the new property
                newProperty.PType = await _propertyRepository.GetPType(newProperty.PTypeId) ?? new PType();
                // Filter the list of amenities selected by the user
                var selectedAmenities = Amenities.Where(a => a.IsChecked).ToList();

                // Add the property to the database and check the return status
                bool returnOk = await _propertyRepository.Create(newProperty);
                if (returnOk)
                {
                    bool amenityReturnOk = await _propertyRepository.AddAmenitiesToProperty(newProperty.PropertyId, selectedAmenities);
                    if (!amenityReturnOk)
                    {
                        _logger.LogWarning("[PropertyController] Failed to add amenities to property.");
                    }
                    return RedirectToAction("List", "Customer");
                }
                else
                {
                    // Prepare data for the view in case of failure
                    var PTypes = await _propertyRepository.GetAllTypes() ?? new List<PType>();
                    var AmenitiesList = await _propertyRepository.GetAllAmenities() ?? new List<Amenity>();
                    if (PTypes == null || Amenities == null)
                    {
                        _logger.LogWarning("[PropertyController] PTypes or Amenities Not found!");
                        return NotFound("PTypes or Amenities Not found!");
                    }
                    else
                    {
                        var createPropertyViewModel = new CreatePropertyViewModel
                        {
                            Property = new Property(),
                            PTypeSelectList = PTypes.Select(PType => new SelectListItem
                            {
                                Value = PType.PTypeId.ToString(),
                                Text = PType.PTypeId.ToString() + ": " + PType.PTypeName
                            }).ToList(),
                            Amenities = (List<Amenity>)AmenitiesList
                        };
                        _logger.LogWarning("[PropertyController] Property creation failed {@property}", newProperty);
                        return View(createPropertyViewModel);
                    }
                }
            }
            catch (Exception)
            {
                // Log any validation errors from the ModelState
                var errors = ModelState.SelectMany(x => x.Value?.Errors?.Select(p => p.ErrorMessage) ?? Enumerable.Empty<string>()).ToList();
                _logger.LogWarning("[PropertyController] Model State is not valid. Errors: {@errors}", errors);
                return BadRequest("Property creation failed.");
            }
            
        }


        // GET: Fetch property details for updating
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            // Fetch property details by id
            var property = await _propertyRepository.GetPropertyById(id);
            // Check if property exists
            if (property == null)
            {
                _logger.LogError("[PropertyController] property not found for the PropertyId {PropertyId:0000}", id);
                return NotFound("Property not found for the PropertyId");
            }

            // Fetch all amenities and property types
            var Amenities = await _propertyRepository.GetAllAmenities() ?? new List<Amenity>();
            var PTypes = await _propertyRepository.GetAllTypes() ?? new List<PType>();
            if (PTypes == null || Amenities == null)
            {
                _logger.LogWarning("[PropertyController] PTypes or Amenities Not found!");
                return NotFound("PTypes or Amenities Not found!");
            }

            // Create a ViewModel for updating the property
            var updatePropertyViewModel = new CreatePropertyViewModel
            {
                Property = property,
                PTypeSelectList = PTypes.Select(PType => new SelectListItem
                {
                    Value = PType.PTypeId.ToString(),
                    Text = PType.PTypeId.ToString() + ": " + PType.PTypeName
                }).ToList(),
                Amenities = (List<Amenity>)Amenities
            };

            return View(updatePropertyViewModel);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int id, CreatePropertyViewModel updatePropertyViewModel)
        {
            try
            {
                // Fetch property by id
                var property = await _propertyRepository.GetPropertyById(id);
                if (property == null)
                {
                    _logger.LogError("[PropertyController] Property not found for the PropertyId {PropertyId:0000}", id);
                    return NotFound("Property not found for the PropertyId");
                }

                // Update property details from ViewModel
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
                        // Create new PropertyImage and add to list
                        var newImage = new PropertyImage { ImageUrl = $"/images/{fileName}", PropertyId = property.PropertyId };
                        newImages.Add(newImage);
                    }
                    // Add new images to the property
                    await _propertyRepository.AddNewImages(property.PropertyId, newImages);
                }
                // Manage Amenities
                // Remove existing amenities and add selected amenities
                await _propertyRepository.RemoveAmenitiesForProperty(property.PropertyId);
                var checkedAmenities = updatePropertyViewModel.Amenities.Where(a => a.IsChecked).ToList();
                await _propertyRepository.AddAmenitiesToProperty(property.PropertyId, checkedAmenities);

                // Update the property and check for success
                var result = await _propertyRepository.Update(property);
                if (result)
                {
                    return RedirectToAction("List", "Customer");
                }
                else
                {
                    // Handle failed update: Reload the Update view with existing data
                    // (Similar to the GET Update action above)
                    // Log a warning, prepare the ViewModel, and return the view
                    var Amenities = await _propertyRepository.GetAllAmenities() ?? new List<Amenity>();
                    var PTypes = await _propertyRepository.GetAllTypes() ?? new List<PType>();
                    if (PTypes == null || Amenities == null)
                    {
                        _logger.LogWarning("[PropertyController] PTypes or Amenities Not found!");
                        return NotFound("PTypes or Amenities Not found!");
                    }

                    updatePropertyViewModel = new CreatePropertyViewModel
                    {
                        Property = property,
                        PTypeSelectList = PTypes.Select(PType => new SelectListItem
                        {
                            Value = PType.PTypeId.ToString(),
                            Text = PType.PTypeId.ToString() + ": " + PType.PTypeName
                        }).ToList(),
                        Amenities = (List<Amenity>)Amenities
                    };

                    _logger.LogWarning("[PropertyController] Property update failed {@property}", updatePropertyViewModel);
                    return View(updatePropertyViewModel);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("[PropertyController] Error updating property: {Error}", e.Message);
                return BadRequest("Failed to update property.");
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteImage(int id)
        {
            // Delete the image by id and get the result
            int result = await _propertyRepository.DeleteImage(id);
            if (result == -1)
            {
                _logger.LogError("[PropertyController] Image not found when deleting the PropertyId {PropertyId:0000}", id);
                return BadRequest("Property not found for the PropertyId");
            }

            // Redirect back to the Update view
            return RedirectToAction("Update", new { id = result });
        }


        // GET: Delete a property
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            // Fetch the property using the given id
            var property = await _propertyRepository.GetPropertyById(id);
            if (property == null)
            {
                // Log and return error if property not found
                _logger.LogError("[PropertyController] Property not found for the PropertyId {PropertyId:0000}", id);
                return BadRequest("Property not found for the PropertyId");

            }
            return View(property);
        }

        // POST: Confirm the deletion
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Delete the property and check the result
            bool returnOk = await _propertyRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[PropertyController] Property deletion failed for the PropertyId {PropertyId:0000}", id);
                return BadRequest("Property deletion failed");
            }
            return RedirectToAction("List","Customer");
        }
    }
}

