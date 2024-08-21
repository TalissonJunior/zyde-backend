using Coffee.Application.ViewModels;
using Zyde.Application.ViewModels.Device;
using Zyde.Application.ViewModels.Position;

namespace Zyde.Application.ViewModels.PositionEvent;

public sealed class PositionEventView : CoffeeView
{
    public DateTime Date { get; set; }
    public bool Value { get; set; }
    public string Code { get; set; }
    public DeviceView Device { get; set; }
    public PositionView Position { get; set; }
}