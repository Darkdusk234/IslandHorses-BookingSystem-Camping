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
        [Tags("Reseptionist")]
        [HttpGet("SearchAvailableSpot")]
        public async Task<IActionResult> GetAvailableCampSpots([FromQuery] SearchAvailableSpotsDto searchDto)
        {
            if (searchDto == null)      // Check if search criteria is null
            {
                return BadRequest("Search criteria cannot be null.");
            }

            if (searchDto. StartDate >= searchDto.EndDate)      // Check if start date is before end date
            {
                return BadRequest("Start date must be before end date.");
            }
            if (searchDto.StartDate < DateTime.Today)       // Check if start date is in the past
            {
                return BadRequest("This is not a time traveling campspot. You silly goose!");
            }

            if (searchDto.NumberOfPeople <= 0)          // Check if number of people is valid
            {
                return BadRequest("You can not be negativ 1 people when you are booking");
            }

            try
            {
                var availableSpots = await _campSpotService.SearchAvailableSpotsAsync(searchDto);   // avaiable spots based on search criteria
                var spotsList = availableSpots.ToList();                            // convert to "avaiable spots" to list for easier handling

                if (spotsList.Any())    // if any spots are avaiable
                {
                    return Ok(new   // return if they found avaiable spots based on criteria
                    {
                        Success = true,
                        Message = $"Found {spotsList.Count} available camping spots for your search criteria.",
                        Count = spotsList.Count,
                        AvailableSpots = spotsList
                    });
                }
                else
                {
                    return Ok(new   // return if no spots are avaiable based on "right" criteria
                    {
                        Success = false,
                        Message = "No available camping spots found for the specified criteria. Please try different dates or requirements.",
                        Count = 0,
                        AvailableSpots = new List<object>()
                    });
                }
            }
            catch (Exception ex)    // if users input is not valid/wrong and they are a silly goose
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while searching for available spots.",
                    Error = ex.Message
                });
            }
        }
    }
}
