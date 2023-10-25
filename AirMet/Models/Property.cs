using System.ComponentModel.DataAnnotations.Schema;

namespace AirMet.Models
{
	public class Property
	{
        public string UserId { get; set; } = string.Empty;
        public virtual Customer Customer { get; set; } = default!;
        public string CustomerId { get; set; } = string.Empty;
        public int PropertyId { get; set; }
		public decimal Price { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description{ get; set; } = string.Empty;

        public int Guest { get; set; }
        public int Bed { get; set; }
        public int BedRooms { get; set; }
        public int BathRooms { get; set; }
        public int PTypeId { get; set; }

        public virtual PType PType { get; set; } = default!;

        public virtual ICollection<PropertyAmenity>? PropertyAmenities { get; set; } = new List<PropertyAmenity>();

        public virtual List<Reservation>? Reservations { get; set; } = new List<Reservation>();
         
        public virtual List<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        [NotMapped]
        public virtual List<IFormFile>? Files { get; set; }
    }
}

