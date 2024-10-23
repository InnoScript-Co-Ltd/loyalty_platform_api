using LoyaltyPlatform.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.DataAccess.Implementation
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly DbLoyaltyPlatformContext _dbLoyaltyPlatformContext;

        public MerchantRepository(DbLoyaltyPlatformContext dbLoyaltyPlatformContext)
        {
            _dbLoyaltyPlatformContext = dbLoyaltyPlatformContext;
        }

        //crud
        public MerchantDTO AddMerchant(MerchantDTO merchantDTO)
        {
            try
            {
                Merchant merchantEntity = new Merchant();
                MerchantConverter.ConvertModelToEntity(merchantDTO, ref merchantEntity);
                _dbLoyaltyPlatformContext.Add(merchantEntity);
                _dbLoyaltyPlatformContext.SaveChanges();

                merchantDTO.Id = merchantEntity.Id;

                LoggerHelper.Instance.LogInfo($"Merchant added successfully with Id: {merchantEntity.Id}");

                return merchantDTO;
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while adding merchant.");
                throw;
            }
        }
        public MerchantPagingDTO GetAllMerchantOrg(PageSortParam pageSortParam)
        {
            try
            {
                var query = _dbLoyaltyPlatformContext.Merchants.AsEnumerable();
                int totalCount = query.Count();

                if (!string.IsNullOrEmpty(pageSortParam.SearchTerm))
                {
                    query = query.Where(p => p.UserName.Contains(pageSortParam.SearchTerm) || p.Email.Contains(pageSortParam.SearchTerm));
                    totalCount = query.Count();
                }

                if (!string.IsNullOrEmpty(pageSortParam.SortField))
                {
                    var param = Expression.Parameter(typeof(Merchant), "p");
                    var property = Expression.Property(param, pageSortParam.SortField);
                    var sortExpression = Expression.Lambda(property, param);

                    string sortMethod = pageSortParam.SortDir == SortDirection.ASC ? "OrderBy" : "OrderByDescending";
                    var orderByMethod = typeof(Queryable).GetMethods()
                        .Where(m => m.Name == sortMethod && m.GetParameters().Length == 2)
                        .Single()
                        .MakeGenericMethod(typeof(Merchant), property.Type);

                    query = (IQueryable<Merchant>)orderByMethod.Invoke(null, new object[] { query, sortExpression });
                }

                if (query.Count() > pageSortParam.PageSize)
                {
                    query = query.Skip((pageSortParam.CurrentPage - 1) * pageSortParam.PageSize)
                                 .Take(pageSortParam.PageSize);
                }

                var merchants = query.Select(x => MerchantConverter.ConvertEntityToModel(x)).ToList();

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

                return new MerchantPagingDTO
                {
                    Paging = pagingResult,
                    Merchants = merchants
                };
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while fetching all merchants.");
                throw;
            }
        }
        public MerchantPagingDTO GetAllMerchant(PageSortParam pageSortParam)
        {
            try
            {
                // Start by defining the query using AsQueryable (so that filtering and sorting happen in the database)
                var query = _dbLoyaltyPlatformContext.Merchants.AsQueryable();

                // Apply search filtering based on the SearchTerm, if provided
                if (!string.IsNullOrEmpty(pageSortParam.SearchTerm))
                {
                    query = query.Where(p => p.UserName.Contains(pageSortParam.SearchTerm) || p.Email.Contains(pageSortParam.SearchTerm));
                }

                // Get the total count after filtering
                int totalCount = query.Count();

                // Apply dynamic sorting based on the SortField (string)
                if (!string.IsNullOrEmpty(pageSortParam.SortField))
                {
                    var param = Expression.Parameter(typeof(Merchant), "p");
                    var property = Expression.Property(param, pageSortParam.SortField);  // Get the property dynamically
                    var sortExpression = Expression.Lambda(property, param);

                    // Apply sorting using reflection based on SortDirection
                    string sortMethod = pageSortParam.SortDir == SortDirection.ASC ? "OrderBy" : "OrderByDescending";
                    var orderByMethod = typeof(Queryable).GetMethods()
                        .Where(m => m.Name == sortMethod && m.GetParameters().Length == 2)
                        .Single()
                        .MakeGenericMethod(typeof(Merchant), property.Type);

                    query = (IQueryable<Merchant>)orderByMethod.Invoke(null, new object[] { query, sortExpression });
                }

                // Apply pagination if necessary
                if (query.Count() > pageSortParam.PageSize)
                {
                    query = query.Skip((pageSortParam.CurrentPage - 1) * pageSortParam.PageSize)
                                 .Take(pageSortParam.PageSize);
                }

                // Execute the query (data is fetched here)
                var merchants = query.Select(x => MerchantConverter.ConvertEntityToModel(x)).ToList();

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
                return new MerchantPagingDTO
                {
                    Paging = pagingResult,
                    Merchants = merchants
                };
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, "Error occurred while fetching all merchants.");
                throw;
            }
        }

        public MerchantDTO GetMerchant(int id)
        {
            try
            {
                return MerchantConverter.ConvertEntityToModel(_dbLoyaltyPlatformContext.Merchants.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while fetching merchant with Id: {id}");
                throw;
            }
        }

        public bool UpdateMerchant(MerchantDTO merchantDTO)
        {
            bool result = false;
            try
            {
                Merchant merchantEntity = _dbLoyaltyPlatformContext.Merchants.FirstOrDefault(x => x.Id == merchantDTO.Id);
                if (merchantEntity == null)
                {
                    return result;
                }

                MerchantConverter.ConvertModelToEntity(merchantDTO, ref merchantEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while updating merchant with Id: {merchantDTO.Id}");
                throw;
            }
            return result;
        }

        public bool DeleteMerchant(int id)
        {
            bool result = false;
            try
            {
                var merchantEntity = _dbLoyaltyPlatformContext.Merchants.FirstOrDefault(x => x.Id == id);
                if (merchantEntity == null)
                {
                    return result;
                }
                _dbLoyaltyPlatformContext.Merchants.Remove(merchantEntity);
                _dbLoyaltyPlatformContext.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.LogError(ex, $"Error occurred while deleting merchant with Id: {id}");
                throw;
            }
            return result;
        }

    }
}
