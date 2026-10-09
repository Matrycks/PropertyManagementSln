using System.Diagnostics;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Client.Data.DbContexts;
using Client.Data.Entities;
using Client.Models;

namespace Client.Controllers;

/// <summary>
/// Controller for managing rental applications. This controller is responsible for handling requests related to rental applications, 
/// including displaying the application form, processing submissions, and managing application data.
/// </summary>
[Authorize(Roles = "Manager")]
public class RentalApplicationController : Controller
{
    private readonly ILogger<RentalApplicationController> _logger;
    private readonly ClientContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public RentalApplicationController(ILogger<RentalApplicationController> logger,
    ClientContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int rentalUnitId)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            // Redirects to the login page if the user is not authenticated
            return Challenge();
        }

        var applicant = _mapper.Map<ApplicantModel>(currentUser);

        return View(new RentalApplicationModel
        {
            RentalUnitId = rentalUnitId,
            Applicant = applicant
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(RentalApplicationModel model, string action)
    {
        switch (action.ToLowerInvariant())
        {
            case "continue":
                model.CurrentStep++;

                ModelState.Clear();
                return View(model);
            case "back":
                model.CurrentStep--;

                ModelState.Clear();
                return View(model);
            case "submit":
                // Save the application to the database
                var rentalApplicationEntity = _mapper.Map<RentalApplication>(model);
                _context.RentalApplications.Add(rentalApplicationEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Confirmation");
            default:
                return BadRequest();
        }
    }
}
