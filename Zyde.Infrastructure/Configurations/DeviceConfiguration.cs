using Zyde.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coffee.Infrastructure.Configurations;

namespace Zyde.Infrastructure.Configurations;

public sealed class DeviceConfiguration : CoffeeConfiguration<Device>
{
    public override void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("t_device");

        builder.Property<string>(device => device.Model)
            .HasColumnName("model")
            .HasColumnType("VARCHAR(250)")
            .IsRequired();

        builder.Property<decimal>(device => device.Price)
            .HasColumnName("price")
            .HasColumnType("DECIMAL(18,2)");

        builder.Property<string>(device => device.Identifier)
            .HasColumnName("identifier")
            .HasColumnType("VARCHAR(300)")
            .IsRequired();

        builder.Property<bool>(device => device.HasTrackingEnabled)
            .HasColumnName("has_tracking_enabled")
            .HasColumnType("BIT(1)")
            .IsRequired();

        builder.Property<int>(device => device.OfflineUpdateIntervalSeconds)
            .HasColumnName("offline_update_interval_seconds")
            .HasColumnType("INT(20)");

        builder.Property<int>(device => device.OnlineUpdateIntervalSeconds)
            .HasColumnName("online_update_interval_seconds")
            .HasColumnType("INT(20)");

        builder.Property<string>(device => device.ProtocolVersion)
            .HasColumnName("protocol_version")
            .HasColumnType("VARCHAR(100)")
            .IsRequired();

        builder.Property<int?>(device => device.SimcardId)
            .HasColumnName("fk_simcard_id");

        builder.HasOne<Simcard>(device => device.Simcard)
            .WithMany()
            .HasForeignKey(device => device.SimcardId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany<Position>(device => device.Positions)
            .WithOne(position => position.Device)
            .HasForeignKey(position => position.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}