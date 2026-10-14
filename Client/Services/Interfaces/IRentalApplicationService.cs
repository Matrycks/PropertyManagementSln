using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;

namespace Client.Services.Interfaces
{
    public interface IRentalApplicationService
    {
        Task<IEnumerable<RentalApplicationModel>> GetAllUserApplicationsAsync(string userId, int? rentalPropertyId = null,
            ApplicationStatus? applicationStatus = null);
        Task<IEnumerable<RentalApplicationModel>> GetAllApplicationsAsync(int? rentalPropertyId = null,
            ApplicationStatus? applicationStatus = null);
        Task<RentalApplicationModel?> GetApplicationByIdAsync(int id);
        Task<int> CreateApplicationAsync(RentalApplicationModel application);
        Task UpdateApplicationAsync(RentalApplicationModel application);
        Task DeleteApplicationAsync(int id);
    }
}