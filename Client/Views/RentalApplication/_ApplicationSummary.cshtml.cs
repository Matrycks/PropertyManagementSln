using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Client.Views.RentalApplication
{
    public class _ApplicationSummary : PageModel
    {
        private readonly ILogger<_ApplicationSummary> _logger;

        public _ApplicationSummary(ILogger<_ApplicationSummary> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}