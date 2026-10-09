using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data.Entities
{
    public class ResidenceHistory
    {
        public int ResidenceHistoryId { get; set; }
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public DateTime MoveInDate { get; set; }
        public DateTime MoveOutDate { get; set; }
        public string LandlordName { get; set; } = null!;
        public string LandlordPhone { get; set; } = null!;
    }
}