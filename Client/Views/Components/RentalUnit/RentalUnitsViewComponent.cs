using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Client.Views.Components.RentalUnit
{
    public class RentalUnitsViewComponent : ViewComponent
    {
        private readonly ILogger<RentalUnitsViewComponent> _logger;
        private readonly IRentalUnitService _rentalUnitService;

        public RentalUnitsViewComponent(ILogger<RentalUnitsViewComponent> logger,
            IRentalUnitService rentalUnitService)
        {
            _logger = logger;
            _rentalUnitService = rentalUnitService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int propertyId)
        {
            try
            {
                var units = await _rentalUnitService.GetByPropertyIdAsync(propertyId);
                return View("~/Views/Shared/Components/RentalUnit/RentalUnits.cshtml", units);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rental units.");

                throw;
            }
        }
    }
}