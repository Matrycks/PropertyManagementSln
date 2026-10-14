using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class RentalApplicationModel
    {
        public int RentalApplicationId { get; set; }
        public string UserId { get; set; } = null!;
        public int RentalUnitId { get; set; }
        public RentalUnitModel? RentalUnit { get; set; }
        public int RentalPropertyId { get; set; }
        public RentalPropertyModel? RentalProperty { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string DateOfBirth { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string State { get; set; } = null!;
        [Required]
        public string ZipCode { get; set; } = null!;
        public List<ResidenceHistoryModel> ResidenceHistory { get; set; } = [];
        public int CurrentStep { get; set; } = 1;
        public bool ApplicantSaved { get; set; }
        public bool ResidenceHistorySaved { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;
    }
}