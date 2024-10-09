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
    public class StateRepository : IStateRepository
    {
        public readonly DbLoyaltyPlatformContext _dbLoyaltyPlatformContext;

        public StateRepository(DbLoyaltyPlatformContext dbLoyaltyPlatformContext)
        {

            _dbLoyaltyPlatformContext = dbLoyaltyPlatformContext;
        }
        public StateDTO AddState(StateDTO stateDTO)
        {
            try
            {
                State stateEntity = new State();
                StateConverter.ConvertModelToEntity(stateDTO, ref stateEntity);
                _dbLoyaltyPlatformContext.Add(stateEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                stateDTO.Id = stateEntity.Id;
                LoggerHelper.Instance.LogInfo($"City added successfully with Id: {stateDTO.Id}");

                return stateDTO;

            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while adding city.");
                throw;
            }
        }

        public bool DeleteState(int id)
        {
            bool result = false;
            try
            {
                var stateEntity = _dbLoyaltyPlatformContext.States.FirstOrDefault(x => x.Id == id);
                if (stateEntity == null)
                {
                    return result;
                }
                _dbLoyaltyPlatformContext.States.Remove(stateEntity);
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

        public bool UpdateState(StateDTO stateDTO)
        {
            bool result = false;
            try
            {
                State stateEntity = _dbLoyaltyPlatformContext.States.FirstOrDefault(x => x.Id == stateDTO.Id);
                if (stateEntity == null)
                {
                    return result;
                }
                StateConverter.ConvertModelToEntity(stateDTO, ref stateEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                result = true;

            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while updating state with Id: {stateDTO.Id}");
                throw;
            }
            return result;
        }
        public StateDTO GetState(int id)
        {
            try
            {
                return StateConverter.ConvertEntityToModel(_dbLoyaltyPlatformContext.States.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while fetching state with Id: {id}");
                throw;
            }
        }

        public StatePagingDTO GetAllState(PageSortParam pageSortParam)
        {
            try
            {
                // Base query with includes for related data
                var query = _dbLoyaltyPlatformContext.States
                    .Include(s => s.Country)
                    .AsQueryable();

                // Filtering based on search term
                if (!string.IsNullOrWhiteSpace(pageSortParam.SearchTerm))
                {
                    query = query.Where(s => s.Name.Contains(pageSortParam.SearchTerm));
                }
                var totalCount = query.Count();

                // Sorting using Dynamic LINQ
                if (!string.IsNullOrEmpty(pageSortParam.SortField))
                {
                    var param = Expression.Parameter(typeof(State), "p");
                    var property = Expression.Property(param, pageSortParam.SortField);  // Get the property dynamically
                    var sortExpression = Expression.Lambda(property, param);

                    // Apply sorting using reflection based on SortDirection
                    string sortMethod = pageSortParam.SortDir == SortDirection.ASC ? "OrderBy" : "OrderByDescending";
                    var orderByMethod = typeof(Queryable).GetMethods()
                        .Where(m => m.Name == sortMethod && m.GetParameters().Length == 2)
                        .Single()
                        .MakeGenericMethod(typeof(State), property.Type);

                    query = (IQueryable<State>)orderByMethod.Invoke(null, new object[] { query, sortExpression });
                }

                if (query.Count() > pageSortParam.PageSize)
                {
                    query = query.Skip((pageSortParam.CurrentPage - 1) * pageSortParam.PageSize)
                                 .Take(pageSortParam.PageSize);
                }
                // Applying pagination
                var states = query.Select(s => StateConverter.ConvertEntityToModel(s)).ToList();

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
                return new StatePagingDTO
                {
                    PagingResult = pagingResult,
                    States = states,
                };
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while fetching all states.");
                throw;
            }
        }
    }
}
