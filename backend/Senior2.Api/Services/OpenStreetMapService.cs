using Senior2.Api.Models;

namespace Senior2.Api.Services;

public class OpenStreetMapService
{
    public async Task<List<PlaceResult>> SearchAsync(string query)
    {
        return await Task.FromResult(new List<PlaceResult>
        {
            new PlaceResult
            {
                Name = "Pigeon Rocks",
                Type = "Beach",
                City = "Beirut"
            }
        });
    }
}
