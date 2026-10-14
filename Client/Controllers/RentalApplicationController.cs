using System.Diagnostics;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Client.Data.DbContexts;
using Client.Data.Entities;
using Client.Models;
using Client.Services.Interfaces;

namespace Client.Controllers;

/// <summary>
/// Controller for managing rental applications. This controller is responsible for handling requests related to rental applications, 
/// including displaying the application form, processing submissions, and managing application data.
/// </summary>
public class RentalApplicationController : Controller
{
    private readonly ILogger<RentalApplicationController> _logger;
    private readonly IRentalPropertyService _rentalPropertyService;
    private readonly IRentalApplicationService _rentalApplicationService;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public RentalApplicationController(ILogger<RentalApplicationController> logger,
        IRentalApplicationService rentalApplicationService, IRentalPropertyService rentalPropertyService,
        IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _rentalApplicationService = rentalApplicationService;
        _rentalPropertyService = rentalPropertyService;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<IActionResult> List(int? rentalPropertyId = null, ApplicationStatus? status = null)
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                // Redirects to the login page if the user is not authenticated
                return Challenge();
            }

            var isManager = User.IsInRole("Manager");

            var properties = await _rentalPropertyService.GetAllPropertiesAsync();
            IEnumerable<RentalApplicationModel> applications = [];

            if (!isManager)
                applications = await _rentalApplicationService.GetAllUserApplicationsAsync(currentUser.Id,
                    rentalPropertyId, status);
            else
                applications = await _rentalApplicationService.GetAllApplicationsAsync(rentalPropertyId, status);

            return View("RentalApplications", new RentalApplicationListViewModel
            {
                Applications = applications,
                PropertyId = rentalPropertyId,
                Properties = properties,
                Status = status
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retieving rental applications.");
            return View("Error");
        }
    }


    [Authorize(Roles = "User")]
    public async Task<IActionResult> Index(int rentalPropertyId = 1, int rentalUnitId = 1)
    {
        // NOTE: Property and Unit Id's are hardcoded for now due to incorrect UI design
        // TODO: Redo UI workflow
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            // Redirects to the login page if the user is not authenticated
            return Challenge();
        }

        var application = _mapper.Map<RentalApplicationModel>(currentUser); // Map User fields
        application.UserId = currentUser.Id;
        application.RentalPropertyId = rentalPropertyId;
        application.RentalUnitId = rentalUnitId;

        return View(application);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(RentalApplicationModel model, string action)
    {
        try
        {
            switch (action.ToLowerInvariant())
            {
                case "continue":
                    model.CurrentStep++;

                    // validate current section.
                    if (!await ValidateCurrentSectionAsync(model))
                    {
                        return View(model);
                    }

                    ModelState.Clear();
                    return View(model);
                case "back":
                    model.CurrentStep--;

                    ModelState.Clear();
                    return View(model);
                case "submit":
                    // TODO: add Rental Unit selection to application process
                    model.RentalUnitId = 1;

                    // Save the application to the database
                    var newRentalApplicationId = await _rentalApplicationService.CreateApplicationAsync(model);

                    return RedirectToAction("Completed", new { rentalApplicationId = newRentalApplicationId });
                default:
                    return BadRequest();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing application.");
            TempData["Error"] = "Error processing application.";

            return BadRequest();
        }
    }

    [Authorize(Roles = "User")]
    public async Task<IActionResult> Completed(int rentalApplicationId)
    {
        try
        {
            var application = await _rentalApplicationService.GetApplicationByIdAsync(rentalApplicationId);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving rental application.");
            return View("Error");
        }
    }

    private async Task<bool> ValidateCurrentSectionAsync(RentalApplicationModel model)
    {
        return true; // For now

        switch (model.CurrentStep)
        {
            // TODO: handle validation
            case 1:
                break;

            default:
                return false;
        }
    }
}
