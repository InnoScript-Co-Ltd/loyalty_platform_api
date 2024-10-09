using LoyaltyPlatform.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.DataAccess.Interface
{
    public interface IStateRepository
    {
        StateDTO AddState(StateDTO stateDTO);
        bool UpdateState(StateDTO stateDTO);
        bool DeleteState(int id);
        StateDTO GetState(int id);

        StatePagingDTO GetAllState(PageSortParam pageSortParam);

    }
}
