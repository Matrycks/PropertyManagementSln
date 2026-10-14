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
    public class RentalApplicationService : IRentalApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentalApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateApplicationAsync(RentalApplicationModel application)
        {
            // Set Applicant ref. for Residence History
            application.ResidenceHistory.ForEach((r) => r.UserId = application.UserId);
            application.Status = ApplicationStatus.Submitted;

            var nApplication = _mapper.Map<RentalApplication>(application);

            await _unitOfWork.RentalApplications.AddAsync(nApplication);
            await _unitOfWork.CompleteAsync();

            return nApplication.RentalApplicationId;
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

        public async Task<IEnumerable<RentalApplicationModel>> GetAllUserApplicationsAsync(string userId, int? rentalPropertyId = null,
            ApplicationStatus? applicationStatus = null)
        {
            var query = _unitOfWork.RentalApplications.Query()
                        .Include(x => x.RentalUnit).Include(x => x.RentalProperty)
                        .Where(x => x.UserId == userId);

            if (rentalPropertyId != null && rentalPropertyId > 0)
                query = query.Where(x => x.RentalPropertyId == rentalPropertyId);

            if (applicationStatus != null)
                query = query.Where(x => x.Status == applicationStatus);

            var applications = await query.ToListAsync();

            return _mapper.Map<IEnumerable<RentalApplicationModel>>(applications);
        }

        public async Task<IEnumerable<RentalApplicationModel>> GetAllApplicationsAsync(int? rentalPropertyId = null,
            ApplicationStatus? applicationStatus = null)
        {
            var query = _unitOfWork.RentalApplications.Query()
                        .Include(x => x.RentalUnit).Include(x => x.RentalProperty)
                        .AsQueryable();

            if (rentalPropertyId != null && rentalPropertyId > 0)
                query = query.Where(x => x.RentalPropertyId == rentalPropertyId);

            if (applicationStatus != null)
                query = query.Where(x => x.Status == applicationStatus);

            var applications = await query.ToListAsync();

            return _mapper.Map<IEnumerable<RentalApplicationModel>>(applications);
        }

        public async Task<RentalApplicationModel?> GetApplicationByIdAsync(int id)
        {
            var application = await _unitOfWork.RentalApplications.Query()
                .Include(x => x.ResidenceHistory)
                .Include(x => x.RentalUnit)
                .ThenInclude(x => x.FloorPlan)
                .SingleOrDefaultAsync(x => x.RentalApplicationId == id);

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