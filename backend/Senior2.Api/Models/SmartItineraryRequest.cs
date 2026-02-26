public class SmartItineraryRequest
{
    public int UserId { get; set; }
    public string Travelers { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal BudgetPerDay { get; set; }
    public string TripType { get; set; }
    public List<string> Activities { get; set; }
    public string Transport { get; set; }
    public string SpecialRequirements { get; set; }
}