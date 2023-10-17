using System;
using Microsoft.EntityFrameworkCore;
using AirMet.Models;

namespace AirMet.DAL
{
	public class PropertyRepository : IPropertyRepository
	{
		private readonly PropertyDbContext _db;

		private readonly ILogger<PropertyRepository> _logger;

		public PropertyRepository(PropertyDbContext db, ILogger<PropertyRepository> logger)
		{
			_db = db;
			_logger = logger;
		}
		public async Task<IEnumerable<Property>?> GetAll()
		{
			try
			{
                return await _db.Properties.Include(p => p.Images).ToListAsync();
            }
			catch (Exception e)
			{
				_logger.LogError("[PropertyRepository] property ToListAsync() failed when GetAll(), error message: {e}", e.Message);
				return null;
			}
			
        }
		public async Task<Property?> GetItemById(int id)
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
		public async Task<bool> Create(Property property)
		{
			try
			{
				_db.Properties.Add(property);
				await _db.SaveChangesAsync();
				return true;
			}
            catch (Exception e)
            {
                _logger.LogError("[PropertyRepository] property creation failed for property {@property}, error message: {e}", property, e.Message);
                return false;
            }
        }
		public async Task<bool> Update(Property property)
		{
			try
			{
				_db.Properties.Update(property);
				await _db.SaveChangesAsync();
				return true;
			}
            catch (Exception e)
            {
                _logger.LogError("[PropertyRepository] property FirstOrDefaultAsync(id) failed when updating the PropertyId {PropertyId:0000}, error message: {e}", property, e.Message);
                return false;
            }
        }
        public async Task<bool> AddNewImages(int propertyId, List<PropertyImage> newImages)
        {
			try
			{
				await _db.PropertyImages.AddRangeAsync(newImages);
				await _db.SaveChangesAsync();
				return true;
			}
            catch (Exception e)
            {
                _logger.LogError("[PropertyRepository] property AddRangeAsync(newImages) failed when AddNewImages for PropertyId {PropertyId:0000}, error message: {e}", propertyId, e.Message);
                return false;
            }
        }
        public async Task<int> DeleteImage(int id)
        {
			try
			{
				var image = await _db.PropertyImages.FirstOrDefaultAsync(i => i.Id == id);
				if (image == null)
				{
                    _logger.LogError("[PropertyRepository] property deletion failed for PropertyId {PropertyId:0000}, error message: {e}", id);
                    return -1;
				}

				// Delete the image file from the server
				var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));
				if (System.IO.File.Exists(imagePath))
				{
					System.IO.File.Delete(imagePath);
				}

				// Delete the image record from the database
				_db.PropertyImages.Remove(image);
				await _db.SaveChangesAsync();

				return image.PropertyId;
			}
            catch (Exception e)
            {
                _logger.LogError("[PropertyRepository] property deletion failed for PropertyId {PropertyId:0000}, error message: {e}", id, e.Message);
                return -1;
            }
        }

        public async Task<bool> Delete(int id)
		{
			try
			{
				var property = await _db.Properties.FindAsync(id);
				if (property == null)
				{
                    _logger.LogError("[PropertyRepository] property deletion failed for PropertyId {PropertyId:0000}, error message: {e}", id);
                    return false;
				}

				_db.Properties.Remove(property);
				await _db.SaveChangesAsync();
				return true;
			}
            catch (Exception e)
            {
                _logger.LogError("[PropertyRepository] property deletion failed for PropertyId {PropertyId:0000}, error message: {e}", id, e.Message);
                return false;
            }
        }
        public async Task<List<Property>> GetAllByUserId(string userId)
        {
            return await _db.Properties.Where(p => p.UserId == userId).ToListAsync();
        }
		public async Task<Customer?> Customer(string customerId)
		{
			return await _db.Customers.Where(p => p.CustomerId == customerId).FirstOrDefaultAsync();
		}
        public Reservation GetReservationById(int reservationId)
        {
            var reservation = _db.Reservations.FirstOrDefault(r => r.ReservationId == reservationId);
            return reservation;
        }

        public List<Reservation> GetReservationsByUserId(string userId)
        {
            var reservations = _db.Reservations.Where(r => r.UserId == userId).ToList();
            return reservations;
        }

        public async Task<bool> AddReservation(Reservation reservation)
        {
            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateReservation(Reservation reservation)
        {
            // Implementation code to update a reservation
            _db.Reservations.Update(reservation);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteReservation(int reservationId)
        {
            // Implementation code to delete a reservation
            var reservation = await _db.Reservations.FindAsync(reservationId);
            if (reservation == null) return false;
            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync();
            return true;
        }
        public List<Reservation> GetReservationsByDate(DateTime date)
        {
            var reservations = _db.Reservations.Where(r => r.Date == date).ToList();
            return reservations;
        }

        public List<Reservation> GetReservationsByNumberOfGuests(int numberOfGuests)
        {
            var reservations = _db.Reservations.Where(r => r.NumberOfGuests == numberOfGuests).ToList();
            return reservations;
        }

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
                _logger.LogError("[PropertyRepository] reservation creation failed for reservation {@reservation}, error message: {e}", reservation, e.Message);
                return false;
            }
        }

    }
}

