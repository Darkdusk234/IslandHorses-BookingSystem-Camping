using System.Runtime.CompilerServices;
using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Services
{
    public interface ICampSiteService
    {
        Task<IEnumerable<CampSite>> GetAllAsync();
        Task<CampSite?> GetByIdAsync(int id);
        Task<CampSite> CreateAsync(CampSite campSite);
        Task UpdateAsync(CampSite campSite);
        Task DeleteAsync(int id);
    }
}