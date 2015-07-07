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
        private string _controlador = "Calificaciones";
        private BridgeController _puente = new BridgeController();

        [HttpPost]
        public bool CargarCalificaciones(int idCurso, int idLapso, int idMateria, List<Object> alumnos, 
            List<Object> examenes, List<Object> notas)
        {
            ConfiguracionInicial(_controlador, "CargarCalificaciones");

            #region Declaración de servicios
            UnitOfWork unidad = new UnitOfWork();
            CourseService courseService = new CourseService(unidad);
            UserService userService = new UserService(unidad);
            AssessmentService assessmentService = new AssessmentService(unidad);
            SubjectService subjectService = new SubjectService(unidad);
            ScoreService scoreService = new ScoreService(unidad);
            NotificationService notificationService = new NotificationService(unidad);
            StudentService studentService = new StudentService(unidad);

            Assessment evaluacion = new Assessment();
            #endregion

            #region Obteniendo: curso, docente & materia
            Course curso = courseService.ObtenerCursoPor_Id(idCurso);
            int grado = curso.Grade;

            User profesor = userService.ObtenerUsuarioPorId(_session.USERID);
            Subject materia = subjectService.ObtenerMateriaPorId(idMateria);
            #endregion
            #region Validación de notas vacías
            if (notas == null || notas[0].ToString() == "")
            {
                TempData["NotasEnBlanco"] = "Por favor agregue por lo menos una nota.";
                return false;
            }
            #endregion
            #region Validación de formato de notas
            for (int i = 0; i <= alumnos.Count - 1; i++)
            {
                #region Primaria
                if (grado <= 6) //Primaria
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
                #endregion
                #region Bachillerato
                else //Bachillerato
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
                #endregion
            }
            #endregion

            #region Ciclo de carga de notas
            try
            {     
                for (int i = 0; i <= alumnos.Count - 1; i++)
                {
                    #region Creación de nueva nota
                    Score nota = new Score();
                    nota.StudentId = Convert.ToInt32(alumnos[i].ToString());
                    nota.AssessmentId = Convert.ToInt32(examenes[i].ToString());
                    #endregion

                    #region Validación de nota nueva
                    Score notaAux = scoreService.ObtenerNotaPor_Alumno_Evaluacion(nota.StudentId, nota.AssessmentId);

                    if (notaAux == null) //Nota nueva
                    {
                        #region Obteniendo la evaluación
                        evaluacion = assessmentService.ObtenerEvaluacionPor_Id(nota.AssessmentId);
                        Student student = studentService.ObtenerAlumnoPorId(nota.StudentId);
                        #endregion
                        #region Creando la notificación respectiva
                        Notification notificacion = new Notification();

                        if (grado <= 6) nota.LetterScore = notas[i].ToString();
                        else nota.NumberScore = float.Parse(notas[i].ToString(), CultureInfo.InvariantCulture); ;

                        notificacion = notificationService.CrearNotificacionAutomatica(
                                ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCORE, nota, profesor);

                        #region SentNotification
                        SentNotification SN = new SentNotification();
                        SN.Sent = true;
                        SN.Student = student;
                        #endregion

                        notificacion.SentNotifications.Add(SN);
                        #endregion
                        #region Agregando score a la bd
                        try
                        {
                            scoreService.GuardarScore(nota);
                            notificationService.GuardarNotification(notificacion);
                        }
                        catch (Exception e)
                        {
                            TempData["NuevoScoreError"] = e.Message;
                            return false;
                        }
                        #endregion
                    }
                    #endregion
                }

                #region Proceso de generado de imágenes para estadísticas móvil
                _puente.Estadisticas_Movil(evaluacion.AssessmentId);
                #endregion

                TempData["NuevoScore"] = "Se agregaron correctamente las notas.";
                return true;
            }
            #endregion
            #region Catch de los errores
            catch (Exception e)
            {
                TempData["NuevoScoreError"] = e.Message;
                return false;
            }
            #endregion
        }

        [HttpGet]
        public ActionResult GestionCalificaciones(CalificacionesModel modelo)
        {
            ConfiguracionInicial(_controlador, "GestionCalificaciones");

            #region Inicialización de variables
            CourseService courseService = new CourseService();
            SubjectService subjectService = new SubjectService();
            SchoolYearService schoolYearService = new SchoolYearService();
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

            listaCursos = courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");

            listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "MateriaId", "Name");

            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");

            return View(modelo);
        }

        [HttpGet]
        public ActionResult ModificarCalificaciones(CalificacionesModel modelo)
        {
            ConfiguracionInicial(_controlador, "ModificarCalificaciones");

            #region Declaración de variables
            CourseService _courseService = new CourseService();
            SubjectService _subjectService = new SubjectService();
            SchoolYearService _schoolYearService = new SchoolYearService();
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

            listaCursos = _courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
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
        public JsonResult ModificarCalificaciones(int idCurso, int idAlumno, int idEvaluacion, string nota, 
            int idMateria)
        {
            ObteniendoSesion();

            #region Inicialización de variables
            List<object> jsonResult = new List<object>();
            #endregion
            #region Inicializando servicios
            UnitOfWork unidad = new UnitOfWork();
            ScoreService scoreService = new ScoreService(unidad);
            CourseService courseService = new CourseService(unidad);
            UserService userService = new UserService(unidad);
            AssessmentService assessmentService = new AssessmentService(unidad);
            SubjectService subjectService = new SubjectService(unidad);
            NotificationService notificationService = new NotificationService(unidad);
            StudentService studentService = new StudentService(unidad);
            #endregion
            #region Obteniendo datos del curso & docente
            Course curso = courseService.ObtenerCursoPor_Id(idCurso);
            User profesor = userService.ObtenerUsuarioPorId(_session.USERID);            
            int grado = curso.Grade;
            #endregion
            #region Validación de nota en blanco
            if (nota == "")
            {
                TempData["NotasEnBlanco"] = "Por favor agregue por lo menos una nota.";
                jsonResult.Add(new { Success = false });
            }
            #endregion
            #region Validacion de formato de nota
            #region Primaria
            if (grado <= 6) //Primaria
            {
                if (!(nota.ToUpper().Equals("A") || nota.ToUpper().Equals("B") || nota.ToUpper().Equals("C") ||
                    nota.ToUpper().Equals("D") || nota.ToUpper().Equals("E")))
                {
                    TempData["PrimariaScoreError"] =
                        "Un curso de primaria solo acepta las siguientes notas: A,B,C,D,E. No acepta números";

                    jsonResult.Add(new { Success = false });
                    return Json(jsonResult);
                }
            }
            #endregion
            #region Bachillerato
            else //Bachillerato
            {
                int notaNum;
                bool res = int.TryParse(nota, out notaNum);

                if (res == false)
                {
                    TempData["BachilleratoScoreError"] = "Un curso de secundaria solo acepta números como notas";
                    jsonResult.Add(new { Success = false });
                    return Json(jsonResult);
                }
                else if (Convert.ToInt32(nota) <= 0 || Convert.ToInt32(nota) > 20)
                {
                    TempData["BachilleratoScoreError"] = "Un curso de secundaria solo acepta valores entre 01 y 20";
                    jsonResult.Add(new { Success = false });
                    return Json(jsonResult);
                }
            }
            #endregion
            #endregion
            #region Modificación de la nota
            try
            {
                #region Creación de nueva nota
                Score score = new Score();
                score.StudentId = idAlumno;
                score.AssessmentId = idEvaluacion;
                if (grado > 6) score.NumberScore = Convert.ToInt32(nota);
                else score.LetterScore = nota;
                #endregion
                #region Modificando score de la bd
                scoreService.ModificarScore(score);

                TempData["NuevoScore"] = "Se modificó correctamente la nota";
                jsonResult.Add(new { Success = true });
                #endregion
                #region Creando la notificación respectiva
                Assessment assessment = assessmentService.ObtenerEvaluacionPor_Id(idEvaluacion);

                Notification auxNotification = notificationService.CrearNotificacionAutomatica(
                    ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_SCORE, score, profesor);

                #region SentNotification respectivo
                Student student = studentService.ObtenerAlumnoPorId(idAlumno);

                SentNotification sentNotification = new SentNotification()
                {
                    Course = null,
                    Student = student,
                    User = null
                };

                auxNotification.SentNotifications.Add(sentNotification);
                #endregion

                notificationService.GuardarNotification(auxNotification);
                #endregion
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["NuevoScoreError"] = e.Message;
                jsonResult.Add(new { Success = false });
            }
            #endregion

            return Json(jsonResult);
        }

        [HttpGet]
        public ActionResult Definitivas(CalificacionesModel modelo)
        {
            ConfiguracionInicial(_controlador, "Definitivas");
            #region Declaración de variables
            CourseService _courseService = new CourseService();
            SubjectService _subjectService = new SubjectService();
            SchoolYearService _schoolYearService = new SchoolYearService();
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

            listaCursos = _courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");

            listaMaterias = new List<Subject>();
            modelo.selectListMaterias = new SelectList(listaMaterias, "MateriaId", "Name");

            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");

            return View(modelo);
        }


        //Por verificar - Rodrigo Uzcátegui 25-06-15

        #region Otros Métodos
        [HttpPost]
        public JsonResult ObtenerNotas(int idCurso, int idLapso, int idMateria,
            List<Object> alumnos, List<Object> examenes)
        {
            UnitOfWork _unidad = new UnitOfWork();
            ScoreService _scoreService = new ScoreService(_unidad);
            CourseService _courseService = new CourseService(_unidad);
            UserService _userService = new UserService(_unidad);
            AssessmentService _assessmentService = new AssessmentService(_unidad);
            SubjectService _subjectService = new SubjectService(_unidad);            
            NotificationService _notificationService = new NotificationService(_unidad);            
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
            UnitOfWork _unidad = new UnitOfWork();
            ScoreService _scoreService = new ScoreService(_unidad);
            CourseService _courseService = new CourseService(_unidad);
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
            UnitOfWork _unidad = new UnitOfWork();
            ScoreService _scoreService = new ScoreService(_unidad);
            List<object> jsonResult = new List<object>();
            CourseService _courseService = new CourseService(_unidad);
            Assessment evaluacion = new Assessment();
            SubjectService _subjectService = new SubjectService();
            AssessmentService _assessmentService = new AssessmentService(_unidad);
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
