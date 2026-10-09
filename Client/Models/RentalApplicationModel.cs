using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class RentalApplicationModel
    {
        public int RentalApplicationId { get; set; }
        public int RentalUnitId { get; set; }
        public RentalUnitModel? RentalUnit { get; set; }
        public ApplicantModel Applicant { get; set; } = null!;
        public List<ResidenceHistoryModel> ResidenceHistory { get; set; } = [];
        public int CurrentStep { get; set; } = 1;
        public bool ApplicantSaved { get; set; }
        public bool ResidenceHistorySaved { get; set; }
    }
}