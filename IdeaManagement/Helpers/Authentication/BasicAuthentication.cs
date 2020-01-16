using IdeaManagement.Helper.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.Http.Filters;


namespace IdeaManagement.Helper.Authentication
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BasicAuthentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization != null)
                {

                    //Taking the parameter from the header  
                    var authToken = actionContext.Request.Headers.Authorization.Parameter;

                    //Passing to a function for authorization  
                    if (TokenManager.ValidateToken(authToken) != null && TokenManager.ValidateToken(authToken).Length > 1)
                    {
                        // setting current principle  
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(TokenManager.ValidateToken(authToken)), null);
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

    }
}
