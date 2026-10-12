using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class RentalPropertyModel
    {
        public int RentalPropertyId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string State { get; set; } = null!;
        [Required]
        public string ZipCode { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int TotalUnits { get; set; }
        public int AvailableUnits { get; set; }

        public List<RentalUnitModel> RentalUnits { get; set; } = [];
    }
}