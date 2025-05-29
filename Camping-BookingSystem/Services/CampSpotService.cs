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
        
        public async Task<IEnumerable<CampSpot>> GetAvailableSpotsMatchingNeeds(DateTime startDate, DateTime endDate, int typeId/*, int nrGuests*/)
        {
            return await _campSpotRepository.GetAvailableCampSpotsAsync(startDate, endDate, typeId/*, 
                nrGuests*/); 
        }

        public async Task UpdateCampSpotAsync(CampSpot campSpot)
        {
            await _campSpotRepository.Update(campSpot);
        }

        public async Task<SearchResult<CampSpot>> SearchAvailableSpotsAsync(SearchAvailableSpotsDto searchDto)
        {
            try
            {
                // Valedation checks
                if (searchDto == null)
                {
                    return new SearchResult<CampSpot>
                    {
                        IsSuccess = false,
                        Message = "Search criteria cannot be null.",
                        AvaiableSpotsCount = 0,
                        AvailableSpots = new List<CampSpot>()
                    };
                }

                if (searchDto.StartDate >= searchDto.EndDate)
                {
                    return new SearchResult<CampSpot>
                    {
                        IsSuccess = false,
                        Message = "Start date must be before end date.",
                        AvaiableSpotsCount = 0,
                        AvailableSpots = new List<CampSpot>()
                    };
                }

                if (searchDto.StartDate < DateTime.Today)
                {
                    return new SearchResult<CampSpot>
                    {
                        IsSuccess = false,
                        Message = "This is not a time traveling campspot. You silly goose!",
                        AvaiableSpotsCount = 0,
                        AvailableSpots = new List<CampSpot>()
                    };
                }

                if (searchDto.NumberOfPeople <= 0)
                {
                    return new SearchResult<CampSpot>
                    {
                        IsSuccess = false,
                        Message = "You can not be negativ 1 people when you are booking",
                        AvaiableSpotsCount = 0,
                        AvailableSpots = new List<CampSpot>()
                    };
                }

                // Anropa repository
                var availableSpots = await _campSpotRepository.SearchAvailableSpots(searchDto);
                var spotsList = availableSpots.ToList();

                if (spotsList.Any())
                {
                    return new SearchResult<CampSpot>
                    {
                        IsSuccess = true,
                        Message = $"Found {spotsList.Count} available camping spots for your search criteria.",
                        AvaiableSpotsCount = spotsList.Count,
                        AvailableSpots = spotsList
                    };
                }
                else
                {
                    return new SearchResult<CampSpot>
                    {
                        IsSuccess = false,
                        Message = "No available camping spots found for the specified criteria. Please try different dates or requirements.",
                        AvaiableSpotsCount = 0,
                        AvailableSpots = new List<CampSpot>()
                    };
                }
            }
            catch (Exception ex)
            {
                return new SearchResult<CampSpot>
                {
                    IsSuccess = false,
                    Message = "An error occurred while searching for available spots.",
                    AvaiableSpotsCount = 0,
                    AvailableSpots = new List<CampSpot>(),
                    ErrorMessage = ex.Message
                };
            }

        }
    }
}
