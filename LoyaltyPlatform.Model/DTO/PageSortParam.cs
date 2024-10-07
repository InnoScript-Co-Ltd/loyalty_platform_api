﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class PageSortParam
    {
        public int PageSize { get; set; } = 10;  //default page size
        public int CurrentPage { get; set; } = 1;        
        public string? SortField { get; set; } = "id";
        public SortDirection SortDir { get; set; } = SortDirection.ASC;
        public string? SearchTerm { get; set; }
    }



    public enum SortDirection
    {
        ASC = 0,   //default as ascending
        DESC
    }

    public class PagingResult
    {
        public int TotalCount { get; set; } = 0;
        public int TotalPages { get; set; } = 1;
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }
        public int FirstRowOnPage { get; set; }
        public int LastRowOnPage { get; set; }
    }
}
