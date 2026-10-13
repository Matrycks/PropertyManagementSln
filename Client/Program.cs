using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Client.Data.DbContexts;
using Client.Data.Entities;
using Client.Data;
using Client.Data.Interfaces;
using Client.Data.Repositories;
using Client.Services.Interfaces;
using Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Set up the database context for dependency injection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ClientContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ClientContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Mapster configuration
// Get the global configuration instance
var config = TypeAdapterConfig.GlobalSettings;

// Scan the current assembly for any classes implementing IRegister
config.Scan(Assembly.GetExecutingAssembly());

// Register the config and Mapster's IMapper service with DI
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRentalPropertyService, RentalPropertyService>();
builder.Services.AddScoped<IRentalUnitService, RentalUnitService>();
builder.Services.AddScoped<IRentalApplicationService, RentalApplicationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

// Seed the database with initial data
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider
        .GetRequiredService<ClientContext>();

    var logger = scope.ServiceProvider
        .GetRequiredService<ILogger<Program>>();

    try
    {
        await DataSeeder.SeedUsers(
        scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>(),
        scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>());

        await DataSeeder.SeedProperties(context);
        await DataSeeder.SeedFloorPlans(context);
        await DataSeeder.SeedRentalUnits(context);

        logger.LogInformation("Database seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

await app.RunAsync();
