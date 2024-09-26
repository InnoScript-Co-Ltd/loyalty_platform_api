using LoyaltyPlatform.Converter;
using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.EntityFramework;
using LoyaltyPlatform.EntityFramework.EntityModel;
using LoyaltyPlatform.Logging;
using LoyaltyPlatform.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.DataAccess.Implementation
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DbLoyaltyPlatformContext _dbLoyaltyPlatformContext;

        public CountryRepository(DbLoyaltyPlatformContext dbLoyaltyPlatformContext)
        {
            _dbLoyaltyPlatformContext = dbLoyaltyPlatformContext;
        }

        //crud
        public CountryDTO AddCountry(CountryDTO countryDTO)
        {
            try
            {
                Country countryEntity = new Country();
                CountryConverter.ConvertModelToEntity(countryDTO, ref countryEntity);
                _dbLoyaltyPlatformContext.Add(countryEntity);
                _dbLoyaltyPlatformContext.SaveChanges();

                countryDTO.Id = countryEntity.Id;

                LoggerHelper.Instance.LogInfo($"Country added successfully with Id: {countryEntity.Id}");

                return countryDTO;
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while adding country.");
                throw;

            }
        }
        public IEnumerable<CountryDTO> GetAllCountry()
        {
            try
            {
                return _dbLoyaltyPlatformContext.Countries.AsEnumerable().Select(x => CountryConverter.ConvertEntityToModel(x)).ToList();
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while fetching all countries.");
                throw;
            }
        }

        public CountryDTO GetCountry(int id)
        {
            try
            {
                return CountryConverter.ConvertEntityToModel(_dbLoyaltyPlatformContext.Countries.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while fetching country with Id: {id}");
                throw;
            }
        }

        public bool UpdateCountry(CountryDTO countryDTO)
        {
            bool result = false;
            try
            {
                Country countryEntity = _dbLoyaltyPlatformContext.Countries.FirstOrDefault(x => x.Id == countryDTO.Id);
                if (countryEntity == null)
                {
                    return result;
                }

                CountryConverter.ConvertModelToEntity(countryDTO, ref countryEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while updating country with Id: {countryDTO.Id}");
                throw;
            }
            return result;
        }

        public bool DeleteCountry(int id)
        {
            bool result = false;
            try
            {
                var countryEntity = _dbLoyaltyPlatformContext.Countries.FirstOrDefault(x => x.Id == id);
                if (countryEntity == null)
                {
                    return result;
                }
                _dbLoyaltyPlatformContext.Countries.Remove(countryEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while deleting country with Id: {id}");
                throw;
            }
            return result;
        }

    }
}
