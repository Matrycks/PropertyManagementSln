using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data.Entities
{
    public class RentalUnit
    {
        public int RentalUnitId { get; set; }
        public int RentalPropertyId { get; set; }
        public RentalProperty RentalProperty { get; set; } = null!;
        public int FloorPlanId { get; set; }
        public FloorPlan FloorPlan { get; set; } = null!;
        public string UnitNumber { get; set; } = null!;
        public decimal Rent { get; set; }
        public bool IsOccupied { get; set; }
    }
}