using LoyaltyPlatform.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.DataAccess.Interface
{
    public interface ICityRepository
    {   
        CityDTO AddCity(CityDTO city);
        bool UpdateCity(CityDTO city);
        CityPagingDTO GetAllCity(PageSortParam pageSortParam);
        bool DeleteCity(int id);
        CityDTO GetCity(int id);
    }
}
