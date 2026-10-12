using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Client.Views.Components.RentalProperty
{
    public class CreateRentalPropertyViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Views/Shared/Components/RentalProperty/CreateRentalProperty.cshtml");
        }
    }
}