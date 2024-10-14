using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class StatePagingDTO
    {
        public PagingResult Paging { get; set; }
        public IEnumerable<StateDTO> States { get; set; }
    }
}
