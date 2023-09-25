using System;
namespace AirMet.Models;
public class Customer : Person
{
    public int CustomerId { get; set; }
    public virtual List<Property>? Properties { get; set; }
}

