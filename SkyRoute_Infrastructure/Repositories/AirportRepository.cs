using Microsoft.EntityFrameworkCore;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

namespace SkyRoute_Infrastructure.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public AirportRepository(IDbContextFactory<AppDbContext> context)
        {
            _contextFactory = context;
        }

        public async Task<IEnumerable<Airport>> GetAirportsWithDetailsAsync(string cityName, string countryName)
        {

            using var context = await _contextFactory.CreateDbContextAsync();

            IQueryable<Airport> query = context.Airports.AsNoTracking();

            if (countryName != null)
            {

                query = query.Where(p => p.City.Country.Name == countryName);

            }

            if (cityName != null)
            {

                query = query.Where(p => p.City.Name == cityName);

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
}
