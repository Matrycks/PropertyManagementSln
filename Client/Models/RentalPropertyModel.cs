using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class RentalPropertyModel
    {
        public int RentalPropertyId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }

        public List<RentalUnitModel> RentalUnits { get; set; } = [];
    }
}