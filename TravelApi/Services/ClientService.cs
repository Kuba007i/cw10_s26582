using Microsoft.EntityFrameworkCore;
using TravelApi.DTOs;
using TravelApi.Models;

namespace TravelApi.Services;

public class ClientService : IClientService
{
    private readonly TripContext _context;

    public ClientService(TripContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteClientAsync(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null || client.ClientTrips.Any())
            return false;

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> AddClientToTripAsync(int idTrip, CreateClientDTO dto)
    {
        // Sprawdź czy trip istnieje i nie jest w przeszłości
        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
        if (trip == null || trip.DateFrom < DateTime.Now)
            return "Trip not found or already started.";

        // Sprawdź czy klient już istnieje
        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);
        if (existingClient != null)
            return "Client with given PESEL already exists.";

        // Utwórz klienta
        var client = new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Pesel = dto.Pesel
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        // Sprawdź czy już przypisany
        var exists = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);
        if (exists)
            return "Client already signed up for this trip.";

        // Dodaj do Client_Trip
        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
        };

        await _context.ClientTrips.AddAsync(clientTrip);
        await _context.SaveChangesAsync();

        return null;
    }
}