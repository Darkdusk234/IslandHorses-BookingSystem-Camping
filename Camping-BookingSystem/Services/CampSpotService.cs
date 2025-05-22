using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;

namespace Camping_BookingSystem.Services
{
    public class CampSpotService : ICampSpotService
    {
        private readonly ICampSpotRepository _campSpotRepository;

        public CampSpotService(ICampSpotRepository campSpotRepository)
        {
            _campSpotRepository = campSpotRepository;
        }

        public Task CreateCampSpotAsync(CampSpot campSpot)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteCampSpotAsync(int id)
        {
            var campSpot = await _campSpotRepository.GetCampSpotById(id);
            if(campSpot != null)
            {
                await _campSpotRepository.Delete(campSpot);
            }
        }

        public async Task<IEnumerable<CampSpot>> GetAllCampSpotsAsync()
        {
            return await _campSpotRepository.GetAll();
        }

        public async Task<IEnumerable<CampSpot>> GetCampSpotsByCampSiteIdAsync(int campSiteId)
        {
            return await _campSpotRepository.GetCampSpotsByCampSiteId(campSiteId);
        }

        public Task<CampSpot> GetCampSpotByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCampSpotAsync(CampSpot campSpot)
        {
            throw new NotImplementedException();
        }
    }
}
