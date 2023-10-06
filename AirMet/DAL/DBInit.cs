using System;
using Microsoft.EntityFrameworkCore;
using AirMet.Models;

namespace AirMet.Models
{
	public static class DBInit
	{
		public static void Seed(IApplicationBuilder app)
		{
			using var serviceScope = app.ApplicationServices.CreateScope();
			PropertyDbContext context = serviceScope.ServiceProvider.GetRequiredService<PropertyDbContext>();
            context.Database.EnsureCreated();

			if(!context.Properties.Any())
			{
				var properties = new List<Property>
				{
					new Property
					{
						Price = 399,
						Address = "Oslo Lufthavn",
						Description = "Very nice home"
					},

                    new Property
                    {
                        Price = 399,
                        Address = "Bergen Lufthavn",
                        Description = "Very nice home"
                    }
                };
				context.AddRange(properties);
				context.SaveChanges();
			}
			if (!context.PropertyImages.Any())
			{
				var imageProperty = new List<PropertyImage>
				{
					new PropertyImage {ImageUrl = "/images/domes1.jpg", PropertyId = 1},
					new PropertyImage {ImageUrl = "/images/domes2.jpg", PropertyId = 1}
				};
				context.AddRange(imageProperty);
				context.SaveChanges();
			}
		}
	}
}

