using Microsoft.EntityFrameworkCore;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

namespace SkyRoute_Infrastructure.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly AppDbContext _context;

        public AirportRepository(AppDbContext context)
        {
            _context = context;
        }

        //DEBUGGER
        public async Task<IEnumerable<Airport>> GetAirportsAsync()
        {
            return await _context.Airports
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Airport>> GetAirportsWithDetailsAsync()
        {
            return await _context.Airports
                .Include(a => a.City)
                .ThenInclude(a => a.Country)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Airport?> GetAirportByIdWithDetailsAsync(int id)
        {
            return await _context.Airports
                .Include(a => a.City)
                .ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
