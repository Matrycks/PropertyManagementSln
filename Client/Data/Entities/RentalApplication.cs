using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data.Entities
{
    public class RentalApplication
    {
        public int RentalApplicationId { get; set; }
        public RentalUnit RentalUnit { get; set; } = null!;

        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; } = null!;
    }
}