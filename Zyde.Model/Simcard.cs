using Coffee.Model;

namespace Zyde.Model;

public sealed class Simcard : CoffeeModel
{
    public string Number { get; set; }
    public string StateCode { get; set; }
    public string Carrier { get; set; }
    public string Type { get; set; }
    public List<SimcardSubscription> Subscriptions { get; set; }

    public override bool IsValid()
    {
        Number = ValidateNullOrEmpty(Number, "Simcard.Number");
        StateCode = ValidateNullOrEmpty(StateCode, "Simcard.StateCode");
        Carrier = ValidateNullOrEmpty(Carrier, "Simcard.Carrier");
        Type = ValidateNullOrEmpty(Type, "Simcard.Type");
        return true;
    }
}