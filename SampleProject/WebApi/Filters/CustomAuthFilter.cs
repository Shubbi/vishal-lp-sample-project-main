using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Core.Services.Users;
using System.Web.Http.Results;
using BusinessEntities;
using System.Security.Claims;
using Core.Services.Orders;

namespace WebApi.Filters
{
    public class CustomAuthFilter : AuthorizeAttribute
    {   
        private readonly string[] _allowedRoles;

        public CustomAuthFilter(            
            params string[] allowedRoles)
        {   
            _allowedRoles = allowedRoles;
            
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var getUserService = (IGetUserService)actionContext.Request.GetDependencyScope().GetService(typeof(IGetUserService));
            var getOrderService = (IGetOrderService)actionContext.Request.GetDependencyScope().GetService(typeof(IGetOrderService));

            if (getUserService == null || getOrderService == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = "Dependency injection failure"
                };
                return;
            }

            var request = actionContext.Request;
            
            //I'm implementing a simple authorization
            //and trying to do authorization based on UserId key provided in Request Header 
            //but we can definitely do more and use Bearer token"

            if(!request.Headers.TryGetValues("UserId", out var userIdList))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Missing or Invalid UserId key" };
                return;
            }

            var userId = userIdList.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var guid))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Missing or Invalid UserId key"};
                return;
            }

            var user = getUserService.GetUser(guid);

            if (user == null)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) { ReasonPhrase = "User not found" };
                return;
            }

            var userTypeString = Enum.GetName(typeof(UserTypes), user.Type);

            //So either you are in Admin role
            //Or nor role specified on the Attribute for controller/method
            //Or they need to match
            if ( userTypeString != "Admin" 
                && _allowedRoles.Length > 0 
                && !_allowedRoles.Contains(userTypeString, StringComparer.OrdinalIgnoreCase)
            )
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "Access denied"
                };
                return;
            }

            if (userTypeString.Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                //Need to replace this with actual call to order service
                if(true)
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                    {                        
                        ReasonPhrase = "Invalid Order"
                    };
                    return;
                }
            }


            var identity = new ClaimsIdentity("Custom");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, userTypeString));

            var principal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = principal;
            HttpContext.Current.User = principal;
        }
    }
}