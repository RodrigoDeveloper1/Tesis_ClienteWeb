using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    public class EvaluacionesController : MaestraController
    {
        string _controlador = "Evaluaciones";
        BridgeController _puente = new BridgeController();

        [HttpGet]
        public ActionResult CrearEvaluacion()
        {
            ConfiguracionInicial(_controlador, "CrearEvaluacion");
            EvaluacionModel modelo = new EvaluacionModel();

            #region Mensajes TempData
            if (TempData["NuevaEvaluacion"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevaEvaluacion"].ToString();
            }
            if (TempData["NuevaEvaluacionError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevaEvaluacionError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorPorcEvaluacion"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorPorcEvaluacion"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorFechasEvaluacion"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorFechasEvaluacion"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ColegioNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ColegioNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["CursoNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["CursoNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["LapsoNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["LapsoNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["MateriaNoSeleccionada"] != null)
            {
                ModelState.AddModelError("", TempData["MateriaNoSeleccionada"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ProfesorNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ProfesorNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["EvaluacionesNoAgregadas"] != null)
            {
                ModelState.AddModelError("", TempData["EvaluacionesNoAgregadas"].ToString());
                modelo.MostrarErrores = "block";
            }

            #endregion

            List<Course> listaCursos = new List<Course>();
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            List<Subject> listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "SubjectId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            List<User> listaProfesores = new List<User>();
            modelo.selectListProfesores = new SelectList(listaProfesores, "UserId", "Name");
            List<string> listaTipos = ConstantRepository.ACTIVITY_LIST.ToList();
            modelo.selectListTipos = new SelectList(listaTipos, "Value");
            List<string> listaTecnicas = ConstantRepository.TECHNIQUE_LIST.ToList();
            modelo.selectListTecnicas = new SelectList(listaTecnicas, "Value");
            List<string> listaInstrumentos = ConstantRepository.INSTRUMENT_LIST.ToList();
            modelo.selectListInstrumentos = new SelectList(listaInstrumentos, "Value");
            return View(modelo);
        }

        [HttpPost]
        public bool CrearEvaluacion(List<Object> values)
        {
            //Si entra en [HttpGet] CrearEvaluacion(), no necesita de ConfiguracionInicial();
            #region Validación de evaluaciones añadidas
            if (values.Count == 5)
            {
                TempData["EvaluacionesNoAgregadas"] = "Por favor inserte al menos una evaluación.";
                return false;
            }
            #endregion
            #region Declaración de servicios
            UnitOfWork unidad = new UnitOfWork();
            AssessmentService assessmentService = new AssessmentService(unidad);
            EventService eventService = new EventService(unidad);
            CASUService casuService = new CASUService(unidad);
            #endregion
            #region Obteniendo ids
            int schoolId = Convert.ToInt32(values[0]);
            int periodId = Convert.ToInt32(values[1]);
            int courseId = Convert.ToInt32(values[2]);
            int subjectId = Convert.ToInt32(values[3]);
            string userId = values[4].ToString();
            #endregion
            #region Obteniendo el CASU respectivo
            CASU casu = casuService.ObtenerCASUPor_Ids(courseId, periodId, subjectId);
            #endregion
            #region Ciclo de asignación de la lista de evaluaciones
            try
            {
                /* A partir del valor #5, de la lista de valores (List<Object> values), empiezan las 
                 * evaluaciones agregadas*/
                for (int i = 5; i <= values.Count - 1; i++)
                {
                    string[] valores = values[i].ToString().Split(','); //Separando los datos de la evaluación
                    #region Creación de nueva evaluacion
                    Assessment evaluacion = new Assessment();
                    evaluacion.Name = valores[0].ToString();
                    evaluacion.Percentage = Convert.ToInt32(valores[1]);
                    evaluacion.StartDate = Convert.ToDateTime(valores[2].ToString());
                    evaluacion.FinishDate = Convert.ToDateTime(valores[3].ToString());
                    evaluacion.StartHour = null;
                    evaluacion.EndHour = null;
                    evaluacion.Activity = valores[4].ToString();
                    evaluacion.CASU = casu;
                    #region Definiendo el tipo de instrumento y el tipo de técnica
                    #region Prueba
                    if (evaluacion.Activity.Equals("Prueba"))
                    {
                        evaluacion.Technique = "Prueba";
                        evaluacion.Instrument = "Prueba escrita estructurada";
                    }
                    #endregion
                    #region Taller
                    if (evaluacion.Activity.Equals("Taller"))
                    {
                        evaluacion.Technique = "Prueba";
                        evaluacion.Instrument = "Prueba escrita estructurada";
                    }
                    #endregion
                    #region Exposición
                    if (evaluacion.Activity.Equals("Exposición"))
                    {
                        evaluacion.Technique = "Observación participante";
                        evaluacion.Instrument = "Prueba oral";
                    }
                    #endregion
                    #region Otro
                    if (evaluacion.Activity.Equals("Otro"))
                    {
                        evaluacion.Technique = "Otro";
                        evaluacion.Instrument = "Otro";
                    }
                    #endregion
                    #endregion
                    #endregion                    
                    #region Validación del porcentaje asociado
                    int porcentaje = assessmentService
                        .ObtenerTotalPorcentajeEvaluacionesPor_Lapso_Curso_Materia(periodId, courseId, subjectId);
                    int porcentajeFinal = porcentaje + evaluacion.Percentage;

                    if (porcentajeFinal > 100)
                    {
                        TempData["ErrorPorcEvaluacion"] = "No se puede agregar la evaluación debido a que su " +
                            "porcentaje excede al máximo permitido (100%). Actualmente para el curso está " + 
                            "asignado el: " + porcentaje + "% del total.";
                        return false;
                    }
                    #endregion
                    #region Validación de las fechas de la evaluación
                    if (evaluacion.StartDate > evaluacion.FinishDate)
                    {
                        TempData["ErrorFechasEvaluacion"] = "No se puede agregar la evaluación debido" +
                            " a que la fecha de inicio es mayor a la fecha fin";
                        return false;
                    }
                    #endregion
                    #region Agregando evaluación a la bd
                    try
                    {
                        evaluacion.Event = eventService.CrearEventoPersonal_SinGuardar(
                                ConstantRepository.PERSONAL_EVENT_CATEGORY_NEW_ASSESSMENT, evaluacion);
                        assessmentService.GuardarAssessment(evaluacion);
                    }
                    #endregion
                    #region Catch del error
                    catch (Exception e)
                    {
                        TempData["NuevaEvaluacionError"] = e.Message;
                        return false;
                    }
                    #endregion
                }
                if (values.Count - 1 > 5)
                    TempData["NuevaEvaluacion"] = "Se agregaron correctamente las evaluaciones.";
                else
                    TempData["NuevaEvaluacion"] = "Se agregó correctamente la evaluación.";
                return true;
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["NuevaEvaluacionError"] = e.Message;
                return false;
            }
            #endregion
        }

        [HttpGet]
        public ActionResult ModificarEvaluacion()
        {
            ConfiguracionInicial(_controlador, "ModificarEvaluacion");
            EvaluacionModel modelo = new EvaluacionModel();

            #region Mensajes TempData
            if (TempData["ModificadaEvaluacion"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["ModificadaEvaluacion"].ToString();
            }
            if (TempData["EvaluacionError"] != null)
            {
                ModelState.AddModelError("", TempData["EvaluacionError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorNoHayEvaluacionesAgregadas"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorNoHayEvaluacionesAgregadas"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion

            List<Course> listaCursos = new List<Course>();
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            List<Subject> listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "SubjectId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            List<User> listaProfesores = new List<User>();
            modelo.selectListProfesores = new SelectList(listaProfesores, "UserId", "Name");
            List<string> listaTipos = ConstantRepository.ACTIVITY_LIST.ToList();
            modelo.listaTiposNormal = listaTipos;
            List<string> listaTecnicas = ConstantRepository.TECHNIQUE_LIST.ToList();
            modelo.listaTecnicasNormal = listaTecnicas;
            List<string> listaInstrumentos = ConstantRepository.INSTRUMENT_LIST.ToList();
            modelo.listaInstrumentosNormal = listaInstrumentos;

            return View(modelo);
        }

        [HttpPost]
        public bool ModificarEvaluacionCreada(string name, string percentage, string startdate,
            string finishdate, string starthour, string endhour, string activity, string technique,
            string instrument, int assessmentid, string courseId, string periodId, string subjectId,
            string userId)
        {
            //Si entra en [HttpGet] ModificarEvaluacion(), no necesita de ConfiguracionInicial();
            #region Declaración de servicios
            UnitOfWork unidad = new UnitOfWork();
            AssessmentService assessmentService = new AssessmentService(unidad);
            NotificationService notificationService = new NotificationService(unidad);
            #endregion
            #region Declaración de variables
            DateTime fechaInicio = Convert.ToDateTime(startdate);
            DateTime fechaFin = Convert.ToDateTime(finishdate);
            List<Notification> listaNotificaciones = new List<Notification>();
            #endregion
            #region Obteniendo la evaluación
            Assessment evaluacion = assessmentService.ObtenerEvaluacionPor_Id(assessmentid);
            #endregion
            #region Asociando nuevos datos a la evaluación
            try
            {
                evaluacion.Name = name;
                evaluacion.Percentage = int.Parse(percentage);
                evaluacion.StartHour = starthour;
                evaluacion.EndHour = endhour;
                evaluacion.Activity = activity;
                evaluacion.Technique = technique;
                evaluacion.Instrument = instrument;                
                evaluacion.Event.StartDate = fechaInicio;
                evaluacion.Event.FinishDate = fechaInicio;
                #region Definiendo las nuevas notificaciones
                if (evaluacion.StartDate != fechaInicio && evaluacion.FinishDate != fechaFin)
                {
                    #region La evaluación no es de un solo día
                    if (evaluacion.StartDate != evaluacion.FinishDate) 
                    {
                        evaluacion.StartDate = fechaInicio;
                        evaluacion.FinishDate = fechaFin;

                        listaNotificaciones = notificationService.CrearNotificacionAutomatica(
                            ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_ASSESSMENT_BOTH_DATES, 
                            evaluacion);
                    }
                    #endregion
                    #region La evaluación es de un solo día
                    else 
                    {
                        evaluacion.StartDate = fechaInicio;
                        evaluacion.FinishDate = fechaInicio;

                        listaNotificaciones = notificationService.CrearNotificacionAutomatica(
                            ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_ASSESSMENT_START_DATE, 
                            evaluacion);
                    }
                    #endregion
                }
                else //Solo se modificó una de las fechas
                {
                    #region Se modificó la fecha inicial
                    if (evaluacion.StartDate != fechaInicio)
                    {
                        evaluacion.StartDate = fechaInicio;

                        listaNotificaciones = notificationService.CrearNotificacionAutomatica(
                            ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_ASSESSMENT_START_DATE, 
                            evaluacion);
                    }
                    #endregion
                    #region Se modificó la fecha final
                    else if (evaluacion.FinishDate != fechaFin)
                    {
                        evaluacion.FinishDate = fechaFin;

                        listaNotificaciones = notificationService.CrearNotificacionAutomatica(
                            ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_ASSESSMENT_FINISH_DATE, 
                            evaluacion);
                    }
                    #endregion
                }
                #endregion
                #region Ciclo de asignación de las notificaciones nuevas
                foreach(Notification notification in listaNotificaciones)
                {
                    evaluacion.Event.Notifications.Add(notification);
                }
                #endregion

                assessmentService.ModificarAssessment(evaluacion);

                TempData["ModificadaEvaluacion"] = "Se modificó correctamente la evaluación '" + 
                    evaluacion.Name + "'";
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["EvaluacionError"] = e.Message;
            }
            #endregion
            return true;
        }

        [HttpPost]
        public bool EliminarEvaluacion(int id)
        {
            ConfiguracionInicial(_controlador, "EliminarEvaluacion");
            UnitOfWork _unidad = new UnitOfWork();
            AssessmentService assessmentService = new AssessmentService(_unidad);
            EventService eventService = new EventService(_unidad);
            Event evento = eventService.ObtenerEventoPor_Evaluacion(id);

            try
            {
                assessmentService.EliminarAssessment(id);
                if(evento != null)
                    eventService.EliminarEvento(evento.EventId);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public ActionResult CrearEvaluacionAvanzada()
        {
            ConfiguracionInicial(_controlador, "CrearEvaluacionAvanzada");
            EvaluacionModel modelo = new EvaluacionModel();

            #region Mensajes TempData
            if (TempData["NuevaEvaluacionAvanzada"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevaEvaluacionAvanzada"].ToString();
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
            else if (TempData["NuevaEvaluacionAvanzadaError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevaEvaluacionAvanzadaError"].ToString());
                modelo.MostrarErrores = "block";
            }
            else if (TempData["ErrorPorcEvaluacionAvanzada"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorPorcEvaluacionAvanzada"].ToString());
                modelo.MostrarErrores = "block";
            }
            else if (TempData["ErrorFechasEvaluacionAvanzada"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorFechasEvaluacionAvanzada"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion

            List<Course> listaCursos = new List<Course>();
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            List<Subject> listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "SubjectId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            List<User> listaProfesores = new List<User>();
            modelo.selectListProfesores = new SelectList(listaProfesores, "UserId", "Name");
            List<string> listaTipos = ConstantRepository.ACTIVITY_LIST.ToList();
            modelo.selectListTipos = new SelectList(listaTipos, "Value");
            List<string> listaTecnicas = ConstantRepository.TECHNIQUE_LIST.ToList();
            modelo.selectListTecnicas = new SelectList(listaTecnicas, "Value");
            List<string> listaInstrumentos = ConstantRepository.INSTRUMENT_LIST.ToList();
            modelo.selectListInstrumentos = new SelectList(listaInstrumentos, "Value");

            return View(modelo);
        }

        [HttpPost]
        public ActionResult CrearEvaluacionAvanzada(EvaluacionModel modelo)
        {
            //Si entra en [HttpGet] CrearEvaluacionAvanzada(), no necesita de ConfiguracionInicial();
            #region Validando el modelo
            if (!ModelState.IsValid)
            {
                TempData["ModelError"] = ModelState.Select(m => m.Value).ToList();
                return RedirectToAction("CrearColegio");
            }
            #endregion
            #region Declaración de servicios
            UnitOfWork unidad = new UnitOfWork();
            AssessmentService assessmentService = new AssessmentService(unidad);
            EventService eventService = new EventService(unidad);
            CASUService casuService = new CASUService(unidad);
            #endregion
            #region Obteniendo ids
            int courseId = modelo.idCurso;
            int subjectId = modelo.idMateria;
            string userId = modelo.idProfesor;
            int periodId = modelo.idLapso;
            int schoolId = modelo.idColegio;
            #endregion
            #region Obteniendo el CASU respectivo
            CASU casu = casuService.ObtenerCASUPor_Ids(courseId, periodId, subjectId);
            #endregion
            #region Definiendo nuevo Assessment
            Assessment evaluacion = new Assessment();
            evaluacion.Name = modelo.Name;
            evaluacion.Percentage = modelo.Percentage;
            evaluacion.StartDate = modelo.StartDate;
            evaluacion.FinishDate = modelo.FinishDate;
            evaluacion.StartHour = modelo.StartHour;
            evaluacion.EndHour = modelo.EndHour;
            evaluacion.Activity = modelo.TipoEvaluacion;
            evaluacion.Technique = modelo.TecnicaEvaluacion;
            evaluacion.Instrument = modelo.InstrumentoEvaluacion;
            evaluacion.CASU = casu;
            #endregion
            #region Validación del porcentaje asociado
            int porcentaje = assessmentService
                .ObtenerTotalPorcentajeEvaluacionesPor_Lapso_Curso_Materia(periodId, courseId, subjectId);
            int porcentajeFinal = porcentaje + evaluacion.Percentage;

            if (porcentajeFinal > 100)
            {
                TempData["NuevaEvaluacionAvanzadaError"] = "No se puede agregar la evaluación debido a que su " +
                    "porcentaje excede al máximo permitido (100%). Actualmente para el curso está " +
                    "asignado el: " + porcentaje + "% del total.";
                return RedirectToAction("CrearEvaluacionAvanzada");
            }
            #endregion
            #region Validación de las fechas de la evaluación
            if (evaluacion.StartDate > evaluacion.FinishDate)
            {
                TempData["NuevaEvaluacionAvanzadaError"] = "No se puede agregar la evaluación debido" +
                    " a que la fecha de inicio es mayor a la fecha fin";
                return RedirectToAction("CrearEvaluacionAvanzada");
            }
            #endregion
            #region Agregando evaluación a la bd
            try
            {
                evaluacion.Event = eventService.CrearEventoPersonal_SinGuardar(
                        ConstantRepository.PERSONAL_EVENT_CATEGORY_NEW_ASSESSMENT, evaluacion);
                assessmentService.GuardarAssessment(evaluacion);

                TempData["NuevaEvaluacionAvanzada"] = "Se ha agregado con éxito la evaluación: '" + 
                    evaluacion.Name + " (" + evaluacion.Percentage + ")'. ";

                return RedirectToAction("CrearEvaluacionAvanzada");
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["NuevaEvaluacionAvanzadaError"] = e.Message;
                return RedirectToAction("CrearEvaluacionAvanzada");
            }
            #endregion
        }






        

        //Por revisar - Rodrigo Uzcátegui 13-05-15
        public ActionResult Evaluaciones(EvaluacionModel modelo)
        {
            ObteniendoSesion();
            CourseService _courseService = new CourseService();
            SubjectService _subjectService = new SubjectService();
            List<Course> listaCursos;
            List<Subject> listaMaterias;                      

            string idsession = (string)Session["UserId"];
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "MateriaId", "Name"); 

      
            return View(modelo);
        }
        
        [HttpGet]
        public ActionResult ModificarEvaluacionProfesor()
        {
            ObteniendoSesion();
            EvaluacionModel modelo = new EvaluacionModel();

            #region Mensajes TempData
            if (TempData["ModificadaEvaluacion"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["ModificadaEvaluacion"].ToString();
            }
            if (TempData["EvaluacionError"] != null)
            {
                ModelState.AddModelError("", TempData["EvaluacionError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorNoHayEvaluacionesAgregadas"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorNoHayEvaluacionesAgregadas"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion

            CourseService _courseService = new CourseService();
            List<Course> listaCursos = new List<Course>();
            string idsession = (string)Session["UserId"];
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            List<Subject> listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "SubjectId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            List<string> listaTipos = ConstantRepository.ACTIVITY_LIST.ToList();
            modelo.listaTiposNormal = listaTipos;
            List<string> listaTecnicas = ConstantRepository.TECHNIQUE_LIST.ToList();
            modelo.listaTecnicasNormal = listaTecnicas;
            List<string> listaInstrumentos = ConstantRepository.INSTRUMENT_LIST.ToList();
            modelo.listaInstrumentosNormal = listaInstrumentos;
            return View(modelo);
        }

        [HttpPost]
        public bool ModificarEvaluacionCreadaProfesor(string name, string percentage, string startdate,
            string finishdate, string starthour, string endhour, string activity, string technique,
            string instrument, int assessmentid, string courseId, string periodId, string subjectId)
        {
            #region Inicializando servicios
            UnitOfWork _unidad = new UnitOfWork();
            AssessmentService assessmentService = new AssessmentService(_unidad);
            CASUService _casuService = new CASUService(_unidad);
            CourseService _courseService = new CourseService(_unidad);
            UserService _userService = new UserService(_unidad);
            EventService _eventService = new EventService(_unidad);
            SessionVariablesRepository _sesssion = new SessionVariablesRepository();
            #endregion
            #region Declaración de variables
            DateTime fechaInicio = Convert.ToDateTime(startdate);
            DateTime fechaFin = Convert.ToDateTime(finishdate);
            #endregion

            Assessment evaluacion = assessmentService.ObtenerEvaluacionPor_Id(assessmentid);
            Event evento = evaluacion.Event;
/*
            #region Modificando evento asociado
            if (evaluacion.StartDate != fechaInicio)
                evaluacion.evento.StartDate = fechaInicio;
            if (evaluacion.FinishDate != fechaFin)
                evaluacion.evento.FinishDate = fechaFin;
            if (evaluacion.StartHour != starthour)
                evaluacion.evento.StartHour = starthour;
            if (evaluacion.EndHour != endhour)
                evaluacion.evento.EndHour = endhour;
            if (evaluacion.Name != name)
                evaluacion.evento.Name = name;

            #endregion
*/

            #region Definiendo nuevo Assessment
            try
            {
              
                

                evaluacion.AssessmentId = assessmentid;
                evaluacion.Name = name;
                evaluacion.Percentage = int.Parse(percentage);
                evaluacion.StartHour = starthour;
                evaluacion.EndHour = endhour;
                evaluacion.Activity = activity;
                evaluacion.Technique = technique;
                evaluacion.Instrument = instrument;
                evaluacion.Event = evento;

            #endregion





                #region modificando Assessment

                assessmentService.ModificarAssessment(evaluacion);
                TempData["ModificadaEvaluacion"] = "Se modificó correctamente la evaluación '" + evaluacion.Name + "'";
            }
            catch (Exception e)
            {
                TempData["EvaluacionError"] = e.Message;
            }

                #endregion



            return true;
        }
                
        [HttpGet]
        public ActionResult CrearEvaluacionProfesor()
        {
            ObteniendoSesion();
            EvaluacionModel modelo = new EvaluacionModel();
            CourseService _courseService = new CourseService();
            #region Mensajes TempData
            if (TempData["NuevaEvaluacion"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevaEvaluacion"].ToString();
            }
            if (TempData["NuevaEvaluacionError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevaEvaluacionError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorPorcEvaluacion"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorPorcEvaluacion"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorFechasEvaluacion"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorFechasEvaluacion"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ColegioNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ColegioNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["CursoNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["CursoNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["LapsoNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["LapsoNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["MateriaNoSeleccionada"] != null)
            {
                ModelState.AddModelError("", TempData["MateriaNoSeleccionada"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ProfesorNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ProfesorNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["EvaluacionesNoAgregadas"] != null)
            {
                ModelState.AddModelError("", TempData["EvaluacionesNoAgregadas"].ToString());
                modelo.MostrarErrores = "block";
            }

            #endregion
            List<Course> listaCursos = new List<Course>();
            string idsession = (string)Session["UserId"];
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            List<Subject> listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "SubjectId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            List<User> listaProfesores = new List<User>();
            modelo.selectListProfesores = new SelectList(listaProfesores, "UserId", "Name");
            List<string> listaTipos = ConstantRepository.ACTIVITY_LIST.ToList();
            modelo.selectListTipos = new SelectList(listaTipos, "Value");
            List<string> listaTecnicas = ConstantRepository.TECHNIQUE_LIST.ToList();
            modelo.selectListTecnicas = new SelectList(listaTecnicas, "Value");
            List<string> listaInstrumentos = ConstantRepository.INSTRUMENT_LIST.ToList();
            modelo.selectListInstrumentos = new SelectList(listaInstrumentos, "Value");
            return View(modelo);
        }

        [HttpPost]
        public bool CrearEvaluacionProfesor(List<Object> values)
        {
            SessionVariablesRepository _session = new SessionVariablesRepository();
            #region Obteniendo ids
            int schoolId = _session.SCHOOLID;
            int periodId = 0;
            int courseId = 0;
            int subjectId = 0;
            string userId = _session.USERID;

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
            #region Validación de id del lapso
            //Dentro de la lista de valores, el tercer valor debe ser el id del lapso.
            if (values.Count == 1 || values[1].ToString() == "")
            {
                TempData["LapsoNoSeleccionado"] = "Por favor seleccione un lapso.";
                return false;
            }
            else
                periodId = Convert.ToInt32(values[1]);
            #endregion
            #region Validación de id del curso
            //Dentro de la lista de valores, el segundo valor debe ser el id del curso.
            if (values.Count == 2 || values[2].ToString() == "")
            {
                TempData["CursoNoSeleccionado"] = "Por favor seleccione un curso.";
                return false;
            }
            else
                courseId = Convert.ToInt32(values[2]);
            #endregion
            #region Validación de id de la materia
            //Dentro de la lista de valores, el cuarto valor debe ser el id de la materia.
            if (values.Count == 3 || values[3].ToString() == "")
            {
                TempData["MateriaNoSeleccionada"] = "Por favor seleccione una materia.";
                return false;
            }
            else
                subjectId = Convert.ToInt32(values[3]);
            #endregion
            #region Validación de id del profesor
            //Dentro de la lista de valores, el quinto valor debe ser el id del profesor.
            if (values.Count == 4 || values[4].ToString() == "")
            {
                TempData["ProfesorNoSeleccionado"] = "Por favor seleccione un profesor.";
                return false;
            }
            else
                userId = values[4].ToString();
            #endregion
            #region Validación de evaluaciones añadidas
            if (values.Count == 5)
            {
                TempData["EvaluacionesNoAgregadas"] = "Por favor inserte al menos una evaluación.";
                return false;
            }
            #endregion
            #region Inicializando servicios
            UnitOfWork _unidad = new UnitOfWork();
            AssessmentService assessmentService = new AssessmentService(_unidad);
            CASUService _casuService = new CASUService(_unidad);
            UserService _userService = new UserService(_unidad);
            CourseService _courseService = new CourseService(_unidad);
            EventService _eventService = new EventService(_unidad);
            PeriodService _periodService = new PeriodService(_unidad);
            SubjectService _subjectService = new SubjectService(_unidad);
            SchoolYearService _schoolYearService = new SchoolYearService(_unidad);
            #endregion
            #region Ciclo de asignación de la lista de evaluaciones
            try
            {
                for (int i = 5; i <= values.Count - 1; i++)
                {
                    string[] valores = values[i].ToString().Split(',');

                    #region Creación de nueva evaluacion
                    Assessment evaluacion = new Assessment();
                    evaluacion.Name = valores[0].ToString();
                    evaluacion.Percentage = Convert.ToInt32(valores[1]);
                    evaluacion.StartDate = Convert.ToDateTime(valores[2].ToString());
                    evaluacion.FinishDate = Convert.ToDateTime(valores[3].ToString());
                    evaluacion.StartHour = null;
                    evaluacion.EndHour = null;
                    evaluacion.Activity = valores[4].ToString();

                    #region Prueba
                    if (evaluacion.Activity.Equals("Prueba"))
                    {
                        evaluacion.Technique = "Prueba";
                        evaluacion.Instrument = "Prueba escrita estructurada";
                    }
                    #endregion
                    #region Taller
                    if (evaluacion.Activity.Equals("Taller"))
                    {
                        evaluacion.Technique = "Prueba";
                        evaluacion.Instrument = "Prueba escrita estructurada";
                    }
                    #endregion
                    #region Exposición
                    if (evaluacion.Activity.Equals("Exposición"))
                    {
                        evaluacion.Technique = "Observación participante";
                        evaluacion.Instrument = "Prueba oral";
                    }
                    #endregion
                    #region Otro
                    if (evaluacion.Activity.Equals("Otro"))
                    {
                        evaluacion.Technique = "Otro";
                        evaluacion.Instrument = "Otro";
                    }
                    #endregion
                    #endregion
                    #region Obteniendo listaAssessments para validar porcentaje
                    CASU casu = _casuService.ObtenerCASUPor_Ids(courseId, periodId, subjectId, _session.USERID);
                    int porcentaje = 0;
                   
                        List<Assessment> listaEvaluacionesParaPorcentaje = casu.Assessments;
                         porcentaje = 0;

                        foreach (Assessment evaluporc in listaEvaluacionesParaPorcentaje)
                        {
                            porcentaje = porcentaje + evaluporc.Percentage;
                        }
                  
                    #endregion
                    #region Agregando evaluación a la bd
                    int porcentFinal = porcentaje + evaluacion.Percentage;
                    if (porcentFinal <= 100)
                    {
                        if (evaluacion.StartDate <= evaluacion.FinishDate)
                        {
                            try
                            {
                                evaluacion.CASU = casu;
                                assessmentService.GuardarAssessment(evaluacion);
                            }
                            catch (Exception e)
                            {
                                TempData["NuevaEvaluacionError"] = e.Message;
                                return false;
                            }
                        }
                        else
                        {
                            TempData["ErrorFechasEvaluacion"] = "No se puede agregar la evaluación debido" +
                                " a que la fecha de inicio es mayor a la fecha fin";

                            return false;
                        }
                    }
                    else
                    {
                        TempData["ErrorPorcEvaluacion"] = "No se puede agregar la evaluación debido a que su " +
                            "porcentaje excede el máximo permitido (100%)";
                        return false;
                    }
                    #endregion
                }

                TempData["NuevaEvaluacion"] = "Se agregaron correctamente las evaluaciones '";
                return true;
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["NuevaEvaluacionError"] = e.Message;
                return false;
            }
            #endregion
        }

        [HttpGet]
        public ActionResult CrearEvaluacionAvanzadaProfesor()
        {
            ObteniendoSesion();
            EvaluacionModel modelo = new EvaluacionModel();

            #region Mensajes TempData
            if (TempData["NuevaEvaluacionAvanzada"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevaEvaluacionAvanzada"].ToString();
            }
            if (TempData["NuevaEvaluacionAvanzadaError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevaEvaluacionAvanzadaError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorPorcEvaluacionAvanzada"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorPorcEvaluacionAvanzada"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ErrorFechasEvaluacionAvanzada"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorFechasEvaluacionAvanzada"].ToString());
                modelo.MostrarErrores = "block";
            }

            #endregion

            CourseService _courseService = new CourseService();
            List<Course> listaCursos = new List<Course>();
            string idsession = (string)Session["UserId"];
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            List<Subject> listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "SubjectId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            List<User> listaProfesores = new List<User>();
            modelo.selectListProfesores = new SelectList(listaProfesores, "UserId", "Name");
            List<string> listaTipos = ConstantRepository.ACTIVITY_LIST.ToList();
            modelo.selectListTipos = new SelectList(listaTipos, "Value");
            List<string> listaTecnicas = ConstantRepository.TECHNIQUE_LIST.ToList();
            modelo.selectListTecnicas = new SelectList(listaTecnicas, "Value");
            List<string> listaInstrumentos = ConstantRepository.INSTRUMENT_LIST.ToList();
            modelo.selectListInstrumentos = new SelectList(listaInstrumentos, "Value");
            return View(modelo);
        }

        [HttpPost]
        public ActionResult CrearEvaluacionAvanzadaProfesor(EvaluacionModel modelo)
        {
            SessionVariablesRepository _session = new SessionVariablesRepository();
            #region Obteniendo ids
            int courseId = modelo.idCurso;
            int subjectId = modelo.idMateria;
            string userId = _session.USERID;
            int periodId = modelo.idLapso;
            int schoolId = _session.SCHOOLID;
            #endregion
            #region Inicializando servicios
            UnitOfWork _unidad = new UnitOfWork();
            AssessmentService assessmentService = new AssessmentService(_unidad);
            CASUService _casuService = new CASUService(_unidad);
            UserService _userService = new UserService(_unidad);
            CourseService _courseService = new CourseService(_unidad);
            EventService _eventService = new EventService(_unidad);
            PeriodService _periodService = new PeriodService(_unidad);
            SubjectService _subjectService = new SubjectService(_unidad);
            SchoolYearService _schoolYearService = new SchoolYearService(_unidad);
            #endregion
            #region Definiendo nuevo Assessment
            Assessment evaluacion = new Assessment();
            evaluacion.Name = modelo.Name;
            evaluacion.Percentage = modelo.Percentage;
            evaluacion.StartDate = modelo.StartDate;
            evaluacion.FinishDate = modelo.FinishDate;
            evaluacion.StartHour = modelo.StartHour;
            evaluacion.EndHour = modelo.EndHour;
            evaluacion.Activity = modelo.TipoEvaluacion;
            evaluacion.Technique = modelo.TecnicaEvaluacion;
            evaluacion.Instrument = modelo.InstrumentoEvaluacion;
            #endregion
            #region Obteniendo listaAssessments para validar porcentaje
            CASU casu = _casuService.ObtenerCASUPor_Ids(courseId, periodId, subjectId, _session.USERID);
            List<Assessment> listaEvaluacionesParaPorcentaje = casu.Assessments;
            int porcentaje = 0;
            foreach (Assessment evaluporc in listaEvaluacionesParaPorcentaje)
            {
                porcentaje = porcentaje + evaluporc.Percentage;
            }
            #endregion
            #region agregando evaluación a la bd
            int porcentFinal = porcentaje + evaluacion.Percentage;
            if (porcentFinal <= 100)
            {
                if (evaluacion.StartDate <= evaluacion.FinishDate)
                {
                    try
                    {
                        evaluacion.CASU = casu;
                        assessmentService.GuardarAssessment(evaluacion);


                        TempData["NuevaEvaluacionAvanzada"] = "Se agregó correctamente la evaluación '" +
                            evaluacion.Name + "'";
                    }
                    catch (Exception e)
                    {
                        TempData["NuevaEvaluacionAvanzadaError"] = e.Message;
                    }
                }
                else
                {
                    TempData["ErrorFechasEvaluacionAvanzada"] = "No se puede agregar la evaluación debido a que la " +
                        "fecha de inicio es mayor a la fecha fin";

                }
            }
            else
            {
                TempData["ErrorPorcEvaluacionAvanzada"] = "No se puede agregar la evaluación debido a que su " +
                    "porcentaje excede el máximo permitido (100%)";
            }

            #endregion
            return RedirectToAction("CrearEvaluacionAvanzada");
        }
    }
}
