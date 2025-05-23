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
            await _spotTypeRepository.AddAsync(spotType);
            await _spotTypeRepository.SaveAsync();
            return spotType;
        }
        public async Task UpdateSpotTypeAsync(SpotType spotType)
        {
            _spotTypeRepository.Update(spotType);
            await _spotTypeRepository.SaveAsync();
        }
        public async Task DeleteSpotTypeAsync(int id)
        {
            var spotType = await _spotTypeRepository.GetByIdAsync(id);
            if (spotType != null)
            {
                _spotTypeRepository.Delete(spotType);
                await _spotTypeRepository.SaveAsync();
            }
        }
    }
}
