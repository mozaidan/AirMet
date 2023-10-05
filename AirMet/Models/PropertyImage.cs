using System;
namespace AirMet.Models
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int PropertyId { get; set; }
        public virtual Property? Property { get; set; }
    }
}

