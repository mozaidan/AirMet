using System;
namespace AirMet.Models
{
	public class PType
	{
        public int PTypeId { get; set; }
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
        public string PTypeName { get; set; } = string.Empty;
    }
}


