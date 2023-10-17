using System;
using AirMet.Models;

namespace AirMet.DAL
{
	public interface IPropertyRepository
	{
		Task<IEnumerable<Property>?> GetAll();
		Task<Property?> GetItemById(int id);
		Task<bool> Create(Property property);
		Task<bool> Update(Property property);
		Task<bool> Delete(int id);
        Task<bool> AddNewImages(int propertyId, List<PropertyImage> newImages);
        Task<int> DeleteImage(int id);
        Task<List<Property>> GetAllByUserId(string userId);
		Task<Customer?> Customer(string customerId);
        Reservation GetReservationById(int reservationId);

        Task<bool> AddReservation(Reservation reservation);
        Task<bool> UpdateReservation(Reservation reservation);
        Task<bool> DeleteReservation(int reservationId);
        Task<bool> Add(Reservation reservation);
        List<Reservation> GetReservationsByUserId(string userId);
        List<Reservation> GetReservationsByDate(DateTime date);
        List<Reservation> GetReservationsByNumberOfGuests(int numberOfGuests);

    }
}

