using LoyaltyPlatform.Model.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.DataAccess.Interface
{
    public interface ICountryRepository
    {
        CountryDTO AddCountry(CountryDTO countryDTO);
        bool DeleteCountry(int id);
        CountryPagingDTO GetAllCountry(PageSortParam pageSortParam);
        CountryDTO GetCountry(int id);
        bool UpdateCountry(CountryDTO countryDTO);
    }
}
