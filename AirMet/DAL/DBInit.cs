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

            if (!context.Customers.Any())
            {
                var Customer = new List<Customer>
                {
                   new Customer
				   {
                     CustomerId = "1",
					 Name = "Talhat Hamdy",
					 Age = "28",
					 Address = "OsloVeien",
					 PhoneNumber = "92983929"

                   },

                    new Customer
                   {
                     CustomerId = "2",
                     Name = "Per Hansen",
                     Age = "47",
                     Address = "SkiVeien",
                     PhoneNumber = "93483929"

                   }
                };
                context.AddRange(Customer);
                context.SaveChanges();
            }

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
			if (!context.Amenities.Any())
			{
				var amanity = new List<Amenity>
				{
					new Amenity { AmenityName = "WiFi", AmenityIcon = "fa-solid fa-wifi", IsChecked = false},
					new Amenity {AmenityName = "Kitchen", AmenityIcon = "fa-solid fa-utensils", IsChecked = false},
                    new Amenity { AmenityName = "Dedicated workspace", AmenityIcon = "fa-solid fa-briefcase", IsChecked = false},
                    new Amenity { AmenityName = "Free street parking", AmenityIcon = "fa-solid fa-parking", IsChecked = false},
                    new Amenity { AmenityName = "Pool", AmenityIcon = "fa-solid fa-swimming-pool", IsChecked = false},
                    new Amenity { AmenityName = "TV with standard cable", AmenityIcon = "fa-solid fa-tv", IsChecked = false},
                    new Amenity { AmenityName = "Hair dryer", AmenityIcon = "fa-solid fa-hair-dryer", IsChecked = false},
                    new Amenity { AmenityName = "Microwave", AmenityIcon = "fa-solid fa-microwave", IsChecked = false},
                    new Amenity { AmenityName = "Indoor fireplace", AmenityIcon = "fa-solid fa-fire", IsChecked = false},
                };
				context.AddRange(amanity);
				context.SaveChanges();
			}

            if (!context.Properties.Any())
			{
				var properties = new List<Property>
				{
					new Property
					{
						Title = "Home",
						Price = 2000,
						Address = "Bro Sweden",
						Description = "Cozy house featuring a comfortable bedroom, well-equipped kitchen, and a refreshing pool, perfect for a relaxing getaway.",
						Guest = 2,
						Bed = 1,
						BedRooms = 1,
						BathRooms = 1,
						PTypeId = 6,
						CustomerId = "1",
						UserId = "1",

					},

                    new Property
                    {
                        Title = "Home",
                        Price = 2500,
                        Address = "Oslo Norway",
                        Description = "Welcome to our charming home with a refreshing pool outside, perfect for leisurely afternoons. Enjoy the convenience of a fully-equipped kitchen, two cozy bedrooms with double beds, and a well-maintained bathroom. Relax and unwind in the tranquil ambiance of this inviting space, designed to make your stay memorable and comfortable.",
                        Guest = 4,
                        Bed = 2,
                        BedRooms = 2,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",

                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 3000,
                        Address = "Pandrup Denmark",
                        Description = "Charming home featuring a refreshing pool, fully equipped kitchen, two comfortable bedrooms with double beds, and a clean bathroom. Ideal for a relaxing and enjoyable stay",
                        Guest = 4,
                        Bed = 2,
                        BedRooms = 2,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",

                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 1500,
                        Address = "Ås Norway",
                        Description = "Charming home with a refreshing pool outside, perfect for relaxation and leisure. The property comprises a comfortable bedroom furnished with a double bed, a well-equipped kitchen, and a clean bathroom. Enjoy the tranquility and convenience of this inviting space.",
                        Guest = 4,
                        Bed = 2,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",

                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 2000,
                        Address = "Moss Norway",
                        Description = "Idyllic home featuring a private pool outside, an inviting kitchen, and a cozy bedroom furnished with a double bed. Enjoy the perfect blend of comfort and serenity in this delightful abode, ideal for a relaxing getaway",
                        Guest = 4,
                        Bed = 2,
                        BedRooms = 2,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",

                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 3500,
                        Address = "Trondheim Norway",
                        Description = "Charming retreat boasting 2 bedrooms, one elegantly furnished with a double bed for adults and the other thoughtfully arranged with a comfortable bed for children. Discover the tranquil ambiance and delightful amenities, perfect for a memorable family vacation",
                        Bed = 2,
                        BedRooms = 2,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",
                    },
                    new Property
                    {
                        Title = "Home",
                        Price = 1500,
                        Address = "Verdal Norway",
                        Description = "Charming retreat boasting 2 bedrooms, one elegantly furnished with a double bed for adults and the other thoughtfully arranged with a comfortable bed for children. Discover the tranquil ambiance and delightful amenities, perfect for a memorable family vacation",
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",
                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 3000,
                        Address = "Dal Norway",
                        Description = "Charming retreat boasting 2 bedrooms, one elegantly furnished with a double bed for adults and the other thoughtfully arranged with a comfortable bed for children. Discover the tranquil ambiance and delightful amenities, perfect for a memorable family vacation",
                        Bed = 2,
                        BedRooms = 2,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",
                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 2000,
                        Address = "London UK",
                        Description = "Charming retreat boasting 2 bedrooms, one elegantly furnished with a double bed for adults and the other thoughtfully arranged with a comfortable bed for children. Discover the tranquil ambiance and delightful amenities, perfect for a memorable family vacation",
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",
                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 1750,
                        Address = "Gdańsk Poland",
                        Description = "Charming retreat boasting 2 bedrooms, one elegantly furnished with a double bed for adults and the other thoughtfully arranged with a comfortable bed for children. Discover the tranquil ambiance and delightful amenities, perfect for a memorable family vacation",
                        Bed = 2,
                        BedRooms = 2,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",
                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 1500,
                        Address = "Copenhagen Denmark",
                        Description = "Charming retreat boasting 2 bedrooms, one elegantly furnished with a double bed for adults and the other thoughtfully arranged with a comfortable bed for children. Discover the tranquil ambiance and delightful amenities, perfect for a memorable family vacation",
                        Bed = 2,
                        BedRooms = 2,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",
                    },

                     new Property
                    {
                        Title = "Home",
                        Price = 1200,
                        Address = "Stockholms Sweden",
                        Description = "Charming retreat boasting 2 bedrooms, one elegantly furnished with a double bed for adults and the other thoughtfully arranged with a comfortable bed for children. Discover the tranquil ambiance and delightful amenities, perfect for a memorable family vacation",
                        Bed = 2,
                        BedRooms = 2,
                        BathRooms = 1,
                        PTypeId = 6,
                        CustomerId = "1",
                        UserId = "1",
                    },

                     new Property
                    {
                        Title = "Home",
                        Price = 1600,
                        Address = "Uppsala Sweden",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest = 2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",

                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 2500,
                        Address = "Opddal Norway",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest = 2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId =1 ,
                        CustomerId = "2",
                        UserId = "2",

                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 3500,
                        Address = " Hamburg Germany",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest = 2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",

                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 1850,
                        Address = "North Berwick, Scotland, UK",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest = 2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",

                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 1400,
                        Address = "Bygdøy Norway",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest = 2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",

                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 3500,
                        Address = "Trondheim Norway",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest =2,
                        Bed = 1,
                        BedRooms = 2,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",
                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 1100,
                        Address = "Ski Norway",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest =2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",
                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 3500,
                        Address = "Dal Norway",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest= 2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",
                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 3500,
                        Address = "Skagen Denmark",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest= 2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",
                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 2700,
                        Address = "Amestrdam Netherlands",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest= 2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",
                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 1400,
                        Address = "Paris France",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest= 2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",
                    },

                    new Property
                    {
                        Title = "Home",
                        Price = 2300,
                        Address = "Berlin Germany",
                        Description = "Charming home with a well-equipped kitchen, a comfortable bedroom featuring a double bed, and a modern bathroom. Perfect for a relaxing and convenient stay",
                        Guest= 2,
                        Bed = 1,
                        BedRooms = 1,
                        BathRooms = 1,
                        PTypeId = 1,
                        CustomerId = "2",
                        UserId = "2",
                    },


                };

				context.AddRange(properties);
				context.SaveChanges();
			}
			if (!context.PropertyImages.Any())
			{
				var imageProperty = new List<PropertyImage>
				{
					new PropertyImage {ImageUrl = "/images/hs1.jpg", PropertyId = 1},
					new PropertyImage {ImageUrl = "/images/hs2.jpg", PropertyId = 1},
                    new PropertyImage {ImageUrl = "/images/hs3.jpg", PropertyId = 1},
                    new PropertyImage {ImageUrl = "/images/hs4.jpg", PropertyId = 1},
                    new PropertyImage {ImageUrl = "/images/hs5.jpg", PropertyId = 1},
                    new PropertyImage {ImageUrl = "/images/hs6.jpg", PropertyId = 1},

                    new PropertyImage {ImageUrl = "/images/h1.jpg", PropertyId = 2},
                    new PropertyImage {ImageUrl = "/images/h2.jpg", PropertyId = 2},
                    new PropertyImage {ImageUrl = "/images/h3.jpg", PropertyId = 2},
                    new PropertyImage {ImageUrl = "/images/h4.jpg", PropertyId = 2},
                    new PropertyImage {ImageUrl = "/images/h5.jpg", PropertyId = 2},
                    new PropertyImage {ImageUrl = "/images/h6.jpg", PropertyId = 2},

                    new PropertyImage {ImageUrl = "/images/1.jpg", PropertyId = 3},
                    new PropertyImage {ImageUrl = "/images/2.jpg", PropertyId = 3},
                    new PropertyImage {ImageUrl = "/images/3.jpg", PropertyId = 3},
                    new PropertyImage {ImageUrl = "/images/4.jpg", PropertyId = 3},
                    new PropertyImage {ImageUrl = "/images/5.jpg", PropertyId = 3},
                    new PropertyImage {ImageUrl = "/images/6.jpg", PropertyId = 3},

                    new PropertyImage {ImageUrl = "/images/7.jpg", PropertyId = 4},
                    new PropertyImage {ImageUrl = "/images/8.jpg", PropertyId = 4},
                    new PropertyImage {ImageUrl = "/images/9.jpg", PropertyId = 4},
                    new PropertyImage {ImageUrl = "/images/10.jpg", PropertyId = 4},

                    new PropertyImage {ImageUrl = "/images/11.jpg", PropertyId = 5},
                    new PropertyImage {ImageUrl = "/images/12.jpg", PropertyId = 5},
                    new PropertyImage {ImageUrl = "/images/13.jpg", PropertyId = 5},
                    new PropertyImage {ImageUrl = "/images/14.jpg", PropertyId = 5},
                    new PropertyImage {ImageUrl = "/images/15.jpg", PropertyId = 5},
                    new PropertyImage {ImageUrl = "/images/16.jpg", PropertyId = 5},

                    new PropertyImage {ImageUrl = "/images/17.jpg", PropertyId = 6},
                    new PropertyImage {ImageUrl = "/images/18.jpg", PropertyId = 6},
                    new PropertyImage {ImageUrl = "/images/19.jpg", PropertyId = 6},
                    new PropertyImage {ImageUrl = "/images/20.jpg", PropertyId = 6},
                    new PropertyImage {ImageUrl = "/images/21.jpg", PropertyId = 6},
                    new PropertyImage {ImageUrl = "/images/22.jpg", PropertyId = 6},

                    new PropertyImage {ImageUrl = "/images/23.jpg", PropertyId = 7},
                    new PropertyImage {ImageUrl = "/images/24.jpg", PropertyId = 7},
                    new PropertyImage {ImageUrl = "/images/25.jpg", PropertyId = 7},
                    new PropertyImage {ImageUrl = "/images/26.jpg", PropertyId = 7},
                    new PropertyImage {ImageUrl = "/images/27.jpg", PropertyId = 7},


                    new PropertyImage {ImageUrl = "/images/28.jpg", PropertyId = 8},
                    new PropertyImage {ImageUrl = "/images/29.jpg", PropertyId = 8},
                    new PropertyImage {ImageUrl = "/images/30.jpg", PropertyId = 8},
                    new PropertyImage {ImageUrl = "/images/31.jpg", PropertyId = 8},
                    new PropertyImage {ImageUrl = "/images/32.jpg", PropertyId = 8},

                    new PropertyImage {ImageUrl = "/images/33.jpg", PropertyId = 9},
                    new PropertyImage {ImageUrl = "/images/34.jpg", PropertyId = 9},
                    new PropertyImage {ImageUrl = "/images/35.jpg", PropertyId = 9},
                    new PropertyImage {ImageUrl = "/images/36.jpg", PropertyId = 9},

                    new PropertyImage {ImageUrl = "/images/37.jpg", PropertyId = 10},
                    new PropertyImage {ImageUrl = "/images/38.jpg", PropertyId = 10},
                    new PropertyImage {ImageUrl = "/images/39.jpg", PropertyId = 10},
                    new PropertyImage {ImageUrl = "/images/40.jpg", PropertyId = 10},
                    new PropertyImage {ImageUrl = "/images/41.jpg", PropertyId = 10},
                    new PropertyImage {ImageUrl = "/images/42.jpg", PropertyId = 10},


                    new PropertyImage {ImageUrl = "/images/43.jpg", PropertyId = 11},
                    new PropertyImage {ImageUrl = "/images/44.jpg", PropertyId = 11},
                    new PropertyImage {ImageUrl = "/images/45.jpg", PropertyId = 11},
                    new PropertyImage {ImageUrl = "/images/46.jpg", PropertyId = 11},
                    new PropertyImage {ImageUrl = "/images/47.jpg", PropertyId = 11},
                    new PropertyImage {ImageUrl = "/images/48.jpg", PropertyId = 11},

                    new PropertyImage {ImageUrl = "/images/49.jpg", PropertyId = 12},
                    new PropertyImage {ImageUrl = "/images/50.jpg", PropertyId = 12},
                    new PropertyImage {ImageUrl = "/images/51.jpg", PropertyId = 12},
                    new PropertyImage {ImageUrl = "/images/52.jpg", PropertyId = 12},
                    new PropertyImage {ImageUrl = "/images/53.jpg", PropertyId = 12},

                    new PropertyImage {ImageUrl = "/images/54.jpg", PropertyId = 13},
                    new PropertyImage {ImageUrl = "/images/55.jpg", PropertyId = 13},
                    new PropertyImage {ImageUrl = "/images/56.jpg", PropertyId = 13},
                    new PropertyImage {ImageUrl = "/images/57.jpg", PropertyId = 13},

                    new PropertyImage {ImageUrl = "/images/58.jpg", PropertyId = 14},
                    new PropertyImage {ImageUrl = "/images/59.jpg", PropertyId = 14},
                    new PropertyImage {ImageUrl = "/images/60.jpg", PropertyId = 14},
                    new PropertyImage {ImageUrl = "/images/61.jpg", PropertyId = 14},

                    new PropertyImage {ImageUrl = "/images/62.jpg", PropertyId = 15},
                    new PropertyImage {ImageUrl = "/images/63.jpg", PropertyId = 15},
                    new PropertyImage {ImageUrl = "/images/64.jpg", PropertyId = 15},
                    new PropertyImage {ImageUrl = "/images/65.jpg", PropertyId = 15},

                    new PropertyImage {ImageUrl = "/images/66.jpg", PropertyId = 16},
                    new PropertyImage {ImageUrl = "/images/67.jpg", PropertyId = 16},
                    new PropertyImage {ImageUrl = "/images/68.jpg", PropertyId = 16},
                    new PropertyImage {ImageUrl = "/images/69.jpg", PropertyId = 16},

                    new PropertyImage {ImageUrl = "/images/70.jpg", PropertyId = 17},
                    new PropertyImage {ImageUrl = "/images/71.jpg", PropertyId = 17},
                    new PropertyImage {ImageUrl = "/images/72.jpg", PropertyId = 17},
                    new PropertyImage {ImageUrl = "/images/73.jpg", PropertyId = 17},

                    new PropertyImage {ImageUrl = "/images/74.jpg", PropertyId = 18},
                    new PropertyImage {ImageUrl = "/images/75.jpg", PropertyId = 18},
                    new PropertyImage {ImageUrl = "/images/76.jpg", PropertyId = 18},
                    new PropertyImage {ImageUrl = "/images/77.jpg", PropertyId = 18},

                    new PropertyImage {ImageUrl = "/images/78.jpg", PropertyId = 19},
                    new PropertyImage {ImageUrl = "/images/79.jpg", PropertyId = 19},
                    new PropertyImage {ImageUrl = "/images/80.jpg", PropertyId = 19},
                    new PropertyImage {ImageUrl = "/images/81.jpg", PropertyId = 19},

                    new PropertyImage {ImageUrl = "/images/82.jpg", PropertyId = 20},
                    new PropertyImage {ImageUrl = "/images/83.jpg", PropertyId = 20},
                    new PropertyImage {ImageUrl = "/images/84.jpg", PropertyId = 20},
                    new PropertyImage {ImageUrl = "/images/85.jpg", PropertyId = 20},

                    new PropertyImage {ImageUrl = "/images/86.jpg", PropertyId = 21},
                    new PropertyImage {ImageUrl = "/images/87.jpg", PropertyId = 21},
                    new PropertyImage {ImageUrl = "/images/88.jpg", PropertyId = 21},
                    new PropertyImage {ImageUrl = "/images/89.jpg", PropertyId = 21},

                    new PropertyImage {ImageUrl = "/images/90.jpg", PropertyId = 22},
                    new PropertyImage {ImageUrl = "/images/91.jpg", PropertyId = 22},
                    new PropertyImage {ImageUrl = "/images/92.jpg", PropertyId = 22},
                    new PropertyImage {ImageUrl = "/images/93.jpg", PropertyId = 22},

                    new PropertyImage {ImageUrl = "/images/94.jpg", PropertyId = 23},
                    new PropertyImage {ImageUrl = "/images/95.jpg", PropertyId = 23},
                    new PropertyImage {ImageUrl = "/images/96.jpg", PropertyId = 23},
                    new PropertyImage {ImageUrl = "/images/97.jpg", PropertyId = 23},

                    new PropertyImage {ImageUrl = "/images/98.jpg", PropertyId = 24},
                    new PropertyImage {ImageUrl = "/images/99.jpg", PropertyId = 24},
                    new PropertyImage {ImageUrl = "/images/100.jpg", PropertyId = 24},
                    new PropertyImage {ImageUrl = "/images/101.jpg", PropertyId = 24},

                };
				context.AddRange(imageProperty);
				context.SaveChanges();
			}

            if (!context.PropertyAmenities.Any())
            {
                var propertyAmenities = new List<PropertyAmenity>
                {
                    new PropertyAmenity {AmenityId = 1, PropertyId = 1},
                    new PropertyAmenity {AmenityId = 2, PropertyId = 1},
                    new PropertyAmenity {AmenityId = 3, PropertyId = 1},
                    new PropertyAmenity {AmenityId = 4, PropertyId = 1},

                };
                context.AddRange(propertyAmenities);
                context.SaveChanges();
            }
        }
	}
}

