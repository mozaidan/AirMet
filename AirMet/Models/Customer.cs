using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AirMet.Models;
public class Customer 
{
    public virtual string? CustomerId { get; set; }
    public virtual string? Name { get; set; }
    public virtual string? Age { get; set; }
    public virtual string? Address { get; set; }
    public virtual string? PhoneNumber { get; set; }
    // Add other properties as needed

    // Navigation property
    public virtual List<Property> Properties { get; set; } = new List<Property>();
    public virtual IdentityUser? IdentityUser { get; set; }
    
}

