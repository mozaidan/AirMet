using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AirMet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirMet.ViewModels;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace AirMet.Controllers {

    public class HomeController : Controller
    {

        private readonly PropertyDbContext _propertyDbContext;

        public HomeController(PropertyDbContext propertyDbContext)
        {
            _propertyDbContext = propertyDbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Property> properties = _propertyDbContext.Properties.ToList();
            foreach (var property in properties)
            {
                if (property.ImageData != null)
                {
                    var image = SixLabors.ImageSharp.Image.Load(property.ImageData);

                    // Resize the image to a smaller size (e.g., 200x150)
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(200, 150), // Adjust size as needed
                        Mode = ResizeMode.Max
                    }));

                    // Optimize the image quality (adjust quality as needed)
                    var jpegEncoder = new JpegEncoder
                    {
                        Quality = 70 // Adjust quality as needed (70 is just an example)
                    };

                    var memoryStream = new MemoryStream();
                    image.Save(memoryStream, jpegEncoder);

                    property.ImageData = memoryStream.ToArray();
                    property.ImageMimeType = "image/jpeg"; // Set the MIME type accordingly
                }
            }
            var itemListViewModel = new PropertyListViewModel(properties, "Index");
            return View(itemListViewModel);
        }
        public IActionResult Aboutus()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var property = _propertyDbContext.Properties.FirstOrDefault(i => i.PropertyId == id);
            if (property == null)
                return NotFound();

            // Resize and optimize the image if ImageData is not null
            if (property.ImageData != null)
            {
                var image = SixLabors.ImageSharp.Image.Load(property.ImageData);

                // Resize the image to a specific size (e.g., 400x300)
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(400, 300),
                    Mode = ResizeMode.Max
                }));

                // Optimize the image quality (adjust quality as needed)
                var jpegEncoder = new JpegEncoder
                {
                    Quality = 70 // Adjust quality as needed (70 is just an example)
                };

                var memoryStream = new MemoryStream();
                image.Save(memoryStream, jpegEncoder);

                property.ImageData = memoryStream.ToArray();
                property.ImageMimeType = "image/jpeg"; // Set the MIME type accordingly
            }

            return View(property);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Property property, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        image.CopyTo(memoryStream);
                        property.ImageData = memoryStream.ToArray();
                        property.ImageMimeType = image.ContentType;
                    }
                }
                _propertyDbContext.Properties.Add(property);
                _propertyDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(property);
        }

    }
}

