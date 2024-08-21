using Zyde.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coffee.Infrastructure.Configurations;

namespace Zyde.Infrastructure.Configurations;

public sealed class SimcardConfiguration : CoffeeConfiguration<Simcard>
{
    public override void Configure(EntityTypeBuilder<Simcard> builder)
    {
        builder.ToTable("t_simcard");

        builder.Property<string>(simcard => simcard.Number)
            .HasColumnName("number")
            .HasColumnType("VARCHAR(20)")
            .IsRequired();

        builder.Property<string>(simcard => simcard.StateCode)
            .HasColumnName("state_code")
            .HasColumnType("VARCHAR(5)")
            .IsRequired();

        builder.Property<string>(simcard => simcard.Carrier)
            .HasColumnName("carrier")
            .HasColumnType("VARCHAR(250)")
            .IsRequired();

        builder.Property<string>(simcard => simcard.Type)
            .HasColumnName("type")
            .HasColumnType("ENUM('GSM','M2M')")
            .IsRequired();

        builder.HasMany<SimcardSubscription>(simcard => simcard.Subscriptions)
            .WithOne(simcardSubscription => simcardSubscription.Simcard)
            .HasForeignKey(simcardSubscription => simcardSubscription.SimcardId)
            .OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}