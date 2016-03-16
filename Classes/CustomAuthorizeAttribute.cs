using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LowStockApp.Classes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                string token = actionContext.Request.Headers.Authorization.Parameter;
                if (token == null)
                {
                    HandleUnauthorizedRequest(actionContext);
                }
                Classes.AppSession appSession = Classes.AppSession.LoadAppSession(token);

                HttpContext.Current.Items.Add("session", appSession);
            }
            catch (Exception ex)
            {
                HandleUnauthorizedRequest(actionContext);
            }
            return;
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var challengeMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            throw new HttpResponseException(challengeMessage);
        }
    }
}