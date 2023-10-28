using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirMet.Models
{
	public class Property
	{
        // Unique identifier for a property
        public int PropertyId { get; set; }

        // Unique identifier for a user
        public string UserId { get; set; } = string.Empty;

        // Navigation property for the customer
        public virtual Customer Customer { get; set; } = default!;

        public string CustomerId { get; set; } = string.Empty;

        // Price for renting/Night
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price must be greater than 0.")]
		public decimal Price { get; set; }

        // Title of the property listing
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,100}", ErrorMessage = "The Title must be numbers or letters and between 2 to 100 characters.")]
        public string Title { get; set; } = string.Empty;

        // Address of the property
        public string Address { get; set; } = string.Empty;

        // Detailed description
        [StringLength(800)]
        public string Description{ get; set; } = string.Empty;

        // Number of guest the property can accommodate, beds availble, bedrooms and bathrooms
        public int Guest { get; set; }
        public int Bed { get; set; }
        public int BedRooms { get; set; }
        public int BathRooms { get; set; }

        // Property Type ID
        public int PTypeId { get; set; }

        // Navigation property for the property type
        public virtual PType PType { get; set; } = default!;

        // List of the amanities for the property
        public virtual ICollection<PropertyAmenity>? PropertyAmenities { get; set; } = new List<PropertyAmenity>();

        // List of reservations
        public virtual List<Reservation>? Reservations { get; set; } = new List<Reservation>();

        // List of images for the property
        public virtual List<PropertyImage> Images { get; set; } = new List<PropertyImage>();

        // List of files to upload (Not mapped to database)
        [NotMapped]
        public virtual List<IFormFile>? Files { get; set; }
    }
}

