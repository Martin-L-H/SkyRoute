using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute_Domain.Entities;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        //SEEDER
        builder.HasData(
            new Country { Id = 1, Name = "United States" },
            new Country { Id = 2, Name = "United Kingdom" },
            new Country { Id = 3, Name = "Argentina"},
            new Country { Id = 4, Name = "Brasil" }
        );
    }
}