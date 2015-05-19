using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    public class NotificacionesController : MaestraController
    {
        private string _controlador = "Notificaciones";
        private BridgeController _puente = new BridgeController();

        [HttpGet]
        public ActionResult NotificacionesAutomaticas()
        {
            ConfiguracionInicial(_controlador, "NotificacionesAutomaticas");
            NotificacionesPersonalizadasModel model = new NotificacionesPersonalizadasModel();

            #region Mensajes TempData
            if (TempData["ConfirmacionNotificacion"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["ConfirmacionNotificacion"].ToString();
            }
            else if (TempData["ErrorNotificacion"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorNotificacion"].ToString());
            }
            #endregion

            model.selectListSujetos = _puente.InicializadorListaSujetosNotificaciones(model.selectListSujetos);
            model.selectListCursos = _puente.InicializadorListaCursos(model.selectListCursos, _session.SCHOOLYEARID);
            model.selectListAtribucion = _puente.InicializadorListaAtribuciones(model.selectListAtribucion);

            return View(model);
        }

        [HttpGet]
        public ActionResult NotificacionesPersonalizadas()
        {
            ConfiguracionInicial(_controlador, "NotificacionesPersonalizadas");

            UserService userService = new UserService();
            User coordinador = userService.ObtenerUsuarioPorId(_session.USERID);
            List<School> listaColegios = new List<School>();
            listaColegios.Add(coordinador.School);

            NotificacionesPersonalizadasModel model = new NotificacionesPersonalizadasModel();
            model.selectListColegios = new SelectList(listaColegios, "SchoolId", "Name");

            #region Mensajes TempData
            if (TempData["ConfirmacionNotificacion"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["ConfirmacionNotificacion"].ToString();
            }
            else if (TempData["ErrorNotificacion"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorNotificacion"].ToString());
            }
            #endregion

            model.selectListSujetos = _puente.InicializadorListaSujetosNotificaciones(model.selectListSujetos);
            model.selectListCursos = _puente.InicializadorListaCursos(model.selectListCursos, _session.SCHOOLYEARID);
            model.selectListAtribucion = _puente.InicializadorListaAtribuciones(model.selectListAtribucion);

            return View(model);
        }

        [HttpPost]
        public JsonResult EnviarNotificacion(string idSujeto, string tipoSujeto, string mensajeNotificacion,
            string tipoNotificacion, string atribucion)
        {
            ConfiguracionInicial(_controlador, "EnviarNotificacion");
            
            #region Declaración de variables
            UnitOfWork unidad = new UnitOfWork();
            SchoolYearService schoolYearService = new SchoolYearService(unidad);
            SchoolYear schoolYear = schoolYearService.ObtenerAnoEscolar(_session.SCHOOLYEARID);
            Student estudiante;
            User usuario;
            User docente; //A quien va dirigida la notificación
            Course curso;

            List<object> jsonResult = new List<object>();
            #endregion

            #region Obteniendo el usuario de la sesión
            UserService _userService = new UserService(unidad);
            usuario = _userService.ObtenerUsuarioDeSesion();
            #endregion
            #region Creando el objeto notificación
            Notification notificacion = new Notification()
            {
                AlertType = tipoNotificacion,
                Attribution = (atribucion == "" ? "N/A" : atribucion),
                Automatic = false,
                Message = mensajeNotificacion,
                SendDate = DateTime.Now,
                User = usuario,
                SchoolYear = schoolYear
            };
            #endregion
            #region Identificando el tipo de sujeto
            #region Sujeto -> Representante
            if (tipoSujeto.Equals("Representante"))
            {
                StudentService _studentService = new StudentService(unidad);
                estudiante = _studentService.ObtenerAlumnoPorId(Convert.ToInt32(idSujeto));

                SentNotification sentNotification = new SentNotification()
                {
                    Course = null,
                    Student = estudiante,
                    User = null
                };

                notificacion.SentNotifications.Add(sentNotification);
            }
            #endregion
            #region Sujeto -> Docente
            else if (tipoSujeto.Equals("Usuario"))
            {
                docente = _userService.ObtenerUsuarioPorId(idSujeto);

                SentNotification sentNotification = new SentNotification()
                {
                    Course = null,
                    Student = null,
                    User = docente
                };

                notificacion.SentNotifications.Add(sentNotification);
            }
            #endregion
            #region Sujeto -> Curso
            else if (tipoSujeto.Equals("Curso"))
            {
                CourseService _courseService = new CourseService(unidad);
                curso = _courseService.ObtenerCursoPor_Id(Convert.ToInt32(idSujeto));

                SentNotification sentNotification = new SentNotification()
                {
                    Course = curso,
                    Student = null,
                    User = null
                };

                notificacion.SentNotifications.Add(sentNotification);
            }
            #endregion
            #endregion
            #region Creando la notificación en bd
            try
            {
                NotificationService _notificationService = new NotificationService(unidad);
                _notificationService.GuardarNotification(notificacion);

                TempData["ConfirmacionNotificacion"] = "Se ha enviado la notificación correctamente.";

                jsonResult.Add(new { success = true });
            }
            catch (Exception e)
            {
                TempData["ErrorNotificacion"] = e.Message;

                jsonResult.Add(new { success = false });
            }
            #endregion

            return Json(jsonResult);
        }











        

        

        
        

        [HttpGet]
        public ActionResult Buzon()
        {
            #region Código anterior
            /*
            #region Inicialización de variables
            NotificacionesPersonalizadasModel model = new NotificacionesPersonalizadasModel();
            _notificationService = (_notificationService == null ? new NotificationService() : _notificationService);
            _representativeService = (_representativeService == null ? new RepresentativeService() : _representativeService);
            List<Notification> notificacionesAutomaticas = new List<Notification>();
            List<Notification> notificacionesPersonales = new List<Notification>();
            List<Representative> listaRepresentantes = new List<Representative>();
            List<string> listaTiposAlerta = new List<string>();
            List<string> listaAtribuciones = new List<string>();
            #endregion
            #region Obteniendo listas de notificaciones
            notificacionesAutomaticas = _notificationService.ObtenerListaNotificacionesAutomaticas()
                .ToList<Notification>();
            notificacionesPersonales = _notificationService.ObtenerListaNotificacionesPersonales()
                .ToList<Notification>();
            
            model.listaNotificacionesAutomaticas = notificacionesAutomaticas;
            model.listaNotificacionesPersonalizadas = notificacionesPersonales;
            #endregion

            model.selectListSujetos = _puente.InicializadorListaSujetosBuzon(model.selectListSujetos);
            
            model.selectListAtribucion = _puente.InicializadorListaAtribuciones(model.selectListAtribucion);

            return View(model);
            */
            /*
            model.listaNotificaciones = notificationController.ObtenerListaNotificaciones().ToList();
             */

            #endregion
            ObteniendoSesion();
            NotificationService notificationService = new NotificationService();
            BuzonNotificaciones model = new BuzonNotificaciones();

            List<Notification> listaNotificaciones = notificationService
                .ObtenerListaNotificacionesPor_Usuario_AnoEscolar(_session.USERID, _session.SCHOOLYEARID);
            
            return View(model);
        }

        [HttpPost]       
        public ActionResult CrearNotificacion(string titulo, string mensaje, string atribucion, 
            string representante, string tipoAlerta)
        {
            #region Declarando variables
            NotificationService _notificationService = new NotificationService();
            RepresentativeService _representativeService = new RepresentativeService();
            Notification notificacion;
            #endregion
            #region Validando los datos de entrada
            #endregion
            #region Obteniendo la lista representantes asociados
            #endregion
            #region Asociando valores a la nueva notificacion
            notificacion = new Notification();
            notificacion.Message = mensaje;
            notificacion.AlertType = tipoAlerta;
            notificacion.Attribution = atribucion;
            //notificacion.Representative = representante;
            notificacion.Automatic = false;
            #endregion
            #region Guardando la nueva notificación
            #endregion

            return RedirectToAction("Inicio", "Index");
       }

        [HttpGet]
        public ActionResult EliminarNotificacion(int id)
        {
            NotificationService notificacionController = new NotificationService();
            notificacionController.EliminarNotification(id);
            return RedirectToAction("Notificaciones", "Buzon");
        }                
    }
}
