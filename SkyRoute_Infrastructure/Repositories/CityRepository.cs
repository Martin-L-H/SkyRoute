using Microsoft.EntityFrameworkCore;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

public class CityRepository : ICityRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public CityRepository(IDbContextFactory<AppDbContext> context)
    {
        _contextFactory = context;
    }

    public async Task<IEnumerable<City>> GetCitiesAsync(string? countryName, string? cityName)
    {

        using var context = await _contextFactory.CreateDbContextAsync();

        IQueryable<City> query = context.Cities
            .AsNoTracking()
            .Include(p => p.Country);

        if (countryName != null && countryName != "")
        {

            query = query.Where(p => p.Country.Name.Contains(countryName));

        }

        if (cityName != null && cityName != "")
        {

            query = query.Where(p => p.Name.Contains(cityName));

        }

        return await query.ToListAsync();

    }
}
