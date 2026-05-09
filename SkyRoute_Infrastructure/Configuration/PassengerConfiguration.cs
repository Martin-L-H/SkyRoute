using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute_Domain.Entities;

public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> builder)
    {

        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Booking)
            .WithMany(c => c.Passengers)
            .HasForeignKey(c => c.BookingId);
    }
}