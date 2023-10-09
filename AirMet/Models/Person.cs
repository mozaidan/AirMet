using System;
using System.ComponentModel.DataAnnotations;

namespace AirMet.Models;
public class Person
{

    [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The Name must contain between 1 and 20 characters")]

    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Age { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]

    public string PhoneNr { get; set; } = string.Empty;
}
