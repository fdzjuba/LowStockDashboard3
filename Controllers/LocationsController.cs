using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LowStockDashboard.Controllers
{
    public class LocationsController : ApiController
    {
        [HttpGet()]
        [ActionName("GetLocations")]
        public IEnumerable<LinnworksAPI.StockLocation> GetLocations()
        {
            object session = HttpContext.Current.Items["session"];

            return LinnworksAPI.InventoryMethods.GetStockLocations(((LowStockApp.Classes.AppSession)session).LinnworksApiSessionToken, ((LowStockApp.Classes.AppSession)session).Server);


        }
    }
}
