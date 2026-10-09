using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Client.Views.RentalApplication
{
    public class _ResidenceHistory : PageModel
    {
        private readonly ILogger<_ResidenceHistory> _logger;

        public _ResidenceHistory(ILogger<_ResidenceHistory> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}