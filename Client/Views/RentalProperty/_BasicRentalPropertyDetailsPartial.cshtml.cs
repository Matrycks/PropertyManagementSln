using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Client.Views.RentalProperty
{
    public class _BasicRentalPropertyDetailsPartial : PageModel
    {
        private readonly ILogger<_BasicRentalPropertyDetailsPartial> _logger;

        public _BasicRentalPropertyDetailsPartial(ILogger<_BasicRentalPropertyDetailsPartial> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}