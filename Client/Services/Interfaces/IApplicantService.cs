using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;

namespace Client.Services.Interfaces
{
    public interface IApplicantService
    {
        Task<ApplicantModel> GetApplicantById(int applicantId);
        Task<ApplicantModel> GetApplicantByUserId(string userId);
    }
}