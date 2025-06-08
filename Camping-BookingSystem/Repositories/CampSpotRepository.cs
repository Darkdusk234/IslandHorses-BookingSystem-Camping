using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;
using Microsoft.EntityFrameworkCore;

namespace Camping_BookingSystem.Repositories
{
    public class CampSpotRepository : ICampSpotRepository
    {
        private readonly CampingDbContext _context;

        public CampSpotRepository(CampingDbContext context)
        {
            _context = context;
        }
        
        public async Task Create(CampSpot campSpot)
        {
            await _context.CampSpots.AddAsync(campSpot);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(CampSpot campSpot)
        {
            _context.CampSpots.Remove(campSpot);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<CampSpot>> GetAll()
        {
            return await _context.CampSpots.ToListAsync();
        }

        public async Task<ICollection<CampSpot>> GetCampSpotsByCampSiteId(int campSiteId)
        {
            return await _context.CampSpots.Where(cs => cs.CampSiteId == campSiteId).ToListAsync();
        }

        public async Task<CampSpot?> GetCampSpotById(int campSpotId)
        {
            return await _context.CampSpots
                //.Include(c => c.SpotType)
                .FirstOrDefaultAsync(c => c.Id == campSpotId);
        }
        
        public async Task<List<SpotsBasedOnDatesRequest>> GetAvailableCampSpotsAsync(DateTime startDate, DateTime endDate, int campSiteId)
        {
            var allSpotsMatchingNeeds = await _context.CampSpots
                .Where(c =>
                    c.CampSiteId == campSiteId &&
                    !c.Bookings.Any(b => (b.StartDate < endDate && b.EndDate > startDate)))
                .Select(c => new SpotsBasedOnDatesRequest()
                {
                    Id = c.Id,
                    CampSiteName = c.CampSite.Name,
                    TypeName = c.SpotType.Name,
                    Capacity = c.SpotType.MaxPersonLimit,
                    Electricity = c.Electricity
                }).ToListAsync();
            
            return allSpotsMatchingNeeds; 
        }

        public async Task Update(CampSpot campSpot)
        {
            _context.CampSpots.Update(campSpot);
            await _context.SaveChangesAsync();
        }
        // As a receptionist, I want to be able to search
        // for vacancies based on type, date and number of guests
        public async Task<IEnumerable<CampSpot>> SearchAvailableSpots(SearchAvailableSpotsDto searchDto)
        {
            var query = _context.CampSpots
                //.Include(cs => cs.CampSite)     // Include the related CampSite, spottypes, bookings
                .Include(cs => cs.SpotType)     
                .Include(cs => cs.Bookings)     
                .AsQueryable();                 // IQueryable allows for further filtering and chaining

            query = query.Where(cs => cs.CampSiteId == searchDto.CampSiteId);   // Get the right campingsite
            
            if (searchDto.SpotTypeId > 0)   // Check if a specific spot type is selected
            {
                query = query.Where(cs => cs.TypeId == searchDto.SpotTypeId);
            }
            
            if (searchDto.NumberOfPeople > 0)   // Check if a specific number of people is specified
            {
                query = query.Where(cs => cs.SpotType != null && cs.SpotType.MaxPersonLimit >= searchDto.NumberOfPeople);
            }

            if (searchDto.RequiresElectricity.HasValue)   // Check if electricity is required
            {
                query = query.Where(cs => cs.Electricity == searchDto.RequiresElectricity.Value);
            }

            query = query.Where(predicate: cs => cs.Bookings != null && !cs.Bookings.Any(b =>     // Check if spot is already booked
            b.StartDate < searchDto.EndDate &&                  // Booking starts before the search end date
            b.EndDate > searchDto.StartDate));                  // Booking ends after the search start date

            return await query.ToListAsync();
        }
    }
}
