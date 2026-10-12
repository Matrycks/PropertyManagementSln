using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;
using Client.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Client.Views.Components.RentalProperty
{
    public class RentalPropertyDetailsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(RentalPropertyModel? property)
        {
            if (property == null)
            {
                return Content("Property not found.");
            }

            return View("~/Views/Shared/Components/RentalProperty/RentalPropertyDetails.cshtml", property);
        }
    }
}