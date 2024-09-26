using LoyaltyPlatform.DataAccess.Helper;
using LoyaltyPlatform.EntityFramework.Helper;
using LoyaltyPlatform.Logging;

namespace LoyaltyPlatform.API.Helpers.ServiceExtensions
{
    public class ConfigHelper
    {
        public static void ConfigureService(WebApplicationBuilder builder)
        {
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            // Configure logging(write to both file and Seq)
            LoggerHelper.Instance.ConfigureLogging("http://localhost:5341", logFilePath);


            //**Register service for EF Core Dependencies
            DbLoyaltyPlatformService.ConfigureServices(builder.Services, builder.Configuration, builder.Configuration.GetSection("ConnectionStrings").GetChildren().First().Key);

            //**Register service for DataAccess Dependencies
            DataAccessService.ConfigureServices(builder.Services, builder.Configuration);
        }


        // Make DB to EnsureCreated, and Initize Default Data
        public static void MigrateDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                //DbMultiClinicDI.DbEnsureCreate(scope);
                DbLoyaltyPlatformService.MigrateDatabase(scope);
            }
        }
    }
}
