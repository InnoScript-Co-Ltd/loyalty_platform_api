using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class TownshipPagingDTO
    {
        public PagingResult Paging { get; set; }
        public IEnumerable<TownshipDTO> Townships { get; set; }
    }
}
