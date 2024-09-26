using LoyaltyPlatform.API.Helpers.ServiceExtensions;
using LoyaltyPlatform.DataAccess.Helper;
using LoyaltyPlatform.EntityFramework.Helper;
using LoyaltyPlatform.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


ConfigHelper.ConfigureService(builder);

LoggerHelper.Instance.LogInfo("Starting the API web host...");

try
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    ConfigHelper.MigrateDatabase(app);

    app.Run();
}
catch (Exception ex)
{
    // Log any fatal errors with exception details
    LoggerHelper.Instance.LogError(ex, "API Host terminated unexpectedly.");
}
finally
{
    // Log when the application shuts down
    LoggerHelper.Instance.LogInfo("Shutting down the API web host...");
    Log.CloseAndFlush();
}
