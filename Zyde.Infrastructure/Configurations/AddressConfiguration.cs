using Zyde.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coffee.Infrastructure.Configurations;

namespace Zyde.Infrastructure.Configurations;

public sealed class AddressConfiguration : CoffeeConfiguration<Address>
{
    public override void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("t_address");

        builder.Property<string>(address => address.Country)
            .HasColumnName("country")
            .HasColumnType("VARCHAR(50)")
            .IsRequired(false);

        builder.Property<string>(address => address.State)
            .HasColumnName("state")
            .HasColumnType("VARCHAR(100)")
            .IsRequired(false);

        builder.Property<string>(address => address.City)
            .HasColumnName("city")
            .HasColumnType("VARCHAR(100)")
            .IsRequired(false);

        builder.Property<string>(address => address.Neighborhood)
            .HasColumnName("neighborhood")
            .HasColumnType("VARCHAR(150)")
            .IsRequired(false);

        builder.Property<string>(address => address.Street)
            .HasColumnName("street")
            .HasColumnType("VARCHAR(200)")
            .IsRequired(false);

        builder.Property<int>(address => address.Number)
            .HasColumnName("number")
            .HasColumnType("INT(10)");

        builder.Property<string>(address => address.ZipCode)
            .HasColumnName("zip_code")
            .HasColumnType("VARCHAR(20)")
            .IsRequired(false);

        base.Configure(builder);
    }
}