using LoyaltyPlatform.EntityFramework.EntityModel;
using LoyaltyPlatform.Logging;
using LoyaltyPlatform.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Converter
{
    public static class MerchantConverter
    {
        public static MerchantDTO ConvertEntityToModel(Merchant merchantEntity)
        {
            if (merchantEntity == null)
            {
                LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(merchantEntity)), "Merchant entity is null");
                throw new ArgumentNullException(nameof(merchantEntity), "Source merchantEntity cannot be null");
            }

            return new MerchantDTO()
            {
                Id = merchantEntity.Id,
                UserName = merchantEntity.UserName,
                Profile = merchantEntity.Profile,
                FullName = merchantEntity.FullName,
                Email = merchantEntity.Email,
                PrefixPhoneNumber = merchantEntity.PrefixPhoneNumber,
                PhoneNumber = merchantEntity.PhoneNumber,
                EmailVerifiedDate = merchantEntity.EmailVerifiedDate,
                PhoneVerifiedDate = merchantEntity.PhoneVerifiedDate
            };
        }

        public static void ConvertModelToEntity(MerchantDTO merchantDTO, ref Merchant merchantEntity)
        {
            try
            {
                if (merchantDTO == null)
                {
                    LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(merchantDTO)), "MerchantDTO is null");
                    throw new ArgumentNullException(nameof(merchantDTO), "Source merchantDTO cannot be null");
                }

                merchantEntity.Id = merchantDTO.Id;
                merchantEntity.UserName = merchantDTO.UserName;
                merchantEntity.Profile = merchantDTO.Profile;
                merchantEntity.FullName = merchantDTO.FullName;
                merchantEntity.Email = merchantDTO.Email;
                merchantEntity.PrefixPhoneNumber = merchantDTO.PrefixPhoneNumber;
                merchantEntity.PhoneNumber = merchantDTO.PhoneNumber;
                merchantEntity.EmailVerifiedDate = merchantDTO.EmailVerifiedDate;
                merchantEntity.PhoneVerifiedDate = merchantDTO.PhoneVerifiedDate;
            }
            catch (ArgumentException ex)
            {
                LoggerHelper.Instance.LogError(ex, "Argument exception during model-to-entity conversion");
                throw;
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Unexpected error during model-to-entity conversion");
                throw;
            }
        }
    }
}
