using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LowStockDashboard.Models
{
    public class StockLevelItem
    {
        public string SKU;
        public string ProductTitle;
        public int OnOrder;
        public int Due;
        public int StockLevel;
        public string Location;

        public static int GetData(List<StockLevelItem> stocklevelitems, int PageNumber, int EntriesPerPage, string LocationId, string LocationName, LowStockApp.Classes.AppSession session)
        {
            string query = string.Format(@"declare @PageNumber int ={0};
declare @EntriesPerPage int = {1};
            declare @locationId varchar(64) = '{2}';

            WITH innerReturn AS
            (
                    SELECT
            
                        SKU = si.ItemNumber,
                        ProductTitle = si.ItemTitle,
                        OnOrder = sl.InOrderBook,
                        Due = sl.OnOrder,
                        StockLevel = sl.Quantity,
                        rownum = ROW_NUMBER() OVER(ORDER BY sl.Quantity - sl.InOrderBook ASC)
            
                    FROM StockLevel sl
            
                    INNER JOIN StockItem si ON si.pkStockItemId = sl.fkStockItemId
            
                    WHERE
            
                        sl.fkStockLocationId = @locationId AND
            
                        si.bLogicalDelete = 0 AND
            
                        sl.Quantity - sl.InOrderBook < sl.MinimumLevel
                )

SELECT

    innerReturn.SKU,
	innerReturn.ProductTitle,
	innerReturn.OnOrder,
	innerReturn.Due,
	innerReturn.StockLevel,	
	(SELECT MAX(rownum) FROM innerReturn) AS 'TotalRows'
FROM innerReturn
WHERE(innerReturn.rownum > (@EntriesPerPage * @PageNumber) - @EntriesPerPage AND
        innerReturn.rownum <= (@EntriesPerPage * @PageNumber))", PageNumber, EntriesPerPage, LocationId.Replace("'", "''"));

            LowStockApp.Classes.QueryResult result = session.GetObject<LowStockApp.Classes.QueryResult>("Dashboards", "ExecuteCustomScriptQuery", "script=" + query);
            int totalItems = 0;

            foreach (Dictionary<string, object> rowItem in result.Results)
            {
                stocklevelitems.Add(new StockLevelItem()
                {
                    Due = int.Parse(rowItem["Due"].ToString()),
                    Location = LocationName,
                    OnOrder = int.Parse(rowItem["OnOrder"].ToString()),
                    ProductTitle = rowItem["ProductTitle"].ToString(),
                    SKU = rowItem["SKU"].ToString(),
                    StockLevel = int.Parse(rowItem["StockLevel"].ToString())
                });
                totalItems = int.Parse(rowItem["TotalRows"].ToString());
            }
            return totalItems;
        }

    }
}