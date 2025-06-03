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
            if (campSite == null)   // check if campsite is null
                throw new ArgumentNullException(nameof(campSite));

            if (string.IsNullOrWhiteSpace(campSite.Name))   // check if name is empty or whitespace
                throw new ArgumentException("Name cannot be empty or whitespace", nameof(campSite));

            await _campSiteRepository.CreateCampSiteAsync(campSite);
            return campSite;
        }

        public async Task UpdateAsync(CampSite campSite)    // update an existing campsite
        {
            if (campSite == null)   // check if campsite is null
                throw new ArgumentNullException(nameof(campSite));

            await _campSiteRepository.UpdateCampSiteAsync(campSite);
        }

        public async Task DeleteAsync(int id)   // delete a campsite
        {
            await _campSiteRepository.DeleteCampSiteAsync(id);
        }
    }
}
