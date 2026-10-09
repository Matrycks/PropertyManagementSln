using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data.Entities
{
    public class RentalProperty
    {
        public int RentalPropertyId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public List<RentalUnit> RentalUnits { get; set; } = [];
    }
}