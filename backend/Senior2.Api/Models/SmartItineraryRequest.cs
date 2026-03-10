using System.ComponentModel.DataAnnotations;

public class SmartItineraryRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Travelers { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal BudgetPerDay { get; set; }

    public string TripType { get; set; } = string.Empty;

    public string ActivitiesJson { get; set; } = string.Empty;

    public string Transport { get; set; } = string.Empty;

    public string SpecialRequirements { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Active";

    // ⭐ NEW OPTION
    public bool IncludeSavedPlaces { get; set; } = false;
    public string? ItineraryJson { get; set; }
}