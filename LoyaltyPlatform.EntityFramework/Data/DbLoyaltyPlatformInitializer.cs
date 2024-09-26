using LoyaltyPlatform.EntityFramework.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.EntityFramework.Data
{
    public static class DbLoyaltyPlatformInitializer
    {
        public static void Initialize(DbLoyaltyPlatformContext context)
        {
            if (context.Countries.Any())
            {
                return;
            }

            Country[] countries = new Country[] {
            new Country{Name="Myanmar",FlagIcon="falg/1.png",MobilePrefixNumber="+95" },
            new Country{Name="Singapore",FlagIcon="falg/1.png",MobilePrefixNumber="+65" }
            };
            context.Countries.AddRange(countries);
            context.SaveChanges();
        }
    }
}
