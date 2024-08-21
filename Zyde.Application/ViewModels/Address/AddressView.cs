using Coffee.Application.ViewModels;

namespace Zyde.Application.ViewModels.Address;

public sealed class AddressView : CoffeeView
{
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Neighborhood { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }
    public string ZipCode { get; set; }
}