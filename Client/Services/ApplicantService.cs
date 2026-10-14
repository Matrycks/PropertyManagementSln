using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;
using Client.Services.Interfaces;

namespace Client.Services
{
    public class ApplicantService : IApplicantService
    {
        public Task<ApplicantModel> GetApplicantById(int applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicantModel> GetApplicantByUserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}