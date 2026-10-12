using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Data.Interfaces;
using Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.Views.Components.RentalProperty
{
    public class RentalUnitDetailsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(RentalUnitModel? rentalUnit)
        {
            if (rentalUnit == null)
            {
                return Content("Rental unit not found.");
            }

            return View("~/Views/Shared/Components/RentalProperty/RentalUnitDetails.cshtml", rentalUnit);
        }
    }
}