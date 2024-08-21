using Zyde.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coffee.Infrastructure.Configurations;

namespace Zyde.Infrastructure.Configurations;

public sealed class PositionConfiguration : CoffeeConfiguration<Position>
{
    public override void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("t_position");

        builder.Property<DateTime>(position => position.Date)
            .HasColumnName("date")
            .HasColumnType("DATETIME")
            .IsRequired();

        builder.Property<decimal>(position => position.Latitude)
            .HasColumnName("latitude")
            .HasColumnType("DECIMAL(18,15)")
            .IsRequired();

        builder.Property<decimal>(position => position.Longitude)
            .HasColumnName("longitude")
            .HasColumnType("DECIMAL(18,15)")
            .IsRequired();

        builder.Property<decimal>(position => position.SpeedInKnots)
            .HasColumnName("speed_in_knots")
            .HasColumnType("DECIMAL(6,2)")
            .IsRequired();

        builder.Property<decimal>(position => position.Course)
            .HasColumnName("course")
            .HasColumnType("DECIMAL(5,2)")
            .IsRequired();

        builder.Property<bool>(position => position.IsMoving)
            .HasColumnName("is_moving")
            .HasColumnType("BIT(1)")
            .IsRequired();

        builder.Property<string>(position => position.Attributes)
            .HasColumnName("attributes")
            .HasColumnType("JSON")
            .IsRequired();

        builder.Property<int?>(position => position.AddressId)
            .HasColumnName("fk_address_id");

        builder.HasOne<Address>(position => position.Address)
            .WithMany()
            .HasForeignKey(position => position.AddressId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property<int>(position => position.DeviceId)
            .HasColumnName("fk_device_id")
            .IsRequired();

        base.Configure(builder);
    }
}