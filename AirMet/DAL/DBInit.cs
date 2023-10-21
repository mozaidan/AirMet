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
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.PTypes.Any())
            {
                var pType = new List<PType>
                {
                   new PType { PTypeName = "House" },
                   new PType { PTypeName = "Apartment" },
                   new PType { PTypeName = "Cabins" },
                   new PType { PTypeName = "Domes" },
                   new PType { PTypeName = "Treehouses" },
                   new PType { PTypeName = "Amazing Pools" }
                };
                context.AddRange(pType);
                context.SaveChanges();
            }
            if (!context.Properties.Any())
			{
				var properties = new List<Property>
				{
					new Property
					{
						Title = "Home",
						Price = 399,
						Address = "Oslo Lufthavn",
						Description = "Very nice home",
						Guest = 1,
						Bed = 2,
						BedRooms = 3,
						BathRooms = 4,
						PTypeId = 1
					},

                    new Property
                    {
						Title = "Home",
                        Price = 399,
                        Address = "Bergen Lufthavn",
                        Description = "Very nice home",
                        Guest = 1,
                        Bed = 2,
                        BedRooms = 3,
                        BathRooms = 4,
						PTypeId = 1
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
					new PropertyImage {ImageUrl = "/images/Home.jpg", PropertyId = 1}
				};
				context.AddRange(imageProperty);
				context.SaveChanges();
			}
			
		}
	}
}

