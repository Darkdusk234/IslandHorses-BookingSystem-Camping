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
    }
}
