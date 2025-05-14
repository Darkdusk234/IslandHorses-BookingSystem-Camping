using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSystem_ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem_ClassLibrary.Data
{
    public class CampSiteRepository : ICampSiteRepository
    {
        private readonly CampingDbContext _context;

        public CampSiteRepository(CampingDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CampSite>> GetAllCampSitesAsync() // Get all campsites
        {
            return await _context.CampSites.ToListAsync();
        }

        public async Task<CampSite?> GetCampSiteByIdAsync(int id) // Get a campsite by ID
        {
            return await _context.CampSites
                     .Include(c => c.CampSpots)
                     .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCampSiteAsync(CampSite campSite)   // Add a new campsite
        {
            await _context.CampSites.AddAsync(campSite);
            await _context.SaveChangesAsync();
        }

        public async Task<CampSite> UpdateCampSiteAsync(CampSite campSite) // Update an existing campsite
        {
            _context.CampSites.Update(campSite);
            await _context.SaveChangesAsync();
            return campSite;
        }

        public async Task DeleteAsync(int id)   // Delete a campsite
        {
            var site = await _context.CampSites.FindAsync(id);
            if (site != null)
            {
                _context.CampSites.Remove(site);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync() // Save changes to the database
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
