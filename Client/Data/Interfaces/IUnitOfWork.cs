using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Data.Entities;

namespace Client.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<RentalProperty> RentalProperties { get; }
        IGenericRepository<RentalUnit> RentalUnits { get; }
        IGenericRepository<RentalApplication> RentalApplications { get; }
        Task<int> CompleteAsync();
    }
}