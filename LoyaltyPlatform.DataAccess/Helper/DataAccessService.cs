using LoyaltyPlatform.DataAccess.Implementation;
using LoyaltyPlatform.DataAccess.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.DataAccess.Helper
{
    public class DataAccessService
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<ICountryRepository, CountryRepository>();
        }
    }
}
