using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;
using Camping_BookingSystem.Repositories;

namespace Camping_BookingSystem.Services
{
    public class CampSpotService : ICampSpotService
    {
        private readonly ICampSpotRepository _campSpotRepository;
        private readonly ICampSiteRepository _campSiteRepository;

        public CampSpotService(ICampSpotRepository campSpotRepository, ICampSiteRepository campSiteRepository)
        {
            _campSpotRepository = campSpotRepository;
            _campSiteRepository = campSiteRepository;
        }

        public async Task<CampSpot> AddCampSpotAsync(CampSpot campSpot)
        {
            await _campSpotRepository.Create(campSpot);
            return campSpot;
        }

        public async Task<(bool success, string? errorMessage)> DeleteCampSpotAsync(int id)
        {
            var campSpot = await _campSpotRepository.GetCampSpotById(id);
            if(campSpot == null)
            {
                return (false, "Camp spot not found.");
            }

            await _campSpotRepository.Delete(campSpot);
            return (true, null);
        }

        public async Task<IEnumerable<CampSpot>> GetAllCampSpotsAsync()
        {
            return await _campSpotRepository.GetAll();
        }

        public async Task<(IEnumerable<CampSpot>?, bool campSiteFound)> GetCampSpotsByCampSiteIdAsync(int campSiteId)
        {
            var campSite = await _campSiteRepository.GetCampSiteByIdAsync(campSiteId);
            if(campSite == null)
            {
                return (null, false);
            }
            var campSpots = await _campSpotRepository.GetCampSpotsByCampSiteId(campSiteId);
            return (campSpots, true);
        }

        public async Task<CampSpot?> GetCampSpotByIdAsync(int id)
        {
            return await _campSpotRepository.GetCampSpotById(id);
        }
        
        public async Task<IEnumerable<CampSpot>> GetAvailableSpotsMatchingNeeds(DateTime startDate, DateTime endDate, int typeId/*, int nrGuests*/)
        {
            return await _campSpotRepository.GetAvailableCampSpotsAsync(startDate, endDate, typeId/*, 
                nrGuests*/); 
        }

        public async Task<(bool success, string? errorMessage)> UpdateCampSpotAsync(int id, CreateCampSpotRequest request)
        {
            var existingCampSpot = await _campSpotRepository.GetCampSpotById(id);
            if (existingCampSpot == null)
            {
                return (false, "Camp spot not found.");
            }

            existingCampSpot.CampSiteId = request.CampSiteId;
            existingCampSpot.TypeId = request.TypeId;
            existingCampSpot.Electricity = request.Electricity;
            await _campSpotRepository.Update(existingCampSpot);

            return (true, null);
        }

        public async Task<IEnumerable<CampSpot>> SearchAvailableSpotsAsync(SearchAvailableSpotsDto searchDto)
        {
            return await _campSpotRepository.SearchAvailableSpots(searchDto);
        }
    }
}
