using Coffee.Model;
using Coffee.Model.DapperAttributes;

namespace Zyde.Model;

public sealed class Position : CoffeeModel
{
    public DateTime Date { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal SpeedInKnots { get; set; }
    public decimal Course { get; set; }
    public bool IsMoving { get; set; }
    public string Attributes { get; set; }
    [UpdateIdOnly]
    public Address Address { get; set; }
    public int? AddressId { get; set; }
    [UpdateIdOnly]
    public Device Device { get; set; }
    public int DeviceId { get; set; }

    public override bool IsValid()
    {
        Date = ValidateNull(Date, "Position.Date");
        Latitude = ValidateZero(Latitude, "Position.Latitude");
        Longitude = ValidateZero(Longitude, "Position.Longitude");
        SpeedInKnots = ValidateZero(SpeedInKnots, "Position.SpeedInKnots");
        Course = ValidateZero(Course, "Position.Course");
        Attributes = ValidateNullOrEmpty(Attributes, "Position.Attributes");
        Device = ValidateNull(Device, "Position.Device");
        return true;
    }
}