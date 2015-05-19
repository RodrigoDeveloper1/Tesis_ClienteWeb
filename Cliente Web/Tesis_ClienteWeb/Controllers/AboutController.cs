using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;

namespace Tesis_ClienteWeb.Controllers
{
    public class AboutController : Controller
    {
        #region Pantalla About

        public ActionResult About()
        {
            return  View(new MaestraModel());
        }

        #endregion
    }
}
