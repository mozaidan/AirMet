using System;
using AirMet.Models;

namespace AirMet.DAL
{
	public interface IPropertyRepository
	{
		Task<IEnumerable<Property>?> GetAll();
        Task<IEnumerable<Property>?> GetAllByTypeId(int typeId);
        Task<bool> Save();
		Task<Property?> GetItemById(int id);
        Task<Property?> GetPropertyByReservationId(int reservationId);
		Task<bool> Create(Property property);
		Task<bool> Update(Property property);
		Task<bool> Delete(int id);
        Task<bool> AddNewImages(int propertyId, List<PropertyImage> newImages);
        Task<int> DeleteImage(int id);
        Task<List<Property>> GetAllByUserId(string userId);
		Task<Customer?> Customer(string customerId);
        Task<Customer?> GetCustomerByReservationId(int reservationId);
        Task<PType?> GetPType(int id);
        Task<IEnumerable<PType>?> GetAllTypes();
        Task<Reservation?> GetReservationById(int reservationId);
        Task<bool> Add(Reservation reservation);
        Task<List<Reservation>> GetReservationsByUserId(string userId);
        Task<bool> UpdateReservation(Reservation reservation);
        Task<bool> DeleteReservation(int id);
        Task<IEnumerable<Reservation>> GetReservationsByPropertyId(int propertyId);
        Task<Reservation?> GetReservationByUserIdAndPropertyId(string userId, int propertyId);
        Task<List<Amenity>> GetAllAmenities();
        Task<bool> RemoveAmenitiesForProperty(int propertyId);
        Task<bool> AddAmenitiesToProperty(int propertyId, List<Amenity> selectedAmenities);
    }
}

