using System;
using AirMet.Models;
using Microsoft.EntityFrameworkCore;

namespace AirMet.Models
{

    public class PropertyDbContext : DbContext
    {
        public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Property> Properties { get; set; }
    }

}
