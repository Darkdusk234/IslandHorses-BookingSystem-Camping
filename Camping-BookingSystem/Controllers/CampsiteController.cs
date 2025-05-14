using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace Camping_BookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsiteController : ControllerBase
    {
        private readonly ICampSiteRepository _campSiteRepository;
        
        public CampsiteController(ICampSiteRepository campSiteRepository)
        {
            _campSiteRepository = campSiteRepository;
        }

        // GET: api/campsite 
        [HttpGet]   // Returns all campsites
        public async Task<IActionResult> GetAllCampSites()
        {
            var campSites = await _campSiteRepository.GetAllCampSitesAsync();
            return Ok(campSites);
        }

        // GET: api/campsite/{id}
        [HttpGet("{id}")]   // Returns a campsite by ID
        public async Task<IActionResult> GetCampSiteById(int id)
        {
            var campSite = await _campSiteRepository.GetCampSiteByIdAsync(id);
            if (campSite == null)
            {
                return NotFound($"Camping plats med id: {id}, finns inte, " +
                    $"Skriv in rätt nästa gång.");
            }
            return Ok(campSite);
        }

        // POST: api/campsite
        [HttpPost]  // Adds/creates a new campsite
        public async Task<IActionResult> AddCampSite([FromBody] CampSite campSite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // ModelState is invalid, breaks DataAnnotations
            }
            await _campSiteRepository.AddCampSiteAsync(campSite);
            return CreatedAtAction(nameof(GetCampSiteById), new { id = campSite.Id }, campSite);
        }

        // PUT: api/campsite/{id}
        [HttpPut("{id}")]   // Updates an existing campsite
        public async Task<IActionResult> UpdateCampSite(int id, [FromBody] CampSite campSite)
        {
            if (id != campSite.Id || !ModelState.IsValid)   
            {
                return BadRequest();    // if id is wrong or breaks DataAnnotations
            }
            await _campSiteRepository.UpdateCampSiteAsync(campSite);
            return NoContent(); 
        }

        // DELETE: api/campsite/{id}
        [HttpDelete("{id}")]    // Deletes a campsite
        public async Task<IActionResult> DeleteCampSite(int id)
        {
            var campSite = await _campSiteRepository.GetCampSiteByIdAsync(id);
            if (campSite == null)
            {
                return NotFound($"Camping plats med id: {id}, finns inte, " +
                    $"Skriv in rätt nästa gång.");
            }

            await _campSiteRepository.DeleteCampSiteAsync(id);
            return NoContent();
        }
    }
   
}
