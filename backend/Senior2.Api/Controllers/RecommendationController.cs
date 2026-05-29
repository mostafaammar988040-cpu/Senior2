using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Senior2.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    public class RecommendationController : ControllerBase
    {
        private readonly RecommendationService _service;

        public RecommendationController(RecommendationService service)
        {
            _service = service;
        }

        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetRecommendations()
        {
            int userId = 1; // temporary test

            var recommendations = await _service.GetRecommendations(userId);

            return Ok(recommendations);
        }
    }
}