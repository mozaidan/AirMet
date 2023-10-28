using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AirMet.Models
{
	public class PType
	{
        // Unique identifier for the property type
        public int PTypeId { get; set; }

        // Name of the property type
        [StringLength(50)]
        public string PTypeName { get; set; } = string.Empty;

        // Navigation property to hold the associated properties
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    }
}


