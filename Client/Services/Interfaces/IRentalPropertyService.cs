using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;

namespace Client.Services.Interfaces
{
    public interface IRentalPropertyService
    {
        Task<IEnumerable<RentalPropertyModel>> GetAllPropertiesAsync();
        Task<RentalPropertyModel?> GetPropertyByIdAsync(int id);
        Task<IEnumerable<RentalPropertyModel>> GetActivePropertiesAsync();
        Task CreatePropertyAsync(RentalPropertyModel property);
        Task UpdatePropertyAsync(RentalPropertyModel property);
        Task DeletePropertyAsync(int id);
    }
}