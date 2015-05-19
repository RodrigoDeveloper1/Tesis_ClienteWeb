using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{

    public class CalificacionesController : MaestraController
    {
        #region Declaración de variables
        private AssessmentService _assessmentService;
        private CourseService _courseService;
        private SubjectService _subjectService;
        private SchoolYearService _schoolYearService;
        private ScoreService _scoreService;
        private UserService _userService;
        private UnitOfWork _unidad;
        private NotificationService _notificationService;
        #endregion

        #region Pantalla Cargar Calificaciones



        [HttpPost]
        public bool CargarCalificaciones(int idCurso, int idLapso, int idMateria, 
                    List<Object> alumnos, List<Object> examenes, List<Object> notas)
        {
            #region Inicializando servicios
            _unidad = new UnitOfWork();
            _scoreService = new ScoreService(_unidad);
            _courseService = new CourseService(_unidad);
            _userService = new UserService (_unidad);
            _assessmentService = new AssessmentService(_unidad);
            _subjectService = new SubjectService(_unidad);
            _notificationService = new NotificationService(_unidad);
            #endregion
            Course curso = _courseService.ObtenerCursoPor_Id(idCurso);
             string idsession = (string)Session["UserId"];
            User profesor = _userService.ObtenerUsuarioPorId(idsession);
            int grado = curso.Grade;

            #region Validación de notas en blanco
            if (notas == null || notas[0].ToString() == "")
            {
                TempData["NotasEnBlanco"] = "Por favor agregue por lo menos una nota.";
                return false;
            }
            #endregion
            #region Validacion de formato de notas
            for (int i = 0; i <= alumnos.Count - 1; i++)
            {

                if (grado == 1 || grado == 2 || grado == 3 || grado == 4 || grado == 5
                    || grado == 6)
                {
                    float notaNum;
                    bool res = float.TryParse(notas[i].ToString(), out notaNum);

                    if (res == true)
                    {
                        TempData["PrimariaScoreError"] =
                            "Un curso de primaria solo acepta las siguientes notas: A,B,C,D,E. No acepta números";
                        return false;
                    }

                }
                else
                {
                    float notaNum;
                    bool res = float.TryParse(notas[i].ToString(), out notaNum);

                    if (res == false)
                    {
                        TempData["BachilleratoScoreError"] =
                            "Un curso de secundaria solo acepta números como notas";
                        return false;
                    }
                }

            }

            #endregion
            #region Ciclo de carga
            try
            {     
                for (int i = 0; i <= alumnos.Count - 1; i++)
                {
                    #region Creación de nuevo score
                    Score nota = new Score();
                    nota.StudentId = Convert.ToInt32(alumnos[i].ToString());
                    nota.AssessmentId = Convert.ToInt32(examenes[i].ToString());
                    Assessment evaluacion = _assessmentService.ObtenerEvaluacionPor_Id(nota.AssessmentId);
                    Subject materia = _subjectService.ObtenerMateriaPorId(idMateria);
                    Score notaNueva = _scoreService.ObtenerNotaPor_Alumno_Evaluacion(nota.StudentId, nota.AssessmentId);
                    if (notaNueva == null)
                    {
                        if (grado == 1 || grado == 2 || grado == 3 || grado == 4 || grado == 5
                            || grado == 6)
                        {

                            nota.NumberScore = 0;
                            nota.LetterScore = notas[i].ToString();
                            _notificationService.CrearNotificacionAutomaticaConSalvadoNotas(ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCORE, profesor, evaluacion, materia, nota.LetterScore, _unidad);

                        }
                        else
                        {

                            nota.NumberScore =float.Parse(notas[i].ToString(), CultureInfo.InvariantCulture);;
                            nota.LetterScore = "";
                            _notificationService.CrearNotificacionAutomaticaConSalvadoNotas(ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCORE, profesor, evaluacion, materia, notas[i].ToString(), _unidad);     

                        }

                    #endregion
                        #region Agregando score a la bd
                        try
                        {

                            _scoreService.GuardarScore(nota);

                        }
                        catch (Exception e)
                        {
                            TempData["NuevoScoreError"] = e.Message;
                            return false;
                        }
                        #endregion
                    }
                }
            #endregion
                TempData["NuevoScore"] = "Se agregaron correctamente las notas '";
                return true;
            }
            catch (Exception e)
            {
                TempData["NuevoScoreError"] = e.Message;
                return false;
            }
        }
        #endregion
        #region Pantalla Modificar Evaluaciones
      
        [HttpGet]
        public ActionResult ModificarCalificaciones(CalificacionesModel modelo)
        {
            ObteniendoSesion();
            #region inicializando variable
            _courseService = (_courseService == null ? new CourseService() : _courseService);
            _subjectService = (_subjectService == null ? new SubjectService() : _subjectService);
            _schoolYearService = new SchoolYearService();
            List<Course> listaCursos;
            List<Subject> listaMaterias;
            #endregion
            #region Mensajes TempData
            if (TempData["NuevoScore"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevoScore"].ToString();
            }
            if (TempData["NuevoScoreError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevoScoreError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["PrimariaScoreError"] != null)
            {
                ModelState.AddModelError("", TempData["PrimariaScoreError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["BachilleratoScoreError"] != null)
            {
                ModelState.AddModelError("", TempData["BachilleratoScoreError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["NotasEnBlanco"] != null)
            {
                ModelState.AddModelError("", TempData["NotasEnBlanco"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion
            string idsession = (string)Session["UserId"];

            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "MateriaId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            List<Student> listaAlumnos = new List<Student>();
            modelo.selectListEstudiantes = new SelectList(listaAlumnos, "StudentId", "Name");
            List<Assessment> listaEvaluaciones = new List<Assessment>();
            modelo.selectListEvaluaciones = new SelectList(listaEvaluaciones, "AssessmentId", "Name");
            return View(modelo);
        }

        [HttpPost]
        public bool ModificarCalificaciones(int idCurso, int idAlumno, int idEvaluacion,
                   string nota, int idMateria)
        {
            #region Inicializando servicios
            _unidad = new UnitOfWork();
            _scoreService = new ScoreService(_unidad);
            _courseService = new CourseService(_unidad);
            _userService = new UserService(_unidad);
            _assessmentService = new AssessmentService(_unidad);
            _subjectService = new SubjectService(_unidad);
            _notificationService = new NotificationService(_unidad);
            #endregion
            Course curso = _courseService.ObtenerCursoPor_Id(idCurso);
            string idsession = (string)Session["UserId"];
            User profesor = _userService.ObtenerUsuarioPorId(idsession);
            int grado = curso.Grade;

            #region Validación de nota en blanco
            if (nota == "")
            {
                TempData["NotasEnBlanco"] = "Por favor agregue por lo menos una nota.";
                return false;
            }
            #endregion
            #region Validacion de formato de nota
           
                if (grado == 1 || grado == 2 || grado == 3 || grado == 4 || grado == 5
                    || grado == 6)
                {
                    int notaNum;
                    bool res = int.TryParse(nota, out notaNum);

                    if (res == true)
                    {
                        TempData["PrimariaScoreError"] =
                            "Un curso de primaria solo acepta las siguientes notas: A,B,C,D,E. No acepta números";
                        return false;
                    }

                }
                else
                {
                    int notaNum;
                    bool res = int.TryParse(nota, out notaNum);

                    if (res == false)
                    {
                        TempData["BachilleratoScoreError"] =
                            "Un curso de secundaria solo acepta números como notas";
                        return false;
                    }
                }

            

            #endregion
            #region Modificación
            try
            {
               
                    #region Creación de nuevo score
                    Score nota1 = new Score();
                    nota1.StudentId = idAlumno;
                    nota1.AssessmentId = idEvaluacion;
                    Assessment evaluacion = _assessmentService.ObtenerEvaluacionPor_Id(idEvaluacion);
                    Subject materia = _subjectService.ObtenerMateriaPorId(idMateria);
                   
                    
                        if (grado == 1 || grado == 2 || grado == 3 || grado == 4 || grado == 5
                            || grado == 6)
                        {

                            nota1.NumberScore = 0;
                            nota1.LetterScore = nota;
                            _notificationService.CrearNotificacionAutomaticaConSalvadoNotas(ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_SCORE, profesor, evaluacion, materia, nota1.LetterScore, _unidad);

                        }
                        else
                        {

                            nota1.NumberScore = Convert.ToInt32(nota);
                            nota1.LetterScore = "";
                            _notificationService.CrearNotificacionAutomaticaConSalvadoNotas(ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_SCORE, profesor, evaluacion, materia, nota, _unidad);

                        }

                    #endregion
                        #region Modificando score de la bd
                        try
                        {

                            _scoreService.ModificarScore(nota1);
                        }
                        catch (Exception e)
                        {
                            TempData["NuevoScoreError"] = e.Message;
                            return false;
                        }
                        #endregion
                    
                
            #endregion
                TempData["NuevoScore"] = "Se modificó correctamente la nota '";
                return true;
            }
            catch (Exception e)
            {
                TempData["NuevoScoreError"] = e.Message;
                return false;
            }
        }

        #endregion
        #region Pantalla Gestión Calificaciones
        [HttpGet]
        public ActionResult GestionCalificaciones(CalificacionesModel modelo)
        {

            ObteniendoSesion();
            #region inicializando variable
            _courseService = (_courseService == null ? new CourseService() : _courseService);
            _subjectService = (_subjectService == null ? new SubjectService() : _subjectService);
            _schoolYearService = new SchoolYearService();
            List<Course> listaCursos;
            List<Subject> listaMaterias;
            #endregion
            #region Mensajes TempData
            if (TempData["NuevoScore"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevoScore"].ToString();
            }
            if (TempData["NuevoScoreError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevoScoreError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["PrimariaScoreError"] != null)
            {
                ModelState.AddModelError("", TempData["PrimariaScoreError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["BachilleratoScoreError"] != null)
            {
                ModelState.AddModelError("", TempData["BachilleratoScoreError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["NotasEnBlanco"] != null)
            {
                ModelState.AddModelError("", TempData["NotasEnBlanco"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion
            string idsession = (string)Session["UserId"];

            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "MateriaId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            return View(modelo);
        }
        
        #endregion
        #region Pantalla Definitivas
        [HttpGet]
        public ActionResult Definitivas(CalificacionesModel modelo)
        {
            ObteniendoSesion();
            #region inicializando variable
            _courseService = (_courseService == null ? new CourseService() : _courseService);
            _subjectService = (_subjectService == null ? new SubjectService() : _subjectService);
            _schoolYearService = new SchoolYearService();
            List<Course> listaCursos;
            List<Subject> listaMaterias;
            #endregion
            #region Mensajes TempData
          
            if (TempData["NotasEnBlanco"] != null)
            {
                ModelState.AddModelError("", TempData["NotasEnBlanco"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion
            string idsession = (string)Session["UserId"];

            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "MateriaId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            return View(modelo);
        }
      
        #endregion
        #region Otros Métodos
        [HttpPost]
        public JsonResult ObtenerNotas(int idCurso, int idLapso, int idMateria,
            List<Object> alumnos, List<Object> examenes)
        {
            _unidad = new UnitOfWork();
            _scoreService = new ScoreService(_unidad);
            _courseService = new CourseService(_unidad);
            _userService = new UserService(_unidad);
            _assessmentService = new AssessmentService(_unidad);
            _subjectService = new SubjectService(_unidad);            
            _notificationService = new NotificationService(_unidad);            
            Course curso = _courseService.ObtenerCursoPor_Id(idCurso);
            string idsession = (string)Session["UserId"];
            User profesor = _userService.ObtenerUsuarioPorId(idsession);
            int grado = curso.Grade;
            List<string> listaNotas = new List<string>();
            List<object> jsonResult = new List<object>();

            for (int i = 0; i <= alumnos.Count - 1; i++)
            {
                for (int t = 0; t <= examenes.Count - 1; t++) { 
                     Score nota = _scoreService.ObtenerNotaPor_Alumno_Evaluacion(Convert.ToInt32(alumnos[i].ToString()), 
                                    Convert.ToInt32(examenes[t].ToString()));
                     if (grado == 1 || grado == 2 || grado == 3 || grado == 4 || grado == 5
                       || grado == 6)
                     {
                         if (nota == null)
                         {
                             jsonResult.Add(new
                             {
                                 nota = " ",
                             });    
                         }
                         else
                         {
                             jsonResult.Add(new
                             {
                                 nota = nota.LetterScore.ToString(),
                             }); 
                             
                         }
                     }
                     else
                     {
                       

                         if (nota == null)
                         {
                             jsonResult.Add(new
                             {
                                 nota = " ",
                             });  
                         }
                         else
                         {
                             System.Text.StringBuilder strBuilder = new System.Text.
                                         StringBuilder(nota.NumberScore.ToString());
                             if (strBuilder.Length >= 2 && strBuilder[1] == ',')
                             {
                                 strBuilder[1] = '.';
                             }
                             else if ( strBuilder.Length>=3 && strBuilder[2] == ',')
                             {
                                 strBuilder[2] = '.';
                             }
                             
                             string str = strBuilder.ToString();

                             jsonResult.Add(new
                             {
                                 nota = str,
                             }); 
                         }
                     }
                }
            }
            
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult ObtenerCalificacionPorCursoAlumnoYExamen(int idCurso, int idAlumno, int idEvaluacion)
        {
            _unidad = new UnitOfWork();
            _scoreService = new ScoreService(_unidad);
            _courseService = new CourseService(_unidad);
            Course curso = _courseService.ObtenerCursoPor_Id(idCurso);           
            int grado = curso.Grade;
            List<object> jsonResult = new List<object>();
            Score notaAlumno = _scoreService.ObtenerNotaPor_Alumno_Evaluacion(idAlumno, idEvaluacion);

            if (grado == 1 || grado == 2 || grado == 3 || grado == 4 || grado == 5
                       || grado == 6)
            {
                
               
                    jsonResult.Add(new
                    {
                        nota = notaAlumno.LetterScore.ToString(),
                    });

            }
            else
            {
               
                    jsonResult.Add(new
                    {
                        nota = notaAlumno.NumberScore.ToString(),
                    });
                
            }
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult ObtenerCalificacionesEstadisticas_SProfesor()
        {
            ObteniendoSesion();
            _unidad = new UnitOfWork();
            _scoreService = new ScoreService(_unidad);
            List<object> jsonResult = new List<object>();
            _courseService = new CourseService(_unidad);
            Assessment evaluacion = new Assessment();
            _subjectService = new SubjectService();
            _assessmentService = new AssessmentService(_unidad);
            CASUService _casuService = new CASUService(_unidad);
            evaluacion = _assessmentService.ObtenerUltimaEvaluacionPor_Docente_AnoEscolar(_session.USERID, _session.SCHOOLYEARID);
            if (evaluacion == null)
                return Json(jsonResult);
            CASU casuEvaluacion = evaluacion.CASU;
            Subject materia = new Subject();
            materia = _subjectService.ObtenerMateriaPorId(casuEvaluacion.SubjectId);
            Course curso = _courseService.ObtenerCursoPor_Id(casuEvaluacion.CourseId);
            int grado = curso.Grade;

            if (grado == 1 || grado == 2 || grado == 3 || grado == 4 || grado == 5
                       || grado == 6)
            {

                List<Score> listaNotasAlumno = _scoreService.ObtenerNotasPor_Evaluacion(evaluacion.AssessmentId);

                foreach (Score notax in listaNotasAlumno)
                {
                    jsonResult.Add(new
                    {
                        curso = curso.Name,
                        materia = materia.Name,
                        nombreevaluacion = evaluacion.Name,
                        grado = "Primaria",
                        nota = notax.LetterScore,
                        alumnoNombre1 = notax.Student.FirstName,
                        alumnoApellido1 = notax.Student.FirstLastName + " " + notax.Student.SecondLastName
                    });
                }
            }
            else
            {
                List<Score> listaNotasAlumno = _scoreService.ObtenerNotasPor_Evaluacion(evaluacion.AssessmentId);

                foreach (Score notax in listaNotasAlumno)
                {
                    jsonResult.Add(new
                    {
                        curso = curso.Name,
                        materia = materia.Name,
                        nombreevaluacion = evaluacion.Name,
                        grado = "Bachillerato",
                        nota = notax.NumberScore,
                        alumnoNombre1 = notax.Student.FirstName,
                        alumnoApellido1 = notax.Student.FirstLastName + " " + notax.Student.SecondLastName
                    });
                }

            }
            return Json(jsonResult);
        }
        #endregion
    }
}
