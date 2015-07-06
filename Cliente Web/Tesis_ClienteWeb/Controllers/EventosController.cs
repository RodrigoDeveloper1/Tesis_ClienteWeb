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
            ConfiguracionInicial(_controlador, "CalendarioEventos");

            EventosModel modelo = new EventosModel();

            #region Mensajes TempData
            if (TempData["NuevoEvento"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevoEvento"].ToString();
            }
            else if (TempData["NuevoEventoError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevoEventoError"].ToString());
                modelo.MostrarErrores = "block";
            }
            else if (TempData["ErrorFechasEvento"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorFechasEvento"].ToString());
                modelo.MostrarErrores = "block";
            }
            else if (TempData["EventosNoAgregados"] != null)
            {
                ModelState.AddModelError("", TempData["EventosNoAgregados"].ToString());
                modelo.MostrarErrores = "block";
            }
            else if (TempData["Error"] != null)
            {
                ModelState.AddModelError("", TempData["Error"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion         
            
            return View(modelo);
        }

        [HttpPost]
        public JsonResult CrearEventoProf(string name, string description, string startdate, string finishdate,
            string starthour, string endhour, string color, string tipoevento)
        {
            ConfiguracionInicial(_controlador, "CrearEventoProf");

            #region Declaración de variables
            List<object> jsonResult = new List<object>();

            UnitOfWork unidad = new UnitOfWork();
            UserService userService = new UserService(unidad);
            EventService eventService = new EventService(unidad);
            SchoolYearService schoolYearService = new SchoolYearService(unidad);

            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            #endregion
            #region Validación del modelo
            if(name == null || name.Equals("")) 
            { 
                TempData["Error"] = "Por favor, especifique un nombre para el evento";
                jsonResult.Add(new { Success = false });

                return Json(jsonResult);
            }
            else if (description == null || description.Equals("")){
                TempData["Error"] = "Por favor, especifique una descripción para el evento";
                jsonResult.Add(new { Success = false });

                return Json(jsonResult);
            }
            else if (finishdate == null || finishdate.Equals("")) 
            {
                TempData["Error"] = "Por favor, especifique una fecha de inicio para el evento";
                jsonResult.Add(new { Success = false });

                return Json(jsonResult);
            }
            else if (startdate == null || startdate.Equals(""))
            {
                TempData["Error"] = "Por favor, especifique una fecha de finalización para el evento";
                jsonResult.Add(new { Success = false });

                return Json(jsonResult);
            }
            #endregion
            #region Validación de las fechas
            StartDate = Convert.ToDateTime(startdate);
            EndDate = Convert.ToDateTime(finishdate);

            if(StartDate > EndDate) {
                TempData["Error"] = "La fecha de inicio del evento es mayor a la fecha de finalización.";
                jsonResult.Add(new { Success = false });

                return Json(jsonResult);
            }
            #endregion
            #region Configurando el formato de las horas
            string[] auxHour = starthour.Split(':');
            starthour = (auxHour[0].Count() == 1 ? "0" + starthour : starthour);

            auxHour = endhour.Split(':');
            endhour = (auxHour[0].Count() == 1 ? "0" + endhour : endhour);
            #endregion

            #region Obteniendo datos del usuario de la sesión
            User user = userService.ObtenerUsuarioPorId(_session.USERID);
            School school = user.School;
            SchoolYear schoolYear = schoolYearService.ObtenerAnoEscolarActivoPorColegio(school.SchoolId);
            #endregion
            #region Definiendo el nuevo evento
            try
            {
                #region Evento
                Event evento = new Event()
                {
                    Name = name,
                    Description = description,
                    StartDate = StartDate,
                    FinishDate = EndDate,
                    StartHour = starthour,
                    EndHour = endhour,
                    Color = color,
                    EventType = tipoevento,
                    DeleteEvent = true,
                    SchoolYear = schoolYear,
                };
                evento.Users.Add(user);
                #endregion
                #region Evento de un día
                if (evento.StartDate == evento.FinishDate)
                    eventService.CrearEventoPersonal(ConstantRepository.PERSONAL_EVENT_CATEGORY_1_DAY, evento, user);
                #endregion
                #region Evento de varios días
                else
                    eventService.CrearEventoPersonal(ConstantRepository.PERSONAL_EVENT_CATEGORY_VARIOUS_DAYS, 
                        evento, user);
                #endregion

                jsonResult.Add(new { Success = true });
                TempData["NuevoEvento"] = "Se ha agregado correctamente el evento: '" + name + "'";

                return Json(jsonResult);
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["NuevoEventoError"] = e.Message;
                jsonResult.Add(new { Success = false });

                return Json(jsonResult);
            }
            #endregion
        }

        [HttpPost]
        public JsonResult EliminarEvento(int id)
        {
            ConfiguracionInicial(_controlador, "EliminarEvento");

            UnitOfWork unidad = new UnitOfWork();
            EventService eventService = new EventService(unidad);
            List<object> jsonResult = new List<object>();

            try 
            {
                Event evento = eventService.ObtenerEventoPorId(id);
                eventService.EliminarEvento(id);

                jsonResult.Add(new { Success = true });
            }
            catch (Exception e) 
            {
                TempData["Error"] = e.Message;
                jsonResult.Add(new { Success = true });
            }

            return Json(jsonResult);
        }







        // Por revisar - Rodrigo Uzcátegui (26-06-15)

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
        #endregion
    }
}