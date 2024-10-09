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
    public static class CityConverter
    {
        public static CityDTO ConvertEntityToModel(City cityEntity)
        {
            if (cityEntity == null)
            {
                LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(cityEntity)), "City entity is null");
                throw new ArgumentNullException(nameof(cityEntity), "Source cityEntity cannot be null");
            }

            return new CityDTO()
            {
                Id = cityEntity.Id,
                Name = cityEntity.Name,
                CountryId = cityEntity.CountryId,
                StateId= cityEntity.StateId,
                StateName=cityEntity.State.Name,
                CountryName = cityEntity.Country.Name
            };
        }

        public static void ConvertModelToEntity(CityDTO cityDTO, ref City cityEntity)
        {
            try
            {
                if (cityDTO == null)
                {
                    LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(cityDTO)), "cityDTO is null");
                    throw new ArgumentNullException(nameof(cityDTO), "Source cityDTO cannot be null");
                }

                cityEntity.Id = cityDTO.Id;
                cityEntity.Name = cityDTO.Name;
                cityEntity.CountryId = cityDTO.CountryId;
                cityEntity.StateId = cityDTO.StateId;
              
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
