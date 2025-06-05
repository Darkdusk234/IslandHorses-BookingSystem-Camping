using BookingSystem_ClassLibrary.Models.DTOs.CampSiteDTO;
using Camping_BookingSystem.Mapping;
using Camping_BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace Camping_BookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsiteController : ControllerBase
    {
        private readonly ICampSiteService _service;

        public CampsiteController(ICampSiteService service)
        {
            _service = service;
        }

        // GET: api/campsite 
        [HttpGet]   // Returns all campsites
        public async Task<IActionResult> GetAllCampSites()
        {
            var campSites = await _service.GetAllAsync();
            var result = campSites.Select(c => c.ToCampSiteDetailsResponse());
            return Ok(result);
        }

        // GET: api/campsite/{id}
        [HttpGet("{id}")]   // Returns a campsite by ID
        public async Task<IActionResult> GetCampSiteById(int id)
        {
            var campSite = await _service.GetByIdAsync(id);
            if (campSite == null)
            {
                return NotFound($"Camping plats med id: {id}, finns inte, " +
                    $"Skriv in rätt nästa gång.");
            }
            return Ok(campSite.ToCampSiteDetailsResponse());
        }

        // POST: api/campsite
        [HttpPost]  // Adds/creates a new campsite
        public async Task<IActionResult> AddCampSite([FromBody] CreateCampSiteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);  // if id is wrong or breaks DataAnnotations

            var campSite = request.ToCampSite();
            await _service.CreateAsync(campSite);

            return CreatedAtAction(nameof(GetCampSiteById), new { id = campSite.Id }, campSite.ToCampSiteDetailsResponse());
        }

        // PUT: api/campsite/{id}
        [HttpPut("{id}")]   // Updates an existing campsite
        public async Task<IActionResult> UpdateCampSite(int id, [FromBody] UpdateCampSiteRequest request)
        {
            if (id != request.Id || !ModelState.IsValid)
                return BadRequest();

            var campSite = await _service.GetByIdAsync(id);
            if (campSite == null)
            {
                return NotFound($"{ id } Finns ej, you silly goose");
            }
            campSite.Name = request.Name;
            campSite.Description = request.Description;
            campSite.Adress = request.Adress;

            await _service.UpdateAsync(campSite);
            return Ok($"{campSite.Name} har blivit uppdaterad");
        }

        // DELETE: api/campsite/{id}
        [HttpDelete("{id}")]    // Deletes a campsite
        public async Task<IActionResult> DeleteCampSite(int id)
        {

            if (id <= 0)
            {
                return BadRequest("Id måste vara större än 0, silly goose");

            }
            if (id <= 2) // Protect core test data (IDs 1-3)
            {
                return BadRequest("Cannot delete core test data (ID 1-3)");
            }

            var campSite = await _service.GetByIdAsync(id);
            if (campSite == null)
                return NotFound($"Finns ingen campingplats med id: {id}");

            await _service.DeleteAsync(id);
            return Ok($"{campSite.Name} har nu tagits bort");
        }
    }
   
}
