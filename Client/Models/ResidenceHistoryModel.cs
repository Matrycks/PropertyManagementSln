using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ResidenceHistoryModel
    {
        public int ResidenceHistoryId { get; set; }
        public string UserId { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string State { get; set; } = null!;
        [Required]
        public string ZipCode { get; set; } = null!;
        [Required]
        public DateTime MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }
        public string? LandlordName { get; set; }
        public string? LandlordPhone { get; set; }
    }
}