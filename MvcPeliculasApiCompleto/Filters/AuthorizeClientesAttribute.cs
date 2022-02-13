using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcPeliculasApiCompleto.Filters
{
    public class AuthorizeClientesAttribute : AuthorizeAttribute
    , IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var usuario = context.HttpContext.User;
            if (usuario.Identity.IsAuthenticated == false)
            {
                //SI NO ESTA VALIDADO, LO ENVIAMOS A LOGIN
                //CONTROLLER, ACTION
                RouteValueDictionary rutalogin =
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Manage"
                            ,
                            action = "Login"
                        });
                RedirectToRouteResult action =
                    new RedirectToRouteResult(rutalogin);
                context.Result = action;
            }
        }
    }
}
