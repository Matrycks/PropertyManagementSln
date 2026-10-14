using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;

namespace Client.Services.Interfaces
{
    public interface IRentalUnitService
    {
        Task<IEnumerable<RentalUnitModel>> GetAllAsync();
        Task<RentalUnitModel?> GetByIdAsync(int id);
        Task<IEnumerable<RentalUnitModel>> GetByPropertyIdAsync(int propertyId);
        Task<IEnumerable<RentalUnitModel>> GetByPropertyIdAsync(int propertyId, int floorPlanId);
        Task<IEnumerable<RentalUnitModel>> GetAvailableAsync();
        Task CreateAsync(RentalUnitModel rentalUnit);
        Task UpdateAsync(RentalUnitModel rentalUnit);
        Task DeleteAsync(int id);
    }
}