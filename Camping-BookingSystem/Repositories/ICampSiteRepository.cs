using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSystem_ClassLibrary.Models;

namespace BookingSystem_ClassLibrary.Data
{
    public interface ICampSiteRepository
    {
        Task<IEnumerable<CampSite>> GetAllCampSitesAsync();
        Task<CampSite?> GetCampSiteByIdAsync(int id);
        Task CreateCampSiteAsync(CampSite campSite);
        Task<CampSite> UpdateCampSiteAsync(CampSite campSite);
        Task DeleteCampSiteAsync(int id);
        
         Task<bool> SaveChangesAsync();
    }
}
