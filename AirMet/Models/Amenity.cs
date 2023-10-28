using System.ComponentModel.DataAnnotations;

namespace AirMet.Models
{
	public class Amenity
	{
        // Unique identifier for an amenity
        public int AmenityId { get; set; }

        // Navigation property to hold the associated PropertyAmenities
        public virtual ICollection<PropertyAmenity>? PropertyAmenities { get; set; }

        // Name of the amenity
        [StringLength(50)]
        public string AmenityName { get; set; } = string.Empty;

        // Icon representing the amenity (font icon name)
        public string AmenityIcon { get; set; } = string.Empty;

        // Boolean to check whether the amenity is availble for a property
        public bool IsChecked { get; set; }
    }
}

