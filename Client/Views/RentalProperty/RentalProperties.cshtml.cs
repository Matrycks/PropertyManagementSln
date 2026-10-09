using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Client.Views.RentalProperty
{
    [Authorize(Roles = "Manager")]
    public class RentalProperties : PageModel
    {
        private readonly ILogger<RentalProperties> _logger;

        public RentalProperties(ILogger<RentalProperties> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}