using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.EntityFramework;
using LoyaltyPlatform.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.DataAccess.Implementation
{
    public class TownshipRepository : ITownshipRepository
    {
        public readonly DbLoyaltyPlatformContext _dbLoyaltyPlatformContext;
        public TownshipRepository(DbLoyaltyPlatformContext dbLoyaltyPlatformContext)
        {
            _dbLoyaltyPlatformContext = dbLoyaltyPlatformContext;
        }

        public TownshipDTO AddTownship(TownshipDTO townshipDTO)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTownship(int id)
        {
            throw new NotImplementedException();
        }

        public TownshipPagingDTO GetAllTownship(PageSortParam pageSortParam)
        {
            throw new NotImplementedException();
        }

        public TownshipDTO GetTownship(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateTownship(TownshipDTO township)
        {
            throw new NotImplementedException();
        }
    }
}
