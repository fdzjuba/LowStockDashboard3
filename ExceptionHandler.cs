using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace LowStockDashboard
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(actionExecutedContext.Exception.Message)
            };
            //send logs to big thing where logs are kept

            base.OnException(actionExecutedContext);
        }
    }
}