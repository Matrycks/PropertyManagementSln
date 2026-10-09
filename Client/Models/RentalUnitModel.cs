using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class RentalUnitModel
    {
        public int RentalUnitId { get; set; }
        public int RentalPropertyId { get; set; }

        public int FloorPlanId { get; set; }
        public FloorPlanModel FloorPlan { get; set; } = null!;

        [Required]
        public string UnitNumber { get; set; } = null!;

        [Required]
        public decimal Rent { get; set; }
    }
}