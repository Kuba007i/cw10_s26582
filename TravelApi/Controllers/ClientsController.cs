using Microsoft.AspNetCore.Mvc;
using TravelApi.DTOs;
using TravelApi.Services;

namespace TravelApi.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var success = await _clientService.DeleteClientAsync(idClient);
        if (!success)
            return BadRequest("Cannot delete client — either not found or assigned to a trip.");

        return NoContent();
    }

    [HttpPost("/api/trips/{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] CreateClientDTO dto)
    {
        var error = await _clientService.AddClientToTripAsync(idTrip, dto);
        if (error != null)
            return BadRequest(error);

        return Ok("Client added to trip.");
    }
}