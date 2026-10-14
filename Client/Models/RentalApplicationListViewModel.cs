using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class RentalApplicationListViewModel
    {
        public IEnumerable<RentalApplicationModel> Applications { get; set; } = [];

        public IEnumerable<RentalPropertyModel> Properties { get; set; } = [];

        public int? PropertyId { get; set; }

        public ApplicationStatus? Status { get; set; }
    }
}