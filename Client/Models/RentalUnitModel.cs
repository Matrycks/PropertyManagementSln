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

        [Required]
        public int RentalPropertyId { get; set; }

        public int FloorPlanId { get; set; }
        public FloorPlanModel? FloorPlan { get; set; }

        [Required]
        public string UnitNumber { get; set; } = null!;

        [Required]
        public decimal Rent { get; set; }
        public bool IsOccupied { get; set; }
    }
}