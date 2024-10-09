using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class CityPagingDTO
    {
        public PagingResult PagingResult {  get; set; }
        public IEnumerable<CityDTO> Cities { get; set; }
    }
}
