using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data.Entities
{
    public class RentalApplication
    {
        public int RentalApplicationId { get; set; }
        public string UserId { get; set; } = null!;
        public int RentalPropertyId { get; set; }
        public RentalProperty? RentalProperty { get; set; }
        public int RentalUnitId { get; set; }
        public RentalUnit? RentalUnit { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public List<ResidenceHistory> ResidenceHistory { get; set; } = [];
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;
    }
}