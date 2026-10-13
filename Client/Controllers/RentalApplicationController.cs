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
[Authorize(Roles = "Manager")]
public class RentalApplicationController : Controller
{
    private readonly ILogger<RentalApplicationController> _logger;
    private readonly IRentalApplicationService _rentalApplicationService;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public RentalApplicationController(ILogger<RentalApplicationController> logger,
        IRentalApplicationService rentalApplicationService, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _rentalApplicationService = rentalApplicationService;
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
        applicant.UserId = currentUser.Id;

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
                    // Save the application to the database
                    //await _rentalApplicationService.CreateApplicationAsync(model);

                    return RedirectToAction("Confirmation");
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
