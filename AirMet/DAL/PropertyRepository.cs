using System;
using Microsoft.EntityFrameworkCore;
using AirMet.Models;

namespace AirMet.DAL
{
	public class PropertyRepository : IPropertyRepository
	{
		private readonly PropertyDbContext _db;

		public PropertyRepository(PropertyDbContext db)
		{
			_db = db;
		}
		public async Task<IEnumerable<Property>> GetAll()
		{
			return await _db.Properties.Include(p => p.Images).ToListAsync();
        }
        public async Task<Property?> GetItemById(int id)
		{
			return await _db.Properties.Include(p => p.Images).FirstOrDefaultAsync(i => i.PropertyId == id);
        }
		public async Task Create(Property property)
		{
			_db.Properties.Add(property);
			await _db.SaveChangesAsync();
		}
		public async Task Update(Property property)
		{
			_db.Properties.Update(property);
			await _db.SaveChangesAsync();
		}
        public async Task AddNewImages(int propertyId, List<PropertyImage> newImages)
        {
            await _db.PropertyImages.AddRangeAsync(newImages);
            await _db.SaveChangesAsync();
        }
        public async Task<int> DeleteImage(int id)
        {
            var image = await _db.PropertyImages.FirstOrDefaultAsync(i => i.Id == id);
            if (image == null)
            {
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

        public async Task<bool> Delete(int id)
		{
			var property = await _db.Properties.FindAsync(id);
			if (property == null)
			{
				return false;
			}

			_db.Properties.Remove(property);
			await _db.SaveChangesAsync();
			return true;
		}
	}
}

