using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EMSUtility
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public void Application_AuthorizeRequest(object sender, EventArgs e)
        {

            try
            {
                //WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;

                //if (winPrincipal.Identity.IsAuthenticated == true)
                //{
                //AreaRegistration.RegisterAllAreas();
                //WebApiConfig.Register(GlobalConfiguration.Configuration);
                //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                //RouteConfig.RegisterRoutes(RouteTable.Routes);
                //BundleConfig.RegisterBundles(BundleTable.Bundles);
                //}
            }
            catch (Exception exception)
            {
                Response.Write("<script LANGUAGE='JavaScript' >alert('" + exception + "')</script>");
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}