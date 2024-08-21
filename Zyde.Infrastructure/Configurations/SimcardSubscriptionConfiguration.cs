using Zyde.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coffee.Infrastructure.Configurations;

namespace Zyde.Infrastructure.Configurations;

public sealed class SimcardSubscriptionConfiguration : CoffeeConfiguration<SimcardSubscription>
{
    public override void Configure(EntityTypeBuilder<SimcardSubscription> builder)
    {
        builder.ToTable("t_simcard_subscription");

        builder.Property<DateTime>(simcardSubscription => simcardSubscription.ExpireDate)
            .HasColumnName("expire_date")
            .HasColumnType("DATETIME")
            .IsRequired();

        builder.Property<decimal>(simcardSubscription => simcardSubscription.Value)
            .HasColumnName("value")
            .HasColumnType("DECIMAL(18,2)")
            .IsRequired();

        builder.Property<int>(simcardSubscription => simcardSubscription.SimcardId)
            .HasColumnName("fk_simcard_id")
            .IsRequired();

        base.Configure(builder);
    }
}