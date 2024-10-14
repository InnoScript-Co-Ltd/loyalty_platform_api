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
        public static CityDTO ConvertEntityToModel(City townshipEntity)
        {
            if (townshipEntity == null)
            {
                LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(townshipEntity)), "City entity is null");
                throw new ArgumentNullException(nameof(townshipEntity), "Source townshipEntity cannot be null");
            }

            return new CityDTO()
            {
                Id = townshipEntity.Id,
                Name = townshipEntity.Name,
                CountryId = townshipEntity.CountryId,
                StateId= townshipEntity.StateId,
                StateName=townshipEntity.State.Name,
                CountryName = townshipEntity.Country.Name
            };
        }

        public static void ConvertModelToEntity(CityDTO cityDTO, ref City townshipEntity)
        {
            try
            {
                if (cityDTO == null)
                {
                    LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(cityDTO)), "cityDTO is null");
                    throw new ArgumentNullException(nameof(cityDTO), "Source cityDTO cannot be null");
                }

                townshipEntity.Id = cityDTO.Id;
                townshipEntity.Name = cityDTO.Name;
                townshipEntity.CountryId = cityDTO.CountryId;
                townshipEntity.StateId = cityDTO.StateId;
              
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
