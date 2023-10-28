using AirMet.Models;

namespace AirMet.DAL
{
    // Defines the interface for porperty-related database operations
	public interface IPropertyRepository
	{
        // Retrieves all properties from the database
		Task<IEnumerable<Property>?> GetAll();
        Task<IEnumerable<Property>?> GetAllByTypeId(int typeId); // By specific type
        Task<IEnumerable<Property>?> GetAllByUserId(string userId); // Owned by specific user

        // Retrieves a single property
        Task<Property?> GetPropertyById(int id);

        // Create, update, delete property, add and delete images in database
		Task<bool> Create(Property property);
		Task<bool> Update(Property property);
		Task<bool> Delete(int id);
        Task<bool> AddNewImages(int propertyId, List<PropertyImage> newImages);
        Task<int> DeleteImage(int id);

        // Retrieves property types from database
        Task<PType?> GetPType(int id);
        Task<IEnumerable<PType>?> GetAllTypes();

        // Retrieves All amenities, remove and add in database
        Task<IEnumerable<Amenity>?> GetAllAmenities();
        Task<bool> RemoveAmenitiesForProperty(int propertyId);
        Task<bool> AddAmenitiesToProperty(int propertyId, List<Amenity> selectedAmenities);

        // Retrieves customer
        Task<Customer?> Customer(string customerId);
    }
}

