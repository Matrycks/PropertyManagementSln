using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class FloorPlanModel
    {
        public int FloorPlanModelId { get; set; }
        public string PictureUrl { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Bedrooms { get; set; }
        public decimal Bathrooms { get; set; }
        public decimal SquareFeet { get; set; }
        public decimal Rent { get; set; }
    }
}