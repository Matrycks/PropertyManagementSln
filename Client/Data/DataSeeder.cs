using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Client.Data.DbContexts;
using Client.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Client.Data
{
    public static class DataSeeder
    {
        public static async Task SeedUsers(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            if (userManager.Users.Any())
            {
                return;
            }

            // Create roles if they don't exist
            var roles = new[] { "User", "Manager" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var managerEmail = "patryckthomas@gmail.com";

            var manager =
                await userManager.FindByEmailAsync(managerEmail);

            if (manager == null)
            {
                manager = new ApplicationUser
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    EmailConfirmed = true,
                    FirstName = "Property",
                    LastName = "Manager"
                };

                await userManager.CreateAsync(
                    manager,
                    "Manager123!");
            }

            if (!await userManager.IsInRoleAsync(manager, "Manager"))
            {
                await userManager.AddToRoleAsync(
                    manager,
                    "Manager");
            }
        }

        public static async Task SeedProperties(ClientContext context)
        {
            if (context.RentalProperties.Any())
            {
                return;
            }

            var propertyFaker = new Faker<RentalProperty>()
            .UseSeed(42) // Ensure consistent data generation across runs
            .RuleFor(p => p.Name, f =>
                $"{f.Address.StreetName()} {f.PickRandom("Apartments", "Residences", "Townhomes", "Estates")}")
            .RuleFor(p => p.Address, f => f.Address.StreetAddress())
            .RuleFor(p => p.City, f => f.Address.City())
            .RuleFor(p => p.State, f => f.Address.StateAbbr())
            .RuleFor(p => p.ZipCode, f => f.Address.ZipCode("#####"))
            .RuleFor(p => p.Description, f =>
                f.PickRandom(
                    $"Spacious property featuring {f.Random.Int(2, 5)} bedrooms and modern amenities.",
                    $"Beautiful rental home with a {f.PickRandom("private yard", "large patio", "garage", "modern kitchen")}.",
                    $"Well-maintained property located in a {f.PickRandom("quiet neighborhood", "family-friendly community", "convenient location")}.",
                    null))
            .RuleFor(p => p.IsActive, f => f.Random.Bool(0.85f));

            // Generate 2 property records
            var fakeProperties = propertyFaker.Generate(2);

            context.RentalProperties.AddRange(fakeProperties);
            await context.SaveChangesAsync();
        }

        public static async Task SeedFloorPlans(ClientContext context)
        {
            if (context.FloorPlans.Any())
            {
                return;
            }

            var floorPlanFaker = new Faker<FloorPlan>()
                .UseSeed(42) // Ensure consistent data generation across runs
                .RuleFor(fp => fp.Name, f => f.Company.CompanyName())
                .RuleFor(fp => fp.Bedrooms, f => f.Random.Int(1, 5))
                .RuleFor(fp => fp.Bathrooms, f => f.Random.Int(1, 4))
                .RuleFor(fp => fp.SquareFeet, f => f.Random.Int(800, 3000))
                .RuleFor(fp => fp.Rent, f => f.Random.Decimal(1000m, 3000m));

            // Generate 5 floor plan records
            var fakeFloorPlans = floorPlanFaker.Generate(5);

            context.FloorPlans.AddRange(fakeFloorPlans);
            await context.SaveChangesAsync();
        }

        public static async Task SeedRentalUnits(ClientContext context)
        {
            if (context.RentalUnits.Any())
            {
                return;
            }

            var rentalUnitFaker = new Faker<RentalUnit>()
                .UseSeed(42) // Ensure consistent data generation across runs
                .RuleFor(ru => ru.UnitNumber, f => f.Random.Int(1, 100).ToString())
                .RuleFor(ru => ru.FloorPlanId, f => f.Random.Int(1, 5))
                .RuleFor(ru => ru.RentalPropertyId, f => f.Random.Int(1, 2))
                .RuleFor(ru => ru.IsOccupied, f => f.Random.Bool(0.8f));

            // Generate 10 rental unit records
            var fakeRentalUnits = rentalUnitFaker.Generate(3);

            context.RentalUnits.AddRange(fakeRentalUnits);
            await context.SaveChangesAsync();
        }
    }
}