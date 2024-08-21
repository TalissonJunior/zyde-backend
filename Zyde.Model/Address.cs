using Coffee.Model;

namespace Zyde.Model;

public sealed class Address : CoffeeModel
{
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Neighborhood { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }
    public string ZipCode { get; set; }
}