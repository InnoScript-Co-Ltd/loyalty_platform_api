using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class MerchantPagingDTO
    {
        public PagingResult Paging { get; set; }
        public IEnumerable<MerchantDTO> Merchants { get; set; }
    }
}
