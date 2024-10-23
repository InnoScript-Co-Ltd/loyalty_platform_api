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
    public static class TownshipConverter
    {
        public static TownshipDTO ConvertEntityToModel(Township townshipEntity)
        {
            if (townshipEntity == null)
            {
                LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(townshipEntity)), "Township entity is null");
                throw new ArgumentNullException(nameof(townshipEntity), "Source townshipEntity cannot be null");
            }

            return new TownshipDTO()
            {
                Id = townshipEntity.Id,
                Name = townshipEntity.Name,
                CityId = townshipEntity.CityId,
                CountryId = townshipEntity.CountryId,
                StateId = townshipEntity.StateId,
                CityName = townshipEntity.City.Name,
                StateName = townshipEntity.State.Name,
                CountryName = townshipEntity.Country.Name
            };
        }

        public static void ConvertModelToEntity(TownshipDTO townshipDTO, ref Township townshipEntity)
        {
            try
            {
                if (townshipDTO == null)
                {
                    LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(townshipDTO)), "townshipDTO is null");
                    throw new ArgumentNullException(nameof(townshipDTO), "Source townshipDTO cannot be null");
                }

                townshipEntity.Id = townshipDTO.Id;
                townshipEntity.Name = townshipDTO.Name;
                townshipEntity.CountryId = townshipDTO.CountryId;
                townshipEntity.StateId = townshipDTO.StateId;
                townshipEntity.CityId = townshipDTO.CityId;

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
