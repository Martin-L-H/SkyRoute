using Microsoft.EntityFrameworkCore;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

public class FlightRepository : IFlightRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public FlightRepository(IDbContextFactory<AppDbContext> context)
    {
        _contextFactory = context;
    }

    public void FlightRestSeats(int flightId, int seatsBought)
    {

        using var context = _contextFactory.CreateDbContext();

        Flight flight = context.Flights.Where(p => p.Id == flightId).First();

        flight.SeatsFree = flight.SeatsFree - seatsBought;

        context.SaveChanges();

    }

    public async Task<Flight?> GetFlightByIdAsync(int flightId)
    {

        using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Flights.AsNoTracking().FirstOrDefaultAsync();

    }

    public async Task<IEnumerable<Flight>> SearchSpecificFlightsAsync(
        int? originId, 
        int? destId, 
        CabinType? cabinType, 
        DateTime? departureDate, 
        int? duration, 
        int? minimumFreeSeats)
    {

        using var context = await _contextFactory.CreateDbContextAsync();

        IQueryable<Flight> query = context
            .Flights
            .AsNoTracking()
            .Include(f => f.AirportOrigin)
            .ThenInclude(a => a.City)
            .ThenInclude(c => c.Country)
            .Include(f => f.AirportDestination)
            .ThenInclude(a => a.City)
            .ThenInclude(c => c.Country);

        //Technically not required, but for modularity sake, the search can be universal if all parameters are null
        if (originId != null)
        {
            query = query.Where(f => f.AirportOriginId == originId);
        }

        if (destId != null)
        {
            query = query.Where(f => f.AirportDestinationId == destId);
        }

        if (minimumFreeSeats != null)
        {
            query = query.Where(f => f.SeatsFree >= minimumFreeSeats);
        }

        //adds the duration filter
        if (duration != null && duration > 1)
        {
            query = query.Where(f => f.DurationMinutes < duration);
        }

        //This way the user can look for specific cabin types or not.
        if (cabinType != null)
        {
            query = query.Where(f => f.CabinType == cabinType);
        }

        //Advice from gemini, using just "TimeDeparture == departureDate would need an exact match down to the ''.
        //Using .date can throw errors in some older SQL servers so I am now using the range within 1 day of the date
        if (departureDate != null)
        {
            DateTime departureDateStart = departureDate.Value.Date;
            DateTime departureDateEnd = departureDateStart.AddDays(1);
            query = query.Where(f => f.TimeDeparture >= departureDateStart && f.TimeDeparture < departureDateEnd);
        }

        return await query.ToListAsync();

    }
}