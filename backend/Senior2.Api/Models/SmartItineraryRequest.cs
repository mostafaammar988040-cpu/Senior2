using System;
using System.ComponentModel.DataAnnotations;

namespace Senior2.Api.Models
{
    public class SmartItineraryRequest
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public Users? User { get; set; }

        [Range(1, 50, ErrorMessage = "Travelers must be between 1 and 50.")]
        public int Travelers { get; set; } = 1;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(0, 10000, ErrorMessage = "Budget per day must be positive.")]
        public decimal BudgetPerDay { get; set; }

        [Required]
        public string TripType { get; set; } = string.Empty;

        public string ActivitiesJson { get; set; } = string.Empty;

        public string Transport { get; set; } = string.Empty;

        public string SpecialRequirements { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Active";

        public bool IncludeSavedPlaces { get; set; } = false;

        public string? ItineraryJson { get; set; }
    }
}