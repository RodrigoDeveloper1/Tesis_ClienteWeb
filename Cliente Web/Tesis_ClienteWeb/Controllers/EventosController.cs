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
        #region Declaración de variables
        #endregion
        #region Declaración de servicios
        private UserService _userService = new UserService();
        private EventService _eventService = new EventService();
        private PeriodService _periodService = new PeriodService();
        private SchoolYearService _schoolYearService = new SchoolYearService();
        #endregion
        
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

        
        
            
       
     




        #region Pantalla Crear Evento

        [HttpGet]
        public ActionResult CrearEvento()
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
        [HttpPost]
        public bool CrearEvento(List<Object> values)
        {
            #region Obteniendo ids
            int schoolId = 0;   
            #endregion
            #region Validación de id del colegio
            //Dentro de la lista de valores, el primer valor debe ser el id del colegio.
            if (values == null || values[0].ToString() == "")
            {
                TempData["ColegioNoSeleccionado"] = "Por favor seleccione un colegio.";
                return false;
            }
            else
                schoolId = Convert.ToInt32(values[0]);
            #endregion
            #region Validación de eventos añadidos
            if (values.Count == 1)
            {
                TempData["EventosNoAgregados"] = "Por favor inserte al menos un evento.";
                return false;
            }
            #endregion
            #region Inicializando servicios
            UnitOfWork _unidad = new UnitOfWork();
            _eventService = new EventService(_unidad);          
            _periodService = new PeriodService(_unidad);
            _schoolYearService = new SchoolYearService(_unidad);
            #endregion
            #region Obteniendo año escolar
            SchoolYear añoEscolar = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(schoolId);
            #endregion
            #region Ciclo de asignación de la lista de eventos
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
                    evento.EventType = valores[4].ToString();
                    evento.SchoolYear = añoEscolar;            

                    #endregion                  
                    #region Agregando evento a la bd

                    if (evento.StartDate <= evento.FinishDate)
                        {
                            try
                            {
                                if (evento.StartDate == evento.FinishDate)
                                {
                                    _eventService.CrearEventoGlobalPantallaEventos(ConstantRepository
                               .GLOBAL_EVENT_CATEGORY_NEW_EVENT_1_DAY, evento);
                                }
                                else {
                                    _eventService.CrearEventoGlobalPantallaEventos(ConstantRepository
                                   .GLOBAL_EVENT_CATEGORY_NEW_EVENT_VARIOUS_DAYS, evento);
                                }
                                
                            }
                            catch (Exception e)
                            {
                                TempData["NuevoEventoError"] = e.Message;
                                return false;
                            }
                        }
                        else
                        {
                            TempData["ErrorFechasEvento"] = "No se puede agregar el evento debido a que la " +
                                "fecha de inicio es mayor a la fecha fin";
                            return false;
                        }
                    
                   
                    #endregion
                }
            #endregion
                TempData["NuevoEvento"] = "Se agregaron correctamente los eventos '";
                return true;
            }
            catch (Exception e)
            {
                TempData["NuevoEventoError"] = e.Message;
                return false;
            }
        }
        #endregion
        #region Pantalla Crear Evento Avanzado
        [HttpGet]
        public ActionResult CrearEventoAvanzado()
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
            List<string> listaTipos = ConstantRepository.EVENT_TYPE_LIST.ToList();
            modelo.selectListTiposEventos = new SelectList(listaTipos, "Value");
            return View(modelo);
        }
        [HttpPost]
        public ActionResult CrearEventoAvanzado(EventosModel modelo)
        {
            #region Obteniendo ids
            int schoolId = modelo.idColegio;
            #endregion
            #region Inicializando servicios
            UnitOfWork _unidad = new UnitOfWork();
            _eventService = new EventService(_unidad);
            _periodService = new PeriodService(_unidad);
            _schoolYearService = new SchoolYearService(_unidad);
            #endregion
            #region Obteniendo año escolar
            SchoolYear añoEscolar = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(schoolId);
            #endregion
            #region Ciclo de asignación de la lista de eventos
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
                    evento.SchoolYear = añoEscolar;  
                    #endregion
                    #region Agregando evento a la bd

                    if (evento.StartDate <= evento.FinishDate)
                    {
                        try
                        {
                            if (evento.StartDate == evento.FinishDate)
                            {
                                _eventService.CrearEventoGlobalPantallaEventos(ConstantRepository
                           .GLOBAL_EVENT_CATEGORY_NEW_EVENT_1_DAY, evento);
                            }
                            else
                            {
                                _eventService.CrearEventoGlobalPantallaEventos(ConstantRepository
                               .GLOBAL_EVENT_CATEGORY_NEW_EVENT_VARIOUS_DAYS, evento);
                            }

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
                
            #endregion
                TempData["NuevoEvento"] = "Se agregaron correctamente los eventos '";                
            }
            catch (Exception e)
            {
                TempData["NuevoEventoError"] = e.Message;
            }
            return RedirectToAction("CrearEventoAvanzado");
        }
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
            _schoolYearService = new SchoolYearService();
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
                        _userService = new UserService(_unidad);
                        _eventService = new EventService(_unidad);
                        _schoolYearService = new SchoolYearService(_unidad);
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
