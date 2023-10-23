using System;
namespace AirMet.Models
{
	public class Amenity
	{
        public int AmenityId { get; set; }
        public virtual ICollection<PropertyAmenity>? PropertyAmenities { get; set; }
        public string AmenityName { get; set; } = string.Empty;
        public string AmenityIcon { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
    }
}

