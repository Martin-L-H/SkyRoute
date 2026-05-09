using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute_Domain.Entities;

public class AirportConfiguration : IEntityTypeConfiguration<Airport>
{
    public void Configure(EntityTypeBuilder<Airport> builder)
    {
        builder.HasKey(a => a.Id);

        //Relationships
        builder.HasOne(c => c.City)
               .WithMany(co => co.Airports)
               .HasForeignKey(c => c.CityId);

        builder.Property(a => a.CodeIATA)
               .IsRequired()
               .HasMaxLength(3);

        //IATA should not repeat
        builder.HasIndex(a => a.CodeIATA).IsUnique();

        //SEEDER
        builder.HasData(
            new Airport { Id = 1, CodeIATA = "JFK", PublicName = "John F. Kennedy Intl", CityId = 1 },
            new Airport { Id = 2, CodeIATA = "LAX", PublicName = "Los Angeles Intl", CityId = 2 },
            new Airport { Id = 3, CodeIATA = "LHR", PublicName = "London Heathrow", CityId = 3 },
            new Airport { Id = 4, CodeIATA = "LGW", PublicName = "London Gatwick", CityId = 3 },
            new Airport { Id = 5, CodeIATA = "MAN", PublicName = "Manchester Airport", CityId = 4 },
            new Airport { Id = 6, CodeIATA = "EWR", PublicName = "Newark Liberty", CityId = 1 },
            new Airport { Id = 7, CodeIATA = "EZE", PublicName = "Aeropuerto Internacional Ezeiza", CityId = 5},
            new Airport { Id = 8, CodeIATA = "GIG", PublicName = "Galeão–Antonio Carlos Jobim International Airport", CityId = 6 }
        );
    }
}