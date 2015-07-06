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

        #region Sección Acciones Maestras
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
                evaluacion.Event.FinishDate = fechaFin;
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
                return RedirectToAction("CrearEvaluacionAvanzada");
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
        #endregion

        #region Sección Usuario Docente
        [HttpGet]
        public ActionResult Evaluaciones()
        {
            ConfiguracionInicial(_controlador, "Evaluaciones");

            #region Declaración de variables
            EvaluacionModel modelo = new EvaluacionModel();
            CourseService courseService = new CourseService();
            List<Course> listaCursos = new List<Course>();
            #endregion
            #region Mensajes TempData
            if (TempData["Success"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["Success"].ToString();
            }
            else if (TempData["Error"] != null)
            {
                modelo.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Error"].ToString());
            }
            #endregion
            #region Obteniendo la lista de cursos del docente en sesión
            listaCursos = courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            #endregion

            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            modelo.idProfesor = _session.USERID;

            return View(modelo);
        }

        [HttpGet]
        public ActionResult AsociacionIndicadoresLiterales(int id)
        {
            ConfiguracionInicial(_controlador, "AsociacionIndicadoresLiterales");

            #region Obteniendo la evaluación respectiva
            AssessmentService assessmentService = new AssessmentService();
            Assessment assessment = assessmentService.ObtenerEvaluacionPor_Id(id);
            #endregion
            #region Inicializando el modelo con la evaluación obtenida
            MatrizIndicadoresLiteralesModel model = new MatrizIndicadoresLiteralesModel(assessment);
            #endregion
            #region Obteniendo la lista de competencias respectiva
            IndicatorService indicatorService = new IndicatorService();            
            List<Competency> listaCompetencias = indicatorService.ObteniendoListaCompetenciasPor_Evaluacion(id);
            model.selectListCompetencies = new SelectList(listaCompetencias, "CompetencyId", "Description");
            #endregion

            return View(model);
        }

        [HttpPost]
        public JsonResult AsociacionIndicadoresLiterales(List<Object> principalIds, List<Object> asignaciones)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<int> listaIds = new List<int>();
            List<int> listaAsignaciones = new List<int>();
            IndicatorService indicatorService = new IndicatorService();
            #endregion

            #region Obteniendo la lista de Ids de las asignaciones
            foreach (string bloqueIds in principalIds)
            {
                string[] bloqueSeparado = bloqueIds.Split(',');
                foreach(string idIndividual in bloqueSeparado)
                {
                    listaIds.Add(Convert.ToInt32(idIndividual));
                }
            }
            #endregion
            #region Obteniendo la lista de valores de las asignaciones
            foreach (string bloqueAsignaciones in asignaciones)
            {
                string[] bloqueSeparado = bloqueAsignaciones.Split(',');
                foreach (string asignacionIndividual in bloqueSeparado)
                {
                    listaAsignaciones.Add(Convert.ToInt32(asignacionIndividual));
                }
            }
            #endregion
            #region Guardando los IndicatorAssignations
            for (int i = 0; i <= listaIds.Count() - 1; i++ )
            {
                IndicatorAssignation IA = indicatorService.ObtenerIndicatorAssignationPor_Id(listaIds[i]);
                IA.Assignation = listaAsignaciones[i];

                try
                {
                    indicatorService.ModificarIndicatorAssignation(IA);
                    
                }
                catch(Exception e)
                {
                    TempData["Error"] = e.Message;
                    jsonResult.Add(new { Success = false });
                }
            }

            TempData["Success"] = "Se actualizaron correctamente los valores de asignación entre" +
                    " indicadores y literales.";

            jsonResult.Add(new { Success = true });
            #endregion

            return Json(jsonResult);
        }

        [HttpGet]
        public ActionResult CrearEvaluacionProfesor()
        {
            ConfiguracionInicial(_controlador, "CrearEvaluacionProfesor");

            #region Declaración de variables
            EvaluacionModel model = new EvaluacionModel();
            CourseService _courseService = new CourseService();
            #endregion
            #region Mensajes TempData
            if (TempData["NuevaEvaluacion"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["NuevaEvaluacion"].ToString();
            }
            else if (TempData["NuevaEvaluacionError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevaEvaluacionError"].ToString());
                model.MostrarErrores = "block";
            }
            else if (TempData["Error"] != null)
            {
                ModelState.AddModelError("", TempData["Error"].ToString());
                model.MostrarErrores = "block";
            }
            else if (TempData["ErrorPorcEvaluacion"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorPorcEvaluacion"].ToString());
                model.MostrarErrores = "block";
            }
            else if (TempData["ErrorFechasEvaluacion"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorFechasEvaluacion"].ToString());
                model.MostrarErrores = "block";
            }
            else if (TempData["ColegioNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ColegioNoSeleccionado"].ToString());
                model.MostrarErrores = "block";
            }
            else if (TempData["CursoNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["CursoNoSeleccionado"].ToString());
                model.MostrarErrores = "block";
            }
            else if (TempData["LapsoNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["LapsoNoSeleccionado"].ToString());
                model.MostrarErrores = "block";
            }
            else if (TempData["MateriaNoSeleccionada"] != null)
            {
                ModelState.AddModelError("", TempData["MateriaNoSeleccionada"].ToString());
                model.MostrarErrores = "block";
            }
            else if (TempData["ProfesorNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ProfesorNoSeleccionado"].ToString());
                model.MostrarErrores = "block";
            }
            else if (TempData["EvaluacionesNoAgregadas"] != null)
            {
                ModelState.AddModelError("", TempData["EvaluacionesNoAgregadas"].ToString());
                model.MostrarErrores = "block";
            }
            #endregion

            #region Obteniendo la lista de cursos
            List<Course> listaCursos = new List<Course>();
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            model.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            #endregion

            List<Subject> listaMaterias = new List<Subject>();
            model.selectListMaterias = new SelectList(listaMaterias, "SubjectId", "Name");

            List<Period> listaLapsos = new List<Period>();
            model.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");

            List<User> listaProfesores = new List<User>();
            model.selectListProfesores = new SelectList(listaProfesores, "UserId", "Name");

            List<string> listaTipos = ConstantRepository.ACTIVITY_LIST.ToList();
            model.selectListTipos = new SelectList(listaTipos, "Value");

            List<string> listaTecnicas = ConstantRepository.TECHNIQUE_LIST.ToList();
            model.selectListTecnicas = new SelectList(listaTecnicas, "Value");

            List<string> listaInstrumentos = ConstantRepository.INSTRUMENT_LIST.ToList();
            model.selectListInstrumentos = new SelectList(listaInstrumentos, "Value");

            return View(model);
        }

        [HttpPost]
        public bool CrearEvaluacionProfesor(List<Object> values)
        {
            //Si entra en [HttpGet] CrearEvaluacionProfesor(), no necesita de ConfiguracionInicial();
            ObteniendoSesion();
            #region Obteniendo ids
            int schoolId = _session.SCHOOLID;
            int periodId = 0;
            int courseId = 0;
            int subjectId = 0;
            string userId = _session.USERID;
            #endregion
            #region Validación de id del lapso
            //Dentro de la lista de valores, el primer valor debe ser el id del lapso.
            if (values.Count == 1 || values[0].ToString() == "")
            {
                TempData["LapsoNoSeleccionado"] = "Por favor seleccione un lapso.";
                return false;
            }
            else
                periodId = Convert.ToInt32(values[0]);
            #endregion
            #region Validación de id del curso
            //Dentro de la lista de valores, el segundo valor debe ser el id del curso.
            if (values.Count == 2 || values[1].ToString() == "")
            {
                TempData["CursoNoSeleccionado"] = "Por favor seleccione un curso.";
                return false;
            }
            else
                courseId = Convert.ToInt32(values[1]);
            #endregion
            #region Validación de id de la materia
            //Dentro de la lista de valores, el tercer valor debe ser el id de la materia.
            if (values.Count == 3 || values[2].ToString() == "")
            {
                TempData["MateriaNoSeleccionada"] = "Por favor seleccione una materia.";
                return false;
            }
            else
                subjectId = Convert.ToInt32(values[2]);
            #endregion
            #region Validación de evaluaciones añadidas
            if (values.Count == 3)
            {
                TempData["EvaluacionesNoAgregadas"] = "Por favor inserte al menos una evaluación.";
                return false;
            }
            #endregion
            
            #region Inicializando servicios
            UnitOfWork unidad = new UnitOfWork();
            AssessmentService assessmentService = new AssessmentService(unidad);
            CASUService casuService = new CASUService(unidad);
            EventService eventService = new EventService(unidad);
            #endregion
            #region Obteniendo el CASU respectivo
            CASU casu = casuService.ObtenerCASUPor_Ids(courseId, periodId, subjectId);
            #endregion
            #region Ciclo de asignación de la lista de evaluaciones
            try
            {
                /* A partir del valor #3, de la lista de valores (List<Object> values), empiezan las 
                 * evaluaciones agregadas*/
                for (int i = 3; i <= values.Count - 1; i++)
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
                        TempData["Error"] = "No se puede agregar la evaluación debido a que su " +
                            "porcentaje excede al máximo permitido (100%). Actualmente para el curso está " +
                            "asignado el: " + porcentaje + "% del total.";

                        return false;
                    }
                    #endregion
                    #region Validación de las fechas de la evaluación
                    if (evaluacion.StartDate > evaluacion.FinishDate)
                    {
                        TempData["Error"] = "No se puede agregar la evaluación debido a que la fecha de " + 
                            "inicio es mayor a la fecha fin";
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
                        TempData["Error"] = e.Message;
                        return false;
                    }
                    #endregion
                }

                if (values.Count - 1 > 3)
                    TempData["NuevaEvaluacion"] = "Se agregaron correctamente las evaluaciones.";
                else
                    TempData["NuevaEvaluacion"] = "Se agregó correctamente la evaluación.";

                return true;
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["Error"] = e.Message;

                return false;
            }
            #endregion
        }

        [HttpGet]
        public ActionResult ModificarEvaluacionProfesor()
        {
            ConfiguracionInicial(_controlador, "ModificarEvaluacionProfesor");
            
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
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
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
                evaluacion.Event.FinishDate = fechaFin;
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
                foreach (Notification notification in listaNotificaciones)
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

        [HttpGet]
        public ActionResult CrearEvaluacionAvanzadaProfesor()
        {
            ConfiguracionInicial(_controlador, "CrearEvaluacionAvanzadaProfesor");
            CrearEvaluacionAvanzadaModel modelo = new CrearEvaluacionAvanzadaModel();

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

            CourseService _courseService = new CourseService();
            List<Course> listaCursos = new List<Course>();
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");

            List<Subject> listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "SubjectId", "Name");

            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");

            List<string> listaTipos = ConstantRepository.ACTIVITY_LIST.ToList();
            modelo.selectListTipos = new SelectList(listaTipos, "Value");

            List<string> listaTecnicas = ConstantRepository.TECHNIQUE_LIST.ToList();
            modelo.selectListTecnicas = new SelectList(listaTecnicas, "Value");

            List<string> listaInstrumentos = ConstantRepository.INSTRUMENT_LIST.ToList();
            modelo.selectListInstrumentos = new SelectList(listaInstrumentos, "Value");

            return View(modelo);
        }

        [HttpPost]
        public ActionResult CrearEvaluacionAvanzadaProfesor(CrearEvaluacionAvanzadaModel modelo)
        {
            //Si entra en [HttpGet] CrearEvaluacionAvanzadaProfesor(), no necesita de ConfiguracionInicial();
            ObteniendoSesion();
            #region Validando el modelo
            if (!ModelState.IsValid)
            {
                TempData["ModelError"] = ModelState.Select(m => m.Value).ToList();
                return RedirectToAction("CrearEvaluacionAvanzadaProfesor");
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
            string userId = _session.USERID;
            int periodId = modelo.idLapso;
            int schoolId = _session.SCHOOLID;
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
                return RedirectToAction("CrearEvaluacionAvanzadaProfesor");
            }
            #endregion
            #region Validación de las fechas de la evaluación
            if (evaluacion.StartDate > evaluacion.FinishDate)
            {
                TempData["NuevaEvaluacionAvanzadaError"] = "No se puede agregar la evaluación debido" +
                    " a que la fecha de inicio es mayor a la fecha fin";
                return RedirectToAction("CrearEvaluacionAvanzadaProfesor");
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

                return RedirectToAction("CrearEvaluacionAvanzadaProfesor");
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["NuevaEvaluacionAvanzadaError"] = e.Message;
                return RedirectToAction("CrearEvaluacionAvanzadaProfesor");
            }
            #endregion
        }
        #endregion

        #region Acciones para todos los usuarios
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
                if (evento != null)
                    eventService.EliminarEvento(evento.EventId);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
