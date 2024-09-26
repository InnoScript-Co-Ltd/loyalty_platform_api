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
    public static class CountryConverter
    {
        public static CountryDTO ConvertEntityToModel(Country countryEntity)
        {            
            if (countryEntity == null)
            {
                LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(countryEntity)), "Country entity is null");
                throw new ArgumentNullException(nameof(countryEntity), "Source countryEntity cannot be null");
            }

            return new CountryDTO()
            {
                Id = countryEntity.Id,
                Name = countryEntity.Name,
                FlagIcon = countryEntity.FlagIcon,
                MobilePrefixNumber = countryEntity.MobilePrefixNumber,
            };
        }
        
        public static void ConvertModelToEntity(CountryDTO countryDTO, ref Country countryEntity)
        {
            try
            {
                if (countryDTO == null)
                {
                    LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(countryDTO)), "CountryDTO is null");
                    throw new ArgumentNullException(nameof(countryDTO), "Source countryDTO cannot be null");
                }
                
                countryEntity.Id = countryDTO.Id;
                countryEntity.Name = countryDTO.Name;
                countryEntity.FlagIcon = countryDTO.FlagIcon;
                countryEntity.MobilePrefixNumber = countryDTO.MobilePrefixNumber;                
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
