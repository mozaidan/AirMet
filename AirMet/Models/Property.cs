using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirMet.Models
{
	public class Property
	{

        //her i wanna to add soverome , type ,senger og guster and antall pepo
		public int PropertyId { get; set; }
		public decimal Price { get; set; }
        public string Address { get; set; } = string.Empty;
        [StringLength(400)]
        public string Description{ get; set; } = string.Empty;

        [Range(1, 4, ErrorMessage = "number of bed must be 0-4")]

        public int NumberOfBed { get; set; }


        [Range(1, 20, ErrorMessage = "number of rooms must be 0-10")]
        public int  NumberOfRom { get; set; }   

        public bool isAvailable { get; set; }   

        public 
        public virtual List<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        [NotMapped]
        public virtual List<IFormFile>? Files { get; set; }
    }
}

