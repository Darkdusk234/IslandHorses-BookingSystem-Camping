using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;

namespace Camping_BookingSystem.Services
{
    public class SpotTypeService : ISpotTypeService
    {
        private readonly ISpotTypeRepository _spotTypeRepository;
        public SpotTypeService(ISpotTypeRepository spotTypeRepository)
        {
            _spotTypeRepository = spotTypeRepository;
        }
        public async Task<IEnumerable<SpotType>> GetAllSpotTypesAsync()
        {
            return await _spotTypeRepository.GetAllAsync();
        }
        public async Task<SpotType?> GetSpotTypeByIdAsync(int id)
        {
            return await _spotTypeRepository.GetByIdAsync(id);
        }
        public async Task<SpotType> CreateSpotTypeAsync(SpotType spotType)
        {
            if (spotType == null)   // check if spotType is null
            {
                throw new ArgumentNullException(nameof(spotType));
            }

            if (string.IsNullOrWhiteSpace(spotType.Name))   // check if name is empty or whitespace
            {
                throw new ArgumentException("Name cannot be empty", nameof(spotType));
            }

            if (spotType.Price < 0)     // check if price is negative
            {
                throw new ArgumentException("Price cannot be negative", nameof(spotType));
            }

            if (spotType.MaxPersonLimit <= 0)   // check if MaxPersonLimit is less than or equal to 0
            {
                throw new ArgumentException("MaxPersonLimit must be greater than 0", nameof(spotType));
            }

            await _spotTypeRepository.AddAsync(spotType);
            await _spotTypeRepository.SaveAsync();
            return spotType;
        }
        public async Task UpdateSpotTypeAsync(SpotType spotType)
        {
            if (spotType == null)   // check if spotType is null
            {
                throw new ArgumentNullException(nameof(spotType));
            }

            await _spotTypeRepository.UpdateAsync(spotType);
            await _spotTypeRepository.SaveAsync();
        }
        public async Task DeleteSpotTypeAsync(int id)
        {
            var spotType = await _spotTypeRepository.GetByIdAsync(id);
            if (spotType != null)
            {
                await _spotTypeRepository.DeleteAsync(spotType);
                await _spotTypeRepository.SaveAsync();
            }
        }
    }
}
