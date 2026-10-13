using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;

namespace Client.Services.Interfaces
{
    public interface IRentalApplicationService
    {
        Task<IEnumerable<RentalApplicationModel>> GetAllApplicationsAsync();
        Task<RentalApplicationModel?> GetApplicationByIdAsync(int id);
        Task CreateApplicationAsync(RentalApplicationModel application);
        Task UpdateApplicationAsync(RentalApplicationModel application);
        Task DeleteApplicationAsync(int id);
    }
}