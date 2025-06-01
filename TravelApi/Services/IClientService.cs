using TravelApi.DTOs;

namespace TravelApi.Services;

public interface IClientService
{
    Task<bool> DeleteClientAsync(int idClient);
    Task<string?> AddClientToTripAsync(int idTrip, CreateClientDTO dto);
}