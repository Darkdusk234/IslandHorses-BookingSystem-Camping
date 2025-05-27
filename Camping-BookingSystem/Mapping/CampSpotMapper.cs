using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;

namespace Camping_BookingSystem.Mapping
{
    public static class CampSpotMapper
    {
        public static CampSpot ToCampSpot(this CreateCampSpotRequest dto)
        {
            return new CampSpot
            {
                CampSiteId = dto.CampSiteId,
                TypeId = dto.TypeId,
                Electricity = dto.Electricity
            };
        }
    }
}
