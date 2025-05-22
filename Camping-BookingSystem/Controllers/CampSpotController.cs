using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;
using Camping_BookingSystem.Mapping;
using Camping_BookingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Camping_BookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampSpotController : ControllerBase
    {
        private readonly ICampSpotService _campSpotService;

        public CampSpotController(ICampSpotService campSpotService)
        {
            _campSpotService = campSpotService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCampSpots()
        {
            var campSpots = await _campSpotService.GetAllCampSpotsAsync();
            return Ok(campSpots);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampSpotById(int id)
        {
            var campSpot = await _campSpotService.GetCampSpotByIdAsync(id);
            if(campSpot == null)
            {
                return NotFound();
            }

            return Ok(campSpot);
        }

        [HttpGet("campSite/{campSiteId}")]
        public async Task<IActionResult> GetCampSpotsByCampSiteId(int campSiteId)
        {
            var campSpots = await _campSpotService.GetCampSpotsByCampSiteIdAsync(campSiteId);
            return Ok(campSpots);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampSpot([FromBody] CreateCampSpotRequest request)
        {
            var campSpot = request.ToCampSpot();
            var createdCampSpot = await _campSpotService.AddCampSpotAsync(campSpot);

            return CreatedAtAction(nameof(GetCampSpotById), new { id = createdCampSpot.Id }, createdCampSpot);
        }
    }
}
