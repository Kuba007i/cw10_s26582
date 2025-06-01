using TravelApi.DTOs;

namespace TravelApi.Services;

public interface ITripService
{
    Task<(IEnumerable<TripDTO> Trips, int TotalPages)> GetTripsAsync(int page, int pageSize);
}