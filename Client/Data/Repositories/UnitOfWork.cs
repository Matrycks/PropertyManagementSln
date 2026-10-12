using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Data.DbContexts;
using Client.Data.Entities;
using Client.Data.Interfaces;

namespace Client.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClientContext _context;
        private IGenericRepository<RentalProperty>? _rentalProperties;
        private IGenericRepository<RentalUnit>? _rentalUnits;

        public UnitOfWork(ClientContext context)
        {
            _context = context;
        }

        public IGenericRepository<RentalProperty> RentalProperties
        {
            get
            {
                _rentalProperties ??= new GenericRepository<RentalProperty>(_context);
                return _rentalProperties;
            }
        }

        public IGenericRepository<RentalUnit> RentalUnits
        {
            get
            {
                _rentalUnits ??= new GenericRepository<RentalUnit>(_context);
                return _rentalUnits;
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}