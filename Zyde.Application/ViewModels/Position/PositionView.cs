using Coffee.Application.ViewModels;
using Zyde.Application.ViewModels.Address;

namespace Zyde.Application.ViewModels.Position;

public sealed class PositionView : CoffeeView
{
    public DateTime Date { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal SpeedInKnots { get; set; }
    public decimal Course { get; set; }
    public bool IsMoving { get; set; }
    public string Attributes { get; set; }
    public AddressView Address { get; set; }
}