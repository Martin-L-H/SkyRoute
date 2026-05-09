using Microsoft.EntityFrameworkCore;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

public class FlightRepository : IFlightRepository
{
    private readonly AppDbContext _context;

    public FlightRepository(AppDbContext context)
    {
        _context = context;
    }

    //DEBUGGER
    public async Task<IEnumerable<Flight>> GetFlightsAsync()
    {
        return await _context.Flights
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Flight>> SearchFlightsAsync(int originId, int destId, CabinType? cabinType, DateTime departureDate)
    {
        //Advice from gemini, using just "TimeDeparture == departureDate would need an exact match down to the ''.
        //Using .date can throw errors in some older SQL servers so I am now using the range within 1 day of the date
        DateTime departureDateStart = departureDate.Date;
        DateTime departureDateEnd = departureDateStart.AddDays(1);

        IQueryable<Flight> query = _context.Flights.AsNoTracking()
            .Include(f => f.AirportOrigin)
            .ThenInclude(f => f.City)
            .ThenInclude(f => f.Country)
            .Include(f => f.AirportDestination)
            .ThenInclude(f => f.City)
            .ThenInclude(f => f.Country)
            .Where(f => f
            .AirportOriginId == originId 
            && f.AirportDestinationId == destId
            && f.CabinType == cabinType
            && f.TimeDeparture >= departureDateStart
            && f.TimeDeparture < departureDateEnd);

        //This way the user can look for specific cabin types or not.
        if (cabinType != null)
        {
            query = query.Where(f => f.CabinType == cabinType);
        }

        return await query.ToListAsync();

    }
    
    public async Task<Flight?> SearchFlightByNumberAsync(string flightNumber)
    {
        return await _context.Flights
            .Include(f => f.AirportOrigin)
            .ThenInclude(f => f.City)
            .ThenInclude(f => f.Country)
            .Include(f => f.AirportDestination)
            .ThenInclude(f => f.City)
            .ThenInclude(f => f.Country)
            .Where(f => f.FlightNumber == flightNumber)
            .AsNoTracking().
            FirstOrDefaultAsync();
    }
}