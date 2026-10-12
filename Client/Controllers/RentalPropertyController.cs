using System.Diagnostics;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Client.Data.DbContexts;
using Client.Models;
using Client.Services.Interfaces;

namespace Client.Controllers;

/// <summary>
/// Controller for handling rental property-related actions such as listing and managing properties.
/// </summary>
[Authorize(Roles = "Manager")]
public class RentalPropertyController : Controller
{
    private readonly ILogger<RentalPropertyController> _logger;
    private readonly IRentalPropertyService _rentalPropertyService;
    private readonly IMapper _mapper;

    public RentalPropertyController(ILogger<RentalPropertyController> logger,
    IRentalPropertyService rentalPropertyService, IMapper mapper)
    {
        _logger = logger;
        _rentalPropertyService = rentalPropertyService;
        _mapper = mapper;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RentalPropertyModel nProperty)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please provide valid property details.";
            return RedirectToAction("RentalProperties");
        }

        try
        {
            await _rentalPropertyService.CreatePropertyAsync(nProperty);
            TempData["Success"] = $"Property {nProperty.Name} created successfully.";

            return RedirectToAction("RentalProperties");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            TempData["Error"] = "Error creating property.";

            return RedirectToAction("RentalProperties");
        }
    }

    // GET: List of all rental properties
    public async Task<IActionResult> RentalProperties()
    {
        var properties = await _rentalPropertyService.GetAllPropertiesAsync();
        return View(properties);
    }

    // GET: Details of a specific rental property
    public async Task<IActionResult> RentalPropertyDetails(int propertyId)
    {
        var property = await _rentalPropertyService.GetPropertyByIdAsync(propertyId);
        if (property == null)
        {
            return NotFound();
        }

        return View(property);
    }

    // GET: Property details for edit form
    [HttpGet]
    public async Task<IActionResult> RentalPropertyDetailsForEdit(int propertyId)
    {
        var property = await _rentalPropertyService.GetPropertyByIdAsync(propertyId);
        if (property == null)
        {
            return NotFound();
        }

        return Json(property);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(RentalPropertyModel property)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please provide valid property details.";
            return RedirectToAction("RentalProperties");
        }

        try
        {
            await _rentalPropertyService.UpdatePropertyAsync(property);
            TempData["Success"] = $"Property {property.Name} updated successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            TempData["Error"] = "Error editing property.";
        }

        return RedirectToAction("RentalProperties");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int propertyId)
    {
        try
        {
            await _rentalPropertyService.DeletePropertyAsync(propertyId);
            TempData["Success"] = $"Property deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            TempData["Error"] = "Error deleting property.";
        }

        return RedirectToAction("RentalProperties");
    }
}
