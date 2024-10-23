using LoyaltyPlatform.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.DataAccess.Interface
{
    public interface IMerchantRepository
    {
        MerchantDTO AddMerchant(MerchantDTO merchantDTO);
        bool DeleteMerchant(int id);
        MerchantPagingDTO GetAllMerchant(PageSortParam pageSortParam);
        MerchantDTO GetMerchant(int id);
        bool UpdateMerchant(MerchantDTO merchantDTO);
    }
}
