using Bogus;
using Client.Controllers;
using Client.Models;
using Client.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Client.Tests;

public class RentalUnitControllerTests
{
    private readonly Mock<IRentalUnitService> _rentalUnitService = new();
    private readonly RentalUnitController _controller;

    public RentalUnitControllerTests()
    {
        _controller = new RentalUnitController(
            NullLogger<RentalUnitController>.Instance,
            _rentalUnitService.Object)
        {
            TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                new TestTempDataProvider())
        };
    }

    [Fact]
    public async Task ListView_ReturnsRentalUnitsViewComponentForProperty()
    {
        const int propertyId = 42;
        var units = new[] { CreateRentalUnit(), CreateRentalUnit() };
        _rentalUnitService
            .Setup(service => service.GetByPropertyIdAsync(propertyId))
            .ReturnsAsync(units);

        var result = await _controller.ListView(propertyId);

        var viewComponent = Assert.IsType<ViewComponentResult>(result);
        Assert.Equal("RentalUnits", viewComponent.ViewComponentName);
        Assert.Same(units, viewComponent.Arguments);
        _rentalUnitService.Verify(service => service.GetByPropertyIdAsync(propertyId), Times.Once);
    }

    [Fact]
    public async Task Create_WhenModelIsValid_CreatesUnitAndRedirectsToProperty()
    {
        var unit = CreateRentalUnit();
        _rentalUnitService
            .Setup(service => service.CreateAsync(unit))
            .Returns(Task.CompletedTask);

        var result = await _controller.Create(unit);

        AssertRedirectedToProperty(result, unit.RentalPropertyId);
        Assert.Equal("Unit created successfully.", _controller.TempData["Success"]);
        _rentalUnitService.Verify(service => service.CreateAsync(unit), Times.Once);
    }

    [Fact]
    public async Task Create_WhenModelIsInvalid_DoesNotCreateUnit()
    {
        var unit = CreateRentalUnit();
        _controller.ModelState.AddModelError(nameof(RentalUnitModel.UnitNumber), "Unit number is required.");

        var result = await _controller.Create(unit);

        AssertRedirectedToProperty(result, unit.RentalPropertyId);
        Assert.Equal("Please provide valid unit details.", _controller.TempData["Error"]);
        _rentalUnitService.Verify(
            service => service.CreateAsync(It.IsAny<RentalUnitModel>()),
            Times.Never);
    }

    [Fact]
    public async Task Create_WhenServiceThrows_SetsErrorMessage()
    {
        var unit = CreateRentalUnit();
        _rentalUnitService
            .Setup(service => service.CreateAsync(unit))
            .Returns(Task.FromException(new InvalidOperationException("Create failed.")));

        var result = await _controller.Create(unit);

        AssertRedirectedToProperty(result, unit.RentalPropertyId);
        Assert.Equal("Error creating rental unit.", _controller.TempData["Error"]);
    }

    [Fact]
    public async Task FormData_WhenUnitExists_ReturnsJson()
    {
        const int unitId = 23;
        var unit = CreateRentalUnit();
        _rentalUnitService
            .Setup(service => service.GetByIdAsync(unitId))
            .ReturnsAsync(unit);

        var result = await _controller.FormData(unitId);

        var json = Assert.IsType<JsonResult>(result);
        Assert.Same(unit, json.Value);
        _rentalUnitService.Verify(service => service.GetByIdAsync(unitId), Times.Once);
    }

    [Fact]
    public async Task FormData_WhenUnitDoesNotExist_ReturnsBadRequest()
    {
        const int unitId = 23;
        _rentalUnitService
            .Setup(service => service.GetByIdAsync(unitId))
            .ReturnsAsync((RentalUnitModel?)null);

        var result = await _controller.FormData(unitId);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Unit doesn't exist.", GetMessage(badRequest.Value));
    }

    [Fact]
    public async Task FormData_WhenServiceThrows_ReturnsBadRequest()
    {
        const int unitId = 23;
        _rentalUnitService
            .Setup(service => service.GetByIdAsync(unitId))
            .Returns(Task.FromException<RentalUnitModel?>(new InvalidOperationException("Lookup failed.")));

        var result = await _controller.FormData(unitId);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error retrieving unit form data.", GetMessage(badRequest.Value));
    }

    [Fact]
    public async Task Edit_WhenModelIsValid_UpdatesUnitAndRedirectsToProperty()
    {
        var unit = CreateRentalUnit();
        _rentalUnitService
            .Setup(service => service.UpdateAsync(unit))
            .Returns(Task.CompletedTask);

        var result = await _controller.Edit(unit);

        AssertRedirectedToProperty(result, unit.RentalPropertyId);
        Assert.Equal("Unit updated successfully.", _controller.TempData["Success"]);
        _rentalUnitService.Verify(service => service.UpdateAsync(unit), Times.Once);
    }

    [Fact]
    public async Task Edit_WhenModelIsInvalid_DoesNotUpdateUnit()
    {
        var unit = CreateRentalUnit();
        _controller.ModelState.AddModelError(nameof(RentalUnitModel.UnitNumber), "Unit number is required.");

        var result = await _controller.Edit(unit);

        AssertRedirectedToProperty(result, unit.RentalPropertyId);
        Assert.Equal("Please provide valid unit details.", _controller.TempData["Error"]);
        _rentalUnitService.Verify(
            service => service.UpdateAsync(It.IsAny<RentalUnitModel>()),
            Times.Never);
    }

    [Fact]
    public async Task Edit_WhenServiceThrows_SetsErrorMessage()
    {
        var unit = CreateRentalUnit();
        _rentalUnitService
            .Setup(service => service.UpdateAsync(unit))
            .Returns(Task.FromException(new InvalidOperationException("Update failed.")));

        var result = await _controller.Edit(unit);

        AssertRedirectedToProperty(result, unit.RentalPropertyId);
        Assert.Equal("Error updating rental unit.", _controller.TempData["Error"]);
    }

    [Fact]
    public async Task Delete_WhenServiceSucceeds_DeletesUnitAndRedirectsToProperty()
    {
        const int propertyId = 42;
        const int unitId = 23;
        _rentalUnitService
            .Setup(service => service.DeleteAsync(unitId))
            .Returns(Task.CompletedTask);

        var result = await _controller.Delete(propertyId, unitId);

        AssertRedirectedToProperty(result, propertyId);
        Assert.Equal("Unit deleted successfully.", _controller.TempData["Success"]);
        _rentalUnitService.Verify(service => service.DeleteAsync(unitId), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenServiceThrows_SetsErrorMessageAndRedirectsToProperty()
    {
        const int propertyId = 42;
        const int unitId = 23;
        _rentalUnitService
            .Setup(service => service.DeleteAsync(unitId))
            .Returns(Task.FromException(new InvalidOperationException("Delete failed.")));

        var result = await _controller.Delete(propertyId, unitId);

        AssertRedirectedToProperty(result, propertyId);
        Assert.Equal("Error deleting rental unit.", _controller.TempData["Error"]);
    }

    private static RentalUnitModel CreateRentalUnit() =>
        new Faker<RentalUnitModel>()
            .RuleFor(unit => unit.RentalPropertyId, faker => faker.Random.Int(1, 1000))
            .RuleFor(unit => unit.FloorPlanId, faker => faker.Random.Int(1, 1000))
            .RuleFor(unit => unit.UnitNumber, faker => faker.Address.BuildingNumber())
            .RuleFor(unit => unit.Rent, faker => faker.Random.Decimal(500, 5000))
            .RuleFor(unit => unit.IsOccupied, faker => faker.Random.Bool())
            .Generate();

    private static string? GetMessage(object? value) =>
        value?.GetType().GetProperty("message")?.GetValue(value) as string;

    private static void AssertRedirectedToProperty(IActionResult result, int propertyId)
    {
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("RentalPropertyDetails", redirect.ActionName);
        Assert.Equal("RentalProperty", redirect.ControllerName);
        Assert.Equal(propertyId, redirect.RouteValues?["propertyId"]);
    }

    private sealed class TestTempDataProvider : ITempDataProvider
    {
        private readonly Dictionary<string, object> _values = [];

        public IDictionary<string, object> LoadTempData(HttpContext context) => _values;

        public void SaveTempData(HttpContext context, IDictionary<string, object> values)
        {
        }
    }
}
