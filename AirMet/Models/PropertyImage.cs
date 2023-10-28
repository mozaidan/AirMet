namespace AirMet.Models
{
    public class PropertyImage
    {
        // Unique identifier for an image
        public int Id { get; set; }

        // URL where the image is stored in images files
        public string ImageUrl { get; set; } = string.Empty;

        // Foreign key to the associated Property
        public int PropertyId { get; set; }

        // Navigation property to the associated Property
        public virtual Property? Property { get; set; }
    }
}

