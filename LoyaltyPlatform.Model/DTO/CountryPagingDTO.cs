using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class CountryPagingDTO
    {
        public PagingResult Paging { get; set; }
        public IEnumerable<CountryDTO> Countries { get; set; }
    }
}
