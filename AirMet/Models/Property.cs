using System;
using System.ComponentModel.DataAnnotations;

namespace AirMet.Models
{
	public class Property
	{
		public int PropertyId { get; set; }
		public decimal Price { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? ImageData { get; set; } // To store the binary data of the image
        public string? ImageMimeType { get; set; } // To store the image file's MIME type

    }
}

