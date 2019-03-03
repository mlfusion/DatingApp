

using System;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.Common.Paging
{
    public class Params
    {
        private int MaxPageSize = 50;// Convert.ToInt32(_configuration.GetSection("Paging:MaxPageSize").Value);
        public int PageNumber { get; set; } = 1;
        private int pageSize = 5; // Convert.ToInt32(_configuration.GetSection("Paging:PageSize").Value);
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value;}
        }
        public string Search{get;set;}
        public string SortOrder {get;set;}
        public string SortColumn { get; set; }
    }
}