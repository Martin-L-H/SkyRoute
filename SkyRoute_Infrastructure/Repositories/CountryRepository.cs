using Microsoft.EntityFrameworkCore;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

public class CountryRepository : ICountryRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public CountryRepository(IDbContextFactory<AppDbContext> context)
    {
        _contextFactory = context;
    }

    public async Task<IEnumerable<Country>> GetCountriesAsync(string? countryName)
    {

        using var context = await _contextFactory.CreateDbContextAsync();

        IQueryable<Country> query = context.Countries
            .AsNoTracking();

        if (countryName != null && countryName != "")
        {

            query = query.Where(p => p.Name.Contains(countryName));

        }

        return await query.ToListAsync();
    }
}
