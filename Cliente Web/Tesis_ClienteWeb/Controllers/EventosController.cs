using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    public class EventosController : MaestraController
    {
        private string _controlador = "Eventos";
        private BridgeController _puente = new BridgeController();

        [HttpGet]
        public ActionResult CrearEvento()
        {
            ConfiguracionInicial(_controlador, "CrearEvento");

            EventosModel modelo = new EventosModel();

            #region Mensajes TempData
            if (TempData["NuevoEvento"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevoEvento"].ToString();
            }
            if (TempData["NuevoEventoError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevoEventoError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorFechasEvento"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorFechasEvento"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ColegioNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ColegioNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["EventosNoAgregados"] != null)
            {
                ModelState.AddModelError("", TempData["EventosNoAgregados"].ToString());
                modelo.MostrarErrores = "block";
            }

            #endregion

            return View(modelo);
        }

        [HttpPost]
        public JsonResult CrearEvento(List<Object> values)
        {
            List<object> jsonResult = new List<object>();

            #region Validación de eventos añadidos
            if (values.Count == 1)
            {
                TempData["EventosNoAgregados"] = "Por favor inserte al menos un evento.";
                jsonResult.Add(new { success = false });

                return Json(jsonResult);
            }
            #endregion
            #region Validando fechas de cada evento
            for (int i = 1; i <= values.Count - 1; i++)
            {
                string[] valores = values[i].ToString().Split(',');

                DateTime StartDate = Convert.ToDateTime(valores[2].ToString());
                DateTime FinishDate = Convert.ToDateTime(valores[3].ToString());

                if (StartDate > FinishDate)
                {
                    TempData["ErrorFechasEvento"] = "No se puede agregar el o los eventos, debido a que uno de" +
                        " ellos tiene la fecha de inicio mayor a la fecha de fin";
                    jsonResult.Add(new { Success = false });

                    return Json(jsonResult);
                }
            }
            #endregion

            #region Declaración de servicios
            UnitOfWork unidad = new UnitOfWork();
            SchoolYearService schoolYearService = new SchoolYearService(unidad);
            EventService eventService = new EventService(unidad);
            #endregion

            #region Obteniendo Colegio y Año escolar
            string[] idAux = values[0].ToString().Split(',');
            int schoolId = Convert.ToInt32(idAux[0]);
            int schoolYearId = Convert.ToInt32(idAux[1]);

            SchoolYear schoolYear = schoolYearService.ObtenerAnoEscolar(schoolId);
            School school = schoolYear.School;
            #endregion
            #region Ciclo de creación de eventos
            try
            {
                for (int i = 1; i <= values.Count - 1; i++)
                {
                    string[] valores = values[i].ToString().Split(',');

                    #region Creación de nuevo evento
                    Event evento = new Event();
                    evento.Name = valores[0].ToString();
                    evento.Description = valores[1].ToString();
                    evento.StartDate = Convert.ToDateTime(valores[2].ToString());
                    evento.FinishDate = Convert.ToDateTime(valores[3].ToString());
                    evento.StartHour = null;
                    evento.EndHour = null;
                    evento.EventType = (evento.StartDate == evento.FinishDate ? "Evento de un día" : 
                        "Evento de varios días");
                    evento.SchoolYear = schoolYear;
                    #endregion

                    #region Agregando evento a la bd
                    try
                    {
                        if (evento.StartDate == evento.FinishDate)
                            eventService.CrearEventoGlobal(
                                ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_EVENT_1_DAY, evento);
                        else
                            eventService.CrearEventoGlobal(
                                ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_EVENT_VARIOUS_DAYS, evento);
                    }
                    catch (Exception e)
                    {
                        TempData["NuevoEventoError"] = e.Message;
                        jsonResult.Add(new { Success = false });

                        return Json(jsonResult);
                    }
                    #endregion
                }

                TempData["NuevoEvento"] = "Se agregaron correctamente los eventos.";

                jsonResult.Add(new { success = true });
                return Json(jsonResult);
            }
            #endregion
            #region Catch de los errores
            catch (Exception e)
            {
                TempData["NuevoEventoError"] = e.Message;
                jsonResult.Add(new { Success = false });

                return Json(jsonResult);
            }
            #endregion
        }

        [HttpGet]
        public ActionResult CrearEventoAvanzado()
        {
            ConfiguracionInicial(_controlador, "CrearEventoAvanzado");

            EventosModel modelo = new EventosModel();

            #region Mensajes TempData
            if (TempData["NuevoEvento"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevoEvento"].ToString();
            }
            if (TempData["NuevoEventoError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevoEventoError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorFechasEvento"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorFechasEvento"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ColegioNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ColegioNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["EventosNoAgregados"] != null)
            {
                ModelState.AddModelError("", TempData["EventosNoAgregados"].ToString());
                modelo.MostrarErrores = "block";
            }
            else if (TempData["ModelError"] != null)
            {
                modelo.MostrarErrores = "block";
                foreach (ModelState error in (List<ModelState>)TempData["ModelError"])
                {
                    if (error.Errors.Count != 0)
                        ModelState.AddModelError("", error.Errors[0].ErrorMessage);
                }
            }

            #endregion

            List<string> listaTipos = ConstantRepository.EVENT_TYPE_LIST.ToList();
            modelo.selectListTiposEventos = new SelectList(listaTipos, "Value");

            return View(modelo);
        }

        [HttpPost]
        public ActionResult CrearEventoAvanzado(EventosModel modelo)
        {
            List<object> jsonResult = new List<object>();
            
            #region Validación del modelo
            if (!ModelState.IsValid)
            {
                TempData["ModelError"] = ModelState.Select(m => m.Value).ToList();
                return RedirectToAction("CrearEventoAvanzado");
            } 
            #endregion
            #region Declaración de servicios
            UnitOfWork unidad = new UnitOfWork();
            SchoolYearService schoolYearService = new SchoolYearService(unidad);
            EventService eventService = new EventService(unidad);
            #endregion
            #region Obteniendo Colegio y Año escolar
            SchoolYear schoolYear = schoolYearService.ObtenerAnoEscolar(modelo.idColegio);
            School school = schoolYear.School;
            #endregion

            #region Creando el evento nuevo
            try
            {
                #region Creación de nuevo evento
                Event evento = new Event();
                evento.Name = modelo.Name;
                evento.Description = modelo.Description; ;
                evento.StartDate = modelo.StartDate;
                evento.FinishDate = modelo.FinishDate;
                evento.StartHour = modelo.StartHour;
                evento.EndHour = modelo.EndHour;
                evento.EventType = modelo.TipoEvento;
                evento.SchoolYear = schoolYear;
                #endregion
                #region Agregando evento a la bd
                if (evento.StartDate <= evento.FinishDate)
                {
                    try
                    {
                        if (evento.StartDate == evento.FinishDate)
                            eventService.CrearEventoGlobal(
                                ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_EVENT_1_DAY, evento);
                        else
                            eventService.CrearEventoGlobal(
                                ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_EVENT_VARIOUS_DAYS, evento);
                    }
                    catch (Exception e)
                    {
                        TempData["NuevoEventoError"] = e.Message;
                    }
                }
                else
                {
                    TempData["ErrorFechasEvento"] = "No se puede agregar el evento debido a que la " +
                        "fecha de inicio es mayor a la fecha fin";
                }
                #endregion

                TempData["NuevoEvento"] = "Se agregaron correctamente los eventos '";
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["NuevoEventoError"] = e.Message;
            }
            #endregion

            return RedirectToAction("CrearEventoAvanzado");
        }









        [HttpGet]
        public ActionResult CalendarioEventos()
        {
            EventosModel modelo = new EventosModel();
            #region Mensajes TempData
            if (TempData["NuevoEvento"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevoEvento"].ToString();
            }
            if (TempData["NuevoEventoError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevoEventoError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorFechasEvento"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorFechasEvento"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["EventosNoAgregados"] != null)
            {
                ModelState.AddModelError("", TempData["EventosNoAgregados"].ToString());
                modelo.MostrarErrores = "block";
            }

            #endregion         
            
            return View(modelo);
        }

        




        #region Pantalla Crear Evento Avanzado
        
        
        #endregion
        #region Pantalla Gestión de Eventos

        [HttpGet]
        public ActionResult GestionEventos()
        {
            EventosModel modelo = new EventosModel();

            #region Mensajes TempData
            if (TempData["NuevoEvento"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevoEvento"].ToString();
            }
            if (TempData["NuevoEventoError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevoEventoError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorFechasEvento"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorFechasEvento"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ColegioNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ColegioNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["EventosNoAgregados"] != null)
            {
                ModelState.AddModelError("", TempData["EventosNoAgregados"].ToString());
                modelo.MostrarErrores = "block";
            }

            #endregion
            return View(modelo);
        }

        #endregion
        #region Gestión de Eventos


        [HttpPost]
        public JsonResult GetListaEventosCalendarioMaestra(int idColegio)
        {
            EventService eventService = new EventService();
            SchoolYearService _schoolYearService = new SchoolYearService();
            SchoolYear schoolYear = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(idColegio);
            List<Event> listaeventos = schoolYear.Events.Where(m => m.DeleteEvent == false).ToList();
            List<object> listaeventosaenviar = new List<object>();

            foreach(Event evento in listaeventos)
            {
                listaeventosaenviar.Add(new { 
                    id = evento.EventId,
                    name = evento.Name,
                    description = evento.Description,
                    startdate = evento.StartDate.Date.ToString(("yyyy-MM-dd")),
                    finishdate = evento.FinishDate.Date.ToString(("yyyy-MM-dd")),
                    starthour = evento.StartHour,
                    endhour = evento.EndHour,
                    color = evento.Color,
                    deleteevent = evento.DeleteEvent,
                    restadiasfechas = (evento.FinishDate.Date - evento.StartDate.Date).Days.ToString()
                });
            }


            return Json(listaeventosaenviar, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearEventoProf(string name, string description, string startdate, string finishdate,
                                        string starthour, string endhour, string color, string tipoevento)
                    {

                        UnitOfWork _unidad = new UnitOfWork();
                        UserService _userService = new UserService(_unidad);
                        EventService _eventService = new EventService(_unidad);
                        SchoolYearService _schoolYearService = new SchoolYearService(_unidad);
                         string idsession = (string)Session["UserId"];
                         User usuarioevento = _userService.ObtenerUsuarioPorId(idsession);
                         School colegio = usuarioevento.School;
                         #region Obteniendo año escolar
                         SchoolYear añoEscolar = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(colegio.SchoolId);
                         #endregion
                        try
                        {
                            Event evento = new Event()
                            {
                                Name = name,
                                Description = description,
                                StartDate = Convert.ToDateTime(startdate),
                                FinishDate = Convert.ToDateTime(finishdate),
                                StartHour = starthour,
                                EndHour = endhour,
                                Color = color,
                                EventType = tipoevento,
                                DeleteEvent = true,
                                SchoolYear = añoEscolar,
                                
                            };
                               if (evento.StartDate <= evento.FinishDate)
                            {
                                try
                                {
                                    if (evento.StartDate == evento.FinishDate)
                                    {
                                        _eventService.CrearEventoPersonalizado(ConstantRepository
                                   .PERSONAL_EVENT_CATEGORY_1_DAY, evento, usuarioevento);
                                    }
                                    else
                                    {
                                       _eventService.CrearEventoPersonalizado(ConstantRepository
                                       .PERSONAL_EVENT_CATEGORY_VARIOUS_DAYS, evento, usuarioevento);
                                    }

                                }
                                catch (Exception e)
                                {
                                    TempData["NuevoEventoError"] = e.Message;
                                    return RedirectToAction("CalendarioEventos");
                                }
                            }
                            else
                            {
                                TempData["ErrorFechasEvento"] = "No se puede agregar el evento debido a que la " +
                                    "fecha de inicio es mayor a la fecha fin";
                                return RedirectToAction("CalendarioEventos");
                            }

                               TempData["NuevoEvento"] = "Se agregaron correctamente los eventos '"; 
                        }
                        catch (Exception e)
                        {
                            TempData["EventError"] = e.Message;
                            return RedirectToAction("CalendarioEventos");
                        }

                        return Json(true);
        }

        [HttpPost]
        public ActionResult EliminarEvento(int id)
        {
            UnitOfWork _unidad = new UnitOfWork();
            EventService eventosService = new EventService(_unidad);
            NotificationService _notificationService = new NotificationService(_unidad);
            Event evento = eventosService.ObtenerEventoPorId(id);
            List<Notification> listanotificaciones = evento.Notifications.ToList<Notification>();
                        
            while (listanotificaciones.Count > 0) 
            {
                _notificationService.EliminarNotification(listanotificaciones[0].NotificationId);
                
            }

            
            eventosService.EliminarEvento(id);
            return RedirectToAction("Eventos", "CalendarioEventos");
        }

        #endregion
    }


}
