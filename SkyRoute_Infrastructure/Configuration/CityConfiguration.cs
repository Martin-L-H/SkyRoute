using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute_Domain.Entities;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        //Relationships
        builder.HasOne(c => c.Country)
               .WithMany(co => co.Cities)
               .HasForeignKey(c => c.CountryId);

        //SEEDER
        builder.HasData(
            new City { Id = 1, Name = "New York", CountryId = 1 },
            new City { Id = 2, Name = "Los Angeles", CountryId = 1 },
            new City { Id = 3, Name = "London", CountryId = 2 },
            new City { Id = 4, Name = "Manchester", CountryId = 2 },
            new City { Id = 5, Name = "Buenos Aires", CountryId = 3},
            new City { Id = 6, Name = "Rio de Janeiro", CountryId = 4 }
        );
    }
}