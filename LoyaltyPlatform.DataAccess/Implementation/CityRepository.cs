using LoyaltyPlatform.Converter;
using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.EntityFramework;
using LoyaltyPlatform.EntityFramework.EntityModel;
using LoyaltyPlatform.Logging;
using LoyaltyPlatform.Model.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoyaltyPlatform.DataAccess.Implementation
{
    public class CityRepository : ICityRepository
    {
        public readonly DbLoyaltyPlatformContext _dbLoyaltyPlatformContext;

        public CityRepository(DbLoyaltyPlatformContext dbLoyaltyPlatformContext)
        {

            _dbLoyaltyPlatformContext = dbLoyaltyPlatformContext;
        }

        public CityDTO AddCity(CityDTO cityDTO)
        {
            try
            {
                City cityEntity = new City();
                CityConverter.ConvertModelToEntity(cityDTO, ref cityEntity);
                _dbLoyaltyPlatformContext.Add(cityEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                cityDTO.Id = cityEntity.Id;
                LoggerHelper.Instance.LogInfo($"City added successfully with Id: {cityEntity.Id}");

                return cityDTO;

            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while adding city.");
                throw;
            }
        }

        public IEnumerable<CityDTO> GetAllCity()
        {
            try
            {
                return _dbLoyaltyPlatformContext.Cities.AsEnumerable().Select(x => CityConverter.ConvertEntityToModel(x)).ToList();
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while fetching all cities.");
                throw;
            }
        }

        public bool UpdateCity(CityDTO cityDTO)
        {
            bool result = false;
            try
            {
                City cityEntity = _dbLoyaltyPlatformContext.Cities.FirstOrDefault(x => x.Id == cityDTO.Id);
                if (cityEntity == null)
                {
                    return result;
                }
                CityConverter.ConvertModelToEntity(cityDTO, ref cityEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                result = true;

            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while updating country with Id: {cityDTO.Id}");
                throw;
            }
            return result;

        }

        public bool DeleteCity(int id)
        {
            bool result = false;
            try
            {
                var cityEntity = _dbLoyaltyPlatformContext.Cities.FirstOrDefault(x => x.Id == id);
                if (cityEntity == null)
                {
                    return result;
                }
                _dbLoyaltyPlatformContext.Cities.Remove(cityEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while deleting city with Id: {id}");
                throw;
            }
            return result;
        }

       

        public CityDTO GetCity(int id)
        {
            try
            {
                return CityConverter.ConvertEntityToModel(_dbLoyaltyPlatformContext.Cities.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while fetching country with Id: {id}");
                throw;
            }
        }

       
    }
}
