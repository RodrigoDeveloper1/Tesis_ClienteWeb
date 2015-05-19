using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tesis_ClienteWeb.Controllers
{
    public class ErroresController : Controller
    {
        #region Pantalla Not Found error 404
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;  
            return View("NotFound");
        }
        #endregion

        #region Pantalla Session Expired
        public ActionResult SessionExpired()
        {
            
            return View("SessionExpired");
        }
        #endregion


        #region Pantalla Sin Privilegios
        public ActionResult SinPrivilegios()
        {

            return View("SinPrivilegios");
        }
        #endregion
    }
}