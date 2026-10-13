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
    public class RentalApplicationService : IRentalApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentalApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateApplicationAsync(RentalApplicationModel application)
        {
            var nApplication = _mapper.Map<RentalApplication>(application);

            await _unitOfWork.RentalApplications.AddAsync(nApplication);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteApplicationAsync(int id)
        {
            var application = await _unitOfWork.RentalApplications.GetByIdAsync(id);
            if (application != null)
            {
                _unitOfWork.RentalApplications.Remove(application);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<RentalApplicationModel>> GetAllApplicationsAsync()
        {
            var applications = await _unitOfWork.RentalApplications.GetAllAsync();
            return _mapper.Map<IEnumerable<RentalApplicationModel>>(applications);
        }

        public async Task<RentalApplicationModel?> GetApplicationByIdAsync(int id)
        {
            var application = await _unitOfWork.RentalApplications.GetByIdAsync(id);
            return application != null ? _mapper.Map<RentalApplicationModel>(application) : null;
        }

        public async Task UpdateApplicationAsync(RentalApplicationModel application)
        {
            var existingApplication = await _unitOfWork.RentalApplications.GetByIdAsync(application.RentalApplicationId)
                ?? throw new KeyNotFoundException("Application not found.");

            var updatedEntity = _mapper.Map(application, existingApplication); // TODO: avoid saving relational properties

            _unitOfWork.RentalApplications.Update(updatedEntity);
            await _unitOfWork.CompleteAsync();
        }
    }
}