using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Senior2.Api.Models;

public class ChatResponse
{
    public string Reply { get; set; }
    public string Intent { get; set; }
    public List<PlaceResult>? Places { get; set; }
    public List<string>? Sources { get; set; }
}
