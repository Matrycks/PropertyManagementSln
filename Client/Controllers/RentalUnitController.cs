using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Client.Data.Entities;
using Client.Data.Interfaces;
using Client.Models;
using Client.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace Client.Controllers
{
    /// <summary>
    /// Controller for handling rental unit-related actions.
    /// </summary>
    [Authorize(Roles = "Manager")]
    public class RentalUnitController : Controller
    {
        private readonly ILogger<RentalUnitController> _logger;
        private readonly IRentalUnitService _rentalUnitService;

        public RentalUnitController(ILogger<RentalUnitController> logger,
            IRentalUnitService rentalUnitService)
        {
            _logger = logger;
            _rentalUnitService = rentalUnitService;
        }

        [AllowAnonymous]
        [Authorize(Roles = "User, Manager")]
        public async Task<IActionResult> List(int floorPlanId, int propertyId = 1) // NOTE: hardcoded until ui workflow is updated
        {
            var units = await _rentalUnitService.GetByPropertyIdAsync(propertyId, floorPlanId);
            return View("RentalUnits", units);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalUnitModel rentalUnit)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please provide valid unit details.";
                return RedirectToAction("RentalPropertyDetails", "RentalProperty", new { propertyId = rentalUnit.RentalPropertyId });
            }

            try
            {
                await _rentalUnitService.CreateAsync(rentalUnit);
                TempData["Success"] = "Unit created successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating rental unit.");

                TempData["Error"] = "Error creating rental unit.";
            }

            return RedirectToAction("RentalPropertyDetails", "RentalProperty", new { propertyId = rentalUnit.RentalPropertyId });
        }

        public async Task<IActionResult> FormData(int unitId)
        {
            try
            {
                var unit = await _rentalUnitService.GetByIdAsync(unitId);

                if (unit == null)
                {
                    return BadRequest(new
                    {
                        message = "Unit doesn't exist."
                    });
                }

                return Json(unit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unit form data.");

                return BadRequest(new
                {
                    message = "Error retrieving unit form data."
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RentalUnitModel rentalUnit)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please provide valid unit details.";
                return RedirectToAction("RentalPropertyDetails", "RentalProperty", new { propertyId = rentalUnit.RentalPropertyId });
            }

            try
            {
                await _rentalUnitService.UpdateAsync(rentalUnit);
                TempData["Success"] = "Unit updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating rental unit.");

                TempData["Error"] = "Error updating rental unit.";
            }

            return RedirectToAction("RentalPropertyDetails", "RentalProperty", new { propertyId = rentalUnit.RentalPropertyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int rentalPropertyId, int rentalUnitId)
        {
            try
            {
                await _rentalUnitService.DeleteAsync(rentalUnitId);
                TempData["Success"] = "Unit deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting rental unit.");

                TempData["Error"] = "Error deleting rental unit.";
            }

            return RedirectToAction("RentalPropertyDetails", "RentalProperty", new { propertyId = rentalPropertyId });
        }
    }
}