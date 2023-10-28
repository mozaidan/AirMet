using AirMet.Models;

namespace AirMet.DAL
{
    // Interface for Reservation repository to abstract databse operations related to reservations
	public interface IReservationRepository
	{
        // Restrieves all reservations
        Task<IEnumerable<Reservation>?> GetReservationsByUserId(string userId);
        Task<IEnumerable<Reservation>?> GetReservationsByPropertyId(int propertyId);

        // Add, Update and Delete a reservation
        Task<bool> Add(Reservation reservation);
        Task<bool> Update(Reservation reservation);
        Task<bool> Delete(int id);

        // Retrieves a single reservation
        Task<Reservation?> GetReservationById(int reservationId);
        Task<Reservation?> GetReservationByUserIdAndPropertyId(string userId, int propertyId);

        // Retrieves a the property
        Task<Property?> GetPropertyByReservationId(int reservationId);
        Task<Property?> GetPropertyById(int id);

        // Retrieves the Customer
        Task<Customer?> GetCustomerByReservationId(int reservationId);
        Task<Customer?> Customer(string customerId);
    }
}

