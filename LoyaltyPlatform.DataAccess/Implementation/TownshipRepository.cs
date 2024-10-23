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
    public class TownshipRepository : ITownshipRepository
    {
        public readonly DbLoyaltyPlatformContext _dbLoyaltyPlatformContext;
        private IEnumerable<TownshipDTO> townships;

        public TownshipRepository(DbLoyaltyPlatformContext dbLoyaltyPlatformContext)
        {
            _dbLoyaltyPlatformContext = dbLoyaltyPlatformContext;
        }

        public TownshipDTO AddTownship(TownshipDTO townshipDTO)
        {
            try
            {
                Township townshipEntity = new Township();
                TownshipConverter.ConvertModelToEntity(townshipDTO, ref townshipEntity);
                var city = _dbLoyaltyPlatformContext.Cities.FirstOrDefault(c => c.Id == townshipDTO.CityId);
                var state = _dbLoyaltyPlatformContext.States.FirstOrDefault(s => s.Id == townshipDTO.StateId);
                var country = _dbLoyaltyPlatformContext.Countries.FirstOrDefault(c => c.Id == townshipDTO.CountryId);

                _dbLoyaltyPlatformContext.Add(townshipEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                townshipDTO.Id = townshipEntity.Id;
                townshipDTO.CityName = city.Name;
                townshipDTO.StateName = state.Name;
                townshipDTO.CountryName = country.Name;
                LoggerHelper.Instance.LogInfo($"City added successfully with Id: {townshipEntity.Id}");

                return townshipDTO;

            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while adding Township.");
                throw;
            }
        }

        public bool DeleteTownship(int id)
        {
            bool result = false;
            try
            {
                var townshipEntity = _dbLoyaltyPlatformContext.Township.FirstOrDefault(x => x.Id == id);
                if (townshipEntity == null)
                {
                    return result;
                }
                _dbLoyaltyPlatformContext.Township.Remove(townshipEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while deleting township with Id: {id}");
                throw;
            }
            return result;
        }

        public TownshipPagingDTO GetAllTownship(PageSortParam pageSortParam)
        {
            try
            {
                // Base query with includes for related data
                var query = _dbLoyaltyPlatformContext.Township
                    .Include(c => c.City)
                    .ThenInclude(s => s.State)
                    .ThenInclude(co => co.Country)
                    .AsQueryable();

                // Filtering based on search term
                if (!string.IsNullOrWhiteSpace(pageSortParam.SearchTerm))
                {
                    query = query.Where(c => c.Name.Contains(pageSortParam.SearchTerm));
                }
                var totalCount = query.Count();

                // Sorting using Dynamic LINQ
                if (!string.IsNullOrEmpty(pageSortParam.SortField))
                {
                    var param = Expression.Parameter(typeof(Township), "p");
                    var property = Expression.Property(param, pageSortParam.SortField);  // Get the property dynamically
                    var sortExpression = Expression.Lambda(property, param);

                    // Apply sorting using reflection based on SortDirection
                    string sortMethod = pageSortParam.SortDir == SortDirection.ASC ? "OrderBy" : "OrderByDescending";
                    var orderByMethod = typeof(Queryable).GetMethods()
                        .Where(m => m.Name == sortMethod && m.GetParameters().Length == 2)
                        .Single()
                        .MakeGenericMethod(typeof(Township), property.Type);

                    query = (IQueryable<Township>)orderByMethod.Invoke(null, new object[] { query, sortExpression });
                }

                if (query.Count() > pageSortParam.PageSize)
                {
                    query = query.Skip((pageSortParam.CurrentPage - 1) * pageSortParam.PageSize)
                                 .Take(pageSortParam.PageSize);
                }
                // Applying pagination
                var townships = query.Select(c => TownshipConverter.ConvertEntityToModel(c)).ToList();


                // Create the paging result
                var pagingResult = new PagingResult
                {
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSortParam.PageSize),
                    PreviousPage = pageSortParam.CurrentPage > 1 ? (int?)pageSortParam.CurrentPage - 1 : null,
                    NextPage = pageSortParam.CurrentPage < (int)Math.Ceiling(totalCount / (double)pageSortParam.PageSize) ? (int?)pageSortParam.CurrentPage + 1 : null,
                    FirstRowOnPage = (pageSortParam.CurrentPage - 1) * pageSortParam.PageSize + 1,
                    LastRowOnPage = Math.Min(pageSortParam.CurrentPage * pageSortParam.PageSize, totalCount)
                };

                // Return the paginated result with cities
                return new TownshipPagingDTO
                {
                    Paging = pagingResult,
                    Townships = townships,
                };
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while fetching all townships.");
                throw;
            }
        }

        public TownshipDTO GetTownship(int id)
        {
            try
            {
                return TownshipConverter.ConvertEntityToModel(_dbLoyaltyPlatformContext.Township.Include(c=>c.City).ThenInclude(s => s.State).ThenInclude(co=>co.Country).FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while fetching country with Id: {id}");
                throw;
            }
        }

        public bool UpdateTownship(TownshipDTO townshipDTO)
        {
            bool result = false;
            try
            {
                Township townshipEntity = _dbLoyaltyPlatformContext.Township.FirstOrDefault(x => x.Id == townshipDTO.Id);
                if (townshipEntity == null)
                {
                    return result;
                }
                TownshipConverter.ConvertModelToEntity(townshipDTO, ref townshipEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                result = true;

            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while updating township with Id: {townshipDTO.Id}");
                throw;
            }
            return result;
        }
    }
}
