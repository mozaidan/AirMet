using System;
using AirMet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AirMet.Models
{

    public class PropertyDbContext : IdentityDbContext
    {
        public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
<<<<<<< HEAD
=======
        public DbSet<PType> PTypes { get; set; }
>>>>>>> 86b410a596466e0daea38b2558ff038226c5088f

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

    }

}
