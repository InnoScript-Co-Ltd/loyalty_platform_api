using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.DataAccess.Interface
{
    public class IMerchantRepository
    {
        MerchantDTO AddMerchant(MerchantDTO merchantDTO);
        bool DeleteMerchant(int id);
        MerchantPagingDTO GetAllMerchant(PageSortParam pageSortParam);
        MerchantDTO Getmerchant(int id);
        bool UpdateMerchant(MerchantDTO merchantDTO);
    }
}
