using System;
using AirMet.Models;

namespace AirMet.DAL
{
	public interface IReservationRepository
	{
        Task<IEnumerable<Reservation>?> GetReservationsByUserId(string userId);
        Task<IEnumerable<Reservation>?> GetReservationsByPropertyId(int propertyId);


        Task<bool> Add(Reservation reservation);
        Task<bool> Update(Reservation reservation);
        Task<bool> Delete(int id);

        
        Task<Reservation?> GetReservationById(int reservationId);
        Task<Reservation?> GetReservationByUserIdAndPropertyId(string userId, int propertyId);


        Task<Property?> GetPropertyByReservationId(int reservationId);
        Task<Property?> GetPropertyById(int id);


        Task<Customer?> GetCustomerByReservationId(int reservationId);
        Task<Customer?> Customer(string customerId);
    }
}

