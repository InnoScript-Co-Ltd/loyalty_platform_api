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
using System.Linq.Expressions;
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
        //public IEnumerable<CountryDTO> GetAllCountry()
        //{
        //    try
        //    {
        //        return _dbLoyaltyPlatformContext.Countries.AsEnumerable().Select(x => CountryConverter.ConvertEntityToModel(x)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.Instance.LogError(ex, "Error occurred while fetching all countries.");
        //        throw;
        //    }
        //}
        public CountryPagingDTO GetAllCountryOrg(PageSortParam pageSortParam)
        {
            try
            {
                var query = _dbLoyaltyPlatformContext.Countries.AsEnumerable();
                
                int totalCount = query.Count();
                
                if (!string.IsNullOrEmpty(pageSortParam.SearchTerm))
                {
                    query = query.Where(p => p.Name.Contains(pageSortParam.SearchTerm) || p.MobilePrefixNumber.Contains(pageSortParam.SearchTerm));
                    totalCount = query.Count();
                }
                
                if (!string.IsNullOrEmpty(pageSortParam.SortField))
                {
                    var param = Expression.Parameter(typeof(Country), "p");
                    var property = Expression.Property(param, pageSortParam.SortField);
                    var sortExpression = Expression.Lambda(property, param);

                    // Apply sorting using reflection based on SortDirection
                    string sortMethod = pageSortParam.SortDir == SortDirection.ASC ? "OrderBy" : "OrderByDescending";
                    var orderByMethod = typeof(Queryable).GetMethods()
                        .Where(m => m.Name == sortMethod && m.GetParameters().Length == 2)
                        .Single()
                        .MakeGenericMethod(typeof(Country), property.Type);

                    query = (IQueryable<Country>)orderByMethod.Invoke(null, new object[] { query, sortExpression });
                }

                //// Get the total count of records (but don't fetch the data yet)
                //int totalCount = query.Count();

                if (query.Count() > pageSortParam.PageSize)
                {
                    // Apply pagination (Skip and Take to limit the number of records)
                    query = query.Skip((pageSortParam.CurrentPage - 1) * pageSortParam.PageSize)
                                 .Take(pageSortParam.PageSize);
                }

                // Execute the query (data is fetched here)
                var countries = query.Select(x => CountryConverter.ConvertEntityToModel(x)).ToList();

                // Calculate pagination details
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSortParam.PageSize);
                var pagingResult = new PagingResult
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    PreviousPage = pageSortParam.CurrentPage > 1 ? pageSortParam.CurrentPage - 1 : (int?)null,
                    NextPage = pageSortParam.CurrentPage < totalPages ? pageSortParam.CurrentPage + 1 : (int?)null,
                    FirstRowOnPage = ((pageSortParam.CurrentPage - 1) * pageSortParam.PageSize) + 1,
                    LastRowOnPage = Math.Min(totalCount, pageSortParam.CurrentPage * pageSortParam.PageSize)
                };
                
                return new CountryPagingDTO
                {
                    Paging = pagingResult,
                    Countries = countries
                };
                //return _dbLoyaltyPlatformContext.Countries.AsEnumerable().Select(x => CountryConverter.ConvertEntityToModel(x)).ToList();
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while fetching all countries.");
                throw;
            }
        }

        public CountryPagingDTO GetAllCountry(PageSortParam pageSortParam)
        {
            try
            {
                // Start by defining the query using AsQueryable (so that filtering and sorting happen in the database)
                var query = _dbLoyaltyPlatformContext.Countries.AsQueryable();

                // Apply search filtering based on the SearchTerm, if provided
                if (!string.IsNullOrEmpty(pageSortParam.SearchTerm))
                {
                    query = query.Where(p => p.Name.Contains(pageSortParam.SearchTerm) || p.MobilePrefixNumber.Contains(pageSortParam.SearchTerm));
                }

                // Get the total count after filtering
                int totalCount = query.Count();

                // Apply dynamic sorting based on the SortField (string)
                if (!string.IsNullOrEmpty(pageSortParam.SortField))
                {
                    var param = Expression.Parameter(typeof(Country), "p");
                    var property = Expression.Property(param, pageSortParam.SortField);  // Get the property dynamically
                    var sortExpression = Expression.Lambda(property, param);

                    // Apply sorting using reflection based on SortDirection
                    string sortMethod = pageSortParam.SortDir == SortDirection.ASC ? "OrderBy" : "OrderByDescending";
                    var orderByMethod = typeof(Queryable).GetMethods()
                        .Where(m => m.Name == sortMethod && m.GetParameters().Length == 2)
                        .Single()
                        .MakeGenericMethod(typeof(Country), property.Type);

                    query = (IQueryable<Country>)orderByMethod.Invoke(null, new object[] { query, sortExpression });
                }

                // Apply pagination if necessary
                if (query.Count() > pageSortParam.PageSize)
                {
                    query = query.Skip((pageSortParam.CurrentPage - 1) * pageSortParam.PageSize)
                                 .Take(pageSortParam.PageSize);
                }

                // Execute the query (data is fetched here)
                var countries = query.Select(x => CountryConverter.ConvertEntityToModel(x)).ToList();

                // Calculate pagination details
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSortParam.PageSize);
                var pagingResult = new PagingResult
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    PreviousPage = pageSortParam.CurrentPage > 1 ? pageSortParam.CurrentPage - 1 : (int?)null,
                    NextPage = pageSortParam.CurrentPage < totalPages ? pageSortParam.CurrentPage + 1 : (int?)null,
                    FirstRowOnPage = ((pageSortParam.CurrentPage - 1) * pageSortParam.PageSize) + 1,
                    LastRowOnPage = Math.Min(totalCount, pageSortParam.CurrentPage * pageSortParam.PageSize)
                };

                // Return the paginated result
                return new CountryPagingDTO
                {
                    Paging = pagingResult,
                    Countries = countries
                };
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
