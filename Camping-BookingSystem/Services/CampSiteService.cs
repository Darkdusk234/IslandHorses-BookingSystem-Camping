using BookingSystem_ClassLibrary.Data;
using System.Runtime.CompilerServices;
using BookingSystem_ClassLibrary.Models;
//using Camping_BookingSystem.Repositories;

namespace Camping_BookingSystem.Services
{
    public class CampSiteService : ICampSiteService
    {
        private readonly ICampSiteRepository _campSiteRepository;   // repository for campsite

        public CampSiteService(ICampSiteRepository campSiteRepository)
        {
            _campSiteRepository = campSiteRepository;
        }

        public async Task<IEnumerable<CampSite>> GetAllAsync()  // get all campsites
        {
            return await _campSiteRepository.GetAllCampSitesAsync();
        }

        public async Task<CampSite?> GetByIdAsync(int id)   // get campsite by id
        {
            return await _campSiteRepository.GetCampSiteByIdAsync(id);
        }

        public async Task<CampSite> CreateAsync(CampSite campSite)  // create a new campsite
        {
            await _campSiteRepository.CreateCampSiteAsync(campSite);
            return campSite;
        }

        public async Task UpdateAsync(CampSite campSite)    // update an existing campsite
        {
            await _campSiteRepository.UpdateCampSiteAsync(campSite);
        }

        public async Task DeleteAsync(int id)   // delete a campsite
        {
            await _campSiteRepository.DeleteCampSiteAsync(id);
        }
    }
}
