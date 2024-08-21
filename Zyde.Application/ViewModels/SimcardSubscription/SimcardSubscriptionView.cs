using Coffee.Application.ViewModels;

namespace Zyde.Application.ViewModels.SimcardSubscription;

public sealed class SimcardSubscriptionView : CoffeeView
{
    public DateTime ExpireDate { get; set; }
    public decimal Value { get; set; }
}