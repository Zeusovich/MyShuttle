using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShuttle.Data;
using MyShuttle.Model;

var builder = WebApplication.CreateBuilder(args);

// Getting connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("MyShuttleConnection");

// Adding DbContext to Services
builder.Services.AddDbContext<MyShuttleContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<ICarrierRepository, CarrierRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IRidesRepository, RidesRepository>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<MyShuttleContext>();

builder.Services.AddControllersWithViews();

// Adds the Endpoint API Explorer service to the project
builder.Services.AddEndpointsApiExplorer();

// Adds Swagger generation service to the project
// It will use API Explorer to generate the OpenAPI schema file
builder.Services.AddSwaggerGen();

var app = builder.Build(); 

// Data Seeding
// Since we're request to retrieve a DbContext class then we have retrieve it in scope since
// it is already registered as scoped class in AddDbContext method.
using (var scope = app.Services.CreateScope())
{
    // Gets the logger for Program class from the services
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Started data seeding");
    try
    {
        // Executes the database initialization code from MyShuttle.Data class library
        MyShuttleDataInitializer.InitializeDatabaseAsync(scope.ServiceProvider, app.Configuration).Wait();
        logger.LogInformation("Completed data seeding");
    }
    catch (Exception ex)
    {
        // If the initialization crashed then we show that in the logs
        logger.LogError(ex, "An error occurred seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Enables swagger generation middleware
    app.UseSwagger();

    // Enables swagger UI to browse all of the APIs in the application.
    app.UseSwaggerUI();
}

// Redirects to HTTPS if it is configured.
app.UseHttpsRedirection();

// Enables to serve static files (js, css, images, pdf, ...) in wwwroot folder
app.UseStaticFiles();

// Enables routing middleware to support routing
app.UseRouting();

// Enables authorization middleware
app.UseAuthorization();

// Configures the default routing for controllers
app.MapControllerRoute(
        name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
