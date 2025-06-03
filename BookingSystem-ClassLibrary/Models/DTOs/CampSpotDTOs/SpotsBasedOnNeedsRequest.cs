namespace BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;

public record SpotsBasedOnDatesRequest
{
    public int Id { get; init; }
    public string CampSiteName { get; init; } 
    public string TypeName { get; init; }
    public int Capacity { get; init; }
    public bool Electricity { get; init; }
}