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
    public static class StateConverter
    {
        public static StateDTO ConvertEntityToModel(State stateEntity)
        {
            if (stateEntity == null)
            {
                LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(stateEntity)), "State entity is null");
                throw new ArgumentNullException(nameof(stateEntity), "Source stateEntity cannot be null");
            }

            return new StateDTO()
            {
                Id = stateEntity.Id,
                Name = stateEntity.Name,
                ZipCode = stateEntity.ZipCode,
                Profile = stateEntity.Profile,
                CountryId = stateEntity.CountryId,
                CountryName=stateEntity.Country.Name
            };
        }

        public static void ConvertModelToEntity(StateDTO stateDTO, ref State stateEntity)
        {
            try
            {
                if (stateDTO == null)
                {
                    LoggerHelper.Instance.LogError(new ArgumentNullException(nameof(stateDTO)), "stateDTO is null");
                    throw new ArgumentNullException(nameof(stateDTO), "Source stateDTO cannot be null");
                }

                stateEntity.Id = stateDTO.Id;
                stateEntity.Name = stateDTO.Name;
                stateEntity.CountryId = stateDTO.CountryId;
                stateEntity.ZipCode = stateDTO.ZipCode; 
                stateEntity.Profile = stateDTO.Profile;

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
