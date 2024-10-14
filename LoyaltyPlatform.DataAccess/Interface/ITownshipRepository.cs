using LoyaltyPlatform.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.DataAccess.Interface
{
    public interface ITownshipRepository
    {
        TownshipDTO AddTownship(TownshipDTO townshipDTO);
        bool UpdateTownship(TownshipDTO township);
        TownshipPagingDTO GetAllTownship(PageSortParam pageSortParam);
        bool DeleteTownship(int id);
        TownshipDTO GetTownship(int id);
    }
}
