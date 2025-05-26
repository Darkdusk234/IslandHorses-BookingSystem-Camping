using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;
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

        public async Task<CampSpot> AddCampSpotAsync(CampSpot campSpot)
        {
            await _campSpotRepository.Create(campSpot);
            return campSpot;
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

        public async Task<CampSpot?> GetCampSpotByIdAsync(int id)
        {
            return await _campSpotRepository.GetCampSpotById(id);
        }

        public async Task UpdateCampSpotAsync(CampSpot campSpot)
        {
            await _campSpotRepository.Update(campSpot);
        }

        public async Task<IEnumerable<CampSpot>> SearchAvailableSpotsAsync(SearchAvailableSpotsDto searchDto)
        {
            return await _campSpotRepository.SearchAvailableSpots(searchDto);
        }
    }
}
