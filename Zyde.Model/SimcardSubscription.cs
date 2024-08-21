using Coffee.Model;
using Coffee.Model.DapperAttributes;

namespace Zyde.Model;

public sealed class SimcardSubscription : CoffeeModel
{
    public DateTime ExpireDate { get; set; }
    public decimal Value { get; set; }
    [UpdateIdOnly]
    public Simcard Simcard { get; set; }
    public int SimcardId { get; set; }

    public override bool IsValid()
    {
        ExpireDate = ValidateNull(ExpireDate, "SimcardSubscription.ExpireDate");
        Value = ValidateZero(Value, "SimcardSubscription.Value");
        Simcard = ValidateNull(Simcard, "SimcardSubscription.Simcard");
        return true;
    }
}