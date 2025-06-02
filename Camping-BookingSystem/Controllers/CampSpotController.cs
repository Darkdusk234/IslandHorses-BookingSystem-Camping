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
        
        //Som receptionist vill jag kunna söka lediga platser baserat på typ, datum och antal gäster
        [HttpGet("searchAvailableOnTypeDateCapacity",Name = "GetFreeSpotsMatchingNeeds")]
        public async Task<ActionResult<IEnumerable<CampSpot>>> GetFreeSpotsMatchingNeeds(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int typeID)
        {
            var availableSpotBasedOnNeeds =  
                await _campSpotService.GetAvailableSpotsMatchingNeeds(startDate, endDate, typeID);
            if (availableSpotBasedOnNeeds == null)
            {
                return NotFound(); 
            }
            return Ok(availableSpotBasedOnNeeds); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampSpot([FromBody] CreateCampSpotRequest request)
        {
            var campSpot = request.ToCampSpot();
            var createdCampSpot = await _campSpotService.AddCampSpotAsync(campSpot);

            return CreatedAtAction(nameof(GetCampSpotById), new { id = createdCampSpot.Id }, createdCampSpot);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampSpot(int id, [FromBody] CreateCampSpotRequest request)
        {
            var existingCampSpot = await _campSpotService.GetCampSpotByIdAsync(id);
            if (existingCampSpot == null)
            {
                return NotFound();
            }

            existingCampSpot.CampSiteId = request.CampSiteId;
            existingCampSpot.TypeId = request.TypeId;
            existingCampSpot.Electricity = request.Electricity;

            await _campSpotService.UpdateCampSpotAsync(existingCampSpot);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampSpot(int id)
        {
            var existingCampSpot = _campSpotService.GetCampSpotByIdAsync(id);
            if (existingCampSpot == null)
            {
                return NotFound();
            }

            await _campSpotService.DeleteCampSpotAsync(id);
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
