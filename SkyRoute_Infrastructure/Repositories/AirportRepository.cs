using Microsoft.EntityFrameworkCore;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

public class AirportRepository : IAirportRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public AirportRepository(IDbContextFactory<AppDbContext> context)
    {
        _contextFactory = context;
    }

    public async Task<IEnumerable<Airport>> GetAirportsWithDetailsAsync(string? countryName, string? cityName, string? airportName)
    {
        
        using var context = await _contextFactory.CreateDbContextAsync();
        
        IQueryable<Airport> query = context.Airports
            .AsNoTracking()
            .Include(p => p.City)
            .ThenInclude(p => p.Country);

        if (!string.IsNullOrWhiteSpace(countryName))
        {

            query = query.Where(p => EF.Functions.Like(p.City.Country.Name, $"%{countryName}%"));

        }

        if (!string.IsNullOrWhiteSpace(cityName))
        {

            query = query.Where(p => EF.Functions.Like(p.City.Name, $"%{cityName}%"));

        }

        if (!string.IsNullOrWhiteSpace(airportName))
        {
            query = query.Where(p => EF.Functions.Like(p.PublicName, $"%{airportName}%"));
        }

        return await query.ToListAsync();

    }
    public async Task<Airport?> GetAirportByIATAWithDetailsAsync(string iata)
    {
        
        using var context = await _contextFactory.CreateDbContextAsync();
        
        IQueryable<Airport> query = context.Airports
            .AsNoTracking()
            .Where(p => p.CodeIATA == iata)
            .Include(p => p.City)
            .ThenInclude(p => p.Country);
        return  await query.FirstOrDefaultAsync();
    }
}