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
            var spotType = new SpotType
            {
                Name = request.Name,
                Price = request.Price
            };
            await _spotTypeService.CreateSpotTypeAsync(spotType);
            return CreatedAtAction(nameof(GetSpotTypeById), new { id = spotType.Id }, spotType.ToSpotTypeDetailsResponse());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpotType(int id, [FromBody] UpdateSpotTypeRequest request)
        {
            var spotType = await _spotTypeService.GetSpotTypeByIdAsync(id);
            if (spotType == null)
            {
                return NotFound();
            }
            spotType.Name = request.Name;
            spotType.Price = request.Price;
            await _spotTypeService.UpdateSpotTypeAsync(spotType);
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
