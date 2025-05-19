using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs;
using BookingSystem_ClassLibrary.Models.DTOs.CampSiteDTO;

namespace Camping_BookingSystem.Mapping
{
    public static class CampSiteMapper
    {
        public static CampSite ToCampSite(this CreateCampSiteRequest dto)
        {
            return new CampSite
            {
                Name = dto.Name,
                Description = dto.Description,
                Adress = dto.Adress,
              
            };
        }

        public static CampSite ToCampSite(this UpdateCampSiteRequest dto)
        {
            return new CampSite
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Adress = dto.Adress,
            };
        }

        public static CampSiteDetailsResponse ToCampSiteDetailsResponse(this CampSite campSite)
        {
            return new CampSiteDetailsResponse
            {
                Id = campSite.Id,
                Name = campSite.Name,
                Description = campSite.Description,
                Adress = campSite.Adress,
            };
        }
    }
}
