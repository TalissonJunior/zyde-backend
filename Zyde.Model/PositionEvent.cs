using Coffee.Model;
using Coffee.Model.DapperAttributes;

namespace Zyde.Model;

public sealed class PositionEvent : CoffeeModel
{
    public DateTime Date { get; set; }
    public bool Value { get; set; }
    public string Code { get; set; }
    [UpdateIdOnly]
    public Device Device { get; set; }
    public int DeviceId { get; set; }
    [UpdateIdOnly]
    public Position Position { get; set; }
    public int PositionId { get; set; }

    public override bool IsValid()
    {
        Date = ValidateNull(Date, "PositionEvent.Date");
        Code = ValidateNullOrEmpty(Code, "PositionEvent.Code");
        Device = ValidateNull(Device, "PositionEvent.Device");
        Position = ValidateNull(Position, "PositionEvent.Position");
        return true;
    }
}