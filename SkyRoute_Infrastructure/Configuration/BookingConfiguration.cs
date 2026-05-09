using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute_Domain.Entities;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {

        builder.HasKey(c => c.Id);

        builder.HasIndex(b => b.ReferenceCode)
            .IsUnique();
    }
}