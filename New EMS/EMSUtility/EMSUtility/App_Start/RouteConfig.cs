using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EMSUtility
{
    [Authorize]
    public class RouteConfig
    {
        [Authorize]
        public static void RegisterRoutes(RouteCollection routes)
        {
            try
            {
                WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;

                //if (winPrincipal.Identity.IsAuthenticated == true)
                //{
                    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

                    routes.MapRoute(
                        name: "Default",
                        url: "{controller}/{action}/{id}",
                        defaults: new { controller = "Employee", action = "Index", id = UrlParameter.Optional }
                    );
               // }
            }
            catch (Exception ex) { }
        }
    }
}
