using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tesis_ClienteWeb.Controllers
{
    public class ContactoController : Controller
    {
        #region Pantalla Contacto

        public ActionResult Contacto()
        {
            return View();
        }

         #endregion

        #region Pantalla Email

        public ActionResult Email()
        {
            return View();
        }

        #endregion
    }
}
