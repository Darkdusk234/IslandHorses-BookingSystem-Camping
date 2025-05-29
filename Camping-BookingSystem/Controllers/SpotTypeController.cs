using Microsoft.AspNetCore.Mvc;
using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.SpotTypeDTOs;
using Camping_BookingSystem.Services;
using Camping_BookingSystem.Mapping;

namespace Camping_BookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SpotTypeController : ControllerBase
    {
        private readonly ISpotTypeService _spotTypeService;
        public SpotTypeController(ISpotTypeService spotTypeService)
        {
            _spotTypeService = spotTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpotTypes()
        {
            var spotTypes = await _spotTypeService.GetAllSpotTypesAsync();
            var response = spotTypes.Select(st => st.ToSpotTypeDetailsResponse());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpotTypeById(int id)
        {
            var spotType = await _spotTypeService.GetSpotTypeByIdAsync(id);
            if (spotType == null)
            {
                return NotFound();
            }
            var response = spotType.ToSpotTypeDetailsResponse();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpotType([FromBody] CreateSpotTypeRequest request)
        {
            if (!ModelState.IsValid)  // valedation check
                return BadRequest(ModelState);

            var spotType = request.ToNewSpotType();  // fixed mapping method name
            await _spotTypeService.CreateSpotTypeAsync(spotType);

            return CreatedAtAction(nameof(GetSpotTypeById), new { id = spotType.Id },
                spotType.ToSpotTypeDetailsResponse());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpotType(int id, [FromBody] UpdateSpotTypeRequest request)
        {
            if (!ModelState.IsValid)  // validation check
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)   // check if the ID in the URL matches the ID in the request body
            {
                return BadRequest("No ID for taht type");
            }
            var existingSpotType = await _spotTypeService.GetSpotTypeByIdAsync(id);
            if (existingSpotType == null)
            {
                return NotFound();
            }

            existingSpotType.Name = request.Name;
            existingSpotType.Price = request.Price;
            existingSpotType.MaxPersonLimit = request.MaxPersonLimit;  // added max person property

            await _spotTypeService.UpdateSpotTypeAsync(existingSpotType);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpotType(int id)
        {
            var spotType = await _spotTypeService.GetSpotTypeByIdAsync(id);
            if (spotType == null)
            {
                return NotFound();
            }
            await _spotTypeService.DeleteSpotTypeAsync(spotType.Id);
            return NoContent();
        }
    }
}
