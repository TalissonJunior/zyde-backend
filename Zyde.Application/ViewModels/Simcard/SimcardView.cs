using Coffee.Application.ViewModels;
using Zyde.Application.ViewModels.SimcardSubscription;

namespace Zyde.Application.ViewModels.Simcard;

public sealed class SimcardView : CoffeeView
{
    public string Number { get; set; }
    public string StateCode { get; set; }
    public string Carrier { get; set; }
    public string Type { get; set; }
    public List<SimcardSubscriptionView> Subscriptions { get; set; }
}