public class UserPreference
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public Users User { get; set; }

    public string PreferencesJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}