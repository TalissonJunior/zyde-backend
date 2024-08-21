using Coffee.Model;
using Coffee.Model.DapperAttributes;

namespace Zyde.Model;

public sealed class Device : CoffeeModel
{
    public string Model { get; set; }
    public decimal Price { get; set; }
    public string Identifier { get; set; }
    public bool HasTrackingEnabled { get; set; }
    public int OfflineUpdateIntervalSeconds { get; set; }
    public int OnlineUpdateIntervalSeconds { get; set; }
    public string ProtocolVersion { get; set; }
    [UpdateIdOnly]
    public Simcard Simcard { get; set; }
    public int? SimcardId { get; set; }
    public List<Position> Positions { get; set; }

    public override bool IsValid()
    {
        Model = ValidateNullOrEmpty(Model, "Device.Model");
        Identifier = ValidateNullOrEmpty(Identifier, "Device.Identifier");
        ProtocolVersion = ValidateNullOrEmpty(ProtocolVersion, "Device.ProtocolVersion");
        return true;
    }
}