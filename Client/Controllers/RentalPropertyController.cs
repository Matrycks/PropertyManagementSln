using System.Diagnostics;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Client.Data.DbContexts;
using Client.Models;

namespace Client.Controllers;

/// <summary>
/// Controller for handling rental property-related actions such as listing and managing properties.
/// </summary>
[Authorize(Roles = "Manager")]
public class RentalPropertyController : Controller
{
    private readonly ILogger<RentalPropertyController> _logger;
    private readonly ClientContext _context;
    private readonly IMapper _mapper;

    public RentalPropertyController(ILogger<RentalPropertyController> logger,
    ClientContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RentalProperties()
    {
        // Get the list of rental properties from the database
        // TODO: use a service to get the properties instead of directly accessing the context
        var properties = _context.RentalProperties.ToList();
        var propertyViewModels = _mapper.Map<List<RentalPropertyModel>>(properties);
        return View(propertyViewModels);
    }

    // GET: /FloorPlans
    public IActionResult FloorPlans()
    {
        var floorPlans = new List<FloorPlanModel>
        {
            new FloorPlanModel
            {
                Name = "The Aspen",
                Bedrooms = 2,
                Bathrooms = 2,
                SquareFeet = 1200,
                Rent = 1500.00m
            },
            new FloorPlanModel
            {
                Name = "The Birch",
                Bedrooms = 3,
                Bathrooms = 2,
                SquareFeet = 1500,
                Rent = 1800.00m
            },
            new FloorPlanModel
            {
                Name = "The Cedar",
                Bedrooms = 1,
                Bathrooms = 1,
                SquareFeet = 800,
                Rent = 1200.00m
            }
        };

        return View("FloorPlans", floorPlans);
    }
}
