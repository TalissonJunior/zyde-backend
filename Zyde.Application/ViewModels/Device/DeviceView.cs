using Coffee.Application.ViewModels;
using Zyde.Application.ViewModels.Simcard;
using Zyde.Application.ViewModels.Position;

namespace Zyde.Application.ViewModels.Device;

public sealed class DeviceView : CoffeeView
{
    public string Model { get; set; }
    public decimal Price { get; set; }
    public string Identifier { get; set; }
    public bool HasTrackingEnabled { get; set; }
    public int OfflineUpdateIntervalSeconds { get; set; }
    public int OnlineUpdateIntervalSeconds { get; set; }
    public string ProtocolVersion { get; set; }
    public SimcardView Simcard { get; set; }
    public List<PositionView> Positions { get; set; }
}