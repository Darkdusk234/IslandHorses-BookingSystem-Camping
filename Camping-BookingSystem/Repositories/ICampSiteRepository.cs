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
        Task<CampSite> AddCampSiteAsync(CampSite campSite);
        Task<CampSite> UpdateCampSiteAsync(CampSite campSite);
        Task<bool> DeleteCampSiteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
