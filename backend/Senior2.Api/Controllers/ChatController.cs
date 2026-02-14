using Microsoft.AspNetCore.Mvc;
using Senior2.Api.Models;
using Senior2.Api.Services;

namespace Senior2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatOrchestratorService _chatService;

    public ChatController(ChatOrchestratorService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message cannot be empty.");

        var result = await _chatService.ProcessAsync(request.Message);

        return Ok(result);
    }
}
