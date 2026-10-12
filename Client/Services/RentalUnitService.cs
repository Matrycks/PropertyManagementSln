using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Data.Entities;
using Client.Data.Interfaces;
using Client.Models;
using Client.Services.Interfaces;
using MapsterMapper;

namespace Client.Services
{
    public class RentalUnitService : IRentalUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentalUnitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(RentalUnitModel rentalUnit)
        {
            var rentalUnitEntity = _mapper.Map<RentalUnit>(rentalUnit);
            await _unitOfWork.RentalUnits.AddAsync(rentalUnitEntity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rentalUnitEntity = await _unitOfWork.RentalUnits.GetByIdAsync(id);
            if (rentalUnitEntity != null)
            {
                _unitOfWork.RentalUnits.Remove(rentalUnitEntity);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<RentalUnitModel>> GetAllAsync()
        {
            var rentalUnits = await _unitOfWork.RentalUnits.GetAllAsync();
            return rentalUnits.Select(ru => _mapper.Map<RentalUnitModel>(ru));
        }

        public async Task<IEnumerable<RentalUnitModel>> GetByPropertyIdAsync(int propertyId)
        {
            var rentalUnits = await _unitOfWork.RentalUnits.FindAsync(ru => ru.RentalPropertyId == propertyId);
            return rentalUnits.Select(ru => _mapper.Map<RentalUnitModel>(ru));
        }

        public async Task<IEnumerable<RentalUnitModel>> GetAvailableAsync()
        {
            var availableRentalUnits = await _unitOfWork.RentalUnits.FindAsync(ru => ru.IsOccupied == false);
            return availableRentalUnits.Select(ru => _mapper.Map<RentalUnitModel>(ru));
        }

        public async Task<RentalUnitModel?> GetByIdAsync(int id)
        {
            var rentalUnitEntity = await _unitOfWork.RentalUnits.GetByIdAsync(id);
            return rentalUnitEntity != null ? _mapper.Map<RentalUnitModel>(rentalUnitEntity) : null;
        }

        public async Task UpdateAsync(RentalUnitModel rentalUnit)
        {
            var rentalUnitEntity = _mapper.Map<RentalUnit>(rentalUnit);
            _unitOfWork.RentalUnits.Update(rentalUnitEntity);
            await _unitOfWork.CompleteAsync();
        }
    }
}