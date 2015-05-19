using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Data.UserExceptions;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    [Authorize]
    public class IndexController : MaestraController
    {
        private string _controlador = "Index";
        private BridgeController _puente = new BridgeController();

        [HttpGet]
        public ActionResult Inicio()
        {
            ConfiguracionInicial(_controlador, "Inicio");

            EventService eventService = new EventService();
            IndexModel model = new IndexModel();
            model.ListaEventos = eventService.ObtenerProximosEventosPor_Usuario(_session.USERID, 5);

            return View(model);
        }

        [HttpGet]
        public ActionResult Header()
        {
            ObteniendoSesion();

            #region Declaración de servicios
            SchoolYearService schoolYearService = new SchoolYearService();
            SchoolService schoolService = new SchoolService();
            UserService userService = new UserService();
            EventService eventService = new EventService();
            NotificationService notificationService = new NotificationService();
            #endregion
            
            MaestraModel model = new  MaestraModel();

            model.ACCESO_MAESTRAS = (_session.ADMINISTRADOR || _session.COORDINADOR ? true : false);
            model.ADMINISTRADOR = _session.ADMINISTRADOR;
            model._USERNAME = _session.USERNAME;

            model.ListaEventosHeader = eventService.ObtenerProximosEventosPor_Usuario(_session.USERID, 3);
            model.nroNotificacionesNoLeidas = notificationService.ObtenerNumeroNotificacionesNoLeidas();
            model.MostrarNroNotificaciones = model.nroNotificacionesNoLeidas == 0 ? "none" : "block";

            if(model.ADMINISTRADOR)
            {
                List<School> listaColegios = schoolService.ObtenerListaColegiosActivos();
                model.selectListColegios = new SelectList(listaColegios, "SchoolId", "Name", _session.SCHOOLID);
            }

            SchoolYear schoolYear = schoolYearService.ObtenerAnoEscolar(_session.SCHOOLYEARID);
            model.ListaPorcentajesPeriodosHeader = _puente.ObtenerPorcentajesPeriodos(DateTime.Now, schoolYear, 
                schoolYear.Periods);

            return View(model);
        }

        [HttpGet]
        public ActionResult Footer()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LeftSide()
        {
            return View();
        }
    }
}
