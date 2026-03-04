using Microsoft.AspNetCore.Http;

namespace Senior2.Api.DTOS
{
    public class UpdateJourneyRequest
    {
        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public IFormFile? Media { get; set; }
    }
}