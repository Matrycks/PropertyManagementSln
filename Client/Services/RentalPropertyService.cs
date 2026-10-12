using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Data.Entities;
using Client.Data.Interfaces;
using Client.Models;
using Client.Services.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Client.Services
{
    public class RentalPropertyService : IRentalPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentalPropertyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RentalPropertyModel>> GetAllPropertiesAsync()
        {
            var query = _unitOfWork.RentalProperties.Query();

            var properties = await (from p in query
                                    select new RentalPropertyModel
                                    {
                                        RentalPropertyId = p.RentalPropertyId,
                                        Name = p.Name,
                                        Address = p.Address,
                                        City = p.City,
                                        State = p.State,
                                        ZipCode = p.ZipCode,
                                        Description = p.Description,
                                        IsActive = p.IsActive,
                                        TotalUnits = p.RentalUnits.Count(),
                                        AvailableUnits = p.RentalUnits.Where(x => x.IsOccupied == false).Count()
                                    }).ToListAsync();

            return properties;
        }

        public async Task<RentalPropertyModel?> GetPropertyByIdAsync(int id)
        {
            var property = await _unitOfWork.RentalProperties.Query()
                .Include(x => x.RentalUnits)
                .ThenInclude(x => x.FloorPlan)
                .SingleOrDefaultAsync(x => x.RentalPropertyId == id);
            return property != null ? _mapper.Map<RentalPropertyModel>(property) : null;
        }

        public async Task<IEnumerable<RentalPropertyModel>> GetActivePropertiesAsync()
        {
            var properties = await _unitOfWork.RentalProperties.FindAsync(p => p.IsActive);
            return properties.Select(p => _mapper.Map<RentalPropertyModel>(p));
        }

        public async Task CreatePropertyAsync(RentalPropertyModel property)
        {
            var newProperty = _mapper.Map<RentalProperty>(property);

            await _unitOfWork.RentalProperties.AddAsync(newProperty);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdatePropertyAsync(RentalPropertyModel property)
        {
            var existingProperty = await _unitOfWork.RentalProperties.GetByIdAsync(property.RentalPropertyId)
                ?? throw new KeyNotFoundException("Property not found.");

            var updatedProperty = _mapper.Map(property, existingProperty);

            _unitOfWork.RentalProperties.Update(updatedProperty);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeletePropertyAsync(int id)
        {
            var property = await _unitOfWork.RentalProperties.GetByIdAsync(id);
            if (property != null)
            {
                _unitOfWork.RentalProperties.Remove(property);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}