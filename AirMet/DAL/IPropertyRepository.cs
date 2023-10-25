using AirMet.Models;

namespace AirMet.DAL
{
	public interface IPropertyRepository
	{
		Task<IEnumerable<Property>?> GetAll();
        Task<IEnumerable<Property>?> GetAllByTypeId(int typeId);
        Task<IEnumerable<Property>?> GetAllByUserId(string userId);

        Task<Property?> GetPropertyById(int id);

		Task<bool> Create(Property property);
		Task<bool> Update(Property property);
		Task<bool> Delete(int id);
        Task<bool> AddNewImages(int propertyId, List<PropertyImage> newImages);
        Task<int> DeleteImage(int id);

        Task<PType?> GetPType(int id);
        Task<IEnumerable<PType>?> GetAllTypes();
       
        Task<IEnumerable<Amenity>?> GetAllAmenities();
        Task<bool> RemoveAmenitiesForProperty(int propertyId);
        Task<bool> AddAmenitiesToProperty(int propertyId, List<Amenity> selectedAmenities);

        
        Task<Customer?> Customer(string customerId);
    }
}

