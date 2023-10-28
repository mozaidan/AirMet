using Microsoft.EntityFrameworkCore;
using AirMet.Models;
using Microsoft.CodeAnalysis;

namespace AirMet.DAL
{
	public class ReservationRepository : IReservationRepository
	{
        private readonly PropertyDbContext _db;
        private readonly ILogger<PropertyRepository> _logger;

        public ReservationRepository(PropertyDbContext db, ILogger<PropertyRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Retrieves all reservations for a specific user
        public async Task<IEnumerable<Reservation>?> GetReservationsByUserId(string userId)
        {
            try
            {
                var reservations = await _db.Reservations.Where(r => r.UserId == userId).ToListAsync();
                return reservations;
            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationController] Reservations List failed for GetReservationsByUserId for User {userId:0000}, error message: {e}", userId, e.Message);
                return null;
            }

        }

        // Retrieves all reservations for a specific property
        public async Task<IEnumerable<Reservation>?> GetReservationsByPropertyId(int propertyId)
        {
            try
            {
                return await _db.Reservations.Where(r => r.PropertyId == propertyId).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationController] Reservations List failed for GetReservationsByPropertyId for PropertyId {PropertyId:0000}, error message: {e}", propertyId, e.Message);
                return null;
            }
        }



        // Adds a new reservation to database
        public async Task<bool> Add(Reservation reservation)
        {
            try
            {
                _db.Reservations.Add(reservation);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationRepository] reservation creation failed for reservation {@reservation}, error message: {e}", reservation, e.Message);
                return false;
            }
        }

        // Updates an existing reservation
        public async Task<bool> Update(Reservation reservation)
        {
            try
            {
                _db.Reservations.Update(reservation);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationController] Reservation Update(reservation) failed when updating the Reservation {Reservation:0000}, error message: {e}", reservation, e.Message);
                return false;
            }
        }

        // Deletes a reservation from the database
        public async Task<bool> Delete(int id)
        {
            try
            {
                var reservation = await _db.Reservations.FindAsync(id);
                if (reservation == null)
                {
                    _logger.LogError("[ReservationController] Reservation deletion failed for ReservationId {ReservationId:0000}", id);
                    return false;
                }

                _db.Reservations.Remove(reservation);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationRepository] Reservation deletion failed for ReservationId {ReservationId:0000}, error message: {e}", id, e.Message);
                return false;
            }
        }



        // Retrieves a reservation by its id
        public async Task<Reservation?> GetReservationById(int reservationId)
        {
            try
            {
                var reservation = await _db.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
                return reservation;
            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationController] Reservation failed for GetReservationById for reservationId {ReservationId:0000}, error message: {e}", reservationId, e.Message);
                return null;
            }
        }

        // Retrieves a reservation for a given user and property id's
        public async Task<Reservation?> GetReservationByUserIdAndPropertyId(string userId, int propertyId)
        {
            try
            {
                return await _db.Reservations.Where(r => r.UserId == userId && r.PropertyId == propertyId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationController] Reservation failed for GetReservationByUserIdAndPropertyId for UserId {UserId:0000}, PropertyId {PropertyId:0000}, error message: {e}", userId, propertyId, e.Message);
                return null;
            }
        }



        // Retrieves the property associated with a given reservation id
        public async Task<Property?> GetPropertyByReservationId(int reservationId)
        {
            try
            {
                var reservation = await _db.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
                if (reservation == null)
                {
                    _logger.LogError("[ReservationController] Reservation FirstOrDefault(id) failed for ReservationId {ReservationId:0000}", reservationId);
                    return null;
                }
                return await _db.Properties.Include(p => p.Images).FirstOrDefaultAsync(i => i.PropertyId == reservation.PropertyId);

            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationController] Property FirstOrDefaultAsync(id) failed when GetPropertyByReservationId for ReservationId {ReservationId:0000}, error message: {e}", reservationId, e.Message);
                return null;
            }
        }

        // Retrieves the property by its id
        public async Task<Property?> GetPropertyById(int id)
        {
            try
            {
                return await _db.Properties.Include(p => p.Images).FirstOrDefaultAsync(i => i.PropertyId == id);

            }
            catch (Exception e)
            {
                _logger.LogError("[PropertyRepository] property FirstOrDefaultAsync(id) failed when GetItemById for PropertyId {PropertyId:0000}, error message: {e}", id, e.Message);
                return null;
            }
        }



        // Retrieves the customer associated with a given reservation id
        public async Task<Customer?> GetCustomerByReservationId(int reservationId)
        {
            try
            {
                return await _db.Customers.Where(p => p.ReservationId == reservationId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationRepository] customer FirstOrDefaultAsync() failed when GetCustomerByReservationId for reservationId {ReservationId:0000}, error message: {e}", reservationId, e.Message);
                return null;
            }
        }

        // Retrieves a customer by their customer id
        public async Task<Customer?> Customer(string customerId)
        {
            try
            {
                Customer? customer = await _db.Customers.Where(p => p.CustomerId == customerId).FirstOrDefaultAsync();
                if (customer == null)
                {
                    _logger.LogError("[ReservationRepository] customer not found for customerId {customerId:0000}, error message: {e}", customerId);
                    return null;
                }
                return customer;

            }
            catch (Exception e)
            {
                _logger.LogError("[ReservationRepository] customer FirstOrDefaultAsync() failed when Customer for customerId {CustomerId:0000}, error message: {e}", customerId, e.Message);
                return null;
            }
        }
    }
}

