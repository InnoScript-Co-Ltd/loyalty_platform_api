using LoyaltyPlatform.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.EntityFramework.Helper
{
    public static class DbLoyaltyPlatformService
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration Configuration, string connectionString)
        {
            services.AddDbContext<DbLoyaltyPlatformContext>(options =>
                options.UseSqlServer($"name=ConnectionStrings:{connectionString}"));

            services.AddDatabaseDeveloperPageExceptionFilter();

        }

        public static void MigrateDatabase(IServiceScope scope)
        {
            var dbContextOptions = scope.ServiceProvider.GetRequiredService<DbLoyaltyPlatformContext>();
            dbContextOptions.Database.Migrate();
            DbLoyaltyPlatformInitializer.Initialize(dbContextOptions);
        }

        public static void DbEnsureCreate(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<DbLoyaltyPlatformContext>();
            context.Database.EnsureCreated();
        }
    }
}
