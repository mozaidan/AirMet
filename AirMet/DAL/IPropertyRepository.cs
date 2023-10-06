using System;
using AirMet.Models;

namespace AirMet.DAL
{
	public interface IPropertyRepository
	{
		Task<IEnumerable<Property>> GetAll();
		Task<Property?> GetItemById(int id);
		Task Create(Property property);
		Task Update(Property property);
		Task<bool> Delete(int id);
        Task AddNewImages(int propertyId, List<PropertyImage> newImages);
        Task<int> DeleteImage(int id);
    }
}

