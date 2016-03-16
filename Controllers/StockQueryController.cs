using LowStockDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LowStockDashboard.Controllers
{
    public class StockQueryController : ApiController
    {
        [HttpPost()]
        [ActionName("GetLowStockLevel")]
        public GetLowStockLevelResponse GetLowStockLevel(GetLowStockLevelRequest request)
        {
            List<StockLevelItem> stocklevel = new List<StockLevelItem>();
            object session = HttpContext.Current.Items["session"];
            int totalItems = StockLevelItem.GetData(stocklevel, request.PageNumber, request.EntriesPerPage, request.LocationId, request.LocationName, (LowStockApp.Classes.AppSession)session);

            return new GetLowStockLevelResponse()
            {
                TotalItems = totalItems,
                rows = stocklevel
            };
        }
        
        public class GetLowStockLevelRequest
        {
            public string LocationId;
            public string LocationName;
            public int PageNumber;
            public int EntriesPerPage;
        }

        public class GetLowStockLevelResponse
        {
            public int TotalItems;
            public List<Models.StockLevelItem> rows = new List<Models.StockLevelItem>();
        }

    }
}
