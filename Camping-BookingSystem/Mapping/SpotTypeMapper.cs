using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.SpotTypeDTOs;

namespace Camping_BookingSystem.Mapping
{
    public static class SpotTypeMapper
    {
        public static SpotType ToSpotType(this CreateSpotTypeRequest dto)
        {
            return new SpotType
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price
            };
        }
        public static SpotType ToSpotType(this UpdateSpotTypeRequest dto)
        {
            return new SpotType
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price
            };
        }
        public static SpotTypeDetailsResponse ToSpotTypeDetailsResponse(this SpotType spotType)
        {
            return new SpotTypeDetailsResponse
            {
                Id = spotType.Id,
                Name = spotType.Name,
                Price = spotType.Price
            };
        }
    }
}
