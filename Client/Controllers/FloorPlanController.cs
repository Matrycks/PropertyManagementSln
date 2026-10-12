using System.Diagnostics;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Client.Data.DbContexts;
using Client.Models;

namespace Client.Controllers;

/// <summary>
/// Controller for handling floor plan-related actions and views.
/// </summary>
public class FloorPlanController : Controller
{
    private readonly ILogger<FloorPlanController> _logger;
    private readonly ClientContext _context;
    private readonly IMapper _mapper;

    public FloorPlanController(ILogger<FloorPlanController> logger,
        ClientContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public IActionResult List()
    {
        // Get the list of floor plans from the database
        // TODO: use a service to get the floor plans instead of directly accessing the context
        var floorPlans = _context.FloorPlans.ToList();
        var floorPlanViewModels = _mapper.Map<List<FloorPlanModel>>(floorPlans);

        return View("FloorPlans", floorPlanViewModels);
    }

    public IActionResult ListOptions()
    {
        var floorPlans = _context.FloorPlans.ToList();
        var floorPlanViewModels = _mapper.Map<List<FloorPlanModel>>(floorPlans);

        return Json(floorPlanViewModels);
    }
}
