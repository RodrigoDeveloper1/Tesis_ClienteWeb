using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Tesis_ClienteWeb.App_Start;
using Tesis_ClienteWeb.Controllers;
using Tesis_ClienteWeb_Data.UserExceptions;

namespace Tesis_ClienteWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            #region Cambiando de motor de vista
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new WebFormViewEngine());
            #endregion

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // Gestión de errores
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;
            
            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                        //Redirecciona el error 404
                    case 404:
                        // clear error on server
                        Server.ClearError();
                        Response.Redirect("/Errores/NotFound");
                        break;
                }
            }

            #region Session Expired Error
            SessionExpiredException sessionExpiredException = exception as SessionExpiredException;

            if (sessionExpiredException != null)
            {
                Server.ClearError();
                Response.Redirect("/Errores/SessionExpired");
            }
            #endregion
        }
    }
}