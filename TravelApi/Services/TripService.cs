using Microsoft.EntityFrameworkCore;
using TravelApi.DTOs;
using TravelApi.Models;

namespace TravelApi.Services;

public class TripService : ITripService
{
    private readonly TripContext _context;

    public TripService(TripContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<TripDTO> Trips, int TotalPages)> GetTripsAsync(int page, int pageSize)
    {
        // Bezpieczne parametry
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var totalTrips = await _context.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await _context.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries) // <- TO jest właściwa relacja do krajów
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries
                    .Select(c => c.Name)
                    .ToList(),
                Clients = t.ClientTrips
                    .Select(ct => new ClientSimpleDTO
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    })
                    .ToList()
            })
            .ToListAsync();

        return (trips, totalPages);
    }
}