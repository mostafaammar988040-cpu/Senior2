using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Senior2.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    [Authorize]
    public class RecommendationController : ControllerBase
    {
        private readonly RecommendationService _service;

        public RecommendationController(RecommendationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendations()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token");

            int userId = int.Parse(userIdClaim.Value);

            var recommendations = await _service.GetRecommendations(userId);

            return Ok(recommendations);
        }
    }
}