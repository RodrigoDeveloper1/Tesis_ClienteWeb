using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;

namespace Tesis_ClienteWeb.Controllers
{
    [Authorize]
    public class PeriodoEscolarController : MaestraController
    {
        public ActionResult CrearPeriodoEscolar()
        {
            PeriodoEscolarModel model = new PeriodoEscolarModel();

            return View(model);
        }
    }
}