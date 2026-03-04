namespace Senior2.Api.DTOs.Suggestion
{
    public class CreateSuggestionDto
    {
        public int UserId { get; set; }

        public string Type { get; set; } = "";

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string? Location { get; set; }
    }
}