using System.Runtime.InteropServices.JavaScript;
using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;
using Camping_BookingSystem.Mapping;
using Camping_BookingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            if (campSpot == null)
            {
                return NotFound("Camp spot not found.");
            }

            return Ok(campSpot);
        }

        [HttpGet("campSite/{campSiteId}")]
        public async Task<IActionResult> GetCampSpotsByCampSiteId(int campSiteId)
        {
            var (campSpots, campSiteFound) = await _campSpotService.GetCampSpotsByCampSiteIdAsync(campSiteId);
            if(!campSiteFound)
            {
                return NotFound("Campsite not found.");
            }
            return Ok(campSpots);
        }
        
        //Som gäst vill jag kunna söka alla lediga platser baserat på datum enbart
        [Tags("Guests")]
        [HttpGet("searchAvailableSpotsMatchingDates",Name = "GetFreeSpotsMatchingDates")]
        public async Task<ActionResult<IEnumerable<CampSpot>>> GetFreeSpotsMatchingDates(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int campSiteId)
        {
            var availableSpotBasedOnNeeds =  
                await _campSpotService.GetAvailableSpotsMatchingDates(startDate, endDate, campSiteId);
            if (availableSpotBasedOnNeeds == null)
            {
                return NotFound(); 
            }
            return Ok(availableSpotBasedOnNeeds); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampSpot([FromBody] CreateCampSpotRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var campSpot = request.ToCampSpot();
                var createdCampSpot = await _campSpotService.AddCampSpotAsync(campSpot);

                return CreatedAtAction(nameof(GetCampSpotById), new { id = createdCampSpot.Id }, createdCampSpot);
            } 
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampSpot(int id, [FromBody] CreateCampSpotRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, errorMessage) = await _campSpotService.UpdateCampSpotAsync(id, request);

            if(!success)
            {
                return NotFound(errorMessage);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampSpot(int id)
        {
            var (success, errorMessage) = await _campSpotService.DeleteCampSpotAsync(id);
            if(!success)
            {
                return NotFound(errorMessage);
            }

            return NoContent();
        }

        [Tags("Receptionist")]
        [HttpGet("SearchAvailableSpot")]
        public async Task<IActionResult> SearchAvailableSpot([FromQuery] SearchAvailableSpotsDto searchDto)
        {
            var result = await _campSpotService.SearchAvailableSpotsAsync(searchDto);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                // if server says no
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    return StatusCode(500, result);
                }
                else
                {
                    // Validetion error
                    return BadRequest(result);
                }
            }
        }
    }
}
