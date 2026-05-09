using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute_Domain.Entities;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.HasOne(f => f.AirportOrigin)
                .WithMany()
                .HasForeignKey(f => f.AirportOriginId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete flights if airport is deleted

        builder.HasOne(f => f.AirportDestination)
            .WithMany()
            .HasForeignKey(f => f.AirportDestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Flight //DOMESTIC JFK TO LAX USING GLOBAL AIR. ESCAPE FROM NEW YORK!
            {
                Id = 1,
                FlightNumber = "BW-99",
                ProviderName = "BudgetWings",
                AirportOriginId = 1,
                AirportDestinationId = 2,
                TimeDeparture = DateTime.Parse("2026-06-01T10:00:00"),
                TimeArrival = DateTime.Parse("2026-06-01T13:00:00"),
                BaseFare = 150.00m,
                CabinType = CabinType.Economy
            },
            new Flight //INTERNATIONAL BUENOS AIRES TO RIO. COME TO BRAZIL!
            {
                Id = 2,
                FlightNumber = "GA-101",
                ProviderName = "GlobalAir",
                AirportOriginId = 7,
                AirportDestinationId = 8,
                TimeDeparture = DateTime.Parse("2026-06-02T15:00:00"),
                TimeArrival = DateTime.Parse("2026-06-02T18:00:00"),
                BaseFare = 200.00m,
                CabinType = CabinType.Business
            }
        );
    }
}