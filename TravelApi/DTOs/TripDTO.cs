namespace TravelApi.DTOs;

public class TripDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }

    public List<string> Countries { get; set; } = new();
    public List<ClientSimpleDTO> Clients { get; set; } = new();
}

public class ClientSimpleDTO
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}