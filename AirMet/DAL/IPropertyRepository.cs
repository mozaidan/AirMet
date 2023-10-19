using System;
using AirMet.Models;

namespace AirMet.DAL
{
	public interface IPropertyRepository
	{
		Task<IEnumerable<Property>?> GetAll();
        Task<IEnumerable<Property>?> GetAllByTypeId(int typeId);
		Task<Property?> GetItemById(int id);
		Task<bool> Create(Property property);
		Task<bool> Update(Property property);
		Task<bool> Delete(int id);
        Task<bool> AddNewImages(int propertyId, List<PropertyImage> newImages);
        Task<int> DeleteImage(int id);
        Task<List<Property>> GetAllByUserId(string userId);
		Task<Customer?> Customer(string customerId);
<<<<<<< HEAD

        Task<Reservation?> GetReservationById(int reservationId);
        Task<bool> AddReservation(Reservation reservation);
        Task<bool> UpdateReservation(Reservation reservation);
        Task<bool> DeleteReservation(int reservationId);
        Task<bool> Add(Reservation reservation);
        Task<List<Reservation>> GetReservationsByUserId(string userId);
        IEnumerable<Reservation> GetReservationsByPropertyId(int propertyId);      
=======
        Task<PType?> GetPType(int id);
        Task<IEnumerable<PType>?> GetAllTypes();

        Task<Reservation?> GetReservationById(int reservationId);
        Task<bool> Add(Reservation reservation);
        Task<List<Reservation>> GetReservationsByUserId(string userId);
        Task<IEnumerable<Reservation>> GetReservationsByPropertyId(int propertyId);      
>>>>>>> 86b410a596466e0daea38b2558ff038226c5088f

    }
}

