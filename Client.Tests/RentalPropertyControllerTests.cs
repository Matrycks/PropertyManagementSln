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

public class RentalPropertyControllerTests
{
    private readonly Mock<IRentalPropertyService> _rentalPropertyService = new();
    private readonly RentalPropertyController _controller;

    public RentalPropertyControllerTests()
    {
        _controller = new RentalPropertyController(
            NullLogger<RentalPropertyController>.Instance,
            _rentalPropertyService.Object,
            null!)
        {
            TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                new TestTempDataProvider())
        };
    }

    [Fact]
    public async Task Create_WhenModelIsValid_CreatesPropertyAndSetsSuccessMessage()
    {
        var property = CreateProperty();
        _rentalPropertyService
            .Setup(service => service.CreatePropertyAsync(property))
            .Returns(Task.CompletedTask);

        var result = await _controller.Create(property);

        AssertRedirectedToRentalProperties(result);
        Assert.Equal($"Property {property.Name} created successfully.", _controller.TempData["Success"]);
        _rentalPropertyService.Verify(service => service.CreatePropertyAsync(property), Times.Once);
    }

    [Fact]
    public async Task Create_WhenModelIsInvalid_DoesNotCreatePropertyAndSetsErrorMessage()
    {
        var property = CreateProperty();
        _controller.ModelState.AddModelError(nameof(RentalPropertyModel.Name), "Name is required.");

        var result = await _controller.Create(property);

        AssertRedirectedToRentalProperties(result);
        Assert.Equal("Please provide valid property details.", _controller.TempData["Error"]);
        _rentalPropertyService.Verify(
            service => service.CreatePropertyAsync(It.IsAny<RentalPropertyModel>()),
            Times.Never);
    }

    [Fact]
    public async Task Create_WhenServiceThrows_SetsErrorMessage()
    {
        var property = CreateProperty();
        _rentalPropertyService
            .Setup(service => service.CreatePropertyAsync(property))
            .Returns(Task.FromException(new InvalidOperationException("Create failed.")));

        var result = await _controller.Create(property);

        AssertRedirectedToRentalProperties(result);
        Assert.Equal("Error creating property.", _controller.TempData["Error"]);
    }

    [Fact]
    public async Task RentalProperties_ReturnsViewWithAllProperties()
    {
        var properties = new[] { CreateProperty(), CreateProperty() };
        _rentalPropertyService
            .Setup(service => service.GetAllPropertiesAsync())
            .ReturnsAsync(properties);

        var result = await _controller.RentalProperties();

        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(properties, view.Model);
        _rentalPropertyService.Verify(service => service.GetAllPropertiesAsync(), Times.Once);
    }

    [Fact]
    public async Task RentalPropertyDetails_WhenPropertyExists_ReturnsViewWithProperty()
    {
        const int propertyId = 17;
        var property = CreateProperty();
        _rentalPropertyService
            .Setup(service => service.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync(property);

        var result = await _controller.RentalPropertyDetails(propertyId);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(property, view.Model);
        _rentalPropertyService.Verify(service => service.GetPropertyByIdAsync(propertyId), Times.Once);
    }

    [Fact]
    public async Task RentalPropertyDetails_WhenPropertyDoesNotExist_ReturnsNotFound()
    {
        const int propertyId = 17;
        _rentalPropertyService
            .Setup(service => service.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync((RentalPropertyModel?)null);

        var result = await _controller.RentalPropertyDetails(propertyId);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task RentalPropertyDetailsForEdit_WhenPropertyExists_ReturnsJson()
    {
        const int propertyId = 17;
        var property = CreateProperty();
        _rentalPropertyService
            .Setup(service => service.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync(property);

        var result = await _controller.RentalPropertyDetailsForEdit(propertyId);

        var json = Assert.IsType<JsonResult>(result);
        Assert.Same(property, json.Value);
        _rentalPropertyService.Verify(service => service.GetPropertyByIdAsync(propertyId), Times.Once);
    }

    [Fact]
    public async Task RentalPropertyDetailsForEdit_WhenPropertyDoesNotExist_ReturnsBadRequest()
    {
        const int propertyId = 17;
        _rentalPropertyService
            .Setup(service => service.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync((RentalPropertyModel?)null);

        var result = await _controller.RentalPropertyDetailsForEdit(propertyId);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(
            "Property doesn't exist.",
            badRequest.Value?.GetType().GetProperty("message")?.GetValue(badRequest.Value));
    }

    [Fact]
    public async Task Edit_WhenModelIsValid_UpdatesPropertyAndSetsSuccessMessage()
    {
        var property = CreateProperty();
        _rentalPropertyService
            .Setup(service => service.UpdatePropertyAsync(property))
            .Returns(Task.CompletedTask);

        var result = await _controller.Edit(property);

        AssertRedirectedToRentalProperties(result);
        Assert.Equal($"Property {property.Name} updated successfully.", _controller.TempData["Success"]);
        _rentalPropertyService.Verify(service => service.UpdatePropertyAsync(property), Times.Once);
    }

    [Fact]
    public async Task Edit_WhenModelIsInvalid_DoesNotUpdatePropertyAndSetsErrorMessage()
    {
        var property = CreateProperty();
        _controller.ModelState.AddModelError(nameof(RentalPropertyModel.Name), "Name is required.");

        var result = await _controller.Edit(property);

        AssertRedirectedToRentalProperties(result);
        Assert.Equal("Please provide valid property details.", _controller.TempData["Error"]);
        _rentalPropertyService.Verify(
            service => service.UpdatePropertyAsync(It.IsAny<RentalPropertyModel>()),
            Times.Never);
    }

    [Fact]
    public async Task Edit_WhenServiceThrows_SetsErrorMessage()
    {
        var property = CreateProperty();
        _rentalPropertyService
            .Setup(service => service.UpdatePropertyAsync(property))
            .Returns(Task.FromException(new InvalidOperationException("Update failed.")));

        var result = await _controller.Edit(property);

        AssertRedirectedToRentalProperties(result);
        Assert.Equal("Error editing property.", _controller.TempData["Error"]);
    }

    [Fact]
    public async Task Delete_WhenServiceSucceeds_SetsSuccessMessage()
    {
        const int propertyId = 17;
        _rentalPropertyService
            .Setup(service => service.DeletePropertyAsync(propertyId))
            .Returns(Task.CompletedTask);

        var result = await _controller.Delete(propertyId);

        AssertRedirectedToRentalProperties(result);
        Assert.Equal("Property deleted successfully.", _controller.TempData["Success"]);
        _rentalPropertyService.Verify(service => service.DeletePropertyAsync(propertyId), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenServiceThrows_SetsErrorMessage()
    {
        const int propertyId = 17;
        _rentalPropertyService
            .Setup(service => service.DeletePropertyAsync(propertyId))
            .Returns(Task.FromException(new InvalidOperationException("Delete failed.")));

        var result = await _controller.Delete(propertyId);

        AssertRedirectedToRentalProperties(result);
        Assert.Equal("Error deleting property.", _controller.TempData["Error"]);
    }

    private static RentalPropertyModel CreateProperty() =>
        new Faker<RentalPropertyModel>()
            .RuleFor(property => property.Name, faker => faker.Company.CompanyName())
            .RuleFor(property => property.Address, faker => faker.Address.StreetAddress())
            .RuleFor(property => property.City, faker => faker.Address.City())
            .RuleFor(property => property.State, faker => faker.Address.StateAbbr())
            .RuleFor(property => property.ZipCode, faker => faker.Address.ZipCode())
            .RuleFor(property => property.Description, faker => faker.Lorem.Sentence())
            .RuleFor(property => property.IsActive, faker => faker.Random.Bool())
            .RuleFor(property => property.TotalUnits, faker => faker.Random.Int(1, 100))
            .RuleFor(property => property.AvailableUnits, (faker, property) =>
                faker.Random.Int(0, property.TotalUnits))
            .Generate();

    private static void AssertRedirectedToRentalProperties(IActionResult result)
    {
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("RentalProperties", redirect.ActionName);
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
