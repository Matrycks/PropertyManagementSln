using System.Security.Claims;
using Client.Controllers;
using Client.Data.Entities;
using Client.Models;
using Client.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Client.Tests;

public class RentalApplicationControllerTests
{
    private readonly Mock<IRentalApplicationService> _rentalApplicationService = new();
    private readonly Mock<IRentalPropertyService> _rentalPropertyService = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly RentalApplicationController _controller;

    public RentalApplicationControllerTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManager = new Mock<UserManager<ApplicationUser>>(
            store.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);

        _controller = new RentalApplicationController(
            NullLogger<RentalApplicationController>.Instance,
            _rentalApplicationService.Object,
            _rentalPropertyService.Object,
            _mapper.Object,
            _userManager.Object)
        {
            TempData = new TempDataDictionary(new DefaultHttpContext(), new TestTempDataProvider())
        };
    }

    [Fact]
    public async Task Index_WhenUserIsAuthenticated_MapsUserToApplicationAndReturnsView()
    {
        const int rentalPropertyId = 9;
        const int rentalUnitId = 7;
        var user = new ApplicationUser
        {
            Id = "user-123",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com"
        };

        var application = new RentalApplicationModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserId = user.Id,
            RentalUnitId = rentalUnitId
        };

        _userManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);
        _mapper
            .Setup(mapper => mapper.Map<RentalApplicationModel>(user))
            .Returns(application);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = CreatePrincipal(user.Id)
            }
        };

        var result = await _controller.Index(rentalPropertyId, rentalUnitId);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<RentalApplicationModel>(view.Model);
        Assert.Equal(user.Id, model.UserId);
        Assert.Equal(rentalPropertyId, model.RentalPropertyId);
        Assert.Equal(rentalUnitId, model.RentalUnitId);
        Assert.Equal(user.FirstName, model.FirstName);
        Assert.Equal(user.LastName, model.LastName);
        _userManager.Verify(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()), Times.Once);
    }

    [Fact]
    public async Task Index_WhenUserIsNotAuthenticated_ReturnsChallengeResult()
    {
        _userManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((ApplicationUser?)null);

        var result = await _controller.Index();

        Assert.IsType<ChallengeResult>(result);
    }

    [Fact]
    public async Task List_WhenUserIsAuthenticatedAndNotManager_ReturnsUserApplications()
    {
        const int rentalPropertyId = 3;
        const int rentalUnitId = 9;
        var user = new ApplicationUser { Id = "user-123", FirstName = "Jane", LastName = "Doe", Email = "jane@example.com" };
        var properties = new[]
        {
            new RentalPropertyModel { RentalPropertyId = 1, Name = "Alpha" },
            new RentalPropertyModel { RentalPropertyId = 2, Name = "Bravo" }
        };
        var applications = new[]
        {
            new RentalApplicationModel { RentalApplicationId = 1, UserId = user.Id, RentalPropertyId = rentalPropertyId, RentalUnitId = rentalUnitId },
            new RentalApplicationModel { RentalApplicationId = 2, UserId = user.Id, RentalPropertyId = rentalPropertyId, RentalUnitId = rentalUnitId }
        };

        _userManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);
        _rentalPropertyService
            .Setup(service => service.GetAllPropertiesAsync())
            .ReturnsAsync(properties);
        _rentalApplicationService
            .Setup(service => service.GetAllUserApplicationsAsync(user.Id, rentalPropertyId, ApplicationStatus.Submitted))
            .ReturnsAsync(applications);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = CreatePrincipal(user.Id)
            }
        };

        var result = await _controller.List(rentalPropertyId, ApplicationStatus.Submitted);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal("RentalApplications", view.ViewName);
        var model = Assert.IsType<RentalApplicationListViewModel>(view.Model);
        Assert.Equal(rentalPropertyId, model.PropertyId);
        Assert.Equal(ApplicationStatus.Submitted, model.Status);
        Assert.Equal(properties, model.Properties);
        Assert.Equal(applications, model.Applications);
        _rentalApplicationService.Verify(
            service => service.GetAllUserApplicationsAsync(user.Id, rentalPropertyId, ApplicationStatus.Submitted),
            Times.Once);
    }

    [Fact]
    public async Task List_WhenUserIsAuthenticatedAndIsManager_ReturnsAllApplications()
    {
        const int rentalPropertyId = 4;
        var user = new ApplicationUser { Id = "manager-456", FirstName = "Max", LastName = "Manager", Email = "manager@example.com" };
        var properties = new[] { new RentalPropertyModel { RentalPropertyId = 4, Name = "Gamma" } };
        var applications = new[]
        {
            new RentalApplicationModel { RentalApplicationId = 10, UserId = "user-1", RentalPropertyId = rentalPropertyId },
            new RentalApplicationModel { RentalApplicationId = 11, UserId = "user-2", RentalPropertyId = rentalPropertyId }
        };

        _userManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);
        _rentalPropertyService
            .Setup(service => service.GetAllPropertiesAsync())
            .ReturnsAsync(properties);
        _rentalApplicationService
            .Setup(service => service.GetAllApplicationsAsync(rentalPropertyId, ApplicationStatus.Approved))
            .ReturnsAsync(applications);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = CreatePrincipal(user.Id, "Manager")
            }
        };

        var result = await _controller.List(rentalPropertyId, ApplicationStatus.Approved);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal("RentalApplications", view.ViewName);
        var model = Assert.IsType<RentalApplicationListViewModel>(view.Model);
        Assert.Equal(rentalPropertyId, model.PropertyId);
        Assert.Equal(ApplicationStatus.Approved, model.Status);
        Assert.Equal(properties, model.Properties);
        Assert.Equal(applications, model.Applications);
        _rentalApplicationService.Verify(
            service => service.GetAllApplicationsAsync(rentalPropertyId, ApplicationStatus.Approved),
            Times.Once);
    }

    [Fact]
    public async Task List_WhenUserIsNotAuthenticated_ReturnsChallengeResult()
    {
        _userManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((ApplicationUser?)null);

        var result = await _controller.List();

        Assert.IsType<ChallengeResult>(result);
    }

    [Fact]
    public async Task Completed_WhenApplicationExists_ReturnsViewWithApplication()
    {
        const int rentalApplicationId = 12;
        var application = new RentalApplicationModel
        {
            RentalApplicationId = rentalApplicationId,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com"
        };

        _rentalApplicationService
            .Setup(service => service.GetApplicationByIdAsync(rentalApplicationId))
            .ReturnsAsync(application);

        var result = await _controller.Completed(rentalApplicationId);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(application, view.Model);
        _rentalApplicationService.Verify(service => service.GetApplicationByIdAsync(rentalApplicationId), Times.Once);
    }

    [Fact]
    public async Task Completed_WhenApplicationDoesNotExist_ReturnsNotFound()
    {
        const int rentalApplicationId = 12;

        _rentalApplicationService
            .Setup(service => service.GetApplicationByIdAsync(rentalApplicationId))
            .ReturnsAsync((RentalApplicationModel?)null);

        var result = await _controller.Completed(rentalApplicationId);

        Assert.IsType<NotFoundResult>(result);
    }

    private static ClaimsPrincipal CreatePrincipal(string userId, params string[] roles) =>
        new(new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }
            .Concat(roles.Select(role => new Claim(ClaimTypes.Role, role))),
            "TestAuth"));

    private sealed class TestTempDataProvider : ITempDataProvider
    {
        private readonly Dictionary<string, object> _values = [];

        public IDictionary<string, object> LoadTempData(HttpContext context) => _values;

        public void SaveTempData(HttpContext context, IDictionary<string, object> values)
        {
        }
    }
}
