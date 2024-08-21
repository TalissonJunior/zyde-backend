using Zyde.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coffee.Infrastructure.Configurations;

namespace Zyde.Infrastructure.Configurations;

public sealed class PositionEventConfiguration : CoffeeConfiguration<PositionEvent>
{
    public override void Configure(EntityTypeBuilder<PositionEvent> builder)
    {
        builder.ToTable("t_position_event");

        builder.Property<DateTime>(positionEvent => positionEvent.Date)
            .HasColumnName("date")
            .HasColumnType("DATETIME")
            .IsRequired();

        builder.Property<bool>(positionEvent => positionEvent.Value)
            .HasColumnName("value")
            .HasColumnType("BIT(1)")
            .IsRequired();

        builder.Property<string>(positionEvent => positionEvent.Code)
            .HasColumnName("code")
            .HasColumnType("ENUM('EngineOn','Charging','LowBatteryAlarm','GeofenceAlarm','PowerCutAlarm','SOSAlarm','VibrationAlarm','TrackingEnabled')")
            .IsRequired();

        builder.Property<int>(positionEvent => positionEvent.DeviceId)
            .HasColumnName("fk_device_id")
            .IsRequired();

        builder.HasOne<Device>(positionEvent => positionEvent.Device)
            .WithMany()
            .HasForeignKey(positionEvent => positionEvent.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<int>(positionEvent => positionEvent.PositionId)
            .HasColumnName("fk_position_id")
            .IsRequired();

        builder.HasOne<Position>(positionEvent => positionEvent.Position)
            .WithMany()
            .HasForeignKey(positionEvent => positionEvent.PositionId)
            .OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}