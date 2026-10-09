using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data.Entities
{
    public class FloorPlan
    {
        public int FloorPlanId { get; set; }
        public string Name { get; set; } = null!;
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int SquareFeet { get; set; }
        public decimal Rent { get; set; }
    }
}