using System;
namespace AirMet.Models
{
	public class PropertyAmenity
	{
        public int PropertyAmenityId { get; set; }
        public int PropertyId { get; set; }
        public virtual Property? Property { get; set; }

        public int AmenityId { get; set; }
        public virtual Amenity? Amenity { get; set; }
    }
}

