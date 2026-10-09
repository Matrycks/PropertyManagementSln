using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data.Entities
{
    public class Resident
    {
        public int ResidentId { get; set; }
        public int ApplicationUserId { get; set; }
        public int ApplicationId { get; set; }
    }
}