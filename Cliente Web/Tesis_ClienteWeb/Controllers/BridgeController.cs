using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    public class Values
    {
        public int idEstudiante { get; set; }
        public int idMateria { get; set; }
        public double acumulado { get; set; }

        public Values(int idEstudiante, int idMateria, double acumulado)
        {
            this.idEstudiante = idEstudiante;
            this.idMateria = idMateria;
            this.acumulado = acumulado;
        }
    }

    public class BridgeController : MaestraController
    {
        public JsonResult ObtenerAnoEscolarActivoEnLabel(int idColegio)
        {
            SchoolYearService schoolYearService = new SchoolYearService();
            SchoolYear schoolYear = schoolYearService.ObtenerAnoEscolarActivoPorColegio(idColegio);
            List<object> jsonResult = new List<object>();

            if (schoolYear == null)
                jsonResult.Add(new { success = false });
            else
                jsonResult.Add(new { 
                    success = true,
                    idAnoEscolar = schoolYear.SchoolYearId,
                    label = schoolYear.StartDate.ToShortDateString() + " - " + 
                        schoolYear.EndDate.ToShortDateString()
                });

            return Json(jsonResult);
        }
        public JsonResult ObtenerListaNombresDeCursosPorAnoEscolar(int idAnoEscolar)
        {
            CourseService courseService = new CourseService();
            List<Course> listaCursos = courseService.ObtenerListaCursosPorAnoEscolar(idAnoEscolar).ToList<Course>();
            List<object> jsonResult = new List<object>();

            if (listaCursos.Count == 0)
                jsonResult.Add(new { success = false });

            else
            {
                foreach (Course curso in listaCursos)
                {
                    jsonResult.Add(new { 
                        success = true,
                        nombreCurso = curso.Name,
                        idCurso = curso.CourseId
                    });
                }
            }

            return Json(jsonResult);
        }
        public JsonResult ObtenerListaNombresDeCursosPorAnoEscolarSinEstudiantes(int idAnoEscolar)
        {
            CourseService courseService = new CourseService();
            List<Course> listaCursos = courseService.ObtenerListaCursosPorAnoEscolarSinEstudiantes(idAnoEscolar);
            List<object> jsonResult = new List<object>();

            if (listaCursos.Count == 0)
                jsonResult.Add(new { success = false });

            else
            {
                foreach (Course curso in listaCursos)
                {
                    jsonResult.Add(new
                    {
                        success = true,
                        nombreCurso = curso.Name,
                        idCurso = curso.CourseId
                    });
                }
            }

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonEstudiantesPorCurso(int idCurso)
        {
            StudentService service = new StudentService();
            List<Student> listaEstudiantes = service.ObtenerListaEstudiantePorCurso(idCurso);
            List<object> jsonResult = new List<object>();

            if (listaEstudiantes.Count == 0)
                jsonResult.Add(new { success = false });

            else
            {
                foreach (Student student in listaEstudiantes)
                {
                    jsonResult.Add(new
                    {
                        success = true,
                        studentId = student.StudentId,
                        numLista = (student.NumberList == 0 ? "" : student.NumberList.ToString()),
                        matricula = (student.RegistrationNumber == 0 ? "" : student.RegistrationNumber.ToString()),
                        apellido1 = student.FirstLastName,
                        apellido2 = student.SecondLastName,
                        nombre1 = student.FirstName,
                        nombre2 = (student.SecondName == null ? "" : student.SecondName)
                    });
                }
            }

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonEstudiantesYRepresentantesPorCurso(int idCurso)
        {
            #region Declaración de variables
            List<Student> listaEstudiantes = new List<Student>();
            List<object> jsonResult = new List<object>();
            StudentService studentService = new StudentService();
            string representante1;
            string representante2;
            #endregion

            listaEstudiantes = studentService.ObtenerListaEstudiantePorCurso(idCurso);

            if (listaEstudiantes.Count == 0)
                jsonResult.Add(new { success = false });

            #region Transformando lista de estudiantes a JsonResult
            foreach (Student estudiante in listaEstudiantes)
            {
                #region Obteniendo representantes
                if (estudiante.Representatives.Count == 2)
                {
                    representante1 = estudiante.Representatives[0].Name + " " +
                                     estudiante.Representatives[0].LastName + " " +
                                     estudiante.Representatives[0].SecondLastName;

                    representante2 = estudiante.Representatives[1].Name + " " +
                                     estudiante.Representatives[1].LastName + " " +
                                     estudiante.Representatives[1].SecondLastName;
                }
                else if (estudiante.Representatives.Count == 1)
                {
                    representante1 = estudiante.Representatives[0].Name + " " +
                                     estudiante.Representatives[0].LastName + " " +
                                     estudiante.Representatives[0].SecondLastName;

                    representante2 = "N/A";
                }
                else
                {
                    representante1 = "N/A";
                    representante2 = "N/A";
                }
                #endregion

                jsonResult.Add(new
                {
                    success = true,
                    nroLista = estudiante.NumberList,
                    apellido = estudiante.FirstLastName + ' ' + estudiante.SecondLastName,
                    nombre = estudiante.FirstName + ' ' + estudiante.SecondName,
                    representante1 = representante1,
                    representante2 = representante2,
                    idEstudiante = estudiante.StudentId
                });
            }
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonInfoRepresentantes(int idEstudiante)
        {
            #region Declaración de variables
            StudentService service = new StudentService();
            Student estudiante = service.ObtenerAlumnoPorId(idEstudiante);
            List<object> jsonResult = new List<object>();
            #endregion

            #region Validación -> Estudiante vacío
            if (estudiante == null)
                jsonResult.Add(new { success = false });
            #endregion
            #region Validación -> Estudiante sin representantes
            else if (estudiante.Representatives.Count == 0) //No tiene representantes
            {
                jsonResult.Add(new
                {
                    #region Banderas de validación
                    success = true,
                    poseeRepresentantes = false
                    #endregion
                });
            }
            #endregion
            #region Validación -> Estudiante con un solo representante
            else if (estudiante.Representatives.Count == 1) //Posee solo un representante
            {
                jsonResult.Add(new
                {
                    #region Banderas de validación
                    success = true,
                    poseeRepresentantes = true,
                    poseeRepresentante_1 = true,
                    poseeRepresentante_2 = false,
                    #endregion
                    #region Representante #1
                    representante1_id = estudiante.Representatives[0].RepresentativeId,
                    representante1_cedula = estudiante.Representatives[0].IdentityNumber,
                    representante1_sexo = (estudiante.Representatives[0].Gender ? 1 : 0), /* 1: Masculino
                                                                                           * 2: Femenino
                                                                                           */
                    representante1_nombre = estudiante.Representatives[0].Name,
                    representante1_apellido1 = estudiante.Representatives[0].LastName,
                    representante1_apellido2 = estudiante.Representatives[0].SecondLastName,
                    representante1_correo = estudiante.Representatives[0].Email
                    #endregion
                });
            }
            #endregion
            #region Validación -> Estudiante con los dos representantes
            else
            {
                jsonResult.Add(new
                {
                    #region Banderas de validación
                    success = true,
                    poseeRepresentantes = true,
                    poseeRepresentante_1 = true,
                    poseeRepresentante_2 = true,
                    #endregion
                    #region Representante #1
                    representante1_id = estudiante.Representatives[0].RepresentativeId,
                    representante1_cedula = estudiante.Representatives[0].IdentityNumber,
                    representante1_sexo = (estudiante.Representatives[0].Gender ? 1 : 0), /* 1: Masculino
                                                                                           * 2: Femenino
                                                                                           */
                    representante1_nombre = estudiante.Representatives[0].Name,
                    representante1_apellido1 = estudiante.Representatives[0].LastName,
                    representante1_apellido2 = estudiante.Representatives[0].SecondLastName,
                    representante1_correo = estudiante.Representatives[0].Email,
                    #endregion
                    #region Representante #2
                    representante2_id = estudiante.Representatives[1].RepresentativeId,
                    representante2_cedula = estudiante.Representatives[1].IdentityNumber,
                    representante2_sexo = (estudiante.Representatives[1].Gender ? 1 : 0), /* 1: Masculino
                                                                                           * 2: Femenino
                                                                                           */
                    representante2_nombre = estudiante.Representatives[1].Name,
                    representante2_apellido1 = estudiante.Representatives[1].LastName,
                    representante2_apellido2 = estudiante.Representatives[1].SecondLastName,
                    representante2_correo = estudiante.Representatives[1].Email
                    #endregion
                });
            }
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonNotificacionesPersonalizadasPorColegio(int idColegio, int idAnoEscolar)
        {
            #region Declaración de variables
            NotificationService notificationService = new NotificationService();
            SchoolYearService schoolYearService = new SchoolYearService();
            RoleService roleService = new RoleService();
            CourseService courseService = new CourseService();

            List<Notification> listaNotificaciones = new List<Notification>();
            SchoolYear schoolYear = new SchoolYear();
            List<object> jsonResult = new List<object>();

            Role rol;
            Course curso;
            DateTime fechaMinima;
            DateTime fechaMaxima;

            string sujeto = "";
            string cursoString = "";
            #endregion

            #region Obteniendo el año escolar & rango de fechas
            schoolYear = schoolYearService.ObtenerAnoEscolar(idAnoEscolar);
            fechaMinima = schoolYear.StartDate;
            fechaMaxima = schoolYear.EndDate;
            #endregion
            #region Obtener lista de notificaciones
            listaNotificaciones = notificationService.ObtenerListaNotificacionesPersonalesPor_Colegio(idColegio, 
                fechaMinima, fechaMaxima);
            #endregion            
            #region Creando objeto Json
            #region No existen notificaciones
            if (listaNotificaciones.Count == 0)
                jsonResult.Add(new { success = false });
            #endregion
            #region Si existen notificaciones
            else
            {
                foreach (Notification notificacion in listaNotificaciones)
                {                    
                    #region Identificando el sujeto
                    foreach (SentNotification SN in notificacion.SentNotifications)
                    {
                        #region Sujeto -> Representante (Seleccionado desde el estudiante)
                        if(SN.Student != null)
                        {
                            sujeto = "Rep. - Almno: " + SN.Student.FirstName + " " + SN.Student.FirstLastName;

                            #region Obteniendo el curso
                            curso = courseService.ObtenerCursoPor_Estudiante_AnoEscolar(SN.Student.StudentId, idAnoEscolar);
                            cursoString = curso.Name;
                            #endregion
                        }
                        #endregion
                        #region Sujeto -> Curso
                        else if (SN.Course != null)
                        {
                            sujeto = "Todo el " + SN.Course.Name;
                            cursoString = SN.Course.Name;
                        }
                        #endregion
                        #region Sujeto -> Usuario
                        else if (SN.User != null)
                        {
                            rol = roleService.ObtenerRolPorId(SN.User.Roles.FirstOrDefault().RoleId);
                            sujeto = rol.Name + " - " + SN.User.UserName;

                            #region Obteniendo el curso
                            curso = notificacion.Attribution == "N/A" ? null :
                                courseService.ObtenerCursoPor_Docente_Notificacion_AnoEscolar(SN.User.Id, notificacion,
                                idAnoEscolar);
                            cursoString = (curso == null ? "N/A" : curso.Name);
                            #endregion
                        }
                        #endregion

                        #region Obteniendo el rol de quien creó la notificación
                        rol = roleService.ObtenerRolPorId(notificacion.User.Roles.FirstOrDefault().RoleId);
                        #endregion
                        #region Añadiendo notificación a la lista de objetos
                        jsonResult.Add(new
                        {
                            success = true,
                            idNotification = notificacion.NotificationId,
                            mensaje = notificacion.Message,
                            fechaEnvio = notificacion.SendDate.ToShortDateString(),
                            nombreUsuario = rol.Name + " - " + notificacion.User.UserName,
                            atribucion = notificacion.Attribution,
                            tipoAlerta = notificacion.AlertType,
                            sujeto = sujeto,
                            curso = cursoString
                        });
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonNotificacionesaAutomaticasPorColegio(int idColegio, int idAnoEscolar)
        {
            #region Declaración de variables
            NotificationService notificationService = new NotificationService();
            SchoolYearService schoolYearService = new SchoolYearService();
            RoleService roleService = new RoleService();
            CourseService courseService = new CourseService();

            List<Notification> listaNotificaciones = new List<Notification>();
            SchoolYear schoolYear = new SchoolYear();
            List<object> jsonResult = new List<object>();

            DateTime fechaMinima;
            DateTime fechaMaxima;
            #endregion

            #region Obteniendo el año escolar & rango de fechas
            schoolYear = schoolYearService.ObtenerAnoEscolar(idAnoEscolar);
            fechaMinima = schoolYear.StartDate;
            fechaMaxima = schoolYear.EndDate;
            #endregion
            #region Obtener lista de notificaciones
            listaNotificaciones = notificationService.ObtenerListaNotificacionesAutomaticasPor_Colegio(idColegio,
                fechaMinima, fechaMaxima);
            #endregion            
            #region Creando objeto Json
            #region No existen notificaciones
            if (listaNotificaciones.Count == 0)
                jsonResult.Add(new { success = false });
            #endregion
            #region Si existen notificaciones
            else
            {
                foreach (Notification notificacion in listaNotificaciones)
                {
                    jsonResult.Add(new {
                        success = true,
                        tipoAlerta = notificacion.AlertType,
                        atribucion = notificacion.Attribution,
                        fechaEnvio = notificacion.SendDate.ToShortDateString(),
                        mensaje = notificacion.Message,
                    });
                }
            }
            #endregion
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerSujetosDeUsuario()
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<User> listaDocentes = new List<User>();

            UserService userService = new UserService();
            #endregion

            #region Obteniendo la lista de docentes
            listaDocentes = userService.ObtenerListaDocentesAsociadosAlCoordinador();
            #endregion
            #region Validación -> Lista vacía
            if(listaDocentes.Count == 0)
                jsonResult.Add(new { success = false });
            #endregion
            #region Creando objeto JSON
            foreach (User docente in listaDocentes)
            {
                jsonResult.Add(new
                {
                    success = true,
                    nombre = docente.UserName,
                    idDocente = docente.Id
                });
            }
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerSujetosDeRepresentante(int idCurso)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<Student> lista = new List<Student>();
            StudentService service = new StudentService();
            #endregion

            #region Obteniendo la lista de estudiantes con representantes
            lista = service.ObtenerListaEstudiantePorCurso(idCurso);
            #endregion
            #region Validación -> Lista vacía
            if (lista.Count == 0)
                jsonResult.Add(new { success = false });
            #endregion
            #region Creando el objeto JSON
            //Primer elemento - Todos los representantes
            jsonResult.Add(new
            {
                success = true,
                nombre = "Todos los representantes",
                idDocente = "Todos los representantes"
            });

            foreach (Student student in lista)
            {
                jsonResult.Add(new
                {
                    success = true,
                    nombre = "Rep. - Almno: " + student.FirstLastName + ", " + student.FirstName,
                    idDocente = student.StudentId
                });
            }
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ListaTiposNotificacion_CoordinadorRepresentante()
        {
            List<object> jsonResult = new List<object>();            

            foreach (string tipoAlerta in ConstantRepository.NOTIFICATION_ALERT_TYPE_COORDINATOR_REPRESENTATIVE)
            {
                jsonResult.Add(new
                {
                    tipo = tipoAlerta
                });
            }

            return Json(jsonResult);
        }
        public JsonResult ListaTiposNotificacion_CoordinadorCurso()
        {
            List<object> jsonResult = new List<object>();

            foreach (string tipoAlerta in ConstantRepository.NOTIFICATION_ALERT_TYPE_COORDINATOR_COURSE)
            {
                jsonResult.Add(new
                {
                    success = true,
                    tipo = tipoAlerta
                });
            }

            return Json(jsonResult);
        }
        public JsonResult ListaTiposNotificacion_CoordinadorDocente()
        {
            List<object> jsonResult = new List<object>();

            foreach (string tipoAlerta in ConstantRepository.NOTIFICATION_ALERT_TYPE_COORDINATOR_TEACHER)
            {
                jsonResult.Add(new
                {
                    success = true,
                    tipo = tipoAlerta
                });
            }

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonListaEventosPorDia(string fecha, string idUsuario)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<Event> listaEventos = new List<Event>();
            EventService service = new EventService();
            DateTime dateTime = Convert.ToDateTime(fecha);
            #endregion

            #region Obteniendo la lista de eventos
            listaEventos = service.ObtenerListaEventosPor_SUsuario(dateTime);
            #endregion
            #region Creando el objeto JSON
            foreach (Event evento in listaEventos)
            {
                jsonResult.Add(new
                {
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
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonListaEventosPorDia_Maestra(int idColegio, string fecha)
        {
            #region Declaración de variables
            UnitOfWork unidad = new UnitOfWork();
            EventService eventService = new EventService(unidad);
            SchoolYearService schoolYearService = new SchoolYearService(unidad);

            List<object> jsonResult = new List<object>();
            List<Event> listaEventos = new List<Event>();

            DateTime dateTime = Convert.ToDateTime(fecha);
            SchoolYear schoolYear = new SchoolYear();
            #endregion

            #region Obteniendo el año escolar
            schoolYear = schoolYearService.ObtenerAnoEscolarActivoPorColegio(idColegio);
            #endregion
            #region Obteniendo la lista de eventos
            listaEventos = schoolYear.Events.Where(m => m.DeleteEvent == false &&
                                                      ((m.StartDate.Month == dateTime.Month &&
                                                        m.StartDate.Year == dateTime.Year) ||
                                                       (m.FinishDate.Month == dateTime.Month &&
                                                        m.StartDate.Month == dateTime.Year))).ToList();

            /* Filtro para eliminar repetidos, sin embargo no deberían eliminarse ya que unos eventos globales 
             * son para primaria, y otros para bachillerato. Rodrigo Uzcátegui - 28-03-15
             */
            //listaEventos = listaEventos.GroupBy(m => m.Name).Select(m => m.First()).ToList<Event>();
            
            #endregion
            #region Creando el objeto JSON
            foreach (Event evento in listaEventos)
            {
                jsonResult.Add(new
                {
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
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonListaEventosDeHoy()
        {
            ObteniendoSesion();

            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<Event> listaEventos = new List<Event>();
            EventService service = new EventService();
            DateTime dateTime = DateTime.Now;
            #endregion
            #region Obteniendo la lista de eventos
            listaEventos = service.ObtenerListaEventosPor_Usuario(_session.USERID, dateTime);
            #endregion
            #region Creando el objeto JSON
            foreach (Event evento in listaEventos)
            {
                jsonResult.Add(new
                {
                    Success = true,
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
            #endregion
            #region Validación de lista de eventos vacía
            if (jsonResult.Count == 0)
                jsonResult.Add(new { Success = false });
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonListaEventosDeHoy_Maestra(int idColegio)
        {
            #region Declaración de variables
            UnitOfWork unidad = new UnitOfWork();
            EventService eventService = new EventService(unidad);
            SchoolYearService schoolYearService = new SchoolYearService(unidad);
            
            List<object> jsonResult = new List<object>();
            List<Event> listaEventos = new List<Event>();
            
            DateTime dateTime = DateTime.Now;
            SchoolYear schoolYear = new SchoolYear();
            #endregion

            #region Obteniendo el año escolar
            schoolYear = schoolYearService.ObtenerAnoEscolarActivoPorColegio(idColegio);
            #endregion
            #region Obteniendo la lista de eventos
            listaEventos = schoolYear.Events.Where(m => m.DeleteEvent == false && 
                                                      ((m.StartDate.Month == dateTime.Month &&
                                                        m.StartDate.Year == dateTime.Year) || 
                                                       (m.FinishDate.Month == dateTime.Month &&
                                                        m.StartDate.Month == dateTime.Year))).ToList();
            #endregion
            #region Creando el objeto JSON
            foreach (Event evento in listaEventos)
            {
                jsonResult.Add(new
                {
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
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonMateriasYDocentes(int idLapso, int idCurso)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<Subject> listaMaterias = new List<Subject>();
            List<User> listaDocentes = new List<User>();

            UnitOfWork unidad = new UnitOfWork();
            SubjectService subjectService = new SubjectService(unidad);
            UserService userService = new UserService(unidad);
            #endregion

            #region Obteniendo la lista de materias
            listaMaterias = subjectService.ObtenerListaMateriasConDocentesPor_Lapso_Curso(idLapso, idCurso);
            #endregion
            #region Creando el objeto JSON
            #region Validación lista de materias vacía
            if (listaMaterias.Count == 0)
            {
                jsonResult.Add(new
                {
                    success = false
                });
            }
            #endregion
            #region Llenando la lista de materias
            else
            {
                foreach (Subject materia in listaMaterias)
                {
                    listaDocentes = userService
                        .ObtenerListaDocentesPor_Materia_Curso_Periodo(materia.SubjectId, idCurso, idLapso);

                    foreach(User docente in listaDocentes)
                    {
                        jsonResult.Add(new
                        {
                            success = true,
                            materia = materia.Name,
                            docente = docente.Name + " " + docente.LastName,
                            idMateria = materia.SubjectId,
                            idDocente = docente.Id
                        });
                    }
                }
            }
            #endregion
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonMaterias(int idColegio, int idCurso)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<Subject> listaMaterias = new List<Subject>();

            UnitOfWork unidad = new UnitOfWork();
            SubjectService subjectService = new SubjectService(unidad);
            CASUService casuService = new CASUService(unidad);
            CourseService courseService = new CourseService(unidad);
            #endregion

            #region Obteniendo el curso respectivo
            Course course = courseService.ObtenerCursoPor_Id(idCurso);
            #endregion
            #region Obteniendo la lista de materias
            listaMaterias = subjectService.ObtenerListaMateriasCASUPor_Colegio_Grado(idColegio, course.Grade, idCurso);
            #endregion
            #region Creando el objeto JSON
            #region Validación lista de materias vacía
            if (listaMaterias.Count == 0)
            {
                jsonResult.Add(new
                {
                    success = false
                });
            }
            #endregion
            #region Llenando la lista de materias
            else
            {
                foreach (Subject materia in listaMaterias)
                {
                    jsonResult.Add(new
                    {
                        success = true,
                        materia = materia.Name,
                        idMateria = materia.SubjectId,
                    });
                }
            }
            #endregion
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerJsonDocentes(int idColegio)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<User> listaDocentes = new List<User>();

            UnitOfWork unidad = new UnitOfWork();
            UserService userService = new UserService(unidad);
            #endregion

            #region Obteniendo la lista de docentes
            listaDocentes = userService.ObtenerListaDocentesPor_Colegio(idColegio);
            #endregion
            #region Creando el objeto JSON
            #region Validación lista de materias vacía
            if (listaDocentes.Count == 0)
            {
                jsonResult.Add(new
                {
                    success = false
                });
            }
            #endregion
            #region Llenando la lista de materias
            else
            {
                foreach (User docente in listaDocentes)
                {
                    jsonResult.Add(new
                    {
                        success = true,
                        docente = docente.LastName + ", " + docente.Name,
                        idDocente = docente.Id,
                    });
                }
            }
            #endregion
            #endregion

            return Json(jsonResult);
        }        
        public JsonResult ObteniendoPorcentajeNotasPor_Evaluacion(int idEvaluacion)
        {
            List<object> jsonResult = new List<object>();
            AssessmentService assessmentService = new AssessmentService();

            Assessment assessment = assessmentService.ObtenerEvaluacionPor_Id(idEvaluacion);
            int nroNotas = assessment.Scores.Count();
            Course course = assessment.CASU.Course;

            double valorPorcentual = (double)(nroNotas * 100) / course.Students.Count();

            jsonResult.Add(new { 
                success = true,
                porcentaje = Math.Round(valorPorcentual, 2) 
            });

            return Json(jsonResult);
        }
        public JsonResult ObtenerNotaDefinitivaPor_Materia(int idCurso, int idMateria)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<Student> listaEstudiantes = new List<Student>();
            List<Assessment> listaEvaluaciones = new List<Assessment>();

            Course curso = new Course();
            Score notaAux = new Score();
            int totalNotas = 0;
            float definitiva1 = 0;
            float definitiva2 = 0;
            float definitiva3 = 0;
            double definitivaFinal1 = 0;
            double definitivaFinal2 = 0;
            double definitivaFinal3 = 0;
            double definitivaFinalBachillerato1 = 0;
            double definitivaFinalBachillerato2 = 0;
            double definitivaFinalBachillerato3 = 0;
            float porcentaje = 0;
            string definitivaPrimaria1 = "";
            string definitivaPrimaria2 = "";
            string definitivaPrimaria3 = "";
            double definitivaFinalMateria = 0;
            double definitivaFinalMateriaPrimaria = 0;
            string finalMateriaPrimaria = "";
            string color = "";
            string colorfuente = "";
            #endregion
            #region Declaración de servicios
            UnitOfWork unidad = new UnitOfWork();
            StudentService studentService = new StudentService(unidad);
            AssessmentService assessmentService = new AssessmentService(unidad);
            CourseService courseService = new CourseService(unidad);
            #endregion
            
            #region Obteniendo la lista de estudiantes por curso
            listaEstudiantes = studentService.ObtenerListaEstudiantePorCurso(idCurso);
            #endregion
            #region Definiendo el grado: Primaria o Bachillerato
            curso = courseService.ObtenerCursoPor_Id(idCurso);
            bool primaria = (curso.Grade <= 6 ? true : false);
            #endregion

            foreach (Student estudiante in listaEstudiantes)
            {
                #region 1er Lapso
                listaEvaluaciones = assessmentService.ObtenerListaEvaluacionesPor_Curso_Materia_Lapso(idCurso,
                    idMateria, ConstantRepository.PERIOD_ONE);

                foreach (Assessment evaluacion in listaEvaluaciones)
                {
                    notaAux = evaluacion.Scores.Where(m => m.StudentId == estudiante.StudentId).FirstOrDefault<Score>();

                    if (primaria)
                    {
                        if (notaAux.LetterScore.Equals("A")) definitiva1 += 5;
                        else if (notaAux.LetterScore.Equals("B")) definitiva1 += 4;
                        else if (notaAux.LetterScore.Equals("C")) definitiva1 += 3;
                        else if (notaAux.LetterScore.Equals("D")) definitiva1 += 2;
                        else if (notaAux.LetterScore.Equals("E")) definitiva1 += 1;
                    }
                    else
                    {
                        porcentaje = (float)evaluacion.Percentage / 100;
                        if (notaAux == null)
                            definitiva1 += 0;
                        else
                            definitiva1 += (notaAux.NumberScore * porcentaje);
                    }
                }


                definitivaFinal1 = Math.Round(definitiva1 / listaEvaluaciones.Count);

                if (primaria)
                {
                    if (definitivaFinal1 <= 5 &&
               definitivaFinal1 > 4)
                        definitivaPrimaria1 = "A";
                    else if (definitivaFinal1 <= 4 &&
               definitivaFinal1 > 3)
                        definitivaPrimaria1 = "B";
                    else if (definitivaFinal1 <= 3 &&
               definitivaFinal1 > 2)
                        definitivaPrimaria1 = "C";
                    else if (definitivaFinal1 <= 2 &&
               definitivaFinal1 > 1)
                        definitivaPrimaria1 = "D";
                    else if (definitivaFinal1 <= 1)
                        definitivaPrimaria1 = "E";
                }
                else
                {
                    definitivaFinalBachillerato1 = Math.Round(definitiva1 / listaEvaluaciones.Count());
                }

                notaAux = new Score();
                #endregion
                #region 2do Lapso
                listaEvaluaciones = assessmentService.ObtenerListaEvaluacionesPor_Curso_Materia_Lapso(idCurso,
                    idMateria, ConstantRepository.PERIOD_TWO);

                foreach (Assessment evaluacion in listaEvaluaciones)
                {

                    notaAux = evaluacion.Scores.Where(m => m.StudentId == estudiante.StudentId).FirstOrDefault<Score>();


                    if (primaria)
                    {
                        if (notaAux.LetterScore.Equals("A")) definitiva2 += 5;
                        else if (notaAux.LetterScore.Equals("B")) definitiva2 += 4;
                        else if (notaAux.LetterScore.Equals("C")) definitiva2 += 3;
                        else if (notaAux.LetterScore.Equals("D")) definitiva2 += 2;
                        else if (notaAux.LetterScore.Equals("E")) definitiva2 += 1;
                    }
                    else
                    {
                        porcentaje = (float)evaluacion.Percentage / 100;
                        if (notaAux == null)
                            definitiva2 += 0;
                        else
                            definitiva2 += (notaAux.NumberScore * porcentaje);
                    }
                }

                definitivaFinal2 = Math.Round(definitiva2 / listaEvaluaciones.Count);
                if (primaria)
                {
                    if (definitivaFinal2 <= 5 &&
                   definitivaFinal2 > 4)
                        definitivaPrimaria2 = "A";
                    else if (definitivaFinal2 <= 4 &&
                   definitivaFinal2 > 3)
                        definitivaPrimaria2 = "B";
                    else if (definitivaFinal2 <= 3 &&
                   definitivaFinal2 > 2)
                        definitivaPrimaria2 = "C";
                    else if (definitivaFinal2 <= 2 &&
                   definitivaFinal2 > 1)
                        definitivaPrimaria2 = "D";
                    else if (definitivaFinal2 <= 1)
                        definitivaPrimaria2 = "E";
                }
                else
                {
                    definitivaFinalBachillerato2 = Math.Round(definitiva2 / listaEvaluaciones.Count());
                }


                notaAux = new Score();
                #endregion
                #region 3er Lapso
                listaEvaluaciones = assessmentService.ObtenerListaEvaluacionesPor_Curso_Materia_Lapso(idCurso,
                    idMateria, ConstantRepository.PERIOD_THREE);

                foreach (Assessment evaluacion in listaEvaluaciones)
                {
                    notaAux = evaluacion.Scores.Where(m => m.StudentId == estudiante.StudentId).FirstOrDefault<Score>();

                    if (primaria)
                    {
                        if (notaAux.LetterScore.Equals("A")) definitiva3 += 5;
                        else if (notaAux.LetterScore.Equals("B")) definitiva3 += 4;
                        else if (notaAux.LetterScore.Equals("C")) definitiva3 += 3;
                        else if (notaAux.LetterScore.Equals("D")) definitiva3 += 2;
                        else if (notaAux.LetterScore.Equals("E")) definitiva3 += 1;
                    }
                    else
                        porcentaje = (float)evaluacion.Percentage / 100;
                    definitiva3 += (notaAux.NumberScore * porcentaje);
                }

                definitivaFinal3 = Math.Round(definitiva3 / listaEvaluaciones.Count);
                if (primaria)
                {
                    if (definitivaFinal3 <= 5 &&
                   definitivaFinal3 > 4)
                        definitivaPrimaria3 = "A";
                    else if (definitivaFinal3 <= 4 &&
                   definitivaFinal3 > 3)
                        definitivaPrimaria3 = "B";
                    else if (definitivaFinal3 <= 3 &&
                   definitivaFinal3 > 2)
                        definitivaPrimaria3 = "C";
                    else if (definitivaFinal3 <= 2 &&
                   definitivaFinal3 > 1)
                        definitivaPrimaria3 = "D";
                    else if (definitivaFinal3 <= 1)
                        definitivaPrimaria3 = "E";

                }
                else
                {
                    definitivaFinalBachillerato3 = Math.Round(definitiva3 / listaEvaluaciones.Count());
                }



                definitivaFinalMateria = definitivaFinal1 + definitivaFinal2 + definitivaFinal3;
                definitivaFinalMateriaPrimaria = Math.Round(definitivaFinalMateria / 3);

                if (primaria)
                {
                    if (definitivaFinalMateriaPrimaria <= 5 &&
                   definitivaFinalMateriaPrimaria > 4)
                        finalMateriaPrimaria = "A";
                    else if (definitivaFinalMateriaPrimaria <= 4 &&
                   definitivaFinalMateriaPrimaria > 3)
                        finalMateriaPrimaria = "B";
                    else if (definitivaFinalMateriaPrimaria <= 3 &&
                   definitivaFinalMateriaPrimaria > 2)
                        finalMateriaPrimaria = "C";
                    else if (definitivaFinalMateriaPrimaria <= 2 &&
                   definitivaFinalMateriaPrimaria > 1)
                        finalMateriaPrimaria = "D";
                    else if (definitivaFinalMateriaPrimaria <= 1)
                        finalMateriaPrimaria = "E";

                    if (finalMateriaPrimaria != "E")
                    {
                        color = "#CEC";
                        colorfuente = "green";
                    }
                    else
                    {
                        color = "#f9998e";
                        colorfuente = "red";
                    }
                }
                else
                {
                    definitivaFinalMateria = Math.Round(definitiva1) + Math.Round(definitiva2) +
                        Math.Round(definitiva3);

                    definitivaFinalMateria = Math.Round(definitivaFinalMateria / 3);

                    if (definitivaFinalMateria >= 10)
                    {
                        color = "#CEC";
                        colorfuente = "green";
                    }
                    else
                    {
                        color = "#f9998e";
                        colorfuente = "red";
                    }
                }

                jsonResult.Add(new
                {
                    success = true,
                    idEstudiante = estudiante.StudentId,
                    numLista = estudiante.NumberList,
                    alumnoNombre = estudiante.FirstName,
                    alumnoApellido = estudiante.FirstLastName + " " + estudiante.SecondLastName,
                    primaria = primaria,
                    definitivaLapso1 = (primaria ? definitivaPrimaria1 : Math.Round(definitiva1).ToString()),
                    definitivaLapso2 = (primaria ? definitivaPrimaria2 : Math.Round(definitiva2).ToString()),
                    definitivaLapso3 = (primaria ? definitivaPrimaria3 : Math.Round(definitiva3).ToString()),
                    definitivaFinal = (primaria ? finalMateriaPrimaria : definitivaFinalMateria.ToString()),
                    color = color,
                    colorfuente = colorfuente,
                });

                //Reiniciando valores
                definitiva1 = 0;
                definitiva2 = 0;
                definitiva3 = 0;
                definitivaFinal1 = 0;
                definitivaFinal2 = 0;
                definitivaFinal3 = 0;
                definitivaPrimaria1 = "";
                definitivaPrimaria2 = "";
                definitivaPrimaria3 = "";
                notaAux = new Score();
                #endregion
            }

            return Json(jsonResult);
        }

        public JsonResult ObtenerJsonIndicadoresLiterales(int idCompetencia, int idEvaluacion)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            int nro = 1; //Para la descripción del indicador.

            UnitOfWork unidad = new UnitOfWork();
            IndicatorService indicatorService = new IndicatorService(unidad);
            #endregion

            List<Indicator> listaIndicadores = indicatorService
                .ObteniendoListaIndicadoresPor_Competencia(idCompetencia);

            foreach(Indicator indicator in listaIndicadores)
            {
                foreach (IndicatorAssessment IA in indicator.IndicatorAssessments
                    .Where(m => m.AssessmentId == idEvaluacion))
                {
                    IndicatorAssessment auxIA = indicatorService.ObtenerIndicatorAssessmentPor_Id
                        (IA.IndicatorId, idCompetencia, idEvaluacion);

                    foreach(IndicatorAssignation IAssignation in auxIA.IndicatorAsignations)
                    {
                        jsonResult.Add(new
                        {
                            Success = true,
                            CompetencyId = idCompetencia,
                            IndicatorId = IAssignation.IndicatorId,
                            AssessmentId = idEvaluacion,
                            Assignation = IAssignation.Assignation,
                            Score = IAssignation.LetterScore,
                            IndicatorDescription = "#" + nro.ToString() + " " + indicator.Description,
                            PrincipalId = IAssignation.IndicatorAssignationId
                        });                        
                    }
                }
                nro++;
            }

            return Json(jsonResult);
        }
        public JsonResult CalculoPorcentajeCompetenciaAlcanzado(List<Object> asignaciones, int idEvaluacion)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();

            AssessmentService assessmentService = new AssessmentService();
            Assessment assessment = new Assessment();

            int proporcion_A = 0, proporcion_B = 0, proporcion_C = 0, proporcion_D = 0, proporcion_E = 0;            
            int sumatoriaIndicadores_A = 0, sumatoriaIndicadores_B = 0, sumatoriaIndicadores_C = 0,
                sumatoriaIndicadores_D = 0, sumatoriaIndicadores_E = 0;
            double calculo_A = 0, calculo_B = 0, calculo_C = 0, calculo_D = 0, calculo_E = 0, 
                sumatoriaCalculos = 0;

            double calculoFinal;
            int valorOptimo;
            int totalAlumnos;
            int nroIndicadores;
            #endregion

            #region Obteniendo diccionario de valores de las asignaciones
            foreach (string bloqueAsignaciones in asignaciones)
            {
                string[] bloqueSeparado = bloqueAsignaciones.Split(',');

                sumatoriaIndicadores_A += Convert.ToInt32(bloqueSeparado[0]);
                sumatoriaIndicadores_B += Convert.ToInt32(bloqueSeparado[1]);
                sumatoriaIndicadores_C += Convert.ToInt32(bloqueSeparado[2]);
                sumatoriaIndicadores_D += Convert.ToInt32(bloqueSeparado[3]);
                sumatoriaIndicadores_E += Convert.ToInt32(bloqueSeparado[4]);                
            }
            #endregion
            #region Obteniendo datos varios
            assessment = assessmentService.ObtenerEvaluacionPor_Id(idEvaluacion);
            totalAlumnos = assessment.CASU.Course.Students.Count();
            nroIndicadores = asignaciones.Count();
            #endregion

            #region Calculando la proporción de notas de cada literal
            foreach (Score scoreAux in assessment.Scores)
            {
                if (scoreAux.LetterScore.ToUpper().Equals("A")) proporcion_A++;
                else if (scoreAux.LetterScore.ToUpper().Equals("B")) proporcion_B++;
                else if (scoreAux.LetterScore.ToUpper().Equals("C")) proporcion_C++;
                else if (scoreAux.LetterScore.ToUpper().Equals("D")) proporcion_D++;
                else if (scoreAux.LetterScore.ToUpper().Equals("E")) proporcion_E++;
            }
            #region Si no están todas las notas cargadas
            int sumaProporciones = proporcion_A + proporcion_B + proporcion_C + proporcion_D + proporcion_E;

            if (sumaProporciones != totalAlumnos)
                proporcion_E += totalAlumnos - sumaProporciones;
            #endregion
            #endregion
            #region Cálculo de cada literal
            calculo_A = ((double)proporcion_A / totalAlumnos) * 5 * sumatoriaIndicadores_A;
            calculo_B = ((double)proporcion_B / totalAlumnos) * 4 * sumatoriaIndicadores_B;
            calculo_C = ((double)proporcion_C / totalAlumnos) * 3 * sumatoriaIndicadores_C;
            calculo_D = ((double)proporcion_D / totalAlumnos) * 2 * sumatoriaIndicadores_D;
            calculo_E = ((double)proporcion_E / totalAlumnos) * 1 * sumatoriaIndicadores_E;
            #endregion
            #region Sumatoria de cálculos
            sumatoriaCalculos = calculo_A + calculo_B + calculo_C + calculo_D + calculo_E;
            #endregion
            #region Valor óptimo
            valorOptimo = 5 * (5 * nroIndicadores);
            #endregion
            #region Cálculo final
            calculoFinal = (double)(sumatoriaCalculos * 100) / valorOptimo;
            #endregion
            jsonResult.Add(new { 
                Success = true, 
                porcentajeCompetencia = Math.Round(calculoFinal, 2) 
            });

            return Json(jsonResult);
        }

        public JsonResult ReportePorEvaluacion(int idEvaluacion)
        {
            ObteniendoSesion();
            
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            
            #region Variables utilitarias
            iTextSharp.text.Image image;
            Chart chart;
            ChartArea chartArea;
            Series chartSeries;
            Phrase phrase;
            Paragraph paragraph;
            PdfPCell cell;
            PdfPTable table;
            string stringTitle;
            string stringAux;
            int intAux = 0;
            int[] studentTableWidths = new int[] { 30, 160, 40 };
            #endregion
            #region Variables de datos
            #region Variables de servicios
            AssessmentService assessmentService = new AssessmentService();
            UserService userService = new UserService();
            StudentService studentService = new StudentService();
            ScoreService scoreService = new ScoreService();
            #endregion
            #region Variables obtenidas por la evaluación
            Assessment evaluacion = assessmentService.ObtenerEvaluacionPor_Id(idEvaluacion);
            User docente = evaluacion.CASU.Teacher;
            Course curso = evaluacion.CASU.Course;
            Subject materia = evaluacion.CASU.Subject;
            int grado = curso.Grade;
            Period lapso = evaluacion.CASU.Period;
            CASU casu = evaluacion.CASU;
            School colegio = lapso.SchoolYear.School;
            List<Score> listaNotas = evaluacion.Scores;
            List<Student> listaEstudiantes = curso.Students.OrderBy(m => m.NumberList).ToList();
            #endregion
            #region Variables obtenidas por la sesión
            User usuario = userService.ObtenerUsuarioPorId(_session.USERID);
            #endregion
            #region Variables calculadas desde servicios
            List<Assessment> listaEvaluacionesRealizadas =
                assessmentService.ObtenerListaEvaluacionesRealizadasPor_Curso_Lapso_Materia_Docente(curso.CourseId, 
                lapso.PeriodId, materia.SubjectId, docente.Id);

            double porcentajeAlumnosAprobados = assessmentService
                .ObtenerPorcentajeAlumnosAprobadosPor_Lapso_Curso_Docente(lapso.PeriodId, curso.CourseId, 
                docente.Id);
            #endregion
            #region Variables calculadas
            int totalNotas = listaNotas.Count();
            int mitadNotas = Convert.ToInt32(Math.Round((double)totalNotas / 2));

            List<Score> top10MejoresNotas = (grado > 6 ?
                listaNotas.OrderByDescending(m => m.NumberScore).Take(10).ToList() :
                listaNotas.OrderByDescending(m => m.ToIntLetterScore(m.LetterScore)).Take(10).ToList());
            List<Score> top10PeoresNotas = (grado > 6 ?
                listaNotas.OrderBy(m => m.NumberScore).Take(10).ToList() :
                listaNotas.OrderBy(m => m.ToIntLetterScore(m.LetterScore)).Take(10).ToList());

            List<Student> listaEstudiantesAprobados = new List<Student>();
            List<Student> listaEstudiantesReprobados = new List<Student>();
            foreach (Score scoreAux in listaNotas)
            {
                #region Bachillerato
                if (grado > 6)
                {
                    if (scoreAux.NumberScore >= 10)
                        listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                    else
                        listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                }
                #endregion
                #region Primaria
                else
                {
                    if (scoreAux.ToIntLetterScore(scoreAux.LetterScore) > 1)
                        listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                    else
                        listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                }
                #endregion
            }            
            int nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

            #region % total de la materia (solo para Bachillerato)
            int porcentajeTotalMateria = 0;
            if (grado > 6)
            {
                foreach (Assessment assessmentAux in listaEvaluacionesRealizadas)
                {
                    porcentajeTotalMateria += assessmentAux.Percentage;
                }
            }
            #endregion

            Dictionary<int, int> listaProporcionNotas = new Dictionary<int,int>();
            int tope = (grado > 6 ? 20 : 5);
            for (int i = 1; i <= tope; i++)
            {
                intAux = (grado > 6 ? listaNotas.Where(m => m.NumberScore == i).Count() :
                    listaNotas.Where(m => m.ToIntLetterScore(m.LetterScore) == i).Count());
                listaProporcionNotas.Add(i, intAux);
            };

            int proporcionNotaMasAlta = listaProporcionNotas.Values.Max();
            int proporcionNotaMasBaja = listaProporcionNotas.Where(m => m.Key > 0 && m.Key < 10 && m.Value != 0)
                .OrderByDescending(m => m.Value).FirstOrDefault().Value;

            int notaMasAlta = listaProporcionNotas.Where(m => m.Value == proporcionNotaMasAlta)
                .OrderByDescending(m => m.Key).FirstOrDefault().Key;
            int notaMasBaja = listaProporcionNotas.Where(m => m.Key > 0 && m.Key < 10 && m.Value != 0)
                .OrderByDescending(m => m.Value).FirstOrDefault().Key;
            #endregion
            #endregion
            #region Variables de rutas
            string headerBackground_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_HEADER_BACKGROUND);
            string logo_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_LOGO);
            string subHeaderBackground_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_SUBHEADER_BACKGROUND);

            //string ServerSide_name = "ReportePorEvaluacion" + "C" + _session.SCHOOLID + "Y" +
                //_session.SCHOOLYEARID + "E" + idEvaluacion.ToString() + "U" + _session.USERID + "-" +
                //DateTime.Now.ToString("yyyy-MM-dd") + "H" + DateTime.Now.ToString("HH-mm-ss");
            string ServerSide_name = "ReportePorEvaluacion";

            string pdfFile_ServerSide_name = ServerSide_name + ".pdf";

            string pdfFile_ServerSide_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_DOWNLOAD_DIRECTORY), pdfFile_ServerSide_name);
            string reportRemains_PieChart_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_PieChart.png");
            string reportRemains_PieChart2_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_PieChart2.png");
            string reportRemains_BarChart1_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_BarChart1.png");
            string reportRemains_BarChart2_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_BarChart2.png");
            #endregion
            #region Variables de fuentes
            iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.WHITE);
            iTextSharp.text.Font titleContentFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13); titleContentFont.SetStyle("underline");
            iTextSharp.text.Font chartTitlefont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12); chartTitlefont.SetStyle("underline");
            
            iTextSharp.text.Font whiteBoldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
            iTextSharp.text.Font whiteFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.WHITE);

            iTextSharp.text.Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            System.Drawing.Font graphBarChartFont = new System.Drawing.Font("Almanac MT", 8);
            System.Drawing.Font graphBarChartFontAxisTitle = new System.Drawing.Font("Helvetica", 8);

            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
            iTextSharp.text.Font cellFont_red = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7, BaseColor.RED);
            #endregion            
            #endregion

            #region Configuración del documento
            #region Declarando documento, escritor PDF
            Document document = new Document(PageSize.LETTER, 36, 36, 7, 36);
            FileStream fileStream = new FileStream(pdfFile_ServerSide_path, FileMode.Create);
            //MemoryStream fileStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
            #endregion
            #region Definiendo metadata
            document.AddAuthor("Faro Atenas, Inc.");

            stringAux = "Reporte emitido para indicar la relación del nro. de alumnos aprobados vs " +
                "reprobados en la evaluación: " + evaluacion.Name;
            stringAux = (grado > 6 ? stringAux + " (" + evaluacion.Percentage + "%)" : stringAux);
            document.AddSubject(stringAux);
            document.AddKeywords("Reporte, Aprobados, Reprobados," + evaluacion.Name);
            document.AddCreator("Faro Atenas - Cliente Web");
            document.AddCreationDate();

            stringAux = "Alumnos Aprobados vs Reprobados en la evaluación: " + evaluacion.Name;
            stringAux = (grado > 6 ? stringAux + " (" + evaluacion.Percentage + "%)" : stringAux);
            document.AddTitle(stringAux);
            #endregion
            document.Open();
            #endregion
            #region Página #1
            #region Sección I: Cabecera
            image = iTextSharp.text.Image.GetInstance(headerBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            image = iTextSharp.text.Image.GetInstance(logo_path);
            image.ScalePercent(19f);
            image.SetAbsolutePosition(0 + 27f, document.PageSize.Height - 117f);
            document.Add(image);

            stringTitle = "REPORTE POR EVALUACIÓN - " + evaluacion.Name.ToUpper();
            stringTitle = (grado > 6 ? stringTitle + " (" + evaluacion.Percentage + "%)" : stringTitle);
            paragraph = new Paragraph(new Chunk(stringTitle, titleFont));
            paragraph.IndentationLeft = 100f;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            if (stringTitle.Count() >= 42) paragraph.Leading = 22;
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("Fecha de emisión del reporte: ", whiteBoldFont));
            phrase.Add(new Chunk(DateTime.Now.ToShortDateString() + ", " + 
                DateTime.Now.ToString("h:mm:ss tt"), whiteFont));
            paragraph = new Paragraph(phrase);
            paragraph.IndentationLeft = 100f;
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("Reporte emitido por: ", whiteBoldFont));
            phrase.Add(new Chunk(usuario.Name + " " + usuario.LastName, whiteFont));
            paragraph = new Paragraph(phrase);
            paragraph.IndentationLeft = 100f;
            paragraph.SpacingAfter = (stringTitle.Count() < 42 ? 59f : 59f-14f);
            document.Add(paragraph);
            #endregion
            #region Sección II: Sub-cabecera & Título
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height - 126f);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Docente encargado: " + docente.Name + " " + docente.LastName,
                whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.SpacingBefore = -18;
            document.Add(paragraph);

            paragraph = new Paragraph("Curso: " + curso.Name + " - " + lapso.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Asignatura: " + materia.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph(new Chunk(stringTitle, titleContentFont));
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.SpacingBefore = 15f;
            paragraph.SpacingAfter = 10f;
            document.Add(paragraph);
            #endregion
            #region Sección III: Datos de presentación
            paragraph = new Paragraph("Datos de presentación:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        Reporte emitido para mostrar información relacionada con la " + 
                "evaluación ", normalFont));
            stringAux = "'" + evaluacion.Name;
            stringAux = (grado > 6 ? stringAux + " (" + evaluacion.Percentage + "%)', " : stringAux + "', ");
            phrase.Add(new Chunk(stringAux, boldFont));
            phrase.Add(new Chunk("realizada el ", normalFont));
            phrase.Add(new Chunk(evaluacion.StartDate.ToString("dd") + " de " +
                evaluacion.StartDate.ToString("MMMM") + " de " + evaluacion.StartDate.ToString("yyyy"), boldFont));
            phrase.Add(new Chunk(", correspondiente al ", normalFont));
            phrase.Add(new Chunk(lapso.Name, boldFont));
            phrase.Add(new Chunk(" del ", normalFont));
            phrase.Add(new Chunk(curso.Name, boldFont));
            phrase.Add(new Chunk(", en la materia de ", normalFont));
            phrase.Add(new Chunk(materia.Name, boldFont));
            phrase.Add(new Chunk(".", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección IV: Datos del curso
            paragraph = new Paragraph("Datos del curso:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("         El curso posee un total de ", normalFont));
            phrase.Add(new Chunk(curso.Students.Count().ToString() + " alumnos", boldFont));
            phrase.Add(new Chunk(". Hasta la fecha de emisión de este reporte se ha(n) realizado ", normalFont));
            phrase.Add(new Chunk(listaEvaluacionesRealizadas.Count.ToString() + " evaluación(es)", boldFont));
            if(grado > 6) //Bachillerato
            {
                phrase.Add(new Chunk(", abarcando un ", normalFont));
                phrase.Add(new Chunk(porcentajeTotalMateria.ToString() + "% ", boldFont));
                phrase.Add(new Chunk("del total del ", normalFont));
                phrase.Add(new Chunk(lapso.Name, boldFont));
            }
            else //Primaria
            {
                phrase.Add(new Chunk(" correspondiente(s) al ", normalFont));
                phrase.Add(new Chunk(lapso.Name, boldFont));
            }
            phrase.Add(new Chunk(". Hasta ahora, han aprobado el ", normalFont));
            phrase.Add(new Chunk(Convert.ToInt32(Math.Round(porcentajeAlumnosAprobados)) + "% ", boldFont));
            phrase.Add(new Chunk("de los alumnos.", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección V: Datos de la evaluación
            paragraph = new Paragraph("Datos de la evaluación:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        La condición para que un alumno esté aprobado en la evaluación," +
                " es haber obtenido un resultado mayor o igual", normalFont));
            if(grado > 6) //Bachillerato
            {
                phrase.Add(new Chunk(" a los ", normalFont));
                phrase.Add(new Chunk("9,5 puntos ", boldFont));
                phrase.Add(new Chunk("del total. "));
            }
            else //Primaria
            {
                phrase.Add(new Chunk(" al literal ", normalFont));
                phrase.Add(new Chunk("D", boldFont));
                phrase.Add(new Chunk(". "));
            }
            phrase.Add(new Chunk("A continuación se detalla el porcentaje de alumnos que aprobaron en " + 
                "comparación con aquellos que reprobaron:", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección VI: Gráfico - Aprobados vs Reprobados
            paragraph = new Paragraph(new Chunk("Gráfica de % de alumnos aprobados vs reprobados", 
                chartTitlefont));
            paragraph.SpacingBefore = 5;
            paragraph.Alignment = Element.ALIGN_CENTER;
            document.Add(paragraph);

            Dictionary<string, int> data
                = new Dictionary<string, int> { 
                    { "Aprobados:", listaEstudiantesAprobados.Count() }, 
                    //{ "Reprobados:", listaEstudiantesReprobados } };
                    { "Reprobados:", nroEstudiantesReprobados } };

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 290;
            chart.Height = 290;

            chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Pie;
            chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
            chartSeries.BorderColor = Color.Black;
            chartSeries.BorderWidth = 2;
            chartSeries.LabelForeColor = Color.White;

            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chartSeries.Points[0].Color = Color.Blue;
            chartSeries.Points[1].Color = Color.Red;
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            MemoryStream memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_PieChart_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            System.Drawing.Image img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            MemoryStream stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 450f, document.PageSize.Height - 780f);
            document.Add(image);
            #endregion
            #endregion
            #region Página #2
            document.NewPage();
            #region Sección VII: Cabecera - 2da página
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Docente encargado: " + docente.Name + " " + docente.LastName,
                whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.SpacingBefore = -18;
            document.Add(paragraph);

            paragraph = new Paragraph("Curso: " + curso.Name + " - " + lapso.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Asignatura: " + materia.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Pág. 2", whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.SpacingBefore = -18;
            document.Add(paragraph);
            #endregion
            #region Sección VIII: Datos de la evaluación - Parte II
            paragraph = new Paragraph("Datos de la evaluación - Parte II:", boldFont);
            paragraph.SpacingBefore = 15;
            document.Add(paragraph);

            paragraph = new Paragraph("        A continuación se indica el listado del top 10 de los" + 
                " resultados más destacados y deficientes obtenidos en la evaluación (no se consideran los" + 
                " alumnos sin notas cargadas):", normalFont);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección IX: Gráficos - Top 10 mejores/peores resultados
            #region Títulos gráficos
            Paragraph tableTitle1 = new Paragraph(new Phrase(new Chunk("Gráfica del top 10 resultados destacados",
                chartTitlefont)));
            Paragraph tableTitle2 = new Paragraph(new Phrase(new Chunk("Gráfica del top 10 resultados deficientes",
                chartTitlefont)));

            PdfPCell cellTitle1 = new PdfPCell(tableTitle1)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0
            };
            PdfPCell cellTitle2 = new PdfPCell(tableTitle2)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0
            };

            PdfPTable titleTable = new PdfPTable(2);
            titleTable.AddCell(cellTitle1);
            titleTable.AddCell(cellTitle2);
            titleTable.SpacingAfter = 15f;
            titleTable.WidthPercentage = 100f;

            document.Add(titleTable);
            #endregion
            #region Gráfico 10 mejores
            Dictionary<int, float> data2 = new Dictionary<int, float>();
            for (int i = 1; i <= 10; i++)
            {
                if(grado > 6) //Bachillerato
                    data2.Add(i, top10MejoresNotas[i - 1].NumberScore);
                else //Primaria
                    data2.Add(i, top10MejoresNotas[i - 1].ToIntLetterScore(top10MejoresNotas[i - 1].LetterScore));
            }

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 255;
            chart.Height = 278;

            chartArea = new ChartArea();
            chartArea.AxisY.LabelStyle.Enabled = true;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisY.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisY.IsLabelAutoFit = true;
            chartArea.AxisY.LineColor = Color.Blue;
            chartArea.AxisY.Maximum = (grado > 6 ? 20 : 5);
            chartArea.AxisY.Title = "Notas obtenidas";
            chartArea.AxisY.TitleFont = graphBarChartFontAxisTitle;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.Lavender;
            chartArea.AxisY.MajorGrid.LineWidth = 6;
            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.LineColor = Color.Blue;
            chartArea.AxisY.MinorTickMark.Enabled = false;
            if(grado <= 6)//Primaria
            {
                chartArea.AxisY.CustomLabels.Add(0,1,"E");
                chartArea.AxisY.CustomLabels.Add(1,2,"D");
                chartArea.AxisY.CustomLabels.Add(2,3,"C");
                chartArea.AxisY.CustomLabels.Add(3,4,"B");
                chartArea.AxisY.CustomLabels.Add(4,5,"A");
            }

            chartArea.AxisX.LabelStyle.Enabled = true;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisX.LineColor = Color.Blue;
            chartArea.AxisX.Maximum = 11;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.MinorTickMark.Enabled = false;
            chartArea.AxisX.LabelAutoFitMinFontSize = 7;

            for (int i = 0; i <= 10; i++)
            {
                if (i != 10)
                {
                    if(grado > 6) //Bachillerato
                        stringAux = "(" + top10MejoresNotas[i].NumberScore + ") ";
                    else //Primaria
                        stringAux = "(" + top10MejoresNotas[i].LetterScore + ") ";

                    stringAux = stringAux + top10MejoresNotas[i].Student.FirstLastName + " " +
                        top10MejoresNotas[i].Student.SecondLastName + ", " +
                        top10MejoresNotas[i].Student.FirstName;

                    chartArea.AxisX.CustomLabels.Add(i + 0.5, i + 1.5, stringAux);
                }
            }
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Column;
            chartSeries.Color = Color.Blue;
            chartSeries.BorderColor = Color.DarkBlue;
            chartSeries.BackGradientStyle = GradientStyle.LeftRight;
            foreach (var item in data2) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_BarChart1_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_BarChart1_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 570, document.PageSize.Height - 440f);
            document.Add(image);
            #endregion
            #region Gráfico 10 peores
            Dictionary<int, float> data3 = new Dictionary<int, float>();
            for (int i = 1; i <= 10; i++)
            {
                if (grado > 6) //Bachillerato
                    data3.Add(i, top10PeoresNotas[i - 1].NumberScore);
                else //Primaria
                    data3.Add(i, top10PeoresNotas[i - 1].ToIntLetterScore(top10PeoresNotas[i - 1].LetterScore));
            }

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 255;
            chart.Height = 278;

            chartArea = new ChartArea();
            chartArea.AxisY.LabelStyle.Enabled = true;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisY.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisY.IsLabelAutoFit = true;
            chartArea.AxisY.LineColor = Color.Red;
            chartArea.AxisY.Maximum = (grado > 6 ? 20 : 5);
            chartArea.AxisY.Title = "Notas obtenidas";
            chartArea.AxisY.TitleFont = graphBarChartFontAxisTitle;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.Lavender;
            chartArea.AxisY.MajorGrid.LineWidth = 6;
            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.LineColor = Color.Red;
            chartArea.AxisY.MinorTickMark.Enabled = false;
            if (grado <= 6)//Primaria
            {
                chartArea.AxisY.CustomLabels.Add(0, 1, "E");
                chartArea.AxisY.CustomLabels.Add(1, 2, "D");
                chartArea.AxisY.CustomLabels.Add(2, 3, "C");
                chartArea.AxisY.CustomLabels.Add(3, 4, "B");
                chartArea.AxisY.CustomLabels.Add(4, 5, "A");
            }

            chartArea.AxisX.LabelStyle.Enabled = true;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisX.LineColor = Color.Red;
            chartArea.AxisX.Maximum = 11;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.MinorTickMark.Enabled = false;
            chartArea.AxisX.LabelAutoFitMinFontSize = 7;

            for (int i = 0; i <= 10; i++)
            {
                if (i != 10)
                {
                    if (grado > 6) //Bachillerato
                        stringAux = "(" + top10PeoresNotas[i].NumberScore + ") ";
                    else //Primaria
                        stringAux = "(" + top10PeoresNotas[i].LetterScore + ") ";

                    stringAux = stringAux + top10PeoresNotas[i].Student.FirstLastName + " " +
                        top10PeoresNotas[i].Student.SecondLastName + ", " +
                        top10PeoresNotas[i].Student.FirstName;

                    chartArea.AxisX.CustomLabels.Add(i + 0.5, i + 1.5, stringAux);
                }
            }
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Column;
            chartSeries.Color = Color.Red;
            chartSeries.BorderColor = Color.DarkRed;
            chartSeries.BackGradientStyle = GradientStyle.LeftRight;
            foreach (var item in data3) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_BarChart2_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_BarChart2_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 302, document.PageSize.Height - 440f);
            document.Add(image);
            #endregion
            #endregion
            #region Sección X: Tabla de alumnos
            #region Texto preliminar
            paragraph = new Paragraph("        A continuación se presenta la tabla de alumnos con los" + 
                " resultados obtenidos por cada uno de estos en la evaluación (los alumnos sin notas cargadas" + 
                " aparecen como 'N/A'):", normalFont);
            paragraph.SpacingBefore = 247;
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Table #1
            #region Configuración de la tabla
            table = new PdfPTable(3);
            table.SetWidths(studentTableWidths);
            table.LockedWidth = true;
            table.TotalWidth = 230;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            #endregion
            #region Configuración de la celda #1
            cell = new PdfPCell(new Phrase(new Chunk("#", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #2
            cell = new PdfPCell(new Phrase(new Chunk("Alumnos", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #3
            cell = new PdfPCell(new Phrase(new Chunk("Nota", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthLeft = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Ciclo de llenado de las celdas
            for (int i = 0; i <= 22; i++)
            {
                #region Celda #1
                stringAux = listaEstudiantes[i].NumberList.ToString();
                cell = new PdfPCell(new Phrase(stringAux, cellFont)) { BorderWidthLeft = 1.75f, 
                    HorizontalAlignment = Element.ALIGN_CENTER };
                if (i == 22) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #2
                stringAux = listaEstudiantes[i].FirstLastName + " " + listaEstudiantes[i].SecondLastName +
                    ", " + listaEstudiantes[i].FirstName + " " + listaEstudiantes[i].SecondName;
                cell = new PdfPCell(new Phrase(stringAux, cellFont)) { BorderWidthLeft = 1.75f, 
                    HorizontalAlignment = Element.ALIGN_LEFT };
                if (i == 22) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #3
                try
                {
                    #region Bachillerato
                    if(grado > 6) //Bachillerato
                    {
                        intAux = Convert.ToInt32(Math.Round(listaNotas.Where(m => m.StudentId == 
                            listaEstudiantes[i].StudentId).FirstOrDefault().NumberScore));
                        if(intAux <= 10) //Aplazado
                            phrase = new Phrase(new Chunk(intAux.ToString(), cellFont_red));
                        else
                            phrase = new Phrase(new Chunk(intAux.ToString(), cellFont));

                    }
                    #endregion
                    #region Primaria
                    else //Primaria
                    {
                        stringAux = listaNotas.Where(m => m.StudentId == 
                            listaEstudiantes[i].StudentId).FirstOrDefault().LetterScore.ToUpper();
                        if(stringAux.Equals("E")) //Aplazado
                            phrase = new Phrase(new Chunk(stringAux, cellFont_red));
                        else
                            phrase = new Phrase(new Chunk(stringAux, cellFont));
                    }
                    #endregion
                }
                catch(NullReferenceException)
                {
                    phrase = new Phrase(new Chunk("N/A", cellFont_red));
                }
                finally
                {
                    cell = new PdfPCell(phrase)
                    {
                        BorderWidthRight = 1.75f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    if (i == 22) cell.BorderWidthBottom = 1.75f;
                    table.AddCell(cell);
                }
                #endregion
            }
            document.Add(table);
            #endregion
            #endregion
            #region Table #2
            #region Configuración de la tabla
            table = new PdfPTable(3);
            table.SetWidths(studentTableWidths);
            table.LockedWidth = true;
            table.TotalWidth = 230;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            #endregion
            #region Configuración de la celda #1
            cell = new PdfPCell(new Phrase(new Chunk("#", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #2
            cell = new PdfPCell(new Phrase(new Chunk("Alumnos", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #3
            cell = new PdfPCell(new Phrase(new Chunk("Nota", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthLeft = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Ciclo de llenado de las celdas
            for (int i = 23; i <= listaEstudiantes.Count() - 1; i++)
            {
                #region Celda #1
                stringAux = listaEstudiantes[i].NumberList.ToString();
                cell = new PdfPCell(new Phrase(stringAux, cellFont))
                {
                    BorderWidthLeft = 1.75f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                if (i == listaEstudiantes.Count() - 1) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #2
                stringAux = listaEstudiantes[i].FirstLastName + " " + listaEstudiantes[i].SecondLastName +
                    ", " + listaEstudiantes[i].FirstName + " " + listaEstudiantes[i].SecondName;
                cell = new PdfPCell(new Phrase(stringAux, cellFont))
                {
                    BorderWidthLeft = 1.75f,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                if (i == listaEstudiantes.Count() - 1) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #3
                try
                {
                    #region Bachillerato
                    if (grado > 6) //Bachillerato
                    {
                        intAux = Convert.ToInt32(Math.Round(listaNotas.Where(m => m.StudentId ==
                            listaEstudiantes[i].StudentId).FirstOrDefault().NumberScore));
                        if (intAux <= 10) //Aplazado
                            phrase = new Phrase(new Chunk(intAux.ToString(), cellFont_red));
                        else
                            phrase = new Phrase(new Chunk(intAux.ToString(), cellFont));

                    }
                    #endregion
                    #region Primaria
                    else //Primaria
                    {
                        stringAux = listaNotas.Where(m => m.StudentId ==
                            listaEstudiantes[i].StudentId).FirstOrDefault().LetterScore.ToUpper();
                        if (stringAux.Equals("E")) //Aplazado
                            phrase = new Phrase(new Chunk(stringAux, cellFont_red));
                        else
                            phrase = new Phrase(new Chunk(stringAux, cellFont));
                    }
                    #endregion
                }
                catch (NullReferenceException)
                {
                    phrase = new Phrase(new Chunk("N/A", cellFont_red));
                }
                finally
                {
                    cell = new PdfPCell(phrase)
                    {
                        BorderWidthRight = 1.75f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    if (i == listaEstudiantes.Count() - 1) cell.BorderWidthBottom = 1.75f;
                    table.AddCell(cell);
                }
                #endregion
            }
            #endregion

            paragraph = new Paragraph();
            paragraph.Add(table);
            paragraph.SpacingBefore = -285;
            document.Add(paragraph);
            #endregion
            #endregion
            #endregion
            #region Página #3
            document.NewPage();
            #region Sección XI: Cabecera - 3era página
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Reporte emitido por: " + usuario.Name + " " + usuario.LastName,
                whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.SpacingBefore = -18;
            document.Add(paragraph);

            paragraph = new Paragraph("Curso: " + curso.Name + " - " + lapso.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Docente encargado: " + docente.Name + " " + docente.LastName,
                whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Pág. 3", whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.SpacingBefore = -18;
            document.Add(paragraph);
            #endregion
            #region Sección XII: Datos de la evaluación - Parte III
            paragraph = new Paragraph("Datos de la evaluación - Parte III:", boldFont);
            paragraph.SpacingBefore = 15;
            document.Add(paragraph);

            phrase = new Phrase();
            #region Bachillerato
            if (grado > 6)
            {
                phrase.Add(new Chunk("        La nota más alta y de mayor frecuencia obtenida por los " +
                "alumnos fue de ", normalFont));
                phrase.Add(new Chunk(notaMasAlta.ToString(), boldFont));
                phrase.Add(new Chunk(" puntos; del cual, la proporción de alumnos que obtuvieron este" +
                    " resultado fue de ", normalFont));
                phrase.Add(new Chunk(proporcionNotaMasAlta.ToString(), boldFont));
                phrase.Add(new Chunk(" del total. Para los resultados deficientes, la nota más baja y de mayor" +
                    " frecuencia obtenida por los alumnos fue de ", normalFont));
                phrase.Add(new Chunk(notaMasBaja.ToString(), boldFont));
                stringAux = (notaMasBaja == 1 ? " punto;" : " puntos;");
                phrase.Add(new Chunk(stringAux + " del cual, la proporción de alumnos que obtuvieron este" +
                    " resultado fue de ", normalFont));
                phrase.Add(new Chunk(proporcionNotaMasBaja.ToString(), boldFont));
                phrase.Add(new Chunk(" del total. A continuación se indica el gráfico que representa la" +
                    " proporción de notas alcanzadas por los alumnos en la evaluación:", normalFont));
            }
            #endregion
            #region Primaria
            else
            {
                phrase.Add(new Chunk("        A continuación se indica el gráfico que representa la" +
                    " proporción de notas alcanzadas por los alumnos en la evaluación:", normalFont));
            }
            #endregion

            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = (grado > 6 ? 10 : 70);
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección XIII: Gráfico - Proporción de notas
            paragraph = new Paragraph(new Chunk("Gráfica de proporción de notas alcanzadas", chartTitlefont));
            paragraph.SpacingBefore = 5;
            paragraph.Alignment = Element.ALIGN_CENTER;
            document.Add(paragraph);

            data = new Dictionary<string, int>();
            for (int i = 1; i <= listaProporcionNotas.Count; i++)
            {
                if (listaProporcionNotas[i] != 0)
                {
                    stringAux = (grado > 6 ? i.ToString() + "pts:" : "Literal " +
                        new Score().ToStringLetterIntScore(i) + ":");
                    intAux = listaProporcionNotas[i];
                    data.Add(stringAux, intAux);
                }
            };

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 670;
            chart.Height = 400;

            chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series("Pie Chart 2");
            chartSeries.ChartType = SeriesChartType.Pie;
            chartSeries.Label = "#AXISLABEL #VALY alumno(s)";
            chartSeries.BorderColor = Color.Black;
            chartSeries.BorderWidth = 2;

            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }

            chart.Series.Add(chartSeries);
            chart.Series["Pie Chart 2"]["PieLabelStyle"] = "Outside";
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_PieChart2_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart2_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 635f, document.PageSize.Height - 618f);
            document.Add(image);
            #endregion
            #endregion
            #region Pasos finales
            document.Close();

            jsonResult.Add(new {
                success = true,
                path = pdfFile_ServerSide_path,                
            });

            TempData["Evaluación"] = true;

            return Json(jsonResult);
            #endregion

            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "attachment; filename=Reporte_por_evaluación_Faro_Atenas.pdf");

            //return new FileStreamResult(fileStream, "application/pdf") {
                //FileDownloadName = "Reporte_por_evaluación_Faro_Atenas.pdf"
            //};
                        
            //return File(fileStream, "application/pdf", "Reporte por evaluación - Faro Atenas.pdf");
        }
        public JsonResult ReportePorMateria(int idMateria, int idLapso, int idCurso)
        {
            ObteniendoSesion();

            #region Declaración de variables
            List<object> jsonResult = new List<object>();

            #region Variables utilitarias
            MemoryStream memoryStream;
            System.Drawing.Image img1;
            MemoryStream stream1;
            byte[] imageByte1;

            iTextSharp.text.Image image;
            Chart chart;
            ChartArea chartArea;
            Series chartSeries;
            Phrase phrase;
            Paragraph paragraph;
            PdfPCell cell;
            PdfPTable table;
            Assessment evaluacion1;
            Assessment evaluacion2;
            Assessment evaluacion3;
            string stringTitle;
            string stringAux;
            int intAux = 0;
            int[] studentTableWidths = new int[] { 30, 160, 40 };
            Dictionary<string, int> data;
            List<Student> listaEstudiantesAprobados;
            List<Student> listaEstudiantesReprobados;
            List<Score> listaNotas;
            int nroEstudiantesReprobados = 0;
            bool unaEvaluacion = false;
            //bool tresEvaluaciones = false;
            bool cuatroEvaluaciones = false;
            bool cincoEvaluaciones = false;
            bool seisEvaluaciones = false;
            float heightCharts1stPage = 640f; 
            float heightCharts2ndPage = 310f;
            Student student;
            #endregion
            #region Variables de datos
            #region Variables de servicios
            SubjectService subjectService = new SubjectService();
            CASUService casuService = new CASUService();
            UserService userService = new UserService();
            AssessmentService assessmentService = new AssessmentService();
            ScoreService scoreService = new ScoreService();
            StudentService studentService = new StudentService();
            #endregion
            #region Variables obtenidas por el CASU
            CASU casu = casuService.ObtenerCASUPor_Ids(idCurso, idLapso, idMateria);
            Subject materia = casu.Subject;
            Course curso = casu.Course;
            int grado = curso.Grade;
            Period lapso = casu.Period;
            SchoolYear anoEscolar = lapso.SchoolYear;
            School colegio = anoEscolar.School;
            User docente = casu.Teacher;
            List<Assessment> listaEvaluaciones = casu.Assessments;
            List<Student> listaEstudiantes = curso.Students.OrderBy(m => m.NumberList).ToList();
            int nroEvaluaciones = listaEvaluaciones.Count();
            #endregion
            #region Variables obtenidas por la sesión
            User usuario = userService.ObtenerUsuarioPorId(_session.USERID);
            #endregion
            #region Variables calculadas desde servicios
            List<Assessment> listaEvaluacionesRealizadas =
                assessmentService.ObtenerListaEvaluacionesRealizadasPor_Curso_Lapso_Materia_Docente(curso.CourseId,
                lapso.PeriodId, materia.SubjectId, docente.Id);

            Dictionary<int, double> listaDefinitivas = scoreService
                .ObtenerDefinitivasPor_Curso_Lapso_Materia_Docente(idCurso, idLapso, idMateria, docente.Id);
            #endregion
            #region Variables calculadas
            #region % Total alcanzado en las evaluaciones realizadas (Solo para Bachillerato)
            int porcentajeTotalMateria = 0;
            if (grado > 6)
            {
                foreach (Assessment assessmentAux in listaEvaluacionesRealizadas)
                {
                    porcentajeTotalMateria += assessmentAux.Percentage;
                }
            }
            #endregion
            #region Listas de Aprobados & Reprobados
            Dictionary<int, double> listaEstudiantesAprobadosDefinitiva = new Dictionary<int,double>();
            Dictionary<int, double> listaEstudiantesReprobadosDefinitiva = new Dictionary<int, double>();

            foreach (KeyValuePair<int, double> valor in listaDefinitivas)
            {
                if(grado > 6) //Bachillerato
                {
                    if (valor.Value >= 10)
                        listaEstudiantesAprobadosDefinitiva.Add(valor.Key, valor.Value);
                    else
                        listaEstudiantesReprobadosDefinitiva.Add(valor.Key, valor.Value);
                }
                else // Primaria
                {
                    if (valor.Value >= 2)
                        listaEstudiantesAprobadosDefinitiva.Add(valor.Key, valor.Value);
                    else
                        listaEstudiantesReprobadosDefinitiva.Add(valor.Key, valor.Value);
                }
            }
            int nroAprobadosDefinitiva = listaEstudiantesAprobadosDefinitiva.Count();
            int nroReprobadosDefinitiva = listaDefinitivas.Count() - nroAprobadosDefinitiva;
            #endregion
            #region Cálculo del promedio
            double promedio = (from double definitiva in listaDefinitivas.Values
                               select definitiva).Average();
            #endregion
            #region Top10MejoresNotas
            Dictionary<int, double> top10MejoresNotas = new Dictionary<int, double>();

            intAux = 1; //Variable de contol del foreach
            foreach (KeyValuePair<int, double> valor in listaDefinitivas.OrderByDescending(key => key.Value))
            {
                if (intAux <= 10)
                {
                    top10MejoresNotas.Add(valor.Key, valor.Value);
                    intAux++;
                }
                else
                    break;
            } 
            #endregion
            #region Top10PeoresNotas
            Dictionary<int, double> top10PeoresNotas = new Dictionary<int, double>();

            intAux = 1; //Variable de contol del foreach
            foreach (KeyValuePair<int, double> valor in listaDefinitivas.OrderBy(key => key.Value))
            {
                if (intAux <= 10)
                {
                    top10PeoresNotas.Add(valor.Key, valor.Value);
                    intAux++;
                }
                else
                    break;
            }
            #endregion            
            #endregion            
            #endregion
            #region Variables de rutas
            string headerBackground_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_HEADER_BACKGROUND);
            string logo_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_LOGO);
            string subHeaderBackground_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_SUBHEADER_BACKGROUND);
            string noData_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_NODATA);

            //string ServerSide_name = "ReportePorMateria" + "C" + _session.SCHOOLID + "Y" +
                //_session.SCHOOLYEARID + "M" + idMateria.ToString() + "P" + lapso.PeriodId.ToString() + 
                //"C" + curso.CourseId.ToString() + "U" + _session.USERID + "-" +
                //DateTime.Now.ToString("yyyy-MM-dd") + "H" + DateTime.Now.ToString("HH-mm-ss");
            string ServerSide_name = "ReportePorMateria";

            string pdfFile_ServerSide_name = ServerSide_name + ".pdf";

            string pdfFile_ServerSide_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_DOWNLOAD_DIRECTORY), pdfFile_ServerSide_name);
            string reportRemains_PieChart1_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_PieChart1_Materia.png");
            string reportRemains_PieChart2_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_PieChart2_Materia.png");
            string reportRemains_PieChart3_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_PieChart3_Materia.png");
            string reportRemains_PieChart4_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_PieChart4_Materia.png");
            string reportRemains_PieChart5_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_PieChart5_Materia.png");
            string reportRemains_PieChart6_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_PieChart6_Materia.png");
            string reportRemains_PieChart7_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_PieChart7_Materia.png");
            string reportRemains_BarChart1_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_BarChart1_Materia.png");
            string reportRemains_BarChart2_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_BarChart2_Materia.png");
            #endregion
            #region Variables de fuentes
            iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.WHITE);
            iTextSharp.text.Font titleContentFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13); titleContentFont.SetStyle("underline");
            iTextSharp.text.Font chartTitlefont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12); chartTitlefont.SetStyle("underline");

            iTextSharp.text.Font whiteBoldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
            iTextSharp.text.Font whiteFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.WHITE);

            iTextSharp.text.Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            System.Drawing.Font graphBarChartFont = new System.Drawing.Font("Almanac MT", 8);
            System.Drawing.Font graphBarChartFontAxisTitle = new System.Drawing.Font("Helvetica", 8);

            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
            iTextSharp.text.Font cellFont_red = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7, BaseColor.RED);
            #endregion            
            #endregion

            #region Configuración del documento
            #region Declarando documento, escritor PDF
            Document document = new Document(PageSize.LETTER, 36, 36, 7, 36);
            FileStream fileStream = new FileStream(pdfFile_ServerSide_path, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
            #endregion
            #region Definiendo metadata
            document.AddAuthor("Faro Atenas, Inc.");

            stringAux = "Reporte emitido para indicar los datos asociados a la materia: " + materia.Name + 
                ", en el " + lapso.Name + " del curso " + curso.Name + ". Año escolar: Desde " + 
                anoEscolar.StartDate.ToString("dd-mm-yyyy") + " hasta " + 
                anoEscolar.EndDate.ToString("dd-mm-yyyy");

            document.AddSubject(stringAux);
            document.AddKeywords("Reporte, Materia, Reprobados," + materia.Name);
            document.AddCreator("Faro Atenas - Cliente Web");
            document.AddCreationDate();

            stringAux = "Reporte de datos por materia: " + materia.Name;
            document.AddTitle(stringAux);
            #endregion
            document.Open();
            #endregion
            #region Página #1
            #region Sección I: Cabecera
            image = iTextSharp.text.Image.GetInstance(headerBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            image = iTextSharp.text.Image.GetInstance(logo_path);
            image.ScalePercent(19f);
            image.SetAbsolutePosition(0 + 27f, document.PageSize.Height - 117f);
            document.Add(image);

            stringTitle = "REPORTE POR MATERIA - " + materia.Name.ToUpper() + " (" + lapso.Name.ToUpper() + ")";
            paragraph = new Paragraph(new Chunk(stringTitle, titleFont));
            paragraph.IndentationLeft = 100f;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            if (stringTitle.Count() >= 42) paragraph.Leading = 22;
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("Fecha de emisión del reporte: ", whiteBoldFont));
            phrase.Add(new Chunk(DateTime.Now.ToShortDateString() + ", " +
                DateTime.Now.ToString("h:mm:ss tt"), whiteFont));
            paragraph = new Paragraph(phrase);
            paragraph.IndentationLeft = 100f;
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("Reporte emitido por: ", whiteBoldFont));
            phrase.Add(new Chunk(usuario.Name + " " + usuario.LastName, whiteFont));
            paragraph = new Paragraph(phrase);
            paragraph.IndentationLeft = 100f;
            paragraph.SpacingAfter = (stringTitle.Count() < 42 ? 59f : 59f - 14f);
            document.Add(paragraph);
            #endregion
            #region Sección II: Sub-cabecera & Título
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height - 126f);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Docente encargado: " + docente.Name + " " + docente.LastName,
                whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.SpacingBefore = -18;
            document.Add(paragraph);

            paragraph = new Paragraph("Curso: " + curso.Name + " - " + lapso.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Asignatura: " + materia.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph(new Chunk(stringTitle, titleContentFont));
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.SpacingBefore = 15f;
            paragraph.SpacingAfter = 10f;
            document.Add(paragraph);
            #endregion
            #region Sección III: Datos de presentación
            paragraph = new Paragraph("Datos de presentación:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        Reporte emitido para mostrar información relacionada con el " +
                "rendimiento académico obtenido por los alumnos del curso '", normalFont));
            phrase.Add(new Chunk(curso.Name, boldFont));
            phrase.Add(new Chunk("', en la materia '", normalFont));
            phrase.Add(new Chunk(materia.Name, boldFont));
            phrase.Add(new Chunk("', para el período escolar del '", normalFont));
            phrase.Add(new Chunk(lapso.Name, boldFont));
            phrase.Add(new Chunk("'.", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección IV: Datos de la materia
            paragraph = new Paragraph("Datos de la materia:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        Para el período escolar definido desde el ", normalFont));
            phrase.Add(new Chunk(lapso.StartDate.ToString("dd") + " de " +
                lapso.StartDate.ToString("MMMM") + " de " + lapso.StartDate.ToString("yyyy"), boldFont));
            phrase.Add(new Chunk(", hasta el ", normalFont));
            phrase.Add(new Chunk(lapso.FinishDate.ToString("dd") + " de " +
                lapso.FinishDate.ToString("MMMM") + " de " + lapso.FinishDate.ToString("yyyy"), boldFont));
            phrase.Add(new Chunk(", correspondiente al ", normalFont));
            phrase.Add(new Chunk(lapso.Name, boldFont));
            phrase.Add(new Chunk(", a la materia se le asignaron ", normalFont));
            phrase.Add(new Chunk(listaEvaluaciones.Count().ToString() + " evaluaciones", boldFont));
            phrase.Add(new Chunk(". Hasta la fecha de emisión del reporte se han realizado ", normalFont));
            phrase.Add(new Chunk(listaEvaluacionesRealizadas.Count().ToString() + " evaluaciones", boldFont));
            if(grado > 6) //Bachillerato
            {
                phrase.Add(new Chunk(", abarcando un ", normalFont));
                phrase.Add(new Chunk(porcentajeTotalMateria.ToString() + "% ", boldFont));
                phrase.Add(new Chunk("del total de la asignatura."));
            }
            else // Primaria
                phrase.Add(new Chunk(".", normalFont));
            
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección V: Datos de las evaluaciones
            paragraph = new Paragraph("Datos de las evaluaciones:", boldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("        A continuación se indica el porcentaje de aprobados y " + 
                "reprobados en las evaluaciones definidas para esta materia:", normalFont);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección VI: Gráficos de las evaluaciones
            #region Materia con 3 o más evaluaciones
            if (nroEvaluaciones >= 3)
            {
                #region Obteniendo datos de las evaluaciones
                evaluacion1 = assessmentService.ObtenerEvaluacionPor_Id(listaEvaluaciones[0].AssessmentId);
                evaluacion2 = assessmentService.ObtenerEvaluacionPor_Id(listaEvaluaciones[1].AssessmentId);
                evaluacion3 = assessmentService.ObtenerEvaluacionPor_Id(listaEvaluaciones[2].AssessmentId);
                #endregion
                #region Títulos de los gráficos
                table = new PdfPTable(3);
                #region Celda #1
                stringAux = evaluacion1.Name;
                stringAux += (grado > 6 ? " (" + evaluacion1.Percentage + "%)" : "");
                paragraph = new Paragraph(new Phrase(new Chunk(stringAux, chartTitlefont)));
                cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                table.AddCell(cell);
                #endregion
                #region Celda #2
                stringAux = evaluacion2.Name;
                stringAux += (grado > 6 ? " (" + evaluacion2.Percentage + "%)" : "");
                paragraph = new Paragraph(new Phrase(new Chunk(stringAux, chartTitlefont)));
                cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                table.AddCell(cell);
                #endregion
                #region Celda #3
                stringAux = evaluacion3.Name;
                stringAux += (grado > 6 ? " (" + evaluacion3.Percentage + "%)" : "");
                paragraph = new Paragraph(new Phrase(new Chunk(stringAux, chartTitlefont)));
                cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                table.AddCell(cell);
                #endregion

                table.SpacingAfter = 15f;
                table.WidthPercentage = 100f;
                document.Add(table);
                #endregion
                #region Gráficos de las evaluaciones
                #region Evaluación #1
                #region Obteniendo las notas
                listaNotas = evaluacion1.Scores;
                #endregion
                #region Si hay notas
                if (listaNotas.Count != 0)
                {
                    #region Cáclulo de # aprobados y reprobados
                    listaEstudiantesAprobados = new List<Student>();
                    listaEstudiantesReprobados = new List<Student>();
                    foreach (Score scoreAux in listaNotas)
                    {
                        #region Bachillerato
                        if (grado > 6)
                        {
                            if (scoreAux.NumberScore >= 10)
                                listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                            else
                                listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                        }
                        #endregion
                        #region Primaria
                        else
                        {
                            if (scoreAux.ToIntLetterScore(scoreAux.LetterScore) > 1)
                                listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                            else
                                listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                        }
                        #endregion
                    }
                    nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                    data = new Dictionary<string, int> { 
                    { "Aprob.:", listaEstudiantesAprobados.Count() }, 
                    { "Reprob.:", nroEstudiantesReprobados } };
                    #endregion
                    #region Dibujando el gráfico
                    chart = new Chart();
                    chart.AntiAliasing = AntiAliasingStyles.All;
                    chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                    chart.Width = 180;
                    chart.Height = 180;

                    chartArea = new ChartArea();
                    chart.ChartAreas.Add(chartArea);

                    chartSeries = new Series();
                    chartSeries.ChartType = SeriesChartType.Pie;
                    chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
                    chartSeries.BorderColor = Color.Black;
                    chartSeries.BorderWidth = 2;
                    chartSeries.LabelForeColor = Color.White;

                    foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                    chartSeries.Points[0].Color = Color.Blue;
                    chartSeries.Points[1].Color = Color.Red;
                    chart.Series.Add(chartSeries);
                    #endregion
                    #region Definiendo un Memory Stream para guardar la imagen
                    memoryStream = new MemoryStream();
                    chart.SaveImage(memoryStream, ChartImageFormat.Png);
                    //chart.SaveImage(reportRemains_PieChart1_path, ChartImageFormat.Png);
                    #endregion
                    #region Obteniendo imagen desde MemoryStream
                    img1 = System.Drawing.Image.FromStream(memoryStream);
                    memoryStream.Close();
                    #endregion
                    #region Transformando imagen a array binario
                    stream1 = new MemoryStream();
                    img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
                    imageByte1 = stream1.ToArray();
                    stream1.Dispose();
                    img1.Dispose();
                    #endregion
                    #region Obteniendo imagen desde array binario
                    //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart1_path);
                    image = iTextSharp.text.Image.GetInstance(imageByte1);
                    memoryStream.Close();
                    #endregion
                }
                #endregion
                #region No hay notas
                else
                    image = iTextSharp.text.Image.GetInstance(noData_path);
                #endregion
                #region Añadiendo imagen
                image.Alignment = Element.ALIGN_CENTER;
                image.SetAbsolutePosition(document.PageSize.Width - 580f, document.PageSize.Height - heightCharts1stPage);
                                            //Restando para izq             //Restando para abajo
                document.Add(image);
                #endregion
                #endregion
                #region Evaluación #2
                #region Obteniendo las notas
                listaNotas = evaluacion2.Scores;
                #endregion
                #region Si hay notas
                if (listaNotas.Count != 0)
                {
                    #region Cáclulo de # aprobados y reprobados
                    listaEstudiantesAprobados = new List<Student>();
                    listaEstudiantesReprobados = new List<Student>();
                    foreach (Score scoreAux in listaNotas)
                    {
                        #region Bachillerato
                        if (grado > 6)
                        {
                            if (scoreAux.NumberScore >= 10)
                                listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                            else
                                listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                        }
                        #endregion
                        #region Primaria
                        else
                        {
                            if (scoreAux.ToIntLetterScore(scoreAux.LetterScore) > 1)
                                listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                            else
                                listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                        }
                        #endregion
                    }
                    nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                    data = new Dictionary<string, int> { 
                    { "Aprob.:", listaEstudiantesAprobados.Count() }, 
                    { "Reprob.:", nroEstudiantesReprobados } };
                    #endregion
                    #region Dibujando el gráfico
                    chart = new Chart();
                    chart.AntiAliasing = AntiAliasingStyles.All;
                    chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                    chart.Width = 180;
                    chart.Height = 180;

                    chartArea = new ChartArea();
                    chart.ChartAreas.Add(chartArea);

                    chartSeries = new Series();
                    chartSeries.ChartType = SeriesChartType.Pie;
                    chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
                    chartSeries.BorderColor = Color.Black;
                    chartSeries.BorderWidth = 2;
                    chartSeries.LabelForeColor = Color.White;

                    foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                    chartSeries.Points[0].Color = Color.Blue;
                    chartSeries.Points[1].Color = Color.Red;
                    chart.Series.Add(chartSeries);
                    #endregion
                    #region Definiendo un Memory Stream para guardar la imagen
                    memoryStream = new MemoryStream();
                    chart.SaveImage(memoryStream, ChartImageFormat.Png);
                    //chart.SaveImage(reportRemains_PieChart2_path, ChartImageFormat.Png);
                    #endregion
                    #region Obteniendo imagen desde MemoryStream
                    img1 = System.Drawing.Image.FromStream(memoryStream);
                    memoryStream.Close();
                    #endregion
                    #region Transformando imagen a array binario
                    stream1 = new MemoryStream();
                    img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
                    imageByte1 = stream1.ToArray();
                    stream1.Dispose();
                    img1.Dispose();
                    #endregion
                    #region Obteniendo imagen desde array binario
                    //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart2_path);
                    image = iTextSharp.text.Image.GetInstance(imageByte1);
                    memoryStream.Close();
                    #endregion                                        
                }
                #endregion
                #region No hay notas
                else
                    image = iTextSharp.text.Image.GetInstance(noData_path);
                #endregion
                #region Añadiendo imagen
                image.Alignment = Element.ALIGN_CENTER;
                image.SetAbsolutePosition(document.PageSize.Width - 400f, document.PageSize.Height - heightCharts1stPage);
                                          //Restando para izq             //Restando para abajo
                document.Add(image);
                #endregion
                #endregion
                #region Evaluación #3
                #region Obteniendo las notas
                listaNotas = evaluacion3.Scores;
                #endregion
                #region Si hay notas
                if (listaNotas.Count != 0)
                {
                    #region Cáclulo de # aprobados y reprobados
                    listaEstudiantesAprobados = new List<Student>();
                    listaEstudiantesReprobados = new List<Student>();
                    foreach (Score scoreAux in listaNotas)
                    {
                        #region Bachillerato
                        if (grado > 6)
                        {
                            if (scoreAux.NumberScore >= 10)
                                listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                            else
                                listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                        }
                        #endregion
                        #region Primaria
                        else
                        {
                            if (scoreAux.ToIntLetterScore(scoreAux.LetterScore) > 1)
                                listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                            else
                                listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                        }
                        #endregion
                    }
                    nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                    data = new Dictionary<string, int> { 
                    { "Aprob.:", listaEstudiantesAprobados.Count() }, 
                    { "Reprob.:", nroEstudiantesReprobados } };
                    #endregion
                    #region Dibujando el gráfico
                    chart = new Chart();
                    chart.AntiAliasing = AntiAliasingStyles.All;
                    chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                    chart.Width = 180;
                    chart.Height = 180;

                    chartArea = new ChartArea();
                    chart.ChartAreas.Add(chartArea);

                    chartSeries = new Series();
                    chartSeries.ChartType = SeriesChartType.Pie;
                    chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
                    chartSeries.BorderColor = Color.Black;
                    chartSeries.BorderWidth = 2;
                    chartSeries.LabelForeColor = Color.White;

                    foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                    chartSeries.Points[0].Color = Color.Blue;
                    chartSeries.Points[1].Color = Color.Red;
                    chart.Series.Add(chartSeries);
                    #endregion
                    #region Definiendo un Memory Stream para guardar la imagen
                    memoryStream = new MemoryStream();
                    chart.SaveImage(memoryStream, ChartImageFormat.Png);
                    //chart.SaveImage(reportRemains_PieChart3_path, ChartImageFormat.Png);
                    #endregion
                    #region Obteniendo imagen desde MemoryStream
                    img1 = System.Drawing.Image.FromStream(memoryStream);
                    memoryStream.Close();
                    #endregion
                    #region Transformando imagen a array binario
                    stream1 = new MemoryStream();
                    img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
                    imageByte1 = stream1.ToArray();
                    stream1.Dispose();
                    img1.Dispose();
                    #endregion
                    #region Obteniendo imagen desde array binario
                    //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart3_path);
                    image = iTextSharp.text.Image.GetInstance(imageByte1);
                    memoryStream.Close();
                    #endregion                                        
                }
                #endregion
                #region No hay notas
                else
                    image = iTextSharp.text.Image.GetInstance(noData_path);
                #endregion
                #region Añadiendo la imagen
                image.Alignment = Element.ALIGN_CENTER;
                image.SetAbsolutePosition(document.PageSize.Width - 220f, document.PageSize.Height - heightCharts1stPage);
                                          //Restando para izq             //Restando para arriba
                document.Add(image);
                #endregion
                #endregion
                #endregion
                #region Detalles de las evaluaciones
                table = new PdfPTable(3);
                #region Celda #1
                #region Evaluando tiempo de realización
                phrase = new Phrase();
                if (evaluacion1.StartDate == evaluacion1.FinishDate)
                {
                    phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                    phrase.Add(new Chunk(evaluacion1.StartDate.ToShortDateString(), normalFont));
                }
                else
                {
                    phrase.Add(new Chunk("Fechas de desarrollo: ", boldFont));
                    phrase.Add(new Chunk(evaluacion1.StartDate.ToShortDateString() + " - "
                        + evaluacion1.FinishDate.ToShortDateString(), normalFont));
                }
                #endregion

                paragraph = new Paragraph(phrase);
                cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                table.AddCell(cell);
                #endregion
                #region Celda #2
                #region Evaluando tiempo de realización
                phrase = new Phrase();
                if (evaluacion2.StartDate == evaluacion2.FinishDate)
                {
                    phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                    phrase.Add(new Chunk(evaluacion2.StartDate.ToShortDateString(), normalFont));
                }
                else
                {
                    phrase.Add(new Chunk("Fechas de desarrollo: ", boldFont));
                    phrase.Add(new Chunk(evaluacion2.StartDate.ToShortDateString() + " - "
                        + evaluacion2.FinishDate.ToShortDateString(), normalFont));
                }
                #endregion

                paragraph = new Paragraph(phrase);
                cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                table.AddCell(cell);
                #endregion
                #region Celda #3
                #region Evaluando tiempo de realización
                phrase = new Phrase();
                if (evaluacion3.StartDate == evaluacion3.FinishDate)
                {
                    phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                    phrase.Add(new Chunk(evaluacion3.StartDate.ToShortDateString(), normalFont));
                }
                else
                {
                    phrase.Add(new Chunk("Fechas de desarrollo: ", boldFont));
                    phrase.Add(new Chunk(evaluacion3.StartDate.ToShortDateString() + " - " 
                        + evaluacion3.FinishDate.ToShortDateString(), normalFont));
                }
                #endregion

                paragraph = new Paragraph(phrase);
                cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                table.AddCell(cell);
                #endregion

                table.SpacingBefore = 170f;
                table.WidthPercentage = 100f;
                document.Add(table);
                #endregion
            }
            #endregion
            #region Materia con menos de 3 evaluaciones (caso muy particular)
            else 
            {
                #region Obteniendo datos de las evaluaciones
                evaluacion1 = assessmentService.ObtenerEvaluacionPor_Id(listaEvaluaciones[0].AssessmentId);
                try
                {
                    evaluacion2 = assessmentService.ObtenerEvaluacionPor_Id(listaEvaluaciones[1].AssessmentId);
                }
                catch (ArgumentOutOfRangeException)
                {
                    evaluacion2 = null;
                    unaEvaluacion = true;
                };
                #endregion
                #region Títulos de los gráficos
                table = (unaEvaluacion ? new PdfPTable(1) : new PdfPTable(2));
                #region Celda #1
                stringAux = evaluacion1.Name;
                stringAux += (grado > 6 ? " (" + evaluacion1.Percentage + "%)" : "");
                paragraph = new Paragraph(new Phrase(new Chunk(stringAux, chartTitlefont)));
                cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                table.AddCell(cell);
                #endregion
                #region Celda #2
                if(!unaEvaluacion)
                {
                    stringAux = evaluacion2.Name;
                    stringAux += (grado > 6 ? " (" + evaluacion2.Percentage + "%)" : "");
                    paragraph = new Paragraph(new Phrase(new Chunk(stringAux, chartTitlefont)));
                    cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                    table.AddCell(cell);
                }
                #endregion
                table.SpacingAfter = 15f;
                table.WidthPercentage = 100f;
                document.Add(table);
                #endregion
                #region Gráficos de las evaluaciones
                #region Evaluación #1
                #region Obteniendo las notas
                listaNotas = evaluacion1.Scores;
                #endregion
                #region Si hay notas
                if (listaNotas.Count != 0)
                {
                    #region Cáclulo de # aprobados y reprobados
                    listaEstudiantesAprobados = new List<Student>();
                    listaEstudiantesReprobados = new List<Student>();
                    foreach (Score scoreAux in listaNotas)
                    {
                        #region Bachillerato
                        if (grado > 6)
                        {
                            if (scoreAux.NumberScore >= 10)
                                listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                            else
                                listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                        }
                        #endregion
                        #region Primaria
                        else
                        {
                            if (scoreAux.ToIntLetterScore(scoreAux.LetterScore) > 1)
                                listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                            else
                                listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                        }
                        #endregion
                    }
                    nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                    data = new Dictionary<string, int> { 
                    { "Aprob.:", listaEstudiantesAprobados.Count() }, 
                    { "Reprob.:", nroEstudiantesReprobados } };
                    #endregion
                    #region Dibujando el gráfico
                    chart = new Chart();
                    chart.AntiAliasing = AntiAliasingStyles.All;
                    chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                    chart.Width = 180;
                    chart.Height = 180;

                    chartArea = new ChartArea();
                    chart.ChartAreas.Add(chartArea);

                    chartSeries = new Series();
                    chartSeries.ChartType = SeriesChartType.Pie;
                    chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
                    chartSeries.BorderColor = Color.Black;
                    chartSeries.BorderWidth = 2;
                    chartSeries.LabelForeColor = Color.White;

                    foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                    chartSeries.Points[0].Color = Color.Blue;
                    chartSeries.Points[1].Color = Color.Red;
                    chart.Series.Add(chartSeries);
                    chart.SaveImage(reportRemains_PieChart1_path, ChartImageFormat.Png);
                    #endregion

                    image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart1_path);
                }
                #endregion
                #region No hay notas
                else
                    image = iTextSharp.text.Image.GetInstance(noData_path);
                #endregion
                #region Añadiendo imagen
                image.Alignment = Element.ALIGN_CENTER;
                if(unaEvaluacion)
                    image.SetAbsolutePosition(document.PageSize.Width - 400f, document.PageSize.Height - 660f);
                                              //Restando para izq             //Restando para abajo
                else //Dos evaluaciones
                    image.SetAbsolutePosition(document.PageSize.Width - 545f, document.PageSize.Height - 660f);
                document.Add(image);
                #endregion
                #endregion
                #region Evaluación #2
                if(!unaEvaluacion)
                {
                    #region Obteniendo las notas
                    listaNotas = evaluacion2.Scores;
                    #endregion
                    #region Si hay notas
                    if (listaNotas.Count != 0)
                    {
                        #region Cáclulo de # aprobados y reprobados
                        listaEstudiantesAprobados = new List<Student>();
                        listaEstudiantesReprobados = new List<Student>();
                        foreach (Score scoreAux in listaNotas)
                        {
                            #region Bachillerato
                            if (grado > 6)
                            {
                                if (scoreAux.NumberScore >= 10)
                                    listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                                else
                                    listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                            }
                            #endregion
                            #region Primaria
                            else
                            {
                                if (scoreAux.ToIntLetterScore(scoreAux.LetterScore) > 1)
                                    listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                                else
                                    listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                            }
                            #endregion
                        }
                        nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                        data = new Dictionary<string, int> { 
                            { "Aprob.:", listaEstudiantesAprobados.Count() }, 
                            { "Reprob.:", nroEstudiantesReprobados } };
                        #endregion
                        #region Dibujando el gráfico
                        chart = new Chart();
                        chart.AntiAliasing = AntiAliasingStyles.All;
                        chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                        chart.Width = 180;
                        chart.Height = 180;

                        chartArea = new ChartArea();
                        chart.ChartAreas.Add(chartArea);

                        chartSeries = new Series();
                        chartSeries.ChartType = SeriesChartType.Pie;
                        chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
                        chartSeries.BorderColor = Color.Black;
                        chartSeries.BorderWidth = 2;
                        chartSeries.LabelForeColor = Color.White;

                        foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                        chartSeries.Points[0].Color = Color.Blue;
                        chartSeries.Points[1].Color = Color.Red;
                        chart.Series.Add(chartSeries);
                        chart.SaveImage(reportRemains_PieChart2_path, ChartImageFormat.Png);
                        #endregion

                        image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart2_path);
                    }
                    #endregion
                    #region No hay notas
                    else
                        image = iTextSharp.text.Image.GetInstance(noData_path);
                    #endregion
                    #region Añadiendo imagen
                    image.Alignment = Element.ALIGN_CENTER;
                    image.SetAbsolutePosition(document.PageSize.Width - 255f, document.PageSize.Height - 660f);
                    //Restando para izq             //Restando para abajo
                    document.Add(image);
                    #endregion
                }
                #endregion
                #endregion
                #region Detalles de las evaluaciones
                table = (unaEvaluacion ? new PdfPTable(1) : new PdfPTable(2));
                #region Celda #1
                #region Evaluando tiempo de realización
                phrase = new Phrase();
                if (evaluacion1.StartDate == evaluacion1.FinishDate)
                {
                    phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                    phrase.Add(new Chunk(evaluacion1.StartDate.ToShortDateString(), normalFont));
                }
                else
                {
                    phrase.Add(new Chunk("Fechas de desarrollo: ", boldFont));
                    phrase.Add(new Chunk(evaluacion1.StartDate.ToShortDateString() + " - "
                        + evaluacion1.FinishDate.ToShortDateString(), normalFont));
                }
                #endregion

                paragraph = new Paragraph(phrase);
                cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                table.AddCell(cell);
                #endregion
                #region Celda #2
                if(!unaEvaluacion)
                {
                    #region Evaluando tiempo de realización
                    phrase = new Phrase();
                    if (evaluacion2.StartDate == evaluacion2.FinishDate)
                    {
                        phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                        phrase.Add(new Chunk(evaluacion2.StartDate.ToShortDateString(), normalFont));
                    }
                    else
                    {
                        phrase.Add(new Chunk("Fechas de desarrollo: ", boldFont));
                        phrase.Add(new Chunk(evaluacion2.StartDate.ToShortDateString() + " - "
                            + evaluacion2.FinishDate.ToShortDateString(), normalFont));
                    }
                    #endregion

                    paragraph = new Paragraph(phrase);
                    cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                    table.AddCell(cell);
                }
                #endregion

                table.SpacingBefore = 170f;
                table.WidthPercentage = 100f;
                document.Add(table);
                #endregion
            }
            #endregion
            #endregion
            #endregion
            #region Página #2
            document.NewPage();
            #region Sección VII: Cabecera - 2da página
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Docente encargado: " + docente.Name + " " + docente.LastName,
                whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.SpacingBefore = -18;
            document.Add(paragraph);

            paragraph = new Paragraph("Curso: " + curso.Name + " - " + lapso.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Asignatura: " + materia.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Pág. 2", whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.SpacingBefore = -18;
            document.Add(paragraph);
            #endregion
            #region Sección VIII: Datos de las evaluaciones - Parte II
            paragraph = new Paragraph("Datos de las evaluaciones - Parte II:", boldFont);
            paragraph.SpacingBefore = 15f;
            paragraph.SpacingAfter = 10f;
            document.Add(paragraph);
            #endregion
            #region Sección IX: Gráficos de las evaluaciones - Parte II
            #region Materia con más de 3 evaluaciones
            if (nroEvaluaciones > 3)
            {
                #region Obteniendo datos de las evaluaciones
                #region 4ta evaluación
                evaluacion1 = assessmentService.ObtenerEvaluacionPor_Id(listaEvaluaciones[3].AssessmentId);
                cuatroEvaluaciones = true;
                #endregion
                #region 5ta Evaluación
                try
                {
                    evaluacion2 = assessmentService.ObtenerEvaluacionPor_Id(listaEvaluaciones[4].AssessmentId);
                    cuatroEvaluaciones = false;
                    cincoEvaluaciones = true;
                    evaluacion3 = null;
                }
                catch (ArgumentOutOfRangeException)
                {
                    evaluacion2 = null;
                    evaluacion3 = null;
                    cuatroEvaluaciones = true;
                };
                #endregion
                #region 6ta Evaluación
                try
                {
                    evaluacion3 = assessmentService.ObtenerEvaluacionPor_Id(listaEvaluaciones[5].AssessmentId);
                    cincoEvaluaciones = false;
                    seisEvaluaciones = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    evaluacion3 = null;
                    if(!cuatroEvaluaciones) cincoEvaluaciones = true;
                }
                #endregion
                #endregion
                #region Títulos de los gráficos
                table = (cuatroEvaluaciones ? new PdfPTable(1) : (cincoEvaluaciones ? new PdfPTable(2) : new PdfPTable(3)));
                #region Celda #1
                stringAux = evaluacion1.Name;
                stringAux += (grado > 6 ? " (" + evaluacion1.Percentage + "%)" : "");
                paragraph = new Paragraph(new Phrase(new Chunk(stringAux, chartTitlefont)));
                cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                table.AddCell(cell);
                #endregion
                #region Celda #2
                if (!cuatroEvaluaciones)
                {
                    stringAux = evaluacion2.Name;
                    stringAux += (grado > 6 ? " (" + evaluacion2.Percentage + "%)" : "");
                    paragraph = new Paragraph(new Phrase(new Chunk(stringAux, chartTitlefont)));
                    cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                    table.AddCell(cell);

                    #region Celda #3
                    if (!cincoEvaluaciones)
                    {
                        stringAux = evaluacion3.Name;
                        stringAux += (grado > 6 ? " (" + evaluacion3.Percentage + "%)" : "");
                        paragraph = new Paragraph(new Phrase(new Chunk(stringAux, chartTitlefont)));
                        cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 };
                        table.AddCell(cell);
                    }
                    #endregion
                }
                #endregion
                table.SpacingAfter = 15f;
                table.WidthPercentage = 100f;
                document.Add(table);
                #endregion
                #region Gráficos de las evaluaciones
                #region Evaluación #4
                #region Obteniendo las notas
                listaNotas = evaluacion1.Scores;
                #endregion
                #region Si hay notas
                if (listaNotas.Count != 0)
                {
                    #region Cáclulo de # aprobados y reprobados
                    listaEstudiantesAprobados = new List<Student>();
                    listaEstudiantesReprobados = new List<Student>();
                    foreach (Score scoreAux in listaNotas)
                    {
                        #region Bachillerato
                        if (grado > 6)
                        {
                            if (scoreAux.NumberScore >= 10)
                                listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                            else
                                listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                        }
                        #endregion
                        #region Primaria
                        else
                        {
                            if (scoreAux.ToIntLetterScore(scoreAux.LetterScore) > 1)
                                listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                            else
                                listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                        }
                        #endregion
                    }
                    nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                    data = new Dictionary<string, int> { 
                    { "Aprob.:", listaEstudiantesAprobados.Count() }, 
                    { "Reprob.:", nroEstudiantesReprobados } };
                    #endregion
                    #region Dibujando el gráfico
                    chart = new Chart();
                    chart.AntiAliasing = AntiAliasingStyles.All;
                    chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                    chart.Width = 180;
                    chart.Height = 180;

                    chartArea = new ChartArea();
                    chart.ChartAreas.Add(chartArea);

                    chartSeries = new Series();
                    chartSeries.ChartType = SeriesChartType.Pie;
                    chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
                    chartSeries.BorderColor = Color.Black;
                    chartSeries.BorderWidth = 2;
                    chartSeries.LabelForeColor = Color.White;

                    foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                    chartSeries.Points[0].Color = Color.Blue;
                    chartSeries.Points[1].Color = Color.Red;
                    chart.Series.Add(chartSeries);
                    #endregion
                    #region Definiendo un Memory Stream para guardar la imagen
                    memoryStream = new MemoryStream();
                    chart.SaveImage(memoryStream, ChartImageFormat.Png);
                    //chart.SaveImage(reportRemains_PieChart4_path, ChartImageFormat.Png);
                    #endregion
                    #region Obteniendo imagen desde MemoryStream
                    img1 = System.Drawing.Image.FromStream(memoryStream);
                    memoryStream.Close();
                    #endregion
                    #region Transformando imagen a array binario
                    stream1 = new MemoryStream();
                    img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
                    imageByte1 = stream1.ToArray();
                    stream1.Dispose();
                    img1.Dispose();
                    #endregion
                    #region Obteniendo imagen desde array binario
                    //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart4_path);
                    image = iTextSharp.text.Image.GetInstance(imageByte1);
                    memoryStream.Close();
                    #endregion                                        
                }
                #endregion
                #region No hay notas
                else
                    image = iTextSharp.text.Image.GetInstance(noData_path);
                #endregion
                #region Añadiendo imagen
                image.Alignment = Element.ALIGN_CENTER;
                if(seisEvaluaciones)
                    image.SetAbsolutePosition(document.PageSize.Width - 580f, document.PageSize.Height - heightCharts2ndPage);
                else if (cincoEvaluaciones)
                    image.SetAbsolutePosition(document.PageSize.Width - 545f, document.PageSize.Height - heightCharts2ndPage);
                else if (cuatroEvaluaciones)
                    image.SetAbsolutePosition(document.PageSize.Width - 400f, document.PageSize.Height - heightCharts2ndPage);
                
                document.Add(image);
                #endregion
                #endregion
                #region Evaluación #5 (Adentro está la 6ta evaluación)
                if(cincoEvaluaciones || seisEvaluaciones)
                {
                    #region Obteniendo las notas
                    listaNotas = evaluacion2.Scores;
                    #endregion
                    #region Si hay notas
                    if (listaNotas.Count != 0)
                    {
                        #region Cáclulo de # aprobados y reprobados
                        listaEstudiantesAprobados = new List<Student>();
                        listaEstudiantesReprobados = new List<Student>();
                        foreach (Score scoreAux in listaNotas)
                        {
                            #region Bachillerato
                            if (grado > 6)
                            {
                                if (scoreAux.NumberScore >= 10)
                                    listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                                else
                                    listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                            }
                            #endregion
                            #region Primaria
                            else
                            {
                                if (scoreAux.ToIntLetterScore(scoreAux.LetterScore) > 1)
                                    listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                                else
                                    listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                            }
                            #endregion
                        }
                        nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                        data = new Dictionary<string, int> { 
                    { "Aprob.:", listaEstudiantesAprobados.Count() }, 
                    { "Reprob.:", nroEstudiantesReprobados } };
                        #endregion
                        #region Dibujando el gráfico
                        chart = new Chart();
                        chart.AntiAliasing = AntiAliasingStyles.All;
                        chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                        chart.Width = 180;
                        chart.Height = 180;

                        chartArea = new ChartArea();
                        chart.ChartAreas.Add(chartArea);

                        chartSeries = new Series();
                        chartSeries.ChartType = SeriesChartType.Pie;
                        chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
                        chartSeries.BorderColor = Color.Black;
                        chartSeries.BorderWidth = 2;
                        chartSeries.LabelForeColor = Color.White;

                        foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                        chartSeries.Points[0].Color = Color.Blue;
                        chartSeries.Points[1].Color = Color.Red;
                        chart.Series.Add(chartSeries);
                        #endregion
                        #region Definiendo un Memory Stream para guardar la imagen
                        memoryStream = new MemoryStream();
                        chart.SaveImage(memoryStream, ChartImageFormat.Png);
                        //chart.SaveImage(reportRemains_PieChart5_path, ChartImageFormat.Png);
                        #endregion
                        #region Obteniendo imagen desde MemoryStream
                        img1 = System.Drawing.Image.FromStream(memoryStream);
                        memoryStream.Close();
                        #endregion
                        #region Transformando imagen a array binario
                        stream1 = new MemoryStream();
                        img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
                        imageByte1 = stream1.ToArray();
                        stream1.Dispose();
                        img1.Dispose();
                        #endregion
                        #region Obteniendo imagen desde array binario
                        //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart5_path);
                        image = iTextSharp.text.Image.GetInstance(imageByte1);
                        memoryStream.Close();
                        #endregion
                    }
                    #endregion
                    #region No hay notas
                    else
                        image = iTextSharp.text.Image.GetInstance(noData_path);
                    #endregion
                    #region Añadiendo imagen
                    image.Alignment = Element.ALIGN_CENTER;
                    if (seisEvaluaciones)
                        image.SetAbsolutePosition(document.PageSize.Width - 400f, document.PageSize.Height - heightCharts2ndPage);
                    else if(cincoEvaluaciones)
                        image.SetAbsolutePosition(document.PageSize.Width - 255f, document.PageSize.Height - heightCharts2ndPage);

                    document.Add(image);
                    #endregion

                    #region Evaluación #6
                    if (seisEvaluaciones)
                    {
                        #region Obteniendo las notas
                        listaNotas = evaluacion3.Scores;
                        #endregion
                        #region Si hay notas
                        if (listaNotas.Count != 0)
                        {
                            #region Cáclulo de # aprobados y reprobados
                            listaEstudiantesAprobados = new List<Student>();
                            listaEstudiantesReprobados = new List<Student>();
                            foreach (Score scoreAux in listaNotas)
                            {
                                #region Bachillerato
                                if (grado > 6)
                                {
                                    if (scoreAux.NumberScore >= 10)
                                        listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                                    else
                                        listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                                }
                                #endregion
                                #region Primaria
                                else
                                {
                                    if (scoreAux.ToIntLetterScore(scoreAux.LetterScore) > 1)
                                        listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                                    else
                                        listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                                }
                                #endregion
                            }
                            nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                            data = new Dictionary<string, int> { 
                                { "Aprob.:", listaEstudiantesAprobados.Count() }, 
                                { "Reprob.:", nroEstudiantesReprobados } };
                            #endregion
                            #region Dibujando el gráfico
                            chart = new Chart();
                            chart.AntiAliasing = AntiAliasingStyles.All;
                            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                            chart.Width = 180;
                            chart.Height = 180;

                            chartArea = new ChartArea();
                            chart.ChartAreas.Add(chartArea);

                            chartSeries = new Series();
                            chartSeries.ChartType = SeriesChartType.Pie;
                            chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
                            chartSeries.BorderColor = Color.Black;
                            chartSeries.BorderWidth = 2;
                            chartSeries.LabelForeColor = Color.White;

                            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                            chartSeries.Points[0].Color = Color.Blue;
                            chartSeries.Points[1].Color = Color.Red;
                            chart.Series.Add(chartSeries);
                            #endregion
                            #region Definiendo un Memory Stream para guardar la imagen
                            memoryStream = new MemoryStream();
                            chart.SaveImage(memoryStream, ChartImageFormat.Png);
                            //chart.SaveImage(reportRemains_PieChart6_path, ChartImageFormat.Png);
                            #endregion
                            #region Obteniendo imagen desde MemoryStream
                            img1 = System.Drawing.Image.FromStream(memoryStream);
                            memoryStream.Close();
                            #endregion
                            #region Transformando imagen a array binario
                            stream1 = new MemoryStream();
                            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
                            imageByte1 = stream1.ToArray();
                            stream1.Dispose();
                            img1.Dispose();
                            #endregion
                            #region Obteniendo imagen desde array binario
                            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart6_path);
                            image = iTextSharp.text.Image.GetInstance(imageByte1);
                            memoryStream.Close();
                            #endregion
                        }
                        #endregion
                        #region No hay notas
                        else
                            image = iTextSharp.text.Image.GetInstance(noData_path);
                        #endregion
                        #region Añadiendo la imagen
                        image.Alignment = Element.ALIGN_CENTER;
                        if(seisEvaluaciones)
                            image.SetAbsolutePosition(document.PageSize.Width - 220f, document.PageSize.Height - heightCharts2ndPage);

                        document.Add(image);
                        #endregion
                    }
                    #endregion
                }
                #endregion
                #endregion
                #region Detalles de las evaluaciones
                table = (seisEvaluaciones ? new PdfPTable(3) : ( cincoEvaluaciones ? new PdfPTable(2) : new PdfPTable(1)));
                #region Celda #1
                #region Evaluando tiempo de realización
                phrase = new Phrase();
                if (evaluacion1.StartDate == evaluacion1.FinishDate)
                {
                    phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                    phrase.Add(new Chunk(evaluacion1.StartDate.ToShortDateString(), normalFont));
                }
                else
                {
                    phrase.Add(new Chunk("Fechas de desarrollo: ", boldFont));
                    phrase.Add(new Chunk(evaluacion1.StartDate.ToShortDateString() + " - "
                        + evaluacion1.FinishDate.ToShortDateString(), normalFont));
                }
                #endregion

                paragraph = new Paragraph(phrase);
                cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                table.AddCell(cell);
                #endregion
                #region Celda #2 (dentro está la Celda #3)
                if(cincoEvaluaciones || seisEvaluaciones)
                {
                    #region Evaluando tiempo de realización
                    phrase = new Phrase();
                    if (evaluacion2.StartDate == evaluacion2.FinishDate)
                    {
                        phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                        phrase.Add(new Chunk(evaluacion2.StartDate.ToShortDateString(), normalFont));
                    }
                    else
                    {
                        phrase.Add(new Chunk("Fechas de desarrollo: ", boldFont));
                        phrase.Add(new Chunk(evaluacion2.StartDate.ToShortDateString() + " - "
                            + evaluacion2.FinishDate.ToShortDateString(), normalFont));
                    }
                    #endregion

                    paragraph = new Paragraph(phrase);
                    cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                    table.AddCell(cell);

                    #region Celda #3
                    if (seisEvaluaciones)
                    {
                        #region Evaluando tiempo de realización
                        phrase = new Phrase();
                        if (evaluacion3.StartDate == evaluacion3.FinishDate)
                        {
                            phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                            phrase.Add(new Chunk(evaluacion3.StartDate.ToShortDateString(), normalFont));
                        }
                        else
                        {
                            phrase.Add(new Chunk("Fechas de desarrollo: ", boldFont));
                            phrase.Add(new Chunk(evaluacion3.StartDate.ToShortDateString() + " - "
                                + evaluacion3.FinishDate.ToShortDateString(), normalFont));
                        }
                        #endregion

                        paragraph = new Paragraph(phrase);
                        cell = new PdfPCell(paragraph) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                        table.AddCell(cell);
                    }
                    #endregion
                }
                #endregion
                
                table.SpacingBefore = 180f;
                table.WidthPercentage = 100f;
                document.Add(table);
                #endregion
            }
            #endregion
            #region Materias con 3 evaluaciones
            //else
                //tresEvaluaciones = true;
            #endregion
            #endregion
            #region Sección X: Gráfico de % aprobados vs reprobados de la materia
            #region Texto preliminar
            phrase = new Phrase();
            phrase.Add(new Chunk("        Según los resultados obtenidos en cada evaluación, el promedio de " + 
                "notas alcanzado en el período escolar establecido (", normalFont));
            phrase.Add(new Chunk(lapso.Name, boldFont));
            phrase.Add(new Chunk("), hasta la fecha de emisión de este reporte, es ", normalFont));
            #region Bachillerato
            if (grado > 6) //Bachillerato
            {
                phrase.Add(new Chunk("de ", normalFont));
                phrase.Add(new Chunk(Math.Round(promedio, 2).ToString() + " puntos", boldFont));
            }
            #endregion
            #region Primaria
            else //Primaria
            {
                if (Math.Round(promedio) == 1) stringAux = "A";
                else if (Math.Round(promedio) == 2) stringAux = "B";
                else if (Math.Round(promedio) == 3) stringAux = "C";
                else if (Math.Round(promedio) == 4) stringAux = "D";
                else if (Math.Round(promedio) == 5) stringAux = "E";

                phrase.Add(new Chunk("el literal " + stringAux, boldFont));
            }
            #endregion
            phrase.Add(new Chunk(". A continuación se presenta el porcentaje de alumnos que aprobaron," +
                " en comparación con aquellos que reprobaron la materia: ", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingBefore = 15;
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Gráfico - Aprobados vs Reprobados
            paragraph = new Paragraph(new Chunk("Gráfica de % de alumnos aprobados vs reprobados - " + 
                materia.Name + " (" + lapso.Name +")", chartTitlefont));
            paragraph.SpacingBefore = 5;
            paragraph.Alignment = Element.ALIGN_CENTER;
            document.Add(paragraph);

            data = new Dictionary<string, int> { 
                    { "Aprobados:", nroAprobadosDefinitiva }, 
                    { "Reprobados:", nroReprobadosDefinitiva } };

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 290;
            chart.Height = 290;

            chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Pie;
            chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
            chartSeries.BorderColor = Color.Black;
            chartSeries.BorderWidth = 2;
            chartSeries.LabelForeColor = Color.White;

            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chartSeries.Points[0].Color = Color.Blue;
            chartSeries.Points[1].Color = Color.Red;
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_PieChart7_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart7_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 450f, document.PageSize.Height - 735f);
            document.Add(image);
            #endregion
            #endregion
            #endregion
            #region Página #3
            document.NewPage();
            #region Sección XI: Cabecera - 3era página
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Docente encargado: " + docente.Name + " " + docente.LastName,
                whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.SpacingBefore = -18;
            document.Add(paragraph);

            paragraph = new Paragraph("Curso: " + curso.Name + " - " + lapso.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Asignatura: " + materia.Name, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Pág. 3", whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.SpacingBefore = -18;
            document.Add(paragraph);
            #endregion
            #region Sección XII: Datos de la materia - Parte II
            paragraph = new Paragraph("Datos de la materia - Parte II:", boldFont);
            paragraph.SpacingBefore = 15;
            document.Add(paragraph);

            paragraph = new Paragraph("        A continuación se indica el listado del top 10 de los" +
                " resultados más destacados y deficientes obtenidos en la materia:", normalFont);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección XIII: Gráficos - Top 10 mejores/peores resultados
            #region Títulos gráficos
            Paragraph tableTitle1 = new Paragraph(new Phrase(new Chunk("Gráfica del top 10 resultados destacados",
                chartTitlefont)));
            Paragraph tableTitle2 = new Paragraph(new Phrase(new Chunk("Gráfica del top 10 resultados deficientes",
                chartTitlefont)));

            PdfPCell cellTitle1 = new PdfPCell(tableTitle1)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0
            };
            PdfPCell cellTitle2 = new PdfPCell(tableTitle2)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0
            };

            PdfPTable titleTable = new PdfPTable(2);
            titleTable.AddCell(cellTitle1);
            titleTable.AddCell(cellTitle2);
            titleTable.SpacingAfter = 15f;
            titleTable.WidthPercentage = 100f;

            document.Add(titleTable);
            #endregion
            #region Gráfico 10 mejores
            Dictionary<int, double> data2 = new Dictionary<int, double>();
            intAux = 1;
            foreach(KeyValuePair<int, double> valor in top10MejoresNotas)
            {
                if(intAux <= 10)
                {
                    data2.Add(intAux, valor.Value);
                    intAux++;
                }
                else
                    break;
            }

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 255;
            chart.Height = 278;

            chartArea = new ChartArea();
            chartArea.AxisY.LabelStyle.Enabled = true;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisY.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisY.IsLabelAutoFit = true;
            chartArea.AxisY.LineColor = Color.Blue;
            chartArea.AxisY.Maximum = (grado > 6 ? 20 : 5);
            chartArea.AxisY.Title = "Notas obtenidas";
            chartArea.AxisY.TitleFont = graphBarChartFontAxisTitle;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.Lavender;
            chartArea.AxisY.MajorGrid.LineWidth = 6;
            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.LineColor = Color.Blue;
            chartArea.AxisY.MinorTickMark.Enabled = false;
            if (grado <= 6)//Primaria
            {
                chartArea.AxisY.CustomLabels.Add(0, 1, "E");
                chartArea.AxisY.CustomLabels.Add(1, 2, "D");
                chartArea.AxisY.CustomLabels.Add(2, 3, "C");
                chartArea.AxisY.CustomLabels.Add(3, 4, "B");
                chartArea.AxisY.CustomLabels.Add(4, 5, "A");
            }

            chartArea.AxisX.LabelStyle.Enabled = true;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisX.LineColor = Color.Blue;
            chartArea.AxisX.Maximum = 11;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.MinorTickMark.Enabled = false;
            chartArea.AxisX.LabelAutoFitMinFontSize = 7;

            intAux = 0; //Variable de control del ciclo
            foreach(KeyValuePair<int, double> valor in top10MejoresNotas)
            {
                if(intAux <= 9)
                {
                    #region Bachillerato
                    if (grado > 6) //Bachillerato
                        stringAux = "(" + Math.Round(valor.Value) + ") ";
                    #endregion
                    #region Primaria
                    else //Primaria
                        stringAux = "";
                    #endregion

                    student = studentService.ObtenerAlumnoPorId(valor.Key);
                    stringAux = stringAux + student.FirstLastName + " " + student.SecondLastName + ", " +
                        student.FirstName;
                    chartArea.AxisX.CustomLabels.Add(intAux + 0.5, intAux + 1.5, stringAux);
                    intAux++;
                }
                else
                    break;
            }

            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Column;
            chartSeries.Color = Color.Blue;
            chartSeries.BorderColor = Color.DarkBlue;
            chartSeries.BackGradientStyle = GradientStyle.LeftRight;
            foreach (var item in data2) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_BarChart1_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_BarChart1_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 570, document.PageSize.Height - 440f);
            document.Add(image);
            #endregion
            #region Gráfico 10 peores
            Dictionary<int, double> data3 = new Dictionary<int, double>();
            intAux = 1;
            foreach(KeyValuePair<int, double> valor in top10PeoresNotas)
            {
                if(intAux <= 10)
                {
                    data3.Add(intAux, valor.Value);
                    intAux++;
                }
                else
                    break;
            }

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 255;
            chart.Height = 278;

            chartArea = new ChartArea();
            chartArea.AxisY.LabelStyle.Enabled = true;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisY.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisY.IsLabelAutoFit = true;
            chartArea.AxisY.LineColor = Color.Red;
            chartArea.AxisY.Maximum = (grado > 6 ? 20 : 5);
            chartArea.AxisY.Title = "Notas obtenidas";
            chartArea.AxisY.TitleFont = graphBarChartFontAxisTitle;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.Lavender;
            chartArea.AxisY.MajorGrid.LineWidth = 6;
            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.LineColor = Color.Red;
            chartArea.AxisY.MinorTickMark.Enabled = false;
            if (grado <= 6)//Primaria
            {
                chartArea.AxisY.CustomLabels.Add(0, 1, "E");
                chartArea.AxisY.CustomLabels.Add(1, 2, "D");
                chartArea.AxisY.CustomLabels.Add(2, 3, "C");
                chartArea.AxisY.CustomLabels.Add(3, 4, "B");
                chartArea.AxisY.CustomLabels.Add(4, 5, "A");
            }

            chartArea.AxisX.LabelStyle.Enabled = true;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisX.LineColor = Color.Red;
            chartArea.AxisX.Maximum = 11;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.MinorTickMark.Enabled = false;
            chartArea.AxisX.LabelAutoFitMinFontSize = 7;

            intAux = 0;
            foreach (KeyValuePair<int, double> valor in top10PeoresNotas)
            {
                if (intAux <= 9)
                {
                    #region Bachillerato
                    if (grado > 6) //Bachillerato
                        stringAux = "(" + Math.Round(valor.Value) + ") ";
                    #endregion
                    #region Primaria
                    else //Primaria
                        stringAux = "";
                    #endregion

                    student = studentService.ObtenerAlumnoPorId(valor.Key);
                    stringAux = stringAux + student.FirstLastName + " " + student.SecondLastName + ", " +
                        student.FirstName;
                    chartArea.AxisX.CustomLabels.Add(intAux + 0.5, intAux + 1.5, stringAux);
                    intAux++;
                }
                else
                    break;
            }
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Column;
            chartSeries.Color = Color.Red;
            chartSeries.BorderColor = Color.DarkRed;
            chartSeries.BackGradientStyle = GradientStyle.LeftRight;
            foreach (var item in data3) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_BarChart2_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_BarChart2_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion

            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 302, document.PageSize.Height - 440f);
            document.Add(image);
            #endregion
            #endregion
            #region Sección XIV: Tabla de alumnos
            #region Texto preliminar
            phrase = new Phrase();
            phrase.Add(new Chunk("        A continuación se presenta la tabla de alumnos con los resultados" + 
                " de sus notas acumuladas, hasta la fecha de emisión de este reporte, en la materia para el ", 
                normalFont));
            phrase.Add(new Chunk(lapso.Name, boldFont));
            phrase.Add(new Chunk(":", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingBefore = 247;
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Table #1
            #region Configuración de la tabla
            table = new PdfPTable(3);
            table.SetWidths(studentTableWidths);
            table.LockedWidth = true;
            table.TotalWidth = 230;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            #endregion
            #region Configuración de la celda #1
            cell = new PdfPCell(new Phrase(new Chunk("#", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #2
            cell = new PdfPCell(new Phrase(new Chunk("Alumnos", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #3
            cell = new PdfPCell(new Phrase(new Chunk("Nota", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthLeft = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Ciclo de llenado de las celdas
            for (int i = 0; i <= 22; i++)
            {
                #region Celda #1
                stringAux = listaEstudiantes[i].NumberList.ToString();
                cell = new PdfPCell(new Phrase(stringAux, cellFont))
                {
                    BorderWidthLeft = 1.75f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                if (i == 22) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #2
                stringAux = listaEstudiantes[i].FirstLastName + " " + listaEstudiantes[i].SecondLastName +
                    ", " + listaEstudiantes[i].FirstName + " " + listaEstudiantes[i].SecondName;
                cell = new PdfPCell(new Phrase(stringAux, cellFont))
                {
                    BorderWidthLeft = 1.75f,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                if (i == 22) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #3
                intAux = Convert.ToInt32(Math.Round(listaDefinitivas[listaEstudiantes[i].StudentId]));
                #region Bachillerato
                if (grado > 6) //Bachillerato
                {
                    if (intAux >= 10) //Aprobado
                        phrase = new Phrase(new Chunk(intAux.ToString(), cellFont));
                    else //Aplazado
                        phrase = new Phrase(new Chunk(intAux.ToString(), cellFont_red));
                }
                #endregion
                #region Primaria
                else
                {
                    stringAux = "";
                    if (intAux == 1) stringAux = "E";
                    else if (intAux == 2) stringAux = "D";
                    else if (intAux == 3) stringAux = "C";
                    else if (intAux == 4) stringAux = "B";
                    else if (intAux == 5) stringAux = "A";

                    if (intAux >=2) //Aprobado
                        phrase = new Phrase(new Chunk(stringAux, cellFont));
                    else
                        phrase = new Phrase(new Chunk(stringAux, cellFont_red));
                }
                #endregion

                cell = new PdfPCell(phrase)
                {
                    BorderWidthRight = 1.75f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                if (i == 22) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
            }
            #endregion
            document.Add(table);
            #endregion
            #region Table #2
            #region Configuración de la tabla
            table = new PdfPTable(3);
            table.SetWidths(studentTableWidths);
            table.LockedWidth = true;
            table.TotalWidth = 230;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            #endregion
            #region Configuración de la celda #1
            cell = new PdfPCell(new Phrase(new Chunk("#", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #2
            cell = new PdfPCell(new Phrase(new Chunk("Alumnos", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #3
            cell = new PdfPCell(new Phrase(new Chunk("Nota", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthLeft = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Ciclo de llenado de las celdas
            for (int i = 23; i <= listaEstudiantes.Count() - 1; i++)
            {
                #region Celda #1
                stringAux = listaEstudiantes[i].NumberList.ToString();
                cell = new PdfPCell(new Phrase(stringAux, cellFont))
                {
                    BorderWidthLeft = 1.75f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                if (i == listaEstudiantes.Count() - 1) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #2
                stringAux = listaEstudiantes[i].FirstLastName + " " + listaEstudiantes[i].SecondLastName +
                    ", " + listaEstudiantes[i].FirstName + " " + listaEstudiantes[i].SecondName;
                cell = new PdfPCell(new Phrase(stringAux, cellFont))
                {
                    BorderWidthLeft = 1.75f,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                if (i == listaEstudiantes.Count() - 1) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #3
                intAux = Convert.ToInt32(Math.Round(listaDefinitivas[listaEstudiantes[i].StudentId]));

                #region Bachillerato
                if (grado > 6) //Bachillerato
                {
                    if (intAux >= 10) //Aprobado
                        phrase = new Phrase(new Chunk(intAux.ToString(), cellFont));
                    else //Aplazado
                        phrase = new Phrase(new Chunk(intAux.ToString(), cellFont_red));
                }
                #endregion
                #region Primaria
                else
                {
                    stringAux = "";
                    if (intAux == 1) stringAux = "E";
                    else if (intAux == 2) stringAux = "D";
                    else if (intAux == 3) stringAux = "C";
                    else if (intAux == 4) stringAux = "B";
                    else if (intAux == 5) stringAux = "A";

                    if (intAux >= 2) //Aprobado
                        phrase = new Phrase(new Chunk(stringAux, cellFont));
                    else
                        phrase = new Phrase(new Chunk(stringAux, cellFont_red));
                }
                #endregion
                
                cell = new PdfPCell(phrase)
                {
                    BorderWidthRight = 1.75f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                if (i == listaEstudiantes.Count() - 1) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
            }
            #endregion
            #region Configuración final de la tabla #2
            paragraph = new Paragraph();
            paragraph.Add(table);
            paragraph.SpacingBefore = -285;
            #endregion
            document.Add(paragraph);
            #endregion
            #endregion
            #endregion
            #region Pasos finales
            document.Close();

            jsonResult.Add(new
            {
                success = true,
                path = pdfFile_ServerSide_path
            });

            TempData["Materia"] = true;
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ReportePorCurso(int idCurso)
        {
            ObteniendoSesion();

            #region Declaración de variables
            List<object> jsonResult = new List<object>();

            #region Variables utilitarias
            MemoryStream memoryStream;
            System.Drawing.Image img1;
            MemoryStream stream1;
            byte[] imageByte1;

            int intAux = 0;
            iTextSharp.text.Image image;
            Chart chart;
            ChartArea chartArea;
            Series chartSeries;
            Phrase phrase;
            Paragraph paragraph;
            PdfPCell cell;
            PdfPTable table;
            Assessment assessmentAux;
            string stringTitle;
            string stringAux;
            float acumulativoNotas_lapso1 = 0;            
            float acumulativoNotas_lapso2 = 0;
            float acumulativoNotas_lapso3 = 0;
            float promedioPorMateria_lapso1 = 0;
            float promedioPorMateria_lapso2 = 0;
            float promedioPorMateria_lapso3 = 0;            
            float floatAux3_lapso1 = 0;
            float floatAux3_lapso2 = 0;
            float floatAux3_lapso3 = 0;
            int[] studentTableWidths2 = new int[] { 25, 55, 110, 110, 110 };
            int[] studentTableWidths = new int[] { 30, 160, 40 };
            List<Student> listaEstudiantesAprobados;
            List<Student> listaEstudiantesReprobados;
            int nroEstudiantesAprobados = 0;
            int nroEstudiantesReprobados = 0;
            int widthHeight_imgSize = 90;
            float valorParaheightCharts_1eraLinea = 190f;
            float heightCharts_1eraLinea = valorParaheightCharts_1eraLinea;
            float heightDistance = 90f;
            float widthCharts_1 = 350f;
            Student student;
            Dictionary<int, double> listaDefinitivas;
            Dictionary<int, double> listaPromediosPorEvaluacion = new Dictionary<int,double>();
            int nroPagina = 3;            
            #endregion
            #region Variables de datos
            #region Variables de servicios
            CourseService courseService = new CourseService();
            CASUService casuService = new CASUService();
            UserService userService = new UserService();
            StudentService studentService = new StudentService();
            AssessmentService assessmentService = new AssessmentService();
            ScoreService scoreService = new ScoreService();
            #endregion            
            #region Variables obtenidas por el curso
            Course curso = courseService.ObtenerCursoPor_Id(idCurso);
            int grado = curso.Grade;
            List<Student> listaEstudiantes = curso.Students.OrderBy(m => m.NumberList).ToList();            
            #endregion
            #region Variables obtenidas por la sesión
            User usuario = userService.ObtenerUsuarioPorId(_session.USERID);
            #endregion
            #region Variables calculadas desde servicios
            Dictionary<int, double> diccionarioDefinitivas = scoreService.ObtenerDefinitivaPor_Curso(curso.CourseId);
            #endregion
            #region Variables calculadas
            #region Top 10 mejores/peores definitivas por curso
            Dictionary<int, double> top10MejoresNotas = new Dictionary<int,double>();
            Dictionary<int, double> top10PeoresNotas = new Dictionary<int, double>();

            int j = 1;
            foreach (KeyValuePair<int, double> valor in diccionarioDefinitivas.OrderByDescending(m => m.Value))
            {
                if (j <= 10)
                    top10MejoresNotas.Add(valor.Key, valor.Value);
                else
                    break;
                j++;
            }

            j = 1;
            foreach (KeyValuePair<int, double> valor in diccionarioDefinitivas.OrderBy(m => m.Value))
            {
                if (j <= 10)
                    top10PeoresNotas.Add(valor.Key, valor.Value);
                else
                    break;
                j++;
            }
            #endregion

            #region Listas de aprobados/reprobados por lapso
            Dictionary<int, double> listaReprobados_1erLapso = new Dictionary<int, double>();
            Dictionary<int, double> listaAprobados_1erLapso = new Dictionary<int, double>();
            Dictionary<int, double> listaReprobados_2doLapso = new Dictionary<int, double>();
            Dictionary<int, double> listaAprobados_2doLapso = new Dictionary<int, double>();
            Dictionary<int, double> listaReprobados_3erLapso = new Dictionary<int, double>();
            Dictionary<int, double> listaAprobados_3erLapso = new Dictionary<int, double>();
            List<Values> listaAprobadosReprobadosDefinitiva = new List<Values>();
            Dictionary<int, int> diccionarioAprobadosReprobadosDefinitiva = new Dictionary<int, int>();            
            #endregion
            #region Cálculo - Inicializando los diccionarios de aprobados/reprobados de los lapsos
            foreach (Student studentAux in listaEstudiantes.OrderBy(m => m.NumberList))
            {
                listaReprobados_1erLapso.Add(studentAux.StudentId, 0);
                listaAprobados_1erLapso.Add(studentAux.StudentId, 0);
                listaReprobados_2doLapso.Add(studentAux.StudentId, 0);
                listaAprobados_2doLapso.Add(studentAux.StudentId, 0);
                listaReprobados_3erLapso.Add(studentAux.StudentId, 0);
                listaAprobados_3erLapso.Add(studentAux.StudentId, 0);
                diccionarioAprobadosReprobadosDefinitiva.Add(studentAux.StudentId, 0);

                foreach (CASU casu in curso.CASUs)
                {
                    CASU casuAux = casuService.ObtenerCASUPor_Ids(casu.CourseId, casu.PeriodId, casu.SubjectId);

                    if(casuAux.Period.Name.Equals("1er Lapso"))
                        listaAprobadosReprobadosDefinitiva.Add(new Values(studentAux.StudentId, 
                            casu.SubjectId, 0)); 
                }
            }
            #endregion

            #region Listas de CASUS por lapso
            List<CASU> listaCASUS_lapso1 = new List<CASU>();
            List<CASU> listaCASUS_lapso2 = new List<CASU>();
            List<CASU> listaCASUS_lapso3 = new List<CASU>();
            #endregion
            #region Colegio, Año escolar y lapsos
            School colegio = new School();
            SchoolYear anoEscolar = new SchoolYear();
            Period lapso1 = new Period();
            Period lapso2 = new Period();
            Period lapso3 = new Period();
            #endregion
            #region # de evaluaciones por lapso
            int nroEvaluaciones_lapso1 = 0;
            int nroEvaluaciones_lapso2 = 0;
            int nroEvaluaciones_lapso3 = 0;
            #endregion
            #region # de evaluaciones realizadas por lapso
            int nroEvaluacionesRealizadas_lapso1 = 0;
            int nroEvaluacionesRealizadas_lapso2 = 0;
            int nroEvaluacionesRealizadas_lapso3 = 0;
            #endregion
            #region Promedio del curso por lapso
            double promedio_lapso1 = 0;
            double promedio_lapso2 = 0;
            double promedio_lapso3 = 0;
            #endregion
            #region # aprobados/reprobados - definitiva
            int nroEstudiantesAprobados_Definitivas = 0;
            int nroEstudiantesReprobados_Definitivas = 0;
            #endregion
            #region Cálculo - Ciclo de obtención de datos de la lista de CASUS
            foreach (CASU casu in curso.CASUs)
            {
                #region Obteniendo el CASU respectivo
                CASU casuAux = casuService.ObtenerCASUPor_Ids(casu.CourseId, casu.PeriodId, casu.SubjectId);
                #endregion
                #region Obteniendo el colegio & año escolar
                colegio = casuAux.Period.SchoolYear.School;
                anoEscolar = casuAux.Period.SchoolYear;
                #endregion
                #region Obteniendo la lista de definitivas del casu respectivo
                listaDefinitivas = scoreService.ObtenerDefinitivasPor_CASU(casuAux);
                #endregion

                #region Datos para el 1er Lapso
                if (casuAux.Period.Name.Equals("1er Lapso"))
                {
                    #region Obteniendo # de evaluaciones
                    nroEvaluaciones_lapso1 += casuAux.Assessments.Count();
                    #endregion
                    #region Obteniendo el lapso
                    lapso1 = casuAux.Period;
                    #endregion
                    #region Agregando el casu a la lista de casus del 1er lapso
                    listaCASUS_lapso1.Add(casuAux);
                    #endregion
                    #region Determinando # de aprobados/reprobados
                    foreach (KeyValuePair<int, double> valor in listaDefinitivas)
                    {
                        listaAprobadosReprobadosDefinitiva.Where(m => m.idEstudiante == valor.Key
                            && m.idMateria == casuAux.SubjectId).FirstOrDefault().acumulado +=
                            valor.Value;

                        if (Math.Round(valor.Value, 2) <= 9) //Reprobados
                            listaReprobados_1erLapso[valor.Key]++;                            
                        else //Aprobados
                            listaAprobados_1erLapso[valor.Key]++;
                    }
                    #endregion
                    #region Ciclo por cada evaluación
                    foreach (Assessment assessment in casuAux.Assessments)
                    {
                        #region Obteniendo la evaluación
                        assessmentAux = assessmentService.ObtenerEvaluacionPor_Id(assessment.AssessmentId);
                        #endregion
                        if (assessmentAux.Scores.Count() != 0)
                        {
                            #region Definiendo # de evaluaciones realizadas
                            nroEvaluacionesRealizadas_lapso1++;
                            #endregion
                            #region Cálculo del promedio #1 - Ciclo de notas
                            foreach (Score score in assessmentAux.Scores)
                            {
                                acumulativoNotas_lapso1 += score.NumberScore;
                            }
                            #region Llenando la lista de evaluaciones con sus promedios respectivos
                            listaPromediosPorEvaluacion.Add(assessmentAux.AssessmentId, 
                                (double)acumulativoNotas_lapso1 / assessmentAux.Scores.Count());
                            #endregion
                            promedioPorMateria_lapso1 += 
                                (float)acumulativoNotas_lapso1 / assessmentAux.Scores.Count();
                            acumulativoNotas_lapso1 = 0;
                            #endregion
                        }
                    }
                    #endregion
                    #region Cálculo del promedio #2 - Valor por evaluaciones
                    if (promedioPorMateria_lapso1 != 0)
                        floatAux3_lapso1 += (float)promedioPorMateria_lapso1 / casuAux.Assessments.Count();
                    promedioPorMateria_lapso1 = 0;
                    #endregion
                }
                #endregion
                #region Datos para el 2do Lapso
                if (casuAux.Period.Name.Equals("2do Lapso"))
                {
                    #region Obteniendo # de evaluaciones
                    nroEvaluaciones_lapso2 += casuAux.Assessments.Count();
                    #endregion
                    #region Obteniendo el lapso
                    lapso2 = casuAux.Period;
                    #endregion
                    #region Agregando el casu a la lista de casus del 2do lapso
                    listaCASUS_lapso2.Add(casuAux);
                    #endregion
                    #region Determinando # de aprobados/reprobados
                    foreach (KeyValuePair<int, double> valor in listaDefinitivas)
                    {
                        listaAprobadosReprobadosDefinitiva.Where(m => m.idEstudiante == valor.Key
                            && m.idMateria == casuAux.SubjectId).FirstOrDefault().acumulado +=
                            valor.Value;

                        if (Math.Round(valor.Value, 2) <= 9) //Reprobados
                            listaReprobados_2doLapso[valor.Key]++;
                        else //Aprobados
                            listaAprobados_2doLapso[valor.Key]++;
                    }
                    #endregion
                    #region Ciclo por cada evaluación
                    foreach (Assessment assessment in casuAux.Assessments)
                    {
                        #region Obteniendo la evaluación
                        assessmentAux = assessmentService.ObtenerEvaluacionPor_Id(assessment.AssessmentId);
                        #endregion
                        if (assessmentAux.Scores.Count() != 0)
                        {
                            #region Definiendo # de evaluaciones realizadas
                            nroEvaluacionesRealizadas_lapso2++;
                            #endregion
                            #region Cálculo del promedio #1 - Ciclo de notas
                            foreach (Score score in assessmentAux.Scores)
                            {
                                acumulativoNotas_lapso2 += score.NumberScore;
                            }
                            #region Llenando la lista de evaluaciones con sus promedios respectivos
                            listaPromediosPorEvaluacion.Add(assessmentAux.AssessmentId,
                                (double)acumulativoNotas_lapso2 / assessmentAux.Scores.Count());
                            #endregion
                            promedioPorMateria_lapso2 +=
                                (float)acumulativoNotas_lapso2 / assessmentAux.Scores.Count();
                            acumulativoNotas_lapso2 = 0;
                            #endregion
                        }
                    }
                    #endregion
                    #region Cálculo del promedio #2 - Valor por evaluaciones
                    if (promedioPorMateria_lapso2 != 0)
                        floatAux3_lapso2 += (float)promedioPorMateria_lapso2 / casuAux.Assessments.Count();
                    promedioPorMateria_lapso2 = 0;
                    #endregion
                }
                #endregion
                #region Datos para el 3er Lapso
                if (casuAux.Period.Name.Equals("3er Lapso"))
                {
                    #region Obteniendo # de evaluaciones
                    nroEvaluaciones_lapso3 += casuAux.Assessments.Count();
                    #endregion
                    #region Obteniendo el lapso
                    lapso3 = casuAux.Period;
                    #endregion
                    #region Agregando el casu a la lista de casus del 3er lapso
                    listaCASUS_lapso3.Add(casuAux);
                    #endregion
                    #region Determinando # de aprobados/reprobados
                    foreach (KeyValuePair<int, double> valor in listaDefinitivas)
                    {
                        listaAprobadosReprobadosDefinitiva.Where(m => m.idEstudiante == valor.Key
                            && m.idMateria == casuAux.SubjectId).FirstOrDefault().acumulado +=
                            valor.Value;

                        if (Math.Round(valor.Value, 2) <= 9) //Reprobados
                            listaReprobados_3erLapso[valor.Key]++;
                        else //Aprobados
                            listaAprobados_3erLapso[valor.Key]++;
                    }
                    #endregion
                    #region Ciclo por cada evaluación
                    foreach (Assessment assessment in casuAux.Assessments)
                    {
                        #region Obteniendo la evaluación
                        assessmentAux = assessmentService.ObtenerEvaluacionPor_Id(assessment.AssessmentId);
                        #endregion
                        if (assessmentAux.Scores.Count() != 0)
                        {
                            #region Definiendo # de evaluaciones realizadas
                            nroEvaluacionesRealizadas_lapso3++;
                            #endregion
                            #region Cálculo del promedio #1 - Ciclo de notas
                            foreach (Score score in assessmentAux.Scores)
                            {
                                acumulativoNotas_lapso3 += score.NumberScore;
                            }
                            #region Llenando la lista de evaluaciones con sus promedios respectivos
                            listaPromediosPorEvaluacion.Add(assessmentAux.AssessmentId,
                                (double)acumulativoNotas_lapso2 / assessmentAux.Scores.Count());
                            #endregion
                            promedioPorMateria_lapso2 +=
                                (float)acumulativoNotas_lapso2 / assessmentAux.Scores.Count();
                            acumulativoNotas_lapso2 = 0;
                            #endregion
                        }
                    }
                    #endregion
                    #region Cálculo del promedio #2 - Valor por evaluaciones
                    if (promedioPorMateria_lapso3 != 0)
                        floatAux3_lapso3 += (float)promedioPorMateria_lapso3 / casuAux.Assessments.Count();
                    promedioPorMateria_lapso3 = 0;
                    #endregion
                }
                #endregion
            }
            #endregion
            #region Cálculo - Definiendo aprobados/reprobados en definitiva
            foreach(Values valor in listaAprobadosReprobadosDefinitiva)
            {
                valor.acumulado = (double)valor.acumulado / 3;
                if (valor.acumulado < 29)
                    diccionarioAprobadosReprobadosDefinitiva[valor.idEstudiante]++;
            }

            foreach (KeyValuePair<int, int> valor in diccionarioAprobadosReprobadosDefinitiva)
            {
                if (valor.Value >= 3)
                    nroEstudiantesReprobados_Definitivas++;
            }
            nroEstudiantesAprobados_Definitivas = listaEstudiantes.Count() - nroEstudiantesReprobados_Definitivas;

            #endregion
            #region Cálculo - Definiendo el promedio de cada lapso
            promedio_lapso1 = Math.Round((float)floatAux3_lapso1 / listaCASUS_lapso1.Count(), 2);
            promedio_lapso2 = Math.Round((float)floatAux3_lapso2 / listaCASUS_lapso2.Count(), 2);
            promedio_lapso3 = Math.Round((float)floatAux3_lapso3 / listaCASUS_lapso3.Count(), 2);
            #endregion                     

            #region # de aprobados/reprobados por lapso
            int nroReprobadosLapso1 = 0;
            int nroReprobadosLapso2 = 0;
            int nroReprobadosLapso3 = 0;
            int nroAprobadosLapso1 = 0;
            int nroAprobadosLapso2 = 0;
            int nroAprobadosLapso3 = 0;
            #endregion
            #region Cálculo - Definiendo # de aprobados/reprobados en lapso I
            foreach (KeyValuePair<int, double> valor in listaReprobados_1erLapso)
            {
                if (valor.Value >= 3)
                    nroReprobadosLapso1++;
                else
                    nroAprobadosLapso1++;
            }
            nroAprobadosLapso1 = listaEstudiantes.Count - nroReprobadosLapso1;
            #endregion
            #region Cálculo - Definiendo # de aprobados/reprobados en lapso II
            foreach (KeyValuePair<int, double> valor in listaReprobados_2doLapso)
            {
                if (valor.Value >= 3)
                    nroReprobadosLapso2++;
                else
                    nroAprobadosLapso2++;
            }
            nroAprobadosLapso2 = listaEstudiantes.Count - nroReprobadosLapso2;
            #endregion
            #region Cálculo - Definiendo # de aprobados/reprobados en lapso III
            foreach (KeyValuePair<int, double> valor in listaReprobados_3erLapso)
            {
                if (valor.Value >= 3)
                    nroReprobadosLapso3++;
                else
                    nroAprobadosLapso3++;
            }
            nroAprobadosLapso3 = listaEstudiantes.Count - nroReprobadosLapso3;
            #endregion
            #endregion
            #endregion
            #region Variables de rutas
            string headerBackground_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_HEADER_BACKGROUND);
            string logo_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_LOGO);
            string subHeaderBackground_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_SUBHEADER_BACKGROUND);

            //string ServerSide_name = "ReportePorCurso" + "C" + _session.SCHOOLID + "Y" +
                //_session.SCHOOLYEARID + "C" + idCurso.ToString() + "U" + _session.USERID + "-" +
                //DateTime.Now.ToString("yyyy-MM-dd") + "H" + DateTime.Now.ToString("HH-mm-ss");
            string ServerSide_name = "ReportePorCurso";

            string pdfFile_ServerSide_name = ServerSide_name + ".pdf";
            string pdfFile_ServerSide_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_DOWNLOAD_DIRECTORY), pdfFile_ServerSide_name);
            string reportRemains_PieChart1_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_PieChart1_Curso.png");
            string reportRemains_BarChart1_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_BarChart1.png");
            string reportRemains_BarChart2_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_SERVER_REMAINS_DIRECTORY), ServerSide_name + "_BarChart2.png");
            string noData_path = Path.Combine(Server.MapPath(ConstantRepository.REPORT_UTILITIES_DIRECTORY), ConstantRepository.REPORT_NODATA_90);
            #endregion
            #region Variables de fuentes
            iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.WHITE);
            iTextSharp.text.Font titleContentFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13); titleContentFont.SetStyle("underline");
            iTextSharp.text.Font chartTitlefont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12); chartTitlefont.SetStyle("underline");

            iTextSharp.text.Font whiteBoldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
            iTextSharp.text.Font whiteFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.WHITE);

            iTextSharp.text.Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            iTextSharp.text.Font underlineFont = FontFactory.GetFont(FontFactory.HELVETICA, 12); underlineFont.SetStyle("underline");

            System.Drawing.Font graphBarChartFont = new System.Drawing.Font("Almanac MT", 8);
            System.Drawing.Font graphBarChartFontAxisTitle = new System.Drawing.Font("Helvetica", 8);

            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
            iTextSharp.text.Font cellFont_red = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7, BaseColor.RED);
            iTextSharp.text.Font cell_boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
            iTextSharp.text.Font cell_normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
            #endregion            
            #endregion
            
            #region Configuración del documento
            #region Declarando documento, escritor PDF
            Document document = new Document(PageSize.LETTER, 36, 36, 7, 36);
            FileStream fileStream = new FileStream(pdfFile_ServerSide_path, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
            #endregion
            #region Definiendo metadata
            document.AddAuthor("Faro Atenas, Inc.");

            stringAux = "Reporte emitido para indicar la relación del nro. de alumnos aprobados vs " +
                "reprobados en el curso: " + curso.Name;
            document.AddSubject(stringAux);
            document.AddKeywords("Reporte, Curso, Definitiva, Total, " + curso.Name);
            document.AddCreator("Faro Atenas - Cliente Web");
            document.AddCreationDate();

            stringAux = "Alumnos datos del curso: " + curso.Name;
            document.AddTitle(stringAux);
            #endregion
            document.Open();
            #endregion            
            #region Página #1
            #region Sección I: Cabecera
            image = iTextSharp.text.Image.GetInstance(headerBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            image = iTextSharp.text.Image.GetInstance(logo_path);
            image.ScalePercent(19f);
            image.SetAbsolutePosition(0 + 27f, document.PageSize.Height - 117f);
            document.Add(image);

            stringTitle = "REPORTE POR CURSO - " + curso.Name.ToUpper();
            paragraph = new Paragraph(new Chunk(stringTitle, titleFont));
            paragraph.IndentationLeft = 100f;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            if (stringTitle.Count() >= 42) paragraph.Leading = 22;
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("Fecha de emisión del reporte: ", whiteBoldFont));
            phrase.Add(new Chunk(DateTime.Now.ToShortDateString() + ", " +
                DateTime.Now.ToString("h:mm:ss tt"), whiteFont));
            paragraph = new Paragraph(phrase);
            paragraph.IndentationLeft = 100f;
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("Reporte emitido por: ", whiteBoldFont));
            phrase.Add(new Chunk(usuario.Name + " " + usuario.LastName, whiteFont));
            paragraph = new Paragraph(phrase);
            paragraph.IndentationLeft = 100f;
            paragraph.SpacingAfter = (stringTitle.Count() < 42 ? 59f : 59f - 14f);
            document.Add(paragraph);
            #endregion
            #region Sección II: Sub-cabecera & Título
            #region Sub-cabecera
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height - 126f);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Año escolar: desde " + anoEscolar.StartDate.ToShortDateString() + ", hasta " + 
                anoEscolar.EndDate.ToShortDateString(), whiteBoldFont);
            document.Add(paragraph);
            #endregion
            #region Título
            paragraph = new Paragraph(new Chunk(stringTitle, titleContentFont));
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.SpacingBefore = 35f;
            paragraph.SpacingAfter = 10f;
            document.Add(paragraph);
            #endregion
            #endregion
            #region Sección III: Datos de presentación
            paragraph = new Paragraph("Datos de presentación:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        Reporte emitido para mostrar información relacionada con el " +
                "curso '", normalFont));
            phrase.Add(new Chunk(curso.Name, boldFont));
            phrase.Add(new Chunk("'. Está conformado por un total de ", normalFont));
            phrase.Add(new Chunk(curso.Students.Count().ToString() + " alumnos.", boldFont));
            phrase.Add(new Chunk(" A continuación se presenta la tabla de datos de cada uno de estos alumnos", 
                normalFont));
            phrase.Add(new Chunk(":", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección IV: Tabla de alumnos
            #region Configuración de la tabla
            table = new PdfPTable(5);
            table.SetWidths(studentTableWidths2);
            table.LockedWidth = true;
            table.TotalWidth = 520;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            #endregion
            #region Configuración de la celda #1
            cell = new PdfPCell(new Phrase(new Chunk("#", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #2
            cell = new PdfPCell(new Phrase(new Chunk("# Registro", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #3
            cell = new PdfPCell(new Phrase(new Chunk("Alumno", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthLeft = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #4
            cell = new PdfPCell(new Phrase(new Chunk("Representante #1", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthLeft = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #5
            cell = new PdfPCell(new Phrase(new Chunk("Representante #2", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthLeft = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Ciclo de llenado de las celdas
            Student lastStudent = listaEstudiantes.Last();
            foreach (Student studentAux in listaEstudiantes)
            {
                student = studentService.ObtenerAlumnoPorId(studentAux.StudentId);
                #region Celda #1
                stringAux = student.NumberList.ToString();
                cell = new PdfPCell(new Phrase(stringAux, cellFont))
                {
                    BorderWidthLeft = 1.75f,
                    BorderWidthRight = 1.75f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                if (studentAux.Equals(lastStudent)) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #2
                stringAux = student.RegistrationNumber.ToString();
                cell = new PdfPCell(new Phrase(stringAux, cellFont)) 
                {
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                if (studentAux.Equals(lastStudent)) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #3
                stringAux = student.SecondLastName + " " + student.FirstLastName + ", " + student.FirstName + " " +
                    student.SecondName;
                cell = new PdfPCell(new Phrase(stringAux, cellFont))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                if (studentAux.Equals(lastStudent)) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #4
                try
                {
                    stringAux = student.Representatives[0].SecondLastName + student.Representatives[0].LastName +
                        ", " + student.Representatives[0].Name;
                }
                catch (ArgumentOutOfRangeException)
                {
                    stringAux = "N/A";
                }
                finally
                {
                    cell = new PdfPCell(new Phrase(stringAux, cellFont))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    if (studentAux.Equals(lastStudent)) cell.BorderWidthBottom = 1.75f;
                    table.AddCell(cell);
                }
                #endregion
                #region Celda #5
                try
                {
                    stringAux = student.Representatives[1].SecondLastName + student.Representatives[1].LastName +
                        ", " + student.Representatives[1].Name;
                }
                catch (ArgumentOutOfRangeException)
                {
                    stringAux = "N/A";
                }
                finally
                {
                    cell = new PdfPCell(new Phrase(stringAux, cellFont))
                    {
                        BorderWidthRight = 1.75f,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    if (studentAux.Equals(lastStudent)) cell.BorderWidthBottom = 1.75f;
                    table.AddCell(cell);
                }
                #endregion
            }
            #endregion
            
            document.Add(table);
            #endregion
            #endregion
            #region Página #2
            document.NewPage();
            #region Sección V: Cabecera - 2da página
            #region Cabecera
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Año escolar: desde " + anoEscolar.StartDate.ToShortDateString() + ", hasta " +
                anoEscolar.EndDate.ToShortDateString(), whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Pág. 2", whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            document.Add(paragraph);
            #endregion
            #region Título
            paragraph = new Paragraph(new Chunk(lapso1.Name + " (" + lapso1.StartDate.ToShortDateString() + 
                " - " + lapso1.FinishDate.ToShortDateString() + ")", titleContentFont));
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.SpacingBefore = 10f;
            paragraph.SpacingAfter = 10f;
            document.Add(paragraph);
            #endregion
            #endregion
            #region Sección VII: Datos del 1er Lapso
            paragraph = new Paragraph("Datos del 1er lapso:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        Para el período escolar definido desde el ", normalFont));
            phrase.Add(new Chunk(lapso1.StartDate.ToString("dd") + " de " +
                lapso1.StartDate.ToString("MMMM") + " de " + lapso1.StartDate.ToString("yyyy"), boldFont));
            phrase.Add(new Chunk(", hasta el ", normalFont));
            phrase.Add(new Chunk(lapso1.FinishDate.ToString("dd") + " de " +
                lapso1.FinishDate.ToString("MMMM") + " de " + lapso1.FinishDate.ToString("yyyy"), boldFont));
            phrase.Add(new Chunk(", correspondiente al ", normalFont));
            phrase.Add(new Chunk(lapso1.Name, boldFont));
            phrase.Add(new Chunk(", a la materia se le asignaron ", normalFont));
            phrase.Add(new Chunk(nroEvaluaciones_lapso1.ToString() + " evaluaciones", boldFont));
            phrase.Add(new Chunk(". Hasta la fecha de emisión del reporte se han realizado ", normalFont));
            phrase.Add(new Chunk(nroEvaluacionesRealizadas_lapso1.ToString() + " evaluaciones", boldFont));
            phrase.Add(new Chunk(".", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección VIII: Promedio de notas
            paragraph = new Paragraph("Promedio de notas:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        El promedio de notas alcanzado por el salón, para el 1er Lapso, " + 
                "hasta la fecha de emisión de este reporte, es de ", normalFont));
            phrase.Add(new Chunk(promedio_lapso1.ToString() + " puntos", boldFont));
            phrase.Add(new Chunk(".", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección IX: % Aprobados vs Reprobados
            paragraph = new Paragraph("Porcentaje (%) de aprobados y reprobados:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        Para que un alumno se considere como aplazado en un curso deberá ", normalFont));
            phrase.Add(new Chunk("reprobar tres (3) materias o más, de las que actualmente está cursando", boldFont));
            phrase.Add(new Chunk(". Bajo esta premisa, a continuación se presenta el porcentaje de aprobados" + 
                " y reprobados hasta la fecha de emisión de este reporte, para el 1er Lapso:", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección X: Gráfico de aprobados vs reprobados
            paragraph = new Paragraph(new Chunk("Gráfica de % de alumnos aprobados vs reprobados",
                chartTitlefont));
            paragraph.SpacingBefore = 5;
            paragraph.Alignment = Element.ALIGN_CENTER;
            document.Add(paragraph);

            Dictionary<string, int> data
                = new Dictionary<string, int> { 
                    { "Aprobados:", nroAprobadosLapso1 }, 
                    { "Reprobados:", nroReprobadosLapso1 } };

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 290;
            chart.Height = 290;

            chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Pie;
            chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
            chartSeries.BorderColor = Color.Black;
            chartSeries.BorderWidth = 2;
            chartSeries.LabelForeColor = Color.White;

            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chartSeries.Points[0].Color = Color.Blue;
            chartSeries.Points[1].Color = Color.Red;
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_PieChart1_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart1_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion                        
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 450f, document.PageSize.Height - 640f);
            document.Add(image);
            #endregion
            #endregion
            #region Página #3-n. Gran ciclo de materias por páginas - Lapso I
            foreach (CASU casu in listaCASUS_lapso1)
            {
                document.NewPage();
                #region Sección: Cabecera
                image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
                image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
                document.Add(image);

                stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
                paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
                document.Add(paragraph);

                paragraph = new Paragraph("Año escolar: desde " + anoEscolar.StartDate.ToShortDateString() + 
                    ", hasta " + anoEscolar.EndDate.ToShortDateString(), whiteBoldFont);
                document.Add(paragraph);

                paragraph = new Paragraph("Pág. " + nroPagina.ToString(), whiteBoldFont);
                paragraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraph);
                nroPagina++;
                #endregion
                #region Sección: Datos de las materias
                paragraph = new Paragraph("Datos de la materia - " + casu.Subject.Name + ":", boldFont);
                paragraph.SpacingBefore = 15f;
                paragraph.SpacingAfter = 10f;
                document.Add(paragraph);
                #endregion
                #region Sección: Datos por evaluación
                int contador = 1;
                foreach (Assessment asses in casu.Assessments)
                {
                    #region Validación de 7 materias (esto debe cambiar en post-tesis)
                    if (contador <= 7)
                    {
                        #region Obteniendo la evaluación
                        assessmentAux = assessmentService.ObtenerEvaluacionPor_Id(asses.AssessmentId);
                        #endregion
                        #region Gráfico de % de aprobados/reprobados
                        #region Si hay notas
                        if (assessmentAux.Scores.Count != 0)
                        {
                            #region Cáclulo de # aprobados y reprobados
                            listaEstudiantesAprobados = new List<Student>();
                            listaEstudiantesReprobados = new List<Student>();
                            foreach (Score scoreAux in assessmentAux.Scores)
                            {
                                #region Bachillerato
                                if (grado > 6)
                                {
                                    if (scoreAux.NumberScore >= 10)
                                        listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                                    else
                                        listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                                }
                                #endregion
                            }
                            nroEstudiantesAprobados = listaEstudiantesAprobados.Count();
                            nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                            data = new Dictionary<string, int> { 
                            { "Aprob.:", nroEstudiantesAprobados }, 
                            { "Reprob.:", nroEstudiantesReprobados } };
                            #endregion
                            #region Dibujando el gráfico
                            chart = new Chart();
                            chart.AntiAliasing = AntiAliasingStyles.All;
                            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                            chart.Width = widthHeight_imgSize;
                            chart.Height = widthHeight_imgSize;

                            chartArea = new ChartArea();
                            chart.ChartAreas.Add(chartArea);

                            chartSeries = new Series();
                            chartSeries.ChartType = SeriesChartType.Pie;
                            chartSeries.Label = "#PERCENT{P0}";
                            chartSeries.BorderColor = Color.Black;
                            chartSeries.BorderWidth = 2;
                            chartSeries.LabelForeColor = Color.White;

                            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                            chartSeries.Points[0].Color = Color.Blue;
                            chartSeries.Points[1].Color = Color.Red;
                            chart.Series.Add(chartSeries);
                            #endregion
                            #region Definiendo un Memory Stream para guardar la imagen
                            memoryStream = new MemoryStream();
                            chart.SaveImage(memoryStream, ChartImageFormat.Png);
                            //chart.SaveImage(reportRemains_PieChart1_path, ChartImageFormat.Png);
                            #endregion
                            #region Obteniendo imagen desde MemoryStream
                            img1 = System.Drawing.Image.FromStream(memoryStream);
                            memoryStream.Close();
                            #endregion
                            #region Transformando imagen a array binario
                            stream1 = new MemoryStream();
                            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
                            imageByte1 = stream1.ToArray();
                            stream1.Dispose();
                            img1.Dispose();
                            #endregion
                            #region Obteniendo imagen desde array binario
                            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart1_path);
                            image = iTextSharp.text.Image.GetInstance(imageByte1);
                            memoryStream.Close();
                            #endregion
                        }
                        #endregion
                        #region No hay notas
                        else
                        {
                            image = iTextSharp.text.Image.GetInstance(noData_path);
                            nroEstudiantesAprobados = 0;
                            nroEstudiantesReprobados = 0;
                        }
                        #endregion
                        #region Añadiendo imagen
                        image.Alignment = Element.ALIGN_CENTER;
                        image.SetAbsolutePosition(document.PageSize.Width - widthCharts_1,
                            document.PageSize.Height - heightCharts_1eraLinea);
                        heightCharts_1eraLinea += heightDistance;
                        document.Add(image);
                        #endregion
                        #endregion                        
                        #region Nombre de la evaluación
                        phrase = new Phrase();
                        phrase.Add(new Chunk("Evaluación: ", boldFont));
                        phrase.Add(new Chunk(assessmentAux.Name + " (" + assessmentAux.Percentage + "%)", normalFont));
                        paragraph = new Paragraph(phrase);
                        document.Add(paragraph);
                        #endregion
                        #region # de aprobados/reprobados
                        phrase = new Phrase();
                        phrase.Add(new Chunk("Aprobados: ", boldFont));
                        phrase.Add(new Chunk(nroEstudiantesAprobados.ToString(), normalFont));
                        phrase.Add(new Chunk(" -  Reprobados: ", boldFont));
                        phrase.Add(new Chunk(nroEstudiantesReprobados.ToString(), normalFont));
                        paragraph = new Paragraph(phrase);
                        paragraph.Alignment = Element.ALIGN_RIGHT;
                        paragraph.SpacingBefore = -18f;
                        document.Add(paragraph);
                        #endregion
                        #region Fecha(s) de la evaluación
                        if (assessmentAux.StartDate.Equals(assessmentAux.FinishDate))
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                            phrase.Add(new Chunk(assessmentAux.StartDate.ToShortDateString(), normalFont));
                            paragraph = new Paragraph(phrase);
                            document.Add(paragraph);
                            intAux = 42;
                        }
                        else
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha de desde: ", boldFont));
                            phrase.Add(new Chunk(assessmentAux.StartDate.ToShortDateString(), normalFont));
                            paragraph = new Paragraph(phrase);
                            document.Add(paragraph);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha de hasta: ", boldFont));
                            phrase.Add(new Chunk(assessmentAux.FinishDate.ToShortDateString(), normalFont));
                            paragraph = new Paragraph(phrase);
                            document.Add(paragraph);
                            intAux = 28;
                        }
                        #endregion
                        #region Promedio de la evaluación
                        try
                        {
                            stringAux = Math.Round(listaPromediosPorEvaluacion[assessmentAux.AssessmentId], 2)
                                .ToString();
                            stringAux += " pts.";
                        }
                        catch (KeyNotFoundException)
                        {
                            stringAux = "N/A";
                        }
                        finally
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Promedio: ", boldFont));
                            phrase.Add(new Chunk(stringAux, normalFont));
                            paragraph = new Paragraph(phrase);
                            paragraph.SpacingAfter = intAux;
                            document.Add(paragraph);
                        }
                        #endregion
                    }
                    #endregion
                    contador++;
                }
                #endregion
                heightCharts_1eraLinea = valorParaheightCharts_1eraLinea;
            }
            #endregion
            #region Página #n+1
            document.NewPage();
            #region Sección: abecera
            #region Cabecera
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Año escolar: desde " + anoEscolar.StartDate.ToShortDateString() + ", hasta " +
                anoEscolar.EndDate.ToShortDateString(), whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Pág. " + nroPagina.ToString(), whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            document.Add(paragraph);
            nroPagina++;
            #endregion
            #region Título
            paragraph = new Paragraph(new Chunk(lapso2.Name + " (" + lapso2.StartDate.ToShortDateString() +
                " - " + lapso2.FinishDate.ToShortDateString() + ")", titleContentFont));
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.SpacingBefore = 10f;
            paragraph.SpacingAfter = 10f;
            document.Add(paragraph);
            #endregion
            #endregion
            #region Sección: Datos del 1er Lapso
            paragraph = new Paragraph("Datos del 2do lapso:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        Para el período escolar definido desde el ", normalFont));
            phrase.Add(new Chunk(lapso2.StartDate.ToString("dd") + " de " +
                lapso2.StartDate.ToString("MMMM") + " de " + lapso2.StartDate.ToString("yyyy"), boldFont));
            phrase.Add(new Chunk(", hasta el ", normalFont));
            phrase.Add(new Chunk(lapso2.FinishDate.ToString("dd") + " de " +
                lapso2.FinishDate.ToString("MMMM") + " de " + lapso2.FinishDate.ToString("yyyy"), boldFont));
            phrase.Add(new Chunk(", correspondiente al ", normalFont));
            phrase.Add(new Chunk(lapso2.Name, boldFont));
            phrase.Add(new Chunk(", a la materia se le asignaron ", normalFont));
            phrase.Add(new Chunk(nroEvaluaciones_lapso2.ToString() + " evaluaciones", boldFont));
            phrase.Add(new Chunk(". Hasta la fecha de emisión del reporte se han realizado ", normalFont));
            phrase.Add(new Chunk(nroEvaluacionesRealizadas_lapso2.ToString() + " evaluaciones", boldFont));
            phrase.Add(new Chunk(".", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección: Promedio de notas
            paragraph = new Paragraph("Promedio de notas:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        El promedio de notas alcanzado por el salón, para el 2do Lapso, " +
                "hasta la fecha de emisión de este reporte, es de ", normalFont));
            phrase.Add(new Chunk(promedio_lapso2.ToString() + " puntos", boldFont));
            phrase.Add(new Chunk(".", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección: % Aprobados vs Reprobados
            paragraph = new Paragraph("Porcentaje (%) de aprobados y reprobados:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        Para que un alumno se considere como aplazado en un curso deberá ", normalFont));
            phrase.Add(new Chunk("reprobar tres (3) materias o más, de las que actualmente está cursando", boldFont));
            phrase.Add(new Chunk(". Bajo esta premisa, a continuación se presenta el porcentaje de aprobados" +
                " y reprobados hasta la fecha de emisión de este reporte, para el 2do Lapso:", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección: Gráfico de aprobados vs reprobados
            paragraph = new Paragraph(new Chunk("Gráfica de % de alumnos aprobados vs reprobados",
                chartTitlefont));
            paragraph.SpacingBefore = 5;
            paragraph.Alignment = Element.ALIGN_CENTER;
            document.Add(paragraph);

            data = new Dictionary<string, int> { 
                    { "Aprobados:", nroAprobadosLapso2 }, 
                    { "Reprobados:", nroReprobadosLapso2 } };

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 290;
            chart.Height = 290;

            chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Pie;
            chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
            chartSeries.BorderColor = Color.Black;
            chartSeries.BorderWidth = 2;
            chartSeries.LabelForeColor = Color.White;

            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chartSeries.Points[0].Color = Color.Blue;
            chartSeries.Points[1].Color = Color.Red;
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_PieChart1_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart1_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 450f, document.PageSize.Height - 640f);
            document.Add(image);
            #endregion
            #endregion
            #region Página #n+2-m. Gran ciclo de materias por páginas - Lapso II
            foreach (CASU casu in listaCASUS_lapso2)
            {
                document.NewPage();
                #region Sección: Cabecera
                image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
                image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
                document.Add(image);

                stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
                paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
                document.Add(paragraph);

                paragraph = new Paragraph("Año escolar: desde " + anoEscolar.StartDate.ToShortDateString() +
                    ", hasta " + anoEscolar.EndDate.ToShortDateString(), whiteBoldFont);
                document.Add(paragraph);

                paragraph = new Paragraph("Pág. " + nroPagina.ToString(), whiteBoldFont);
                paragraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraph);
                nroPagina++;
                #endregion
                #region Sección: Datos de las materias
                paragraph = new Paragraph("Datos de la materia - " + casu.Subject.Name + ":", boldFont);
                paragraph.SpacingBefore = 15f;
                paragraph.SpacingAfter = 10f;
                document.Add(paragraph);
                #endregion
                #region Sección: Datos por evaluación
                int contador = 1;
                foreach (Assessment asses in casu.Assessments)
                {
                    #region Validación de 7 materias (esto debe cambiar en post-tesis)
                    if (contador <= 7)
                    {
                        #region Obteniendo la evaluación
                        assessmentAux = assessmentService.ObtenerEvaluacionPor_Id(asses.AssessmentId);
                        #endregion
                        #region Gráfico de % de aprobados/reprobados
                        #region Si hay notas
                        if (assessmentAux.Scores.Count != 0)
                        {
                            #region Cáclulo de # aprobados y reprobados
                            listaEstudiantesAprobados = new List<Student>();
                            listaEstudiantesReprobados = new List<Student>();
                            foreach (Score scoreAux in assessmentAux.Scores)
                            {
                                #region Bachillerato
                                if (grado > 6)
                                {
                                    if (scoreAux.NumberScore >= 10)
                                        listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                                    else
                                        listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                                }
                                #endregion
                            }
                            nroEstudiantesAprobados = listaEstudiantesAprobados.Count();
                            nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                            data = new Dictionary<string, int> { 
                            { "Aprob.:", nroEstudiantesAprobados }, 
                            { "Reprob.:", nroEstudiantesReprobados } };
                            #endregion
                            #region Dibujando el gráfico
                            chart = new Chart();
                            chart.AntiAliasing = AntiAliasingStyles.All;
                            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                            chart.Width = widthHeight_imgSize;
                            chart.Height = widthHeight_imgSize;

                            chartArea = new ChartArea();
                            chart.ChartAreas.Add(chartArea);

                            chartSeries = new Series();
                            chartSeries.ChartType = SeriesChartType.Pie;
                            chartSeries.Label = "#PERCENT{P0}";
                            chartSeries.BorderColor = Color.Black;
                            chartSeries.BorderWidth = 2;
                            chartSeries.LabelForeColor = Color.White;

                            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                            chartSeries.Points[0].Color = Color.Blue;
                            chartSeries.Points[1].Color = Color.Red;
                            chart.Series.Add(chartSeries);
                            #endregion
                            #region Definiendo un Memory Stream para guardar la imagen
                            memoryStream = new MemoryStream();
                            chart.SaveImage(memoryStream, ChartImageFormat.Png);
                            //chart.SaveImage(reportRemains_PieChart1_path, ChartImageFormat.Png);
                            #endregion
                            #region Obteniendo imagen desde MemoryStream
                            img1 = System.Drawing.Image.FromStream(memoryStream);
                            memoryStream.Close();
                            #endregion
                            #region Transformando imagen a array binario
                            stream1 = new MemoryStream();
                            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
                            imageByte1 = stream1.ToArray();
                            stream1.Dispose();
                            img1.Dispose();
                            #endregion
                            #region Obteniendo imagen desde array binario
                            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart1_path);
                            image = iTextSharp.text.Image.GetInstance(imageByte1);
                            memoryStream.Close();
                            #endregion
                        }
                        #endregion
                        #region No hay notas
                        else
                        {
                            image = iTextSharp.text.Image.GetInstance(noData_path);
                            nroEstudiantesAprobados = 0;
                            nroEstudiantesReprobados = 0;
                        }
                        #endregion
                        #region Añadiendo imagen
                        image.Alignment = Element.ALIGN_CENTER;
                        image.SetAbsolutePosition(document.PageSize.Width - widthCharts_1,
                            document.PageSize.Height - heightCharts_1eraLinea);
                        heightCharts_1eraLinea += heightDistance;
                        document.Add(image);
                        #endregion
                        #endregion
                        #region Nombre de la evaluación
                        phrase = new Phrase();
                        phrase.Add(new Chunk("Evaluación: ", boldFont));
                        phrase.Add(new Chunk(assessmentAux.Name + " (" + assessmentAux.Percentage + "%)", normalFont));
                        paragraph = new Paragraph(phrase);
                        document.Add(paragraph);
                        #endregion
                        #region # de aprobados/reprobados
                        phrase = new Phrase();
                        phrase.Add(new Chunk("Aprobados: ", boldFont));
                        phrase.Add(new Chunk(nroEstudiantesAprobados.ToString(), normalFont));
                        phrase.Add(new Chunk(" -  Reprobados: ", boldFont));
                        phrase.Add(new Chunk(nroEstudiantesReprobados.ToString(), normalFont));
                        paragraph = new Paragraph(phrase);
                        paragraph.Alignment = Element.ALIGN_RIGHT;
                        paragraph.SpacingBefore = -18f;
                        document.Add(paragraph);
                        #endregion
                        #region Fecha(s) de la evaluación
                        if (assessmentAux.StartDate.Equals(assessmentAux.FinishDate))
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                            phrase.Add(new Chunk(assessmentAux.StartDate.ToShortDateString(), normalFont));
                            paragraph = new Paragraph(phrase);
                            document.Add(paragraph);
                            intAux = 42;
                        }
                        else
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha de desde: ", boldFont));
                            phrase.Add(new Chunk(assessmentAux.StartDate.ToShortDateString(), normalFont));
                            paragraph = new Paragraph(phrase);
                            document.Add(paragraph);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha de hasta: ", boldFont));
                            phrase.Add(new Chunk(assessmentAux.FinishDate.ToShortDateString(), normalFont));
                            paragraph = new Paragraph(phrase);
                            document.Add(paragraph);
                            intAux = 28;
                        }
                        #endregion
                        #region Promedio de la evaluación
                        try
                        {
                            stringAux = Math.Round(listaPromediosPorEvaluacion[assessmentAux.AssessmentId], 2)
                                .ToString();
                            stringAux += " pts.";
                        }
                        catch (KeyNotFoundException)
                        {
                            stringAux = "N/A";
                        }
                        finally
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Promedio: ", boldFont));
                            phrase.Add(new Chunk(stringAux, normalFont));
                            paragraph = new Paragraph(phrase);
                            paragraph.SpacingAfter = intAux;
                            document.Add(paragraph);
                        }
                        #endregion
                    }
                    #endregion
                    contador++;
                }
                #endregion
                heightCharts_1eraLinea = valorParaheightCharts_1eraLinea;
            }
            #endregion
            #region Página #m+1
            document.NewPage();
            #region Sección: Cabecera
            #region Cabecera
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Año escolar: desde " + anoEscolar.StartDate.ToShortDateString() + ", hasta " +
                anoEscolar.EndDate.ToShortDateString(), whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Pág. " + nroPagina.ToString(), whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            document.Add(paragraph);
            nroPagina++;
            #endregion
            #region Título
            paragraph = new Paragraph(new Chunk(lapso3.Name + " (" + lapso3.StartDate.ToShortDateString() +
                " - " + lapso3.FinishDate.ToShortDateString() + ")", titleContentFont));
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.SpacingBefore = 10f;
            paragraph.SpacingAfter = 10f;
            document.Add(paragraph);
            #endregion
            #endregion
            #region Sección: Datos del 3er Lapso
            paragraph = new Paragraph("Datos del 3er lapso:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        Para el período escolar definido desde el ", normalFont));
            phrase.Add(new Chunk(lapso3.StartDate.ToString("dd") + " de " +
                lapso3.StartDate.ToString("MMMM") + " de " + lapso3.StartDate.ToString("yyyy"), boldFont));
            phrase.Add(new Chunk(", hasta el ", normalFont));
            phrase.Add(new Chunk(lapso3.FinishDate.ToString("dd") + " de " +
                lapso3.FinishDate.ToString("MMMM") + " de " + lapso3.FinishDate.ToString("yyyy"), boldFont));
            phrase.Add(new Chunk(", correspondiente al ", normalFont));
            phrase.Add(new Chunk(lapso3.Name, boldFont));
            phrase.Add(new Chunk(", a la materia se le asignaron ", normalFont));
            phrase.Add(new Chunk(nroEvaluaciones_lapso3.ToString() + " evaluaciones", boldFont));
            phrase.Add(new Chunk(". Hasta la fecha de emisión del reporte se han realizado ", normalFont));
            phrase.Add(new Chunk(nroEvaluacionesRealizadas_lapso3.ToString() + " evaluaciones", boldFont));
            phrase.Add(new Chunk(".", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección: Promedio de notas
            paragraph = new Paragraph("Promedio de notas:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        El promedio de notas alcanzado por el salón, para el 3er Lapso, " +
                "hasta la fecha de emisión de este reporte, es de ", normalFont));
            phrase.Add(new Chunk(promedio_lapso3.ToString() + " puntos", boldFont));
            phrase.Add(new Chunk(".", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección: % Aprobados vs Reprobados
            paragraph = new Paragraph("Porcentaje (%) de aprobados y reprobados:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        Para que un alumno se considere como aplazado en un curso deberá ", normalFont));
            phrase.Add(new Chunk("reprobar tres (3) materias o más, de las que actualmente está cursando", boldFont));
            phrase.Add(new Chunk(". Bajo esta premisa, a continuación se presenta el porcentaje de aprobados" +
                " y reprobados hasta la fecha de emisión de este reporte, para el 3er Lapso:", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección: Gráfico de aprobados vs reprobados
            paragraph = new Paragraph(new Chunk("Gráfica de % de alumnos aprobados vs reprobados",
                chartTitlefont));
            paragraph.SpacingBefore = 5;
            paragraph.Alignment = Element.ALIGN_CENTER;
            document.Add(paragraph);

            data = new Dictionary<string, int> { 
                    { "Aprobados:", nroAprobadosLapso3 }, 
                    { "Reprobados:", nroReprobadosLapso3 } };

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 290;
            chart.Height = 290;

            chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Pie;
            chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
            chartSeries.BorderColor = Color.Black;
            chartSeries.BorderWidth = 2;
            chartSeries.LabelForeColor = Color.White;

            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chartSeries.Points[0].Color = Color.Blue;
            chartSeries.Points[1].Color = Color.Red;
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_PieChart1_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart1_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 450f, document.PageSize.Height - 640f);
            document.Add(image);
            #endregion
            #endregion
            #region Página #m+2-x. Gran ciclo de materias por páginas - Lapso III
            foreach (CASU casu in listaCASUS_lapso3)
            {
                document.NewPage();
                #region Sección: Cabecera
                image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
                image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
                document.Add(image);

                stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
                paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
                document.Add(paragraph);

                paragraph = new Paragraph("Año escolar: desde " + anoEscolar.StartDate.ToShortDateString() +
                    ", hasta " + anoEscolar.EndDate.ToShortDateString(), whiteBoldFont);
                document.Add(paragraph);

                paragraph = new Paragraph("Pág. " + nroPagina.ToString(), whiteBoldFont);
                paragraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraph);
                nroPagina++;
                #endregion
                #region Sección: Datos de las materias
                paragraph = new Paragraph("Datos de la materia - " + casu.Subject.Name + ":", boldFont);
                paragraph.SpacingBefore = 15f;
                paragraph.SpacingAfter = 10f;
                document.Add(paragraph);
                #endregion
                #region Sección: Datos por evaluación
                int contador = 1;
                foreach (Assessment asses in casu.Assessments)
                {
                    #region Validación de 7 materias (esto debe cambiar en post-tesis)
                    if (contador <= 7)
                    {
                        #region Obteniendo la evaluación
                        assessmentAux = assessmentService.ObtenerEvaluacionPor_Id(asses.AssessmentId);
                        #endregion
                        #region Gráfico de % de aprobados/reprobados
                        #region Si hay notas
                        if (assessmentAux.Scores.Count != 0)
                        {
                            #region Cáclulo de # aprobados y reprobados
                            listaEstudiantesAprobados = new List<Student>();
                            listaEstudiantesReprobados = new List<Student>();
                            foreach (Score scoreAux in assessmentAux.Scores)
                            {
                                #region Bachillerato
                                if (grado > 6)
                                {
                                    if (scoreAux.NumberScore >= 10)
                                        listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                                    else
                                        listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                                }
                                #endregion
                            }
                            nroEstudiantesAprobados = listaEstudiantesAprobados.Count();
                            nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();

                            data = new Dictionary<string, int> { 
                            { "Aprob.:", nroEstudiantesAprobados }, 
                            { "Reprob.:", nroEstudiantesReprobados } };
                            #endregion
                            #region Dibujando el gráfico
                            chart = new Chart();
                            chart.AntiAliasing = AntiAliasingStyles.All;
                            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                            chart.Width = widthHeight_imgSize;
                            chart.Height = widthHeight_imgSize;

                            chartArea = new ChartArea();
                            chart.ChartAreas.Add(chartArea);

                            chartSeries = new Series();
                            chartSeries.ChartType = SeriesChartType.Pie;
                            chartSeries.Label = "#PERCENT{P0}";
                            chartSeries.BorderColor = Color.Black;
                            chartSeries.BorderWidth = 2;
                            chartSeries.LabelForeColor = Color.White;

                            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
                            chartSeries.Points[0].Color = Color.Blue;
                            chartSeries.Points[1].Color = Color.Red;
                            chart.Series.Add(chartSeries);
                            #endregion
                            #region Definiendo un Memory Stream para guardar la imagen
                            memoryStream = new MemoryStream();
                            chart.SaveImage(memoryStream, ChartImageFormat.Png);
                            //chart.SaveImage(reportRemains_PieChart1_path, ChartImageFormat.Png);
                            #endregion
                            #region Obteniendo imagen desde MemoryStream
                            img1 = System.Drawing.Image.FromStream(memoryStream);
                            memoryStream.Close();
                            #endregion
                            #region Transformando imagen a array binario
                            stream1 = new MemoryStream();
                            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
                            imageByte1 = stream1.ToArray();
                            stream1.Dispose();
                            img1.Dispose();
                            #endregion
                            #region Obteniendo imagen desde array binario
                            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart1_path);
                            image = iTextSharp.text.Image.GetInstance(imageByte1);
                            memoryStream.Close();
                            #endregion
                        }
                        #endregion
                        #region No hay notas
                        else
                        {
                            image = iTextSharp.text.Image.GetInstance(noData_path);
                            nroEstudiantesAprobados = 0;
                            nroEstudiantesReprobados = 0;
                        }
                        #endregion
                        #region Añadiendo imagen
                        image.Alignment = Element.ALIGN_CENTER;
                        image.SetAbsolutePosition(document.PageSize.Width - widthCharts_1,
                            document.PageSize.Height - heightCharts_1eraLinea);
                        heightCharts_1eraLinea += heightDistance;
                        document.Add(image);
                        #endregion
                        #endregion
                        #region Nombre de la evaluación
                        phrase = new Phrase();
                        phrase.Add(new Chunk("Evaluación: ", boldFont));
                        phrase.Add(new Chunk(assessmentAux.Name + " (" + assessmentAux.Percentage + "%)", normalFont));
                        paragraph = new Paragraph(phrase);
                        document.Add(paragraph);
                        #endregion
                        #region # de aprobados/reprobados
                        phrase = new Phrase();
                        phrase.Add(new Chunk("Aprobados: ", boldFont));
                        phrase.Add(new Chunk(nroEstudiantesAprobados.ToString(), normalFont));
                        phrase.Add(new Chunk(" -  Reprobados: ", boldFont));
                        phrase.Add(new Chunk(nroEstudiantesReprobados.ToString(), normalFont));
                        paragraph = new Paragraph(phrase);
                        paragraph.Alignment = Element.ALIGN_RIGHT;
                        paragraph.SpacingBefore = -18f;
                        document.Add(paragraph);
                        #endregion
                        #region Fecha(s) de la evaluación
                        if (assessmentAux.StartDate.Equals(assessmentAux.FinishDate))
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha de presentación: ", boldFont));
                            phrase.Add(new Chunk(assessmentAux.StartDate.ToShortDateString(), normalFont));
                            paragraph = new Paragraph(phrase);
                            document.Add(paragraph);
                            intAux = 42;
                        }
                        else
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha de desde: ", boldFont));
                            phrase.Add(new Chunk(assessmentAux.StartDate.ToShortDateString(), normalFont));
                            paragraph = new Paragraph(phrase);
                            document.Add(paragraph);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha de hasta: ", boldFont));
                            phrase.Add(new Chunk(assessmentAux.FinishDate.ToShortDateString(), normalFont));
                            paragraph = new Paragraph(phrase);
                            document.Add(paragraph);
                            intAux = 28;
                        }
                        #endregion
                        #region Promedio de la evaluación
                        try
                        {
                            stringAux = Math.Round(listaPromediosPorEvaluacion[assessmentAux.AssessmentId], 2)
                                .ToString();
                            stringAux += " pts.";
                        }
                        catch (KeyNotFoundException)
                        {
                            stringAux = "N/A";
                        }
                        finally
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Promedio: ", boldFont));
                            phrase.Add(new Chunk(stringAux, normalFont));
                            paragraph = new Paragraph(phrase);
                            paragraph.SpacingAfter = intAux;
                            document.Add(paragraph);
                        }
                        #endregion
                    }
                    #endregion
                    contador++;
                }
                #endregion
                heightCharts_1eraLinea = valorParaheightCharts_1eraLinea;
            }
            #endregion
            #region Página x+1
            document.NewPage();
            #region Sección: Cabecera
            #region Cabecera
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Año escolar: desde " + anoEscolar.StartDate.ToShortDateString() + ", hasta " +
                anoEscolar.EndDate.ToShortDateString(), whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Pág. " + nroPagina.ToString(), whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            document.Add(paragraph);
            nroPagina++;
            #endregion
            #region Título
            paragraph = new Paragraph(new Chunk("Datos de resultados definitivos del curso", titleContentFont));
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.SpacingBefore = 10f;
            paragraph.SpacingAfter = 10f;
            document.Add(paragraph);
            #endregion
            #endregion
            #region Datos de definitivas
            paragraph = new Paragraph("Datos de resultados definitivos:", boldFont);
            document.Add(paragraph);

            phrase = new Phrase();
            phrase.Add(new Chunk("        El curso ", normalFont));
            phrase.Add(new Chunk(curso.Name, boldFont));
            phrase.Add(new Chunk(", definido para el año escolar desde el ", normalFont));
            phrase.Add(new Chunk(anoEscolar.StartDate.ToString("dd") + " de " +
                anoEscolar.StartDate.ToString("MMMM") + " del " + anoEscolar.StartDate.ToString("yyyy"),
                boldFont));
            phrase.Add(new Chunk(", hasta el ", normalFont));
            phrase.Add(new Chunk(anoEscolar.EndDate.ToString("dd") + " de " +
                anoEscolar.EndDate.ToString("MMMM") + " del " + anoEscolar.EndDate.ToString("yyyy"),
                boldFont));
            phrase.Add(new Chunk(", para la fecha de emisión de este reporte, obtuvo el siguiente porcentaje" +
                " de alumnos aprobados y reprobados (para considerar un alumno como reprobado en un curso, " +
                "debe haber ", normalFont));
            phrase.Add(new Chunk("reprobado tres (3) materias o más en los resultados de las definitivas",
                boldFont));
            phrase.Add(new Chunk("):", normalFont));
            paragraph = new Paragraph(phrase);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Gráfico - Aprobados vs Reprobados
            paragraph = new Paragraph(new Chunk("Gráfica de % de alumnos aprobados vs reprobados",
                chartTitlefont));
            paragraph.SpacingBefore = 5;
            paragraph.Alignment = Element.ALIGN_CENTER;
            document.Add(paragraph);

            data = new Dictionary<string, int> { 
                    { "Aprobados:", nroEstudiantesAprobados_Definitivas }, 
                    { "Reprobados:", nroEstudiantesReprobados_Definitivas } };

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 290;
            chart.Height = 290;

            chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Pie;
            chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
            chartSeries.BorderColor = Color.Black;
            chartSeries.BorderWidth = 2;
            chartSeries.LabelForeColor = Color.White;

            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chartSeries.Points[0].Color = Color.Blue;
            chartSeries.Points[1].Color = Color.Red;
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_PieChart1_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_PieChart1_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 450f, document.PageSize.Height - 520f);
            document.Add(image);
            #endregion
            #endregion
            #region Página x+2
            document.NewPage();
            #region Sección: Cabecera
            image = iTextSharp.text.Image.GetInstance(subHeaderBackground_path);
            image.SetAbsolutePosition(0, document.PageSize.Height - image.Height);
            document.Add(image);

            stringAux = (colegio.Name.Contains("Colegio") ? colegio.Name.Replace("Colegio", "") : colegio.Name);
            paragraph = new Paragraph("Colegio: " + stringAux, whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Año escolar: desde " + anoEscolar.StartDate.ToShortDateString() + ", hasta " +
                anoEscolar.EndDate.ToShortDateString(), whiteBoldFont);
            document.Add(paragraph);

            paragraph = new Paragraph("Pág. " + nroPagina.ToString(), whiteBoldFont);
            paragraph.Alignment = Element.ALIGN_RIGHT;
            document.Add(paragraph);
            #endregion
            #region Sección: Datos de definitivas - Parte II
            paragraph = new Paragraph("Datos de resultados definitivos - Parte II:", boldFont);
            paragraph.SpacingBefore = 15;
            document.Add(paragraph);

            paragraph = new Paragraph("        A continuación se indica el listado del top 10 de los" +
                " resultados más destacados y deficientes obtenidos a lo largo del desarrollo del " +
                curso.Name + ":", normalFont);
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Sección: Gráficos - Top 10 mejores/peores resultados
            #region Títulos gráficos
            Paragraph tableTitle1 = new Paragraph(new Phrase(new Chunk("Gráfica del top 10 resultados destacados",
                chartTitlefont)));
            Paragraph tableTitle2 = new Paragraph(new Phrase(new Chunk("Gráfica del top 10 resultados deficientes",
                chartTitlefont)));

            PdfPCell cellTitle1 = new PdfPCell(tableTitle1)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0
            };
            PdfPCell cellTitle2 = new PdfPCell(tableTitle2)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0
            };

            PdfPTable titleTable = new PdfPTable(2);
            titleTable.AddCell(cellTitle1);
            titleTable.AddCell(cellTitle2);
            titleTable.SpacingAfter = 15f;
            titleTable.WidthPercentage = 100f;

            document.Add(titleTable);
            #endregion
            #region Gráfico 10 mejores
            j = 1;
            Dictionary<int, int> data2 = new Dictionary<int, int>();
            foreach(KeyValuePair<int, double> valor in top10MejoresNotas)
            {
                data2.Add(j, Convert.ToInt32(Math.Round(valor.Value)));
                j++;
            }

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 255;
            chart.Height = 278;

            chartArea = new ChartArea();
            chartArea.AxisY.LabelStyle.Enabled = true;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisY.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisY.IsLabelAutoFit = true;
            chartArea.AxisY.LineColor = Color.Blue;
            chartArea.AxisY.Maximum = (grado > 6 ? 20 : 5);
            chartArea.AxisY.Title = "Notas obtenidas";
            chartArea.AxisY.TitleFont = graphBarChartFontAxisTitle;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.Lavender;
            chartArea.AxisY.MajorGrid.LineWidth = 6;
            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.LineColor = Color.Blue;
            chartArea.AxisY.MinorTickMark.Enabled = false;
            if (grado <= 6)//Primaria
            {
                chartArea.AxisY.CustomLabels.Add(0, 1, "E");
                chartArea.AxisY.CustomLabels.Add(1, 2, "D");
                chartArea.AxisY.CustomLabels.Add(2, 3, "C");
                chartArea.AxisY.CustomLabels.Add(3, 4, "B");
                chartArea.AxisY.CustomLabels.Add(4, 5, "A");
            }

            chartArea.AxisX.LabelStyle.Enabled = true;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisX.LineColor = Color.Blue;
            chartArea.AxisX.Maximum = 11;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.MinorTickMark.Enabled = false;
            chartArea.AxisX.LabelAutoFitMinFontSize = 7;

            int i = 0;
            foreach(KeyValuePair<int, double> valor in top10MejoresNotas)
            {
                student = studentService.ObtenerAlumnoPorId(valor.Key);

                if (grado > 6) //Bachillerato
                    stringAux = "(" + Math.Round(valor.Value) + ") ";

                stringAux = stringAux + student.FirstLastName + " " + student.SecondLastName + ", " +
                        student.FirstName;

                chartArea.AxisX.CustomLabels.Add(i + 0.5, i + 1.5, stringAux);
                i++;
            }
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Column;
            chartSeries.Color = Color.Blue;
            chartSeries.BorderColor = Color.DarkBlue;
            chartSeries.BackGradientStyle = GradientStyle.LeftRight;
            foreach (var item in data2) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_BarChart1_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_BarChart1_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 570, document.PageSize.Height - 440f);
            document.Add(image);
            #endregion
            #region Gráfico 10 peores
            j = 1;
            Dictionary<int, int> data3 = new Dictionary<int, int>();
            foreach (KeyValuePair<int, double> valor in top10PeoresNotas)
            {
                data3.Add(j, Convert.ToInt32(Math.Round(valor.Value)));
                j++;
            }

            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 255;
            chart.Height = 278;

            chartArea = new ChartArea();
            chartArea.AxisY.LabelStyle.Enabled = true;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisY.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisY.IsLabelAutoFit = true;
            chartArea.AxisY.LineColor = Color.Red;
            chartArea.AxisY.Maximum = (grado > 6 ? 20 : 5);
            chartArea.AxisY.Title = "Notas obtenidas";
            chartArea.AxisY.TitleFont = graphBarChartFontAxisTitle;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.Lavender;
            chartArea.AxisY.MajorGrid.LineWidth = 6;
            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.LineColor = Color.Red;
            chartArea.AxisY.MinorTickMark.Enabled = false;
            if (grado <= 6)//Primaria
            {
                chartArea.AxisY.CustomLabels.Add(0, 1, "E");
                chartArea.AxisY.CustomLabels.Add(1, 2, "D");
                chartArea.AxisY.CustomLabels.Add(2, 3, "C");
                chartArea.AxisY.CustomLabels.Add(3, 4, "B");
                chartArea.AxisY.CustomLabels.Add(4, 5, "A");
            }

            chartArea.AxisX.LabelStyle.Enabled = true;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisX.LineColor = Color.Red;
            chartArea.AxisX.Maximum = 11;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.MinorTickMark.Enabled = false;
            chartArea.AxisX.LabelAutoFitMinFontSize = 7;

            i = 0;
            foreach (KeyValuePair<int, double> valor in top10PeoresNotas)
            {
                student = studentService.ObtenerAlumnoPorId(valor.Key);

                if (grado > 6) //Bachillerato
                    stringAux = "(" + Math.Round(valor.Value) + ") ";

                stringAux = stringAux + student.FirstLastName + " " + student.SecondLastName + ", " +
                        student.FirstName;

                chartArea.AxisX.CustomLabels.Add(i + 0.5, i + 1.5, stringAux);
                i++;
            }
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Column;
            chartSeries.Color = Color.Red;
            chartSeries.BorderColor = Color.DarkRed;
            chartSeries.BackGradientStyle = GradientStyle.LeftRight;
            foreach (var item in data3) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chart.Series.Add(chartSeries);
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(reportRemains_BarChart2_path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Obteniendo imagen desde array binario
            //image = iTextSharp.text.Image.GetInstance(reportRemains_BarChart2_path);
            image = iTextSharp.text.Image.GetInstance(imageByte1);
            memoryStream.Close();
            #endregion
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(document.PageSize.Width - 302, document.PageSize.Height - 440f);
            document.Add(image);
            #endregion
            #endregion
            #region Sección X: Tabla de alumnos
            #region Texto preliminar
            paragraph = new Paragraph("        A continuación se presenta la tabla de alumnos con los" +
                " resultados obtenidos por cada uno de estos, a lo largo del curso de " + curso.Name + ":", 
                normalFont);
            paragraph.SpacingBefore = 247;
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(paragraph);
            #endregion
            #region Table #1
            #region Configuración de la tabla
            table = new PdfPTable(3);
            table.SetWidths(studentTableWidths);
            table.LockedWidth = true;
            table.TotalWidth = 230;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            #endregion
            #region Configuración de la celda #1
            cell = new PdfPCell(new Phrase(new Chunk("#", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #2
            cell = new PdfPCell(new Phrase(new Chunk("Alumnos", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #3
            cell = new PdfPCell(new Phrase(new Chunk("Nota", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthLeft = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Ciclo de llenado de las celdas

            i = 0;
            foreach(KeyValuePair<int, double> valor in diccionarioDefinitivas)
            {
                if (i <= 22)
                {
                    student = studentService.ObtenerAlumnoPorId(valor.Key);
                    #region Celda #1
                    stringAux = student.NumberList.ToString();
                    cell = new PdfPCell(new Phrase(stringAux, cellFont))
                    {
                        BorderWidthLeft = 1.75f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    if (i == 22) cell.BorderWidthBottom = 1.75f;
                    table.AddCell(cell);
                    #endregion
                    #region Celda #2
                    stringAux = student.FirstLastName + " " + student.SecondLastName +
                        ", " + student.FirstName + " " + student.SecondName;
                    cell = new PdfPCell(new Phrase(stringAux, cellFont))
                    {
                        BorderWidthLeft = 1.75f,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    if (i == 22) cell.BorderWidthBottom = 1.75f;
                    table.AddCell(cell);
                    #endregion
                    #region Celda #3
                    if (grado > 6) //Bachillerato
                    {
                        intAux = Convert.ToInt32(Math.Round(valor.Value));
                        if (intAux <= 9) //Aplazado
                            phrase = new Phrase(new Chunk(intAux.ToString(), cellFont_red));
                        else //Aprobado
                            phrase = new Phrase(new Chunk(intAux.ToString(), cellFont));
                    }

                    cell = new PdfPCell(phrase)
                    {
                        BorderWidthRight = 1.75f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    if (i == 22) cell.BorderWidthBottom = 1.75f;
                    table.AddCell(cell);
                    #endregion
                }
                else
                    break;
                i++;
            }
            document.Add(table);
            #endregion
            #endregion
            #region Table #2
            #region Configuración de la tabla
            table = new PdfPTable(3);
            table.SetWidths(studentTableWidths);
            table.LockedWidth = true;
            table.TotalWidth = 230;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            #endregion
            #region Configuración de la celda #1
            cell = new PdfPCell(new Phrase(new Chunk("#", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #2
            cell = new PdfPCell(new Phrase(new Chunk("Alumnos", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthRight = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Configuración de la celda #3
            cell = new PdfPCell(new Phrase(new Chunk("Nota", whiteBoldFont)))
            {
                BackgroundColor = BaseColor.BLUE,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 1.75f,
                BorderWidthLeft = 1f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            #endregion
            #region Ciclo de llenado de las celdas
            for (i = 24; i <= listaEstudiantes.Count() - 1; i++)
            {
                student = studentService.ObtenerAlumnoPorId(i);

                #region Celda #1
                stringAux = student.NumberList.ToString();
                cell = new PdfPCell(new Phrase(stringAux, cellFont))
                {
                    BorderWidthLeft = 1.75f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                if (i == listaEstudiantes.Count() - 1) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #2
                stringAux = student.FirstLastName + " " + student.SecondLastName +
                    ", " + student.FirstName + " " + student.SecondName;
                cell = new PdfPCell(new Phrase(stringAux, cellFont))
                {
                    BorderWidthLeft = 1.75f,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                if (i == listaEstudiantes.Count() - 1) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
                #region Celda #3
                #region Bachillerato
                if (grado > 6) //Bachillerato
                {
                    intAux = Convert.ToInt32(Math.Round(diccionarioDefinitivas[i]));
                    if (intAux <= 9) //Aplazado
                        phrase = new Phrase(new Chunk(intAux.ToString(), cellFont_red));
                    else //Aprobado
                        phrase = new Phrase(new Chunk(intAux.ToString(), cellFont));
                };
                #endregion 

                cell = new PdfPCell(phrase)
                {
                    BorderWidthRight = 1.75f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                if (i == listaEstudiantes.Count() - 1) cell.BorderWidthBottom = 1.75f;
                table.AddCell(cell);
                #endregion
            }
            #endregion

            paragraph = new Paragraph();
            paragraph.Add(table);
            paragraph.SpacingBefore = -285;
            document.Add(paragraph);
            #endregion
            #endregion
            #endregion
            #region Pasos finales
            document.Close();

            jsonResult.Add(new
            {
                success = true,
                path = pdfFile_ServerSide_path
            });

            TempData["Curso"] = true;
            #endregion

            return Json(jsonResult);
        }

        public JsonResult Estadistica_RendimientoAcademicoPor_Materia_Lapso(int idCurso)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<Assessment> listaEvaluaciones = new List<Assessment>();
            List<Subject> listaMaterias = new List<Subject>();
            Course curso = new Course();
            string lapso = "";
            double promedio = 0;
            double promedioEvaluacion = 0;
            double promedioMateria = 0;
            string promedioLiteral = "";
            int nroAlumnos = 0;
            int grado = 0;

            UnitOfWork unitOfWork = new UnitOfWork();
            CourseService courseService = new CourseService(unitOfWork);
            AssessmentService assessmentService = new AssessmentService(unitOfWork);
            SubjectService subjectService = new SubjectService(unitOfWork);
            #endregion

            #region Obteniendo el curso, el nro de alumnos y el grado
            curso = courseService.ObtenerCursoPor_Id(idCurso);
            nroAlumnos = curso.Students.Count;
            grado = curso.Grade;
            #endregion
            #region Obteniendo la lista de materias del curso
            listaMaterias = subjectService.ObtenerListaMateriasPor_Curso(idCurso);
            #endregion

            #region Lapso I
            lapso = "1er Lapso";
            foreach (Subject materia in listaMaterias)
            {
                listaEvaluaciones = assessmentService.ObtenerListaEvaluacionesPor_Curso_Materia_Lapso(
                    idCurso, materia.SubjectId, lapso);

                foreach (Assessment evaluacion in listaEvaluaciones)
                {
                    foreach (Score score in evaluacion.Scores)
                    {
                        #region Bachillerato
                        if (grado > 6)
                            promedio += (double)score.NumberScore;
                        #endregion
                        #region Primaria
                        else
                        {
                            if (score.LetterScore.Equals("A") || score.LetterScore.Equals("a")) promedio += 5;
                            else if (score.LetterScore.Equals("B") || score.LetterScore.Equals("b")) promedio += 4;
                            else if (score.LetterScore.Equals("C") || score.LetterScore.Equals("c")) promedio += 3;
                            else if (score.LetterScore.Equals("D") || score.LetterScore.Equals("d")) promedio += 2;
                            else if (score.LetterScore.Equals("E") || score.LetterScore.Equals("e")) promedio += 1;
                        }
                        #endregion
                    }

                    promedio = (double)(promedio / nroAlumnos);
                    promedioEvaluacion += promedio;
                    promedio = 0; //Inicializando la variable
                }

                promedioEvaluacion = (double)(promedioEvaluacion / (listaEvaluaciones.Count));
                if (promedioEvaluacion.CompareTo(Double.NaN) != 0 )                
                    promedioMateria += promedioEvaluacion;                

                promedioEvaluacion = 0; //Inicializando la variable
                
                #region Literal del promedio. Solo para primaria

				if(grado <= 6) //Solo primaria
				{
                    if (Math.Round(promedioMateria) <= 5 && Math.Round(promedioMateria) > 4) promedioLiteral = "A";
                    else if (Math.Round(promedioMateria) <= 4 && Math.Round(promedioMateria) > 3) promedioLiteral = "B";
                    else if (Math.Round(promedioMateria) <= 3 && Math.Round(promedioMateria) > 2) promedioLiteral = "C";
					else if (Math.Round(promedioMateria) <= 2 && Math.Round(promedioMateria) > 1) promedioLiteral = "D";
					else if (Math.Round(promedioMateria) <= 1)  promedioLiteral = "E";	
				}
                
                #endregion

                jsonResult.Add(new { 
                    materia = materia.Name,
                    promedio = promedioMateria,
                    promedio_literal = (grado > 6 ? "" : promedioLiteral),
                    grado = (grado > 6 ? "Bachillerato" : "Primaria"),
                    lapso = lapso
                });

                promedioLiteral = ""; //Inicializando la variable
                promedioMateria = 0; //Inicializando la variable
            }
            #endregion
            #region Lapso II
            lapso = "2do Lapso";
            foreach (Subject materia in listaMaterias)
            {
                listaEvaluaciones = assessmentService.ObtenerListaEvaluacionesPor_Curso_Materia_Lapso(
                    idCurso, materia.SubjectId, lapso);

                foreach (Assessment evaluacion in listaEvaluaciones)
                {
                    foreach (Score score in evaluacion.Scores)
                    {
                        #region Bachillerato
                        if (grado > 6)
                            promedio += (double)score.NumberScore;
                        #endregion
                        #region Primaria
                        else
                        {
                            if (score.LetterScore.Equals("A") || score.LetterScore.Equals("a")) promedio += 5;
                            else if (score.LetterScore.Equals("B") || score.LetterScore.Equals("b")) promedio += 4;
                            else if (score.LetterScore.Equals("C") || score.LetterScore.Equals("c")) promedio += 3;
                            else if (score.LetterScore.Equals("D") || score.LetterScore.Equals("d")) promedio += 2;
                            else if (score.LetterScore.Equals("E") || score.LetterScore.Equals("e")) promedio += 1;
                        }
                        #endregion
                    }

                    promedio = (double)(promedio / nroAlumnos);
                    promedioEvaluacion += promedio;
                    promedio = 0; //Inicializando la variable
                }

                promedioEvaluacion = (double)(promedioEvaluacion / (listaEvaluaciones.Count));
                if (promedioEvaluacion.CompareTo(Double.NaN) != 0)
                    promedioMateria += promedioEvaluacion;

                promedioEvaluacion = 0; //Inicializando la variable

                #region Literal del promedio. Solo para primaria

                if (grado <= 6) //Solo primaria
                {
                    if (Math.Round(promedioMateria) <= 5 && Math.Round(promedioMateria) > 4) promedioLiteral = "A";
                    else if (Math.Round(promedioMateria) <= 4 && Math.Round(promedioMateria) > 3) promedioLiteral = "B";
                    else if (Math.Round(promedioMateria) <= 3 && Math.Round(promedioMateria) > 2) promedioLiteral = "C";
                    else if (Math.Round(promedioMateria) <= 2 && Math.Round(promedioMateria) > 1) promedioLiteral = "D";
                    else if (Math.Round(promedioMateria) <= 1) promedioLiteral = "E";	
                }

                #endregion

                jsonResult.Add(new
                {
                    materia = materia.Name,
                    promedio = promedioMateria,
                    promedio_literal = (grado > 6 ? "" : promedioLiteral),
                    grado = (grado > 6 ? "Bachillerato" : "Primaria"),
                    lapso = lapso
                });

                promedioLiteral = ""; //Inicializando la variable
                promedioMateria = 0; //Inicializando la variable
            }
            #endregion
            #region Lapso III
            lapso = "3er Lapso";
            foreach (Subject materia in listaMaterias)
            {
                listaEvaluaciones = assessmentService.ObtenerListaEvaluacionesPor_Curso_Materia_Lapso(
                    idCurso, materia.SubjectId, lapso);

                foreach (Assessment evaluacion in listaEvaluaciones)
                {
                    foreach (Score score in evaluacion.Scores)
                    {
                        #region Bachillerato
                        if (grado > 6)
                            promedio += (double)score.NumberScore;
                        #endregion
                        #region Primaria
                        else
                        {
                            if (score.LetterScore.Equals("A") || score.LetterScore.Equals("a")) promedio += 5;
                            else if (score.LetterScore.Equals("B") || score.LetterScore.Equals("b")) promedio += 4;
                            else if (score.LetterScore.Equals("C") || score.LetterScore.Equals("c")) promedio += 3;
                            else if (score.LetterScore.Equals("D") || score.LetterScore.Equals("d")) promedio += 2;
                            else if (score.LetterScore.Equals("E") || score.LetterScore.Equals("e")) promedio += 1;
                        }
                        #endregion
                    }

                    promedio = (double)(promedio / nroAlumnos);
                    promedioEvaluacion += promedio;
                    promedio = 0; //Inicializando la variable
                }

                promedioEvaluacion = (double)(promedioEvaluacion / (listaEvaluaciones.Count));
                if (promedioEvaluacion.CompareTo(Double.NaN) != 0)
                    promedioMateria += promedioEvaluacion;

                promedioEvaluacion = 0; //Inicializando la variable

                #region Literal del promedio. Solo para primaria

                if (grado <= 6) //Solo primaria
                {
                    if (Math.Round(promedioMateria) <= 5 && Math.Round(promedioMateria) > 4) promedioLiteral = "A";
                    else if (Math.Round(promedioMateria) <= 4 && Math.Round(promedioMateria) > 3) promedioLiteral = "B";
                    else if (Math.Round(promedioMateria) <= 3 && Math.Round(promedioMateria) > 2) promedioLiteral = "C";
                    else if (Math.Round(promedioMateria) <= 2 && Math.Round(promedioMateria) > 1) promedioLiteral = "D";
                    else if (Math.Round(promedioMateria) <= 1) promedioLiteral = "E";	
                }
                #endregion

                jsonResult.Add(new
                {
                    materia = materia.Name,
                    promedio = promedioMateria,
                    promedio_literal = (grado > 6 ? "" : promedioLiteral),
                    grado = (grado > 6 ? "Bachillerato" : "Primaria"),
                    lapso = lapso
                });

                promedioLiteral = ""; //Inicializando la variable
                promedioMateria = 0; //Inicializando la variable
            }
            #endregion
            
            return Json(jsonResult);
        }
        public JsonResult Estadistica_ObtenerNotasUltimaEvaluacionPor_Profesor()
        {
            List<object> jsonResult = new List<object>();
            ObteniendoSesion();

            if(_session.ADMINISTRADOR) //Es administrador
                jsonResult.Add(new { success = false });
            else //No es administrador
            {
                #region Declaración de variables
                List<Score> listaNotas;
                Assessment evaluacion;
                string grado;
                int gradoInt = 0;

                UnitOfWork unidad = new UnitOfWork();
                ScoreService scoreService = new ScoreService(unidad);
                AssessmentService assessmentService = new AssessmentService(unidad);
                #endregion
                #region Obteniendo la última evaluación
                evaluacion = assessmentService.ObtenerUltimaEvaluacionPor_Docente_AnoEscolar(_session.USERID,
                    _session.SCHOOLYEARID);
                #endregion
                #region Evaluación vacía
                if (evaluacion == null)
                    jsonResult.Add(new { success = false });
                #endregion
                #region Evaluación no vacía
                else
                {
                    gradoInt = evaluacion.CASU.Course.Grade;

                    #region Definiendo Primaria o Bachillerato
                    grado = (evaluacion.CASU.Course.Grade <= 6 ? "Primaria" : "Bachillerato");
                    #endregion
                    #region Obteniendo la lista de notas
                    listaNotas = scoreService.ObtenerNotasPor_Evaluacion(evaluacion.AssessmentId);

                    listaNotas = (gradoInt > 6 ? listaNotas.OrderByDescending(m => m.NumberScore).ToList() :
                        listaNotas.OrderByDescending(m => m.ToIntLetterScore(m.LetterScore)).ToList());
                    #endregion
                    #region Creando objeto JSON
                    foreach (Score nota in listaNotas)
                    {
                        jsonResult.Add(new
                        {
                            success = true,
                            curso = evaluacion.CASU.Course.Name,
                            materia = evaluacion.CASU.Subject.Name,
                            nombreevaluacion = evaluacion.Name,
                            grado = grado,
                            nota = (grado.Equals("Primaria") ? nota.LetterScore.ToUpper() :
                                   nota.NumberScore.ToString()),
                            alumnoNombre1 = nota.Student.FirstName,
                            alumnoApellido1 = nota.Student.FirstLastName + " " + nota.Student.SecondLastName,
                            idEvaluacion = evaluacion.AssessmentId,
                            totalAlumnos = evaluacion.CASU.Course.Students.Count()
                        });
                    }
                    #endregion
                }
                #endregion
            }

            return Json(jsonResult);
        }
        public JsonResult Estadistica_ObtenerNotasPor_Evaluacion(int idEvaluacion)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();

            #region Declaración de servicios
            UnitOfWork unidad = new UnitOfWork();
            AssessmentService assessmentService = new AssessmentService(unidad);
            ScoreService scoreService = new ScoreService(unidad);
            #endregion
            #endregion

            Assessment evaluacion = assessmentService.ObtenerEvaluacionPor_Id(idEvaluacion);
            List<Score> listaNotasAlumno = scoreService.ObtenerNotasPor_Evaluacion(idEvaluacion);
            Course curso = evaluacion.CASU.Course;
            int grado = curso.Grade;

            foreach (Score notax in listaNotasAlumno)
            {
                jsonResult.Add(new
                {
                    success = true,
                    grado = (grado <= 6 ? "Primaria" : "Bachillerato"),
                    nota = (grado <= 6 ? notax.LetterScore.ToUpper() : notax.NumberScore.ToString()),
                    alumnoNombre1 = notax.Student.FirstName,
                    alumnoApellido1 = notax.Student.FirstLastName + " " + notax.Student.SecondLastName,
                    totalAlumnos = evaluacion.CASU.Course.Students.Count()
                });
            };

            return Json(jsonResult);
        }
        public bool Estadisticas_Movil(int idEvaluacion)
        {
            ObteniendoSesion();

            #region Declaración de variables
            AssessmentService assessmentService = new AssessmentService();
            Assessment assessment = assessmentService.ObtenerEvaluacionPor_Id(idEvaluacion);
            CASU casu = assessment.CASU;
            int grado = casu.Course.Grade;
            List<Student> listaEstudiantes = casu.Course.Students;
            List<Score> listaNotas = assessment.Scores;

            List<Student> listaEstudiantesAprobados = new List<Student>();
            List<Student> listaEstudiantesReprobados = new List<Student>();
            List<Score> top10MejoresNotas = new List<Score>();
            #endregion

            #region Gráfico #1
            #region Obtneniendo lista de estudiantes aprobados/reprobados
            foreach (Score scoreAux in listaNotas)
            {
                #region Bachillerato
                if (grado > 6)
                {
                    if (scoreAux.NumberScore >= 10)
                        listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                    else
                        listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                }
                #endregion
                #region Primaria
                else
                {
                    if (scoreAux.ToIntLetterScore(scoreAux.LetterScore) > 1)
                        listaEstudiantesAprobados.Add(scoreAux.Student); //Aprobado
                    else
                        listaEstudiantesReprobados.Add(scoreAux.Student); //Reprobado
                }
                #endregion
            }
            int nroEstudiantesReprobados = listaEstudiantes.Count() - listaEstudiantesAprobados.Count();
            #endregion
            #region Creando la imágen PieChart
            Dictionary<string, int> data
                = new Dictionary<string, int> { 
                    { "Aprobados:", listaEstudiantesAprobados.Count() }, 
                    { "Reprobados:", nroEstudiantesReprobados } };

            Chart chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 180;
            chart.Height = 180;

            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            Series chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Pie;
            chartSeries.Label = "#AXISLABEL #VALY (#PERCENT{P0})";
            chartSeries.BorderColor = Color.Black;
            chartSeries.BorderWidth = 2;
            chartSeries.LabelForeColor = Color.White;

            foreach (var item in data) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chartSeries.Points[0].Color = Color.Blue;
            chartSeries.Points[1].Color = Color.Red;
            chart.Series.Add(chartSeries);
            #endregion
            #region Definiendo un Memory Stream para guardar la imagen
            MemoryStream memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(path, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            //System.Drawing.Image img1 = System.Drawing.Image.FromFile(path);
            System.Drawing.Image img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            MemoryStream stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] imageByte1 = stream1.ToArray();
            string imageBase64_1 = Convert.ToBase64String(imageByte1);
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Invocando el Web Service
            WS_Security.Service1SoapClient WSClient = new WS_Security.Service1SoapClient();
            WS_Security_localhost.Service1 WSClient_localhost = new WS_Security_localhost.Service1();

            WSClient.StatisticsImageGenerator(
                ConstantRepository.MOBILE_STATISTICS_CODE_AprobadosVsReprobados,
                casu.CourseId, 
                casu.Period.SchoolYear.SchoolYearId, 
                casu.Period.SchoolYear.School.SchoolId,
                imageBase64_1);
            #endregion
            #endregion
            #region Gráfico #2
            #region Declaración de variables
            System.Drawing.Font graphBarChartFont = new System.Drawing.Font("Almanac MT", 8);
            System.Drawing.Font graphBarChartFontAxisTitle = new System.Drawing.Font("Helvetica", 8);
            string stringAux = "";
            #endregion
            #region Obteniendo la lista top 10 mejores notas
            top10MejoresNotas = (grado > 6 ?
                listaNotas.OrderByDescending(m => m.NumberScore).Take(10).ToList() :
                listaNotas.OrderByDescending(m => m.ToIntLetterScore(m.LetterScore)).Take(10).ToList());
            #endregion
            #region Definiendo la data
            Dictionary<int, float> data2 = new Dictionary<int, float>();
            for (int i = 1; i <= 10; i++)
            {
                try
                {
                    if (grado > 6) //Bachillerato
                        data2.Add(i, top10MejoresNotas[i - 1].NumberScore);
                    else //Primaria
                        data2.Add(i, top10MejoresNotas[i - 1].ToIntLetterScore(top10MejoresNotas[i - 1].LetterScore));
                }
                catch (ArgumentOutOfRangeException)
                {
                    data2.Add(i, 1);
                }
            }
            #endregion
            #region Desarrollando la gráfica
            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 255;
            chart.Height = 278;

            chartArea = new ChartArea();
            chartArea.AxisY.LabelStyle.Enabled = true;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisY.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisY.IsLabelAutoFit = true;
            chartArea.AxisY.LineColor = Color.Blue;
            chartArea.AxisY.Maximum = (grado > 6 ? 20 : 5);
            chartArea.AxisY.Title = "Notas obtenidas";
            chartArea.AxisY.TitleFont = graphBarChartFontAxisTitle;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.Lavender;
            chartArea.AxisY.MajorGrid.LineWidth = 6;
            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.LineColor = Color.Blue;
            chartArea.AxisY.MinorTickMark.Enabled = false;
            if (grado <= 6)//Primaria
            {
                chartArea.AxisY.CustomLabels.Add(0, 1, "E");
                chartArea.AxisY.CustomLabels.Add(1, 2, "D");
                chartArea.AxisY.CustomLabels.Add(2, 3, "C");
                chartArea.AxisY.CustomLabels.Add(3, 4, "B");
                chartArea.AxisY.CustomLabels.Add(4, 5, "A");
            }

            chartArea.AxisX.LabelStyle.Enabled = true;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisX.LineColor = Color.Blue;
            chartArea.AxisX.Maximum = 11;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.MinorTickMark.Enabled = false;
            chartArea.AxisX.LabelAutoFitMinFontSize = 7;

            for (int i = 0; i <= 10; i++)
            {
                if (i != 10)
                {
                    try
                    {
                        if (grado > 6) //Bachillerato
                            stringAux = "(" + top10MejoresNotas[i].NumberScore + ") ";
                        else //Primaria
                            stringAux = "(" + top10MejoresNotas[i].LetterScore + ") ";

                        stringAux = stringAux + top10MejoresNotas[i].Student.FirstLastName + " " +
                            top10MejoresNotas[i].Student.SecondLastName + ", " +
                            top10MejoresNotas[i].Student.FirstName;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        stringAux = "N/A";
                    }

                    chartArea.AxisX.CustomLabels.Add(i + 0.5, i + 1.5, stringAux);
                }
            }
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Column;
            chartSeries.Color = Color.Blue;
            chartSeries.BorderColor = Color.DarkBlue;
            chartSeries.BackGradientStyle = GradientStyle.LeftRight;
            foreach (var item in data2) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chart.Series.Add(chartSeries);
            #endregion
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(path2, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            imageBase64_1 = Convert.ToBase64String(imageByte1);
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Invocando el Web Service
            WSClient = new WS_Security.Service1SoapClient();

            WSClient.StatisticsImageGenerator(
                ConstantRepository.MOBILE_STATISTICS_CODE_Top10ResultadosDestacados,
                casu.CourseId,
                casu.Period.SchoolYear.SchoolYearId,
                casu.Period.SchoolYear.School.SchoolId,
                imageBase64_1);
            #endregion
            #endregion
            #region Gráfico #3
            #region Declaración de variables
            graphBarChartFont = new System.Drawing.Font("Almanac MT", 8);
            graphBarChartFontAxisTitle = new System.Drawing.Font("Helvetica", 8);
            stringAux = "";
            #endregion
            #region Obteniendo la lista top 10 mejores notas
            top10MejoresNotas = (grado > 6 ?
                listaNotas.OrderBy(m => m.NumberScore).Take(10).ToList() :
                listaNotas.OrderBy(m => m.ToIntLetterScore(m.LetterScore)).Take(10).ToList());
            #endregion
            #region Definiendo la data
            data2 = new Dictionary<int, float>();
            for (int i = 1; i <= 10; i++)
            {
                try
                {
                    if (grado > 6) //Bachillerato
                        data2.Add(i, top10MejoresNotas[i - 1].NumberScore);
                    else //Primaria
                        data2.Add(i, top10MejoresNotas[i - 1].ToIntLetterScore(top10MejoresNotas[i - 1].LetterScore));
                }
                catch (ArgumentOutOfRangeException)
                {
                    data2.Add(i, 1);
                }
            }
            #endregion
            #region Desarrollando la gráfica
            chart = new Chart();
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Width = 255;
            chart.Height = 278;

            chartArea = new ChartArea();
            chartArea.AxisY.LabelStyle.Enabled = true;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisY.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisY.IsLabelAutoFit = true;
            chartArea.AxisY.LineColor = Color.Blue;
            chartArea.AxisY.Maximum = (grado > 6 ? 20 : 5);
            chartArea.AxisY.Title = "Notas obtenidas";
            chartArea.AxisY.TitleFont = graphBarChartFontAxisTitle;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.Lavender;
            chartArea.AxisY.MajorGrid.LineWidth = 6;
            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.LineColor = Color.Blue;
            chartArea.AxisY.MinorTickMark.Enabled = false;
            if (grado <= 6)//Primaria
            {
                chartArea.AxisY.CustomLabels.Add(0, 1, "E");
                chartArea.AxisY.CustomLabels.Add(1, 2, "D");
                chartArea.AxisY.CustomLabels.Add(2, 3, "C");
                chartArea.AxisY.CustomLabels.Add(3, 4, "B");
                chartArea.AxisY.CustomLabels.Add(4, 5, "A");
            }

            chartArea.AxisX.LabelStyle.Enabled = true;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.LabelStyle.Font = graphBarChartFont;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisX.LineColor = Color.Blue;
            chartArea.AxisX.Maximum = 11;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.MinorTickMark.Enabled = false;
            chartArea.AxisX.LabelAutoFitMinFontSize = 7;

            for (int i = 0; i <= 10; i++)
            {
                if (i != 10)
                {
                    try
                    {
                        if (grado > 6) //Bachillerato
                            stringAux = "(" + top10MejoresNotas[i].NumberScore + ") ";
                        else //Primaria
                            stringAux = "(" + top10MejoresNotas[i].LetterScore + ") ";

                        stringAux = stringAux + top10MejoresNotas[i].Student.FirstLastName + " " +
                            top10MejoresNotas[i].Student.SecondLastName + ", " +
                            top10MejoresNotas[i].Student.FirstName;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        stringAux = "N/A";
                    }

                    chartArea.AxisX.CustomLabels.Add(i + 0.5, i + 1.5, stringAux);
                }
            }
            chart.ChartAreas.Add(chartArea);

            chartSeries = new Series();
            chartSeries.ChartType = SeriesChartType.Column;
            chartSeries.Color = Color.Red;
            chartSeries.BorderColor = Color.DarkRed;
            chartSeries.BackGradientStyle = GradientStyle.LeftRight;
            foreach (var item in data2) { chartSeries.Points.AddXY(item.Key, item.Value); }
            chart.Series.Add(chartSeries);
            #endregion
            #region Definiendo un Memory Stream para guardar la imagen
            memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream, ChartImageFormat.Png);
            //chart.SaveImage(path2, ChartImageFormat.Png);
            #endregion
            #region Obteniendo imagen desde MemoryStream
            img1 = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            #endregion
            #region Transformando imagen a array binario
            stream1 = new MemoryStream();
            img1.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            imageByte1 = stream1.ToArray();
            imageBase64_1 = Convert.ToBase64String(imageByte1);
            stream1.Dispose();
            img1.Dispose();
            #endregion
            #region Invocando el Web Service
            WSClient = new WS_Security.Service1SoapClient();
            //WSClient_localhost = new WS_Security_localhost.Service1();

            WSClient.StatisticsImageGenerator(
                ConstantRepository.MOBILE_STATISTICS_CODE_Top10ResultadosDeficientes,
                casu.CourseId,
                casu.Period.SchoolYear.SchoolYearId,
                casu.Period.SchoolYear.School.SchoolId,
                imageBase64_1);
            #endregion
            #endregion

            return true;
        }

        public JsonResult CargarArchivoExcelAlumnosEnTabla_Prueba()
        {            
            try
            {
                #region Subiendo archivo excel al servidor
                HttpPostedFileBase archivoExcel = Request.Files[0];
                Stream stream = archivoExcel.InputStream;
                string fileName = Path.GetFileName(archivoExcel.FileName);
                string path = Path.Combine(Server.MapPath("~/App_Uploads/Lista_Alumnos"), fileName);

                using (FileStream fileStream = System.IO.File.Create(path))
                {
                    stream.CopyTo(fileStream);
                }
                #endregion
                
                #region Variables del workbook excel
                XLWorkbook workbook = new XLWorkbook(path);
                IXLWorksheet worksheet = workbook.Worksheet(1);
                IXLRow firstRowUsed = worksheet.FirstRowUsed();
                IXLRangeRow infoRow = firstRowUsed.RowUsed();
                List<object> jsonResult = new List<object>();
                bool bajarFilas = true;
                #endregion
                #region Variables de columnas - Varía según el formato de la plantilla
                int ColumnaNroLista = 1;
                int ColumnaMatricula = 2;
                int ColumnaApellido1 = 3;
                int ColumnaApellido2 = 4;
                int ColumnaNombre1 = 5;
                int ColumnaNombre2 = 6;
                #endregion
                
                while (!infoRow.Cell(ColumnaNroLista).IsEmpty() ||
                       !infoRow.Cell(ColumnaMatricula).IsEmpty() ||
                       !infoRow.Cell(ColumnaApellido1).IsEmpty() ||
                       !infoRow.Cell(ColumnaApellido2).IsEmpty() ||
                       !infoRow.Cell(ColumnaNombre1).IsEmpty() ||
                       !infoRow.Cell(ColumnaNombre2).IsEmpty())
                {
                    #region Fila a empezar - Varía según el formato de la plantilla
                    if(bajarFilas)
                    {
                        infoRow = infoRow.RowBelow(); //Varía según el formato de la plantilla
                        bajarFilas = false;
                    }                        
                    #endregion
                    #region Variables de datos - Varía según el formato de la plantilla
                    #endregion
                    #region Asociando el estudiante al JSON
                    jsonResult.Add(new
                    {
                        numLista = infoRow.Cell(ColumnaNroLista).GetString(),
                        matricula = infoRow.Cell(ColumnaMatricula).GetString(),
                        apellido1 = infoRow.Cell(ColumnaApellido1).GetString(),
                        apellido2 = infoRow.Cell(ColumnaApellido2).GetString(),
                        nombre1 = infoRow.Cell(ColumnaNombre1).GetString(),
                        nombre2 = infoRow.Cell(ColumnaNombre2).GetString()
                    });
                    #endregion

                    infoRow = infoRow.RowBelow();
                }

                return Json(jsonResult);
            }
            catch (Exception e)
            {
                TempData["CatchError"] = e.Message;
                throw e;
            }
        }
        public JsonResult CargarArchivoExcelAlumnosEnTabla_AgustinianoCristoRey()
        {
            List<object> jsonResult = new List<object>();

            try
            {
                #region Subiendo archivo excel al servidor
                HttpPostedFileBase archivoExcel = Request.Files[0];
                Stream stream = archivoExcel.InputStream;
                string fileName = Path.GetFileName(archivoExcel.FileName);
                string path = Path.Combine(Server.MapPath("~/App_Uploads/Lista_Alumnos"), fileName);

                using (FileStream fileStream = System.IO.File.Create(path))
                {
                    stream.CopyTo(fileStream);
                }
                #endregion
                
                #region Variables del workbook excel
                XLWorkbook workbook = new XLWorkbook(path);
                IXLWorksheet worksheet = workbook.Worksheet(1);
                IXLRow firstRowUsed = worksheet.FirstRowUsed();
                IXLRangeRow infoRow = firstRowUsed.RowUsed();
                
                bool bajarFilas = true;
                #endregion
                #region Variables de columnas - Varía según el formato de la plantilla
                int ColumnaNroLista = -1;
                int ColumnaMatricula = 0;
                int NombreCompletoAlumno = 1;
                #endregion

                while (!infoRow.Cell(ColumnaNroLista).IsEmpty() ||
                       !infoRow.Cell(ColumnaMatricula).IsEmpty() ||
                       !infoRow.Cell(NombreCompletoAlumno).IsEmpty())
                {
                    #region Fila a empezar - Varía según el formato de la plantilla
                    if (bajarFilas)
                    {
                        infoRow = infoRow.RowBelow(); //Fila #1
                        infoRow = infoRow.RowBelow(); //Fila #2
                        infoRow = infoRow.RowBelow(); //Fila #3
                        infoRow = infoRow.RowBelow(); //Fila #4
                        infoRow = infoRow.RowBelow(); //Fila #5
                        infoRow = infoRow.RowBelow(); //Fila #6
                        infoRow = infoRow.RowBelow(); //Fila #7
                        infoRow = infoRow.RowBelow(); //Fila #8
                        infoRow = infoRow.RowBelow(); //Fila #9
                        infoRow = infoRow.RowBelow(); //Fila #10
                        bajarFilas = false;
                    }
                    #endregion

                    #region Variables de datos - Varía según el formato de la plantilla
                    string[] nombreCompleto_split = infoRow.Cell(NombreCompletoAlumno).GetString().Split(',');
                    string[] apellido_split = nombreCompleto_split[0].Split(' ');
                    string[] nombre_split = nombreCompleto_split[1].Split(' ');

                    //Porque el split del nombre comienza con un espacio. Ej. ' Rodrigo José'
                    string nombre1 = nombre_split[1];
                    string nombre2 = nombre_split.Count() == 3 ? nombre_split[2] : "";

                    /* Observación (07-05-15) Rodrigo Uzcátegui - No está considerando aquellos apellidos y nombres
                     * con artículos (Ej. Pedro De La Rosa)
                     */
                    #endregion
                    #region Asociando el estudiante al JSON
                    jsonResult.Add(new
                    {
                        success = true,
                        numLista = infoRow.Cell(ColumnaNroLista).GetString(),
                        matricula = infoRow.Cell(ColumnaMatricula).GetString(),
                        apellido1 = apellido_split[0],
                        apellido2 = apellido_split[1],
                        nombre1 = nombre1, 
                        nombre2 = nombre2  
                    });
                    #endregion

                    infoRow = infoRow.RowBelow();
                }
            }
            catch (Exception e)
            {
                TempData["CatchError"] = e.Message;
                jsonResult.Add(new { success = false });
            }

            return Json(jsonResult);
        }

        public SelectList InicializadorListaAtribuciones(SelectList lista)
        {
            #region Declaración de variables
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            NotificationService service = new NotificationService();
            #endregion

            diccionario = service.ObtenerDiccionarioAtribuciones();
            lista = new SelectList(diccionario, "Key", "Value");

            return lista;
        }
        public SelectList InicializadorListaCursos(SelectList lista, int idAnoEscolar)
        {
            Dictionary<int, string> diccionario = new Dictionary<int, string>();
            CourseService service = new CourseService();

            diccionario = service.ObtenerDiccionarioCursosPorAnoEscolar(idAnoEscolar);
            lista = new SelectList(diccionario, "Key", "Value");

            return lista;
        }
        public SelectList InicializadorListaCursosPorDocente(SelectList lista, int idAnoEscolar, string idUsuario)
        {
            Dictionary<int, string> diccionario = new Dictionary<int, string>();
            CourseService service = new CourseService();

            diccionario = service.ObtenerDiccionarioCursosPorAnoEscolar(idAnoEscolar, idUsuario);
            lista = new SelectList(diccionario, "Key", "Value");

            return lista;
        }
        public SelectList InicializadorListaSujetosNotificaciones(SelectList lista)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            diccionario.Add("Representante", "Representante");
            diccionario.Add("Usuario", "Usuario");

            lista = new SelectList(diccionario, "Key", "Value");

            return lista;
        }
        public SelectList InicializadorListaSujetosNotificacionesDocente(SelectList lista)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            diccionario.Add("Representante", "Representante");

            lista = new SelectList(diccionario, "Key", "Value");

            return lista;
        }
        public SelectList InicializadorListaSujetosBuzon(SelectList lista)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            diccionario.Add("Representante", "Un representante");
            diccionario.Add("Curso", "Un curso completo");

            lista = new SelectList(diccionario, "Key", "Value");

            return lista;
        }
        public SelectList InicializadorListaSexos(SelectList lista)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            int llave = 0;

            foreach (string tipoSexo in ConstantRepository.SEX_LIST_LARGE)
            {
                diccionario.Add(llave.ToString(), tipoSexo);
                llave++;
            }

            lista = new SelectList(diccionario, "Key", "Value");

            return lista;
        }
        public SelectList InicializadorListaTiposCedula(SelectList lista)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();

            foreach(string tipoCedula in ConstantRepository.IDENTITY_NUMBER_TYPE_LIST)
            {
                diccionario.Add(tipoCedula, tipoCedula);
            }

            lista = new SelectList(diccionario, "Key", "Value");

            return lista;
        }
        public SelectList InicializadorListaEstatus(SelectList lista)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            diccionario.Add("Activo", "Activo");
            diccionario.Add("Inactivo", "Inactivo");

            lista = new SelectList(diccionario, "Key", "Value");

            return lista;
        }
        public SelectList InicializadorListaEstatusYNoAplica(SelectList lista)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            diccionario.Add("Activo", "Activo");
            diccionario.Add("Inactivo", "Inactivo");
            diccionario.Add("No aplica", "No aplica");

            lista = new SelectList(diccionario, "Key", "Value");

            return lista;
        }
        public List<Profile> InicializadorListaPerfiles()
        {
            List<Profile> listaPerfiles = new List<Profile>();
            ProfileService profileService = new ProfileService();
            listaPerfiles = profileService.ObtenerListaPerfiles().OrderBy(m => m.ControllerName).ToList<Profile>();
            listaPerfiles = (listaPerfiles.Count == 0) ? new List<Profile>() : listaPerfiles;

            return listaPerfiles;
        }
        public List<Role> InicializadorListaRoles()
        {
            List<Role> listaRoles = new List<Role>();
            RoleService roleService = new RoleService();
            listaRoles = roleService.ObtenerListaRoles().OrderBy(m => m.Name).ToList<Role>();

            return listaRoles;
        }
        public List<AddRoleViewModel.PersonalProfile> InicializadorListaPerfilesParaRol()
        {
            #region Inicialización de variables
            List<Profile> listaPerfiles = new List<Profile>();
            List<AddRoleViewModel.PersonalProfile> listaPerfilesParaRol =
                new List<AddRoleViewModel.PersonalProfile>();
            ProfileService _profileService = new ProfileService();
            #endregion
            #region Obteniendo lista global de perfiles
            listaPerfiles = _profileService.ObtenerListaPerfiles().ToList<Profile>();
            listaPerfiles = (listaPerfiles.Count == 0) ? new List<Profile>() : listaPerfiles;
            #endregion
            #region Asignando lista perfiles al modelo
            foreach (Profile perfil in listaPerfiles)
            {
                listaPerfilesParaRol.Add(new AddRoleViewModel.PersonalProfile()
                {
                    profile = perfil
                });
            }
            #endregion

            return listaPerfilesParaRol;
        }
        public List<CrearAnoEscolarModel.PersonalSchool> InicializarListaColegiosPersonales(
            List<CrearAnoEscolarModel.PersonalSchool> listaColegiosPersonales)
        {
            SchoolService _schoolService = new SchoolService();
            List<School> listaColegios = _schoolService.ObtenerListaColegios().ToList<School>();
            CrearAnoEscolarModel.PersonalSchool colegioPersonal;

            foreach (School colegio in listaColegios)
            {
                colegioPersonal = new CrearAnoEscolarModel.PersonalSchool();
                colegioPersonal.colegio = colegio;
                colegioPersonal.isSelected = false;
                colegioPersonal.anoEscolarActivo = _schoolService.ColegioPoseeAnoEscolarActivo(colegio.SchoolId);

                listaColegiosPersonales.Add(colegioPersonal);
            }

            return listaColegiosPersonales;
        }

        public JsonResult ObtenerJsonEvaluacionesPor_Curso_Materia_Lapso_Docente(int idMateria, int idCurso,
            int idLapso, string idDocente)
        {
            #region Declaración de variables
            List<Assessment> listaEvaluaciones = new List<Assessment>();
            List<object> jsonResult = new List<object>();
            AssessmentService assessmentService = new AssessmentService();
            int Grado = 0;
            #endregion

            #region Obteniendo lista de evaluaciones
            listaEvaluaciones = assessmentService
                .ObtenerListaEvaluacionesPor_Curso_Materia_Docente_Lapso(idMateria, idCurso, idDocente, idLapso);
            #endregion
            #region Transformando lista de evaluaciones a JsonResult
            foreach (Assessment evaluacion in listaEvaluaciones)
            {
                Grado = evaluacion.CASU.Course.Grade;

                jsonResult.Add(new
                {
                    nombre = evaluacion.Name,
                    tecnica = evaluacion.Technique,
                    actividad = evaluacion.Activity,
                    instrumento = evaluacion.Instrument,
                    porcentaje = (Grado <= 6 ? "N/A" : evaluacion.Percentage.ToString()),
                    fechainicio = evaluacion.StartDate.ToString(),
                    fechafin = evaluacion.FinishDate.ToString(),
                    idEvaluacion = evaluacion.AssessmentId
                });
            }
            #endregion

            return Json(jsonResult);
        }        

        public JsonResult ObtenerAnosEscolaresSinPeriodos(int idColegio)
        {
            #region Declaración de variables
            SchoolYearService _schoolYearService = new SchoolYearService();
            List<object> jsonResult = new List<object>();
            #endregion
            #region Obteniendo lista de años escolares
            List<SchoolYear> listaAnosEscolares =
                _schoolYearService.ObtenerAnosEscolaresSinPeriodosPorColegio(idColegio);
            #endregion
            #region Creando objeto JSON
            foreach (SchoolYear schoolYear in listaAnosEscolares)
            {
                jsonResult.Add(
                    new
                    {
                        idAnoEscolar = schoolYear.SchoolYearId,
                        AnoEscolar = schoolYear.StartDate.ToShortDateString() + " - " +
                        schoolYear.EndDate.ToShortDateString()
                    });
            }
            #endregion

            return Json(jsonResult);
        }
        public ReadOnlyDictionary<int, bool> ObtenerAccesoAccionesMaestras(string roleName)
        {
            RoleService roleService = new RoleService();
            ReadOnlyDictionary<int, bool> diccionario = roleService.ObtenerAccesoAccionesMaestras(roleName);

            return diccionario;
        }
        public JsonResult ObtenerSelectListProfesores(string idLapso, string idCurso, string idMateria)
        {
            #region Declaración de variables
            List<User> listaProfesores = new List<User>();
            List<object> jsonResult = new List<object>();
            CourseService _courseService = new CourseService();
            #endregion
            #region Obtener lista de profesores
            listaProfesores = _courseService.ObtenerListaProfesoresPorLapsoCursoMateria(
                int.Parse(idLapso), 
                int.Parse(idCurso), 
                int.Parse(idMateria)
            );
            #endregion

            #region Validación de lista nula
            if (listaProfesores.Count == 0 ||
               (listaProfesores.Count == 1 && listaProfesores[0] == null))
            {
                jsonResult.Add(new { 
                    Success = false ,
                    TipoError = "No docentes"
                });
            }
            #endregion
            #region Devolviendo la lista de docentes
            else
            {
                foreach (User profesor in listaProfesores)
                {
                    jsonResult.Add(new
                    {
                        Success = true,
                        nombre = profesor.LastName + ", " + profesor.Name,
                        idProfesor = profesor.Id,
                    });
                }
            }
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerTablaMateriaPorIdColegio(int idColegio)
        {
            #region Declaración de variables
            List<Subject> listaMaterias = new List<Subject>();
            List<object> jsonResult = new List<object>();
            SubjectService _subjectService = new SubjectService();
            #endregion
            #region Obteniendo lista de materias
            listaMaterias = _subjectService.ObtenerListaMateriasPorColegio(idColegio);
            #endregion
            #region Transformando lista de materias a JsonResult
            foreach (Subject materia in listaMaterias)
            {
                jsonResult.Add(new
                {
                    nombre = materia.Name,
                    codigo = materia.SubjectCode,
                    pensum = materia.GeneralPurpose,
                    idMateria = materia.SubjectId,
                    grado = materia.Grade
                });
            }
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerTablaMateriasPorIdCurso(int idCurso)
        {
            #region Declaración de variables
            List<Subject> listaMaterias = new List<Subject>();
            //Course Curso = new Course();
            List<object> jsonResult = new List<object>();
            SubjectService _subjectService = new SubjectService();
            //_courseService = (_courseService == null ? new CourseService() : _courseService);
            #endregion
            #region Obteniendo lista de estudiantes y curso
            string idsession = (string)Session["UserId"];
            listaMaterias = _subjectService.ObtenerListaMateriasPorCurso(idCurso, idsession);
            #endregion
            #region Transformando lista de materias a JsonResult
            foreach (Subject materia in listaMaterias)
            {

                jsonResult.Add(new
                {
                    nombre = materia.Name,
                    codigo = materia.SubjectCode,
                    pensum = materia.GeneralPurpose,
                    idMateria = materia.SubjectId,

                });
            }
            #endregion
            return Json(jsonResult);
        }
        public JsonResult ObtenerTablaCrearMateriaPorIdCurso(int idCurso, string idProfesor)
        {
            #region Declaración de variables
            List<Subject> listaMaterias = new List<Subject>();
            //Course Curso = new Course();
            List<object> jsonResult = new List<object>();
            SubjectService _subjectService = new SubjectService();
            //_courseService = (_courseService == null ? new CourseService() : _courseService);
            #endregion
            #region Obteniendo lista de estudiantes y curso
            listaMaterias = _subjectService.ObtenerListaMateriasPorCurso(idCurso, idProfesor);
            #endregion
            #region Transformando lista de materias a JsonResult
            foreach (Subject materia in listaMaterias)
            {

                jsonResult.Add(new
                {
                    nombre = materia.Name,
                    codigo = materia.SubjectCode,
                    pensum = materia.GeneralPurpose,
                    idMateria = materia.SubjectId,

                });
            }
            #endregion
            return Json(jsonResult);
        }
        public JsonResult ObtenerTablaCrearMateriaPorIdColegio(int idColegio, int grado)
        {
            #region Declaración de variables
            List<Subject> listaMaterias = new List<Subject>();
            List<object> jsonResult = new List<object>();
            SubjectService _subjectService = new SubjectService();
            #endregion
            #region Obteniendo lista de materias
            listaMaterias = _subjectService.ObtenerListaMateriasPor_Colegio_Grado(idColegio, grado);
            #endregion
            #region No hay data
            if(listaMaterias.Count == 0)
            {
                jsonResult.Add(new { success = false });
            }
            #endregion
            #region Si hay data. Obteniendo JSON
            else
            {
                foreach (Subject materia in listaMaterias)
                {
                    jsonResult.Add(new
                    {
                        success = true,
                        nombre = materia.Name,
                        codigo = materia.SubjectCode,
                        pensum = materia.GeneralPurpose,
                        idMateria = materia.SubjectId,
                        grado = materia.Grade
                    });
                }
            }
            #endregion
            return Json(jsonResult);
        }
        public JsonResult ObtenerSelectListMaterias(int idCurso)
        {
            #region Declaración de variables
            List<Subject> listaMaterias = new List<Subject>();
            List<object> jsonResult = new List<object>();
            SubjectService _subjectService = new SubjectService();
            #endregion
            #region Obtener lista de cursos
            string idsession = (string)Session["UserId"];
            listaMaterias = _subjectService.ObtenerListaMateriasPorCurso(idCurso, idsession);
            #endregion

            foreach (Subject materia in listaMaterias)
            {

                jsonResult.Add(new
                {
                    nombre = materia.Name,
                    idMateria = materia.SubjectId,

                });
            }
            return Json(jsonResult);
        }
        public JsonResult ObtenerSelectListMateriasPorLapsoYCurso(string idLapso, string idCurso)
        {
            #region Declaración de variables
            List<Subject> listaMaterias = new List<Subject>();
            List<object> jsonResult = new List<object>();
            SubjectService _subjectService = new SubjectService();
            #endregion
            #region Obtener lista de cursos
            listaMaterias = _subjectService.ObtenerListaMateriasPorLapsoYCurso
                                                           (int.Parse(idLapso), int.Parse(idCurso));
            #endregion

            foreach (Subject materia in listaMaterias)
            {

                jsonResult.Add(new
                {
                    nombre = materia.Name,
                    idMateria = materia.SubjectId,

                });
            }
            return Json(jsonResult);
        }
        public JsonResult ObtenerDatosModificarMateria(int idMateria)
        {
            #region Declaración de variables
            Subject materia = new Subject();
            List<object> jsonResult = new List<object>();
            SubjectService _subjectService = new SubjectService();
            #endregion
            #region Obteniendo la materia
            materia = _subjectService.ObtenerMateriaPorId(idMateria);

            #endregion
            #region Transformando lista de materias a JsonResult

            jsonResult.Add(new
            {
                nombre = materia.Name,
                codigo = materia.SubjectCode,
                pensum = materia.GeneralPurpose

            });

            #endregion
            return Json(jsonResult);
        }
        public JsonResult ObtenerTablaAlumnosPorIdCurso(int idCurso)
        {
            #region Declaración de variables
            List<Student> listaEstudiantes = new List<Student>();
            Course Curso = new Course();
            List<object> jsonResult = new List<object>();
            StudentService _studentService = new StudentService();
            CourseService _courseService = new CourseService();
            #endregion
            #region Obteniendo lista de estudiantes y curso
            listaEstudiantes = _studentService.ObtenerListaEstudiantePorCurso(idCurso)
                .OrderBy(m => m.FirstLastName).ThenBy(m => m.SecondLastName).ToList();
            
            Curso = _courseService.ObtenerCursoPor_Id(idCurso);
            #endregion
            #region Transformando lista de estudiantes a JsonResult
            foreach (Student estudiante in listaEstudiantes)
            {
                jsonResult.Add(new
                {
                    numerolista = estudiante.NumberList,
                    matricula = estudiante.RegistrationNumber,
                    apellido1 = estudiante.FirstLastName,
                    apellido2 = estudiante.SecondLastName,
                    nombre1 = estudiante.FirstName,
                    nombre2 = estudiante.SecondName == null ? "" : estudiante.SecondName,
                    idEstudiante = estudiante.StudentId
                });
            }



            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerDetalleCurso(int idCurso)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();

            CourseService courseService = new CourseService();
            SubjectService subjectService = new SubjectService();
            PeriodService periodService = new PeriodService();
            AssessmentService assessmentService = new AssessmentService();
            #endregion
            #region Obteniendo curso y datos asociados
            Course course = courseService.ObtenerCursoPor_Id(idCurso);
            SchoolYear schoolYear = course.CASUs[0].Period.SchoolYear;
            Period lapsoActual = periodService.ObtenerPeriodoActivoPor_AnoEscolar(schoolYear.SchoolYearId);
            List<Period> listaLapsos = schoolYear.Periods;
            
            string fechaInicio_AnoEscolar = schoolYear.StartDate.ToString("dd-MM-yyyy");
            string fechaFin_AnoEscolar = schoolYear.EndDate.ToString("dd-MM-yyyy");
            int nroMaterias = subjectService.ObtenerListaMateriasPor_Curso(idCurso).Count();
            int nroEstudiantes = course.Students.Count();
            int nroEvaluaciones = assessmentService.ObtenerListaEvaluacionesPor_Curso(idCurso).Count();
            #endregion
            #region Obteniendo objeto JSON
            jsonResult.Add(new
            {
                nombrecurso = course.Name,
                numeroestudiantes = nroEstudiantes,
                lapsoencurso = lapsoActual.Name,
                periodoescolar = fechaInicio_AnoEscolar + " - " + fechaFin_AnoEscolar,
                primerlapso = listaLapsos[0].StartDate.ToString("dd-MM-yyyy") + " - " + 
                              listaLapsos[0].FinishDate.ToString("dd-MM-yyyy"),
                segundolapso = listaLapsos[1].StartDate.ToString("dd-MM-yyyy") + " - " + 
                               listaLapsos[1].FinishDate.ToString("dd-MM-yyyy"),
                tercerlapso = listaLapsos[2].StartDate.ToString("dd-MM-yyyy") + " - " + 
                              listaLapsos[2].FinishDate.ToString("dd-MM-yyyy"),
                cantidadMaterias = nroMaterias,
                cantidadEvaluaciones = nroEvaluaciones
            });
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerSelectListAlumnosConNotas(int idMateria, int idCurso, int idLapso, int idEvaluacion)
        {
            #region Declaración de variables
            List<Student> listaAlumnos = new List<Student>();
            List<object> jsonResult = new List<object>();
            StudentService _studentService = new StudentService();
            #endregion
            #region Obtener lista de estudiantes
            listaAlumnos = _studentService.ObtenerListaEstudiantesConNotas(idCurso).ToList<Student>();
            #endregion

            foreach (Student alumno in listaAlumnos)
            {
                jsonResult.Add(new
                {
                    nombre = alumno.FirstLastName + ", " + alumno.FirstName,
                    idAlumno = alumno.StudentId,
                });
            }
            return Json(jsonResult);
        }
        public void CargarAlumnos(string firstlastname, string secondlastname, string firstname,
            string secondname)
        {
            #region Inicializando el servicio de estudiantes
            StudentService _studentService = new StudentService();
            #endregion
            #region Creando el nuevo estudiante
            Student estudiante = new Student()
            {
                FirstLastName = firstlastname,
                SecondLastName = secondlastname,
                FirstName = firstname,
                SecondName = (secondname == "" ? null : secondname),
            };
            #endregion
            #region Salvando el alumno nuevo
            _studentService.GuardarStudent(estudiante);
            #endregion
        }
        public JsonResult ObtenerTablaEvaluacionesPorMateriaCursoYLapso(int idMateria, int idCurso, int idLapso)
        {
            ObteniendoSesion();

            #region Declaración de variables
            List<Assessment> listaEvaluaciones = new List<Assessment>();

            List<object> jsonResult = new List<object>();
            AssessmentService assessmentService = new AssessmentService();

            #endregion

            #region Obteniendo lista de evaluaciones
            listaEvaluaciones = assessmentService
                .ObtenerListaEvaluacionesPor_Curso_Materia_Docente_Lapso(idMateria, idCurso, _session.USERID, idLapso);
            #endregion
            #region Transformando lista de evaluaciones a JsonResult
            foreach (Assessment evaluacion in listaEvaluaciones)
            {

                jsonResult.Add(new
                {
                    nombre = evaluacion.Name,
                    tecnica = evaluacion.Technique,
                    actividad = evaluacion.Activity,
                    instrumento = evaluacion.Instrument,
                    porcentaje = evaluacion.Percentage,
                    fechainicio = evaluacion.StartDate.ToString(),
                    fechafin = evaluacion.FinishDate.ToString(),
                    idEvaluacion = evaluacion.AssessmentId

                });
            }
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerTablaModificarEvaluaciones(int idMateria, int idCurso, string idProfesor,
            int idLapso, int idColegio)
        {
            #region Declaración de variables
            List<Assessment> listaEvaluaciones = new List<Assessment>();
            List<object> jsonResult = new List<object>();
            AssessmentService assessmentService = new AssessmentService();
            #endregion
            #region Obteniendo lista de evaluaciones
            string idsession = (string)Session["UserId"];
            try
            {
                listaEvaluaciones = assessmentService
                    .ObtenerListaEvaluacionesPor_Curso_Materia_Docente_Lapso(idMateria, idCurso, idProfesor, idLapso);
            }
            catch (Exception e)
            {
                TempData["ErrorNoHayEvaluacionesAgregadas"] = "No existen evaluaciones asociadas a este curso," +
                 " lapso, materia o profesor. ";
                TempData["null"] = e.Message;

                return Json(jsonResult);
            }
            #endregion
            #region Transformando lista de evaluaciones a JsonResult
            foreach (Assessment evaluacion in listaEvaluaciones)
            {
                DateTime fechainiciodateonly = evaluacion.StartDate.Date;
                DateTime fechafindateonly = evaluacion.FinishDate.Date;
                jsonResult.Add(new
                {
                    nombre = evaluacion.Name,
                    tecnica = evaluacion.Technique,
                    actividad = evaluacion.Activity,
                    instrumento = evaluacion.Instrument,
                    porcentaje = evaluacion.Percentage,
                    fechainicio = fechainiciodateonly.ToString("dd-MM-yyyy"),
                    fechafin = fechafindateonly.ToString("dd-MM-yyyy"),
                    idevaluacion = evaluacion.AssessmentId

                });
            }
            #endregion
            return Json(jsonResult);
        }
        public JsonResult ObtenerTablaModificarEvaluaciones_S(int idMateria, int idCurso,
            int idLapso)
        {
            #region Declaración de variables
            List<Assessment> listaEvaluaciones = new List<Assessment>();
            List<object> jsonResult = new List<object>();
            AssessmentService assessmentService = new AssessmentService();
            SessionVariablesRepository _session = new SessionVariablesRepository();
            #endregion
            #region Obteniendo lista de evaluaciones
            string idsession = (string)Session["UserId"];
            try
            {
                listaEvaluaciones = assessmentService
                    .ObtenerListaEvaluacionesPor_Curso_Materia_Docente_Lapso(idMateria, idCurso, _session.USERID,
                idLapso);
            }
            catch (Exception e)
            {
                TempData["ErrorNoHayEvaluacionesAgregadas"] = "No existen evaluaciones asociadas a este curso," +
                 " lapso, materia o profesor. ";
                TempData["null"] = e.Message;

                return Json(jsonResult);
            }
            #endregion
            #region Transformando lista de evaluaciones a JsonResult
            foreach (Assessment evaluacion in listaEvaluaciones)
            {
                DateTime fechainiciodateonly = evaluacion.StartDate.Date;
                DateTime fechafindateonly = evaluacion.FinishDate.Date;
                jsonResult.Add(new
                {
                    nombre = evaluacion.Name,
                    tecnica = evaluacion.Technique,
                    actividad = evaluacion.Activity,
                    instrumento = evaluacion.Instrument,
                    porcentaje = evaluacion.Percentage,
                    fechainicio = fechainiciodateonly.ToString("dd-MM-yyyy"),
                    fechafin = fechafindateonly.ToString("dd-MM-yyyy"),
                    idevaluacion = evaluacion.AssessmentId

                });
            }
            #endregion
            return Json(jsonResult);
        }
        public JsonResult ObtenerDatosModificarEvaluacion(int idEvaluacion)
        {
            #region Declaración de variables
            Assessment evaluacion = new Assessment();
            List<object> jsonResult = new List<object>();
            AssessmentService assessmentService = new AssessmentService();
            #endregion
            #region Obteniendo levaluacion
            evaluacion = assessmentService.ObtenerEvaluacionPor_Id(idEvaluacion);
            #endregion
            #region Transformando lista de evaluaciones a JsonResult

            DateTime fechainiciodateonly = evaluacion.StartDate.Date;
            DateTime fechafindateonly = evaluacion.FinishDate.Date;
            jsonResult.Add(new
            {
                nombre = evaluacion.Name,
                tecnica = evaluacion.Technique,
                actividad = evaluacion.Activity,
                instrumento = evaluacion.Instrument,
                porcentaje = evaluacion.Percentage,
                fechainicio = fechainiciodateonly.ToString("dd-MM-yyyy"),
                fechafin = fechafindateonly.ToString("dd-MM-yyyy"),
                horainicio = evaluacion.StartHour,
                horafin = evaluacion.EndHour

            });

            #endregion
            return Json(jsonResult);
        }
        public JsonResult ObtenerSelectListCursos(int idLapso)
        {
            #region Declaración de variables
            List<Course> listaCursos = new List<Course>();
            List<object> jsonResult = new List<object>();
            CourseService _courseService = new CourseService();
            #endregion
            #region Obtener lista de cursos
            listaCursos = _courseService.ObtenerListaCursosPorLapso(idLapso).ToList<Course>();
            #endregion

            foreach (Course curso in listaCursos)
            {
                jsonResult.Add(new
                {
                    nombre = curso.Name,
                    idCurso = curso.CourseId,
                });
            }
            return Json(jsonResult);
        }
        public JsonResult ObtenerSelectListLapsos(int idColegio)
        {
            #region Declaración de variables

            SchoolYear anoEscolarActivo = new SchoolYear();
            List<Period> listaLapsos = new List<Period>();
            List<object> jsonResult = new List<object>();
            SchoolYearService _schoolYearService = new SchoolYearService();
            #endregion
            #region Obtener ano escolar y lista de periodos
            anoEscolarActivo = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(idColegio);
            listaLapsos = anoEscolarActivo.Periods;
            #endregion

            foreach (Period lapso in listaLapsos)
            {

                jsonResult.Add(new
                {
                    nombre = lapso.Name,
                    idLapso = lapso.PeriodId,

                });
            }
            return Json(jsonResult);
        }
        public JsonResult ObtenerSelectListEvaluacionesDeCASUS(int idMateria, int idLapso, int idCurso)
        {
            ObteniendoSesion();

            #region Declaración de variables
            List<object> jsonResult = new List<object>();

            List<Assessment> listaEvaluaciones = new List<Assessment>();
            AssessmentService assessmentService = new AssessmentService();
            #endregion

            listaEvaluaciones = assessmentService
                .ObtenerListaEvaluacionesPor_Curso_Materia_Docente_Lapso(idMateria, idCurso, _session.USERID,
                idLapso);

            foreach (Assessment evaluacion in listaEvaluaciones)
            {

                jsonResult.Add(new
                {
                    nombre = evaluacion.Name,
                    porcentaje = evaluacion.Percentage,
                    idEvaluacion = evaluacion.AssessmentId
                });
            }

            return Json(jsonResult);
        }
        public JsonResult ObtenerSelectListEvaluacionesDeCASUSEstadisticas(int idMateria, int idLapso,
            int idCurso)
        {
            #region Declaración de variables
            List<Assessment> listaEvaluaciones = new List<Assessment>();
            List<object> jsonResult = new List<object>();
            List<Score> listaNotas = new List<Score>();
            List<Student> listaEstudiantes = new List<Student>();
            Subject materia = new Subject();
            string tipoCurso = "";
            bool aprobado = false;

            UnitOfWork _unidad = new UnitOfWork();
            AssessmentService assessmentService = new AssessmentService(_unidad);
            CourseService _courseService = new CourseService(_unidad);
            SubjectService subjectService = new SubjectService(_unidad);
            StudentService studentService = new StudentService(_unidad);
            #endregion

            #region Obteniendo el curso & grado
            Course curso = _courseService.ObtenerCursoPor_Id(idCurso);
            int grado = curso.Grade;
            #endregion
            #region Obteniendo la materia
            materia = subjectService.ObtenerMateriaPorId(idMateria);
            #endregion
            #region Obteniendo la lista de estudiantes
            listaEstudiantes = studentService.ObtenerListaEstudiantesConNotasPor_Materia_Lapso(idCurso,
                idMateria, idLapso);
            #endregion
            #region Creando objeto JSON
            foreach (Student estudiante in listaEstudiantes)
            {
                double calculoAux = 0;
                string notaLetra = "";

                #region Cálculo para primaria
                if (grado == 1 || grado == 2 || grado == 3 || grado == 4 || grado == 5 || grado == 6)
                {
                    tipoCurso = "Primaria";

                    #region Obteniendo el número de evaluaciones

                    int nroEvaluaciones = assessmentService
                            .ObtenerListaEvaluacionesPor_Curso_Materia_Lapso(idCurso, idMateria, idLapso).Count;

                    int notaMaxima = nroEvaluaciones * 5;
                    int notaMaximaB = nroEvaluaciones * 4;
                    int notaMaximaC = nroEvaluaciones * 3;
                    int notaMaximaD = nroEvaluaciones * 2;
                    int notaMaximaE = nroEvaluaciones * 1;
                    #endregion
                    #region Calculando rendimiento de notas del alumno
                    foreach (Score nota in estudiante.Scores)
                    {
                        if (nota.LetterScore.Equals("A") || nota.LetterScore.Equals("a")) calculoAux += 5;
                        else if (nota.LetterScore.Equals("B") || nota.LetterScore.Equals("b")) calculoAux += 4;
                        else if (nota.LetterScore.Equals("C") || nota.LetterScore.Equals("c")) calculoAux += 3;
                        else if (nota.LetterScore.Equals("D") || nota.LetterScore.Equals("d")) calculoAux += 2;
                        else if (nota.LetterScore.Equals("E") || nota.LetterScore.Equals("e")) calculoAux += 1;
                    }
                    #endregion

                    aprobado = (calculoAux >= notaMaxima / 2 ? true : false);

                    if (calculoAux >= 0 && calculoAux <= notaMaximaE)
                    {
                        notaLetra = "E";
                    }
                    else if (calculoAux > notaMaximaE && calculoAux <= notaMaximaD)
                    {
                        notaLetra = "D";
                    }
                    else if (calculoAux > notaMaximaD && calculoAux <= notaMaximaC)
                    {
                        notaLetra = "C";
                    }
                    else if (calculoAux > notaMaximaC && calculoAux <= notaMaximaB)
                    {
                        notaLetra = "B";
                    }
                    else if (calculoAux > notaMaximaB && calculoAux <= notaMaxima)
                    {
                        notaLetra = "A";
                    }
                }
                #endregion
                #region Cálculo para bachillerato
                else
                {
                    tipoCurso = "Bachillerato";

                    #region Calculando rendimiento de notas del alumno
                    foreach (Score nota in estudiante.Scores)
                    {
                        double calculoPrevio = 0;
                        Assessment evaluacion = assessmentService.ObtenerEvaluacionPor_Id(nota.AssessmentId);
                        calculoPrevio = (double)evaluacion.Percentage / (double)100;
                        calculoAux += calculoPrevio * nota.NumberScore;
                    }
                    #endregion

                    aprobado = (calculoAux >= 10 ? true : false);
                }
                #endregion
                #region Objeto JSON
                jsonResult.Add(new
                {
                    success = true,
                    estudiante = estudiante.FirstLastName + " " + estudiante.SecondLastName + ", " +
                                 estudiante.FirstName,
                    estudianteId = estudiante.StudentId,
                    calculo = calculoAux,
                    materia = materia.Name,
                    materiaId = materia.SubjectId,
                    aprobado = aprobado,
                    grado = tipoCurso,
                    letranota = notaLetra
                });
                #endregion
            }
            #endregion

            return Json(jsonResult);
        }
        public JsonResult ObtenerSelectListLapsosProfesor(int idCurso)
        {
            #region Declaración de variables
            int idColegio = (int)Session["SchoolId"];
            SchoolYear anoEscolarActivo = new SchoolYear();
            List<Period> listaLapsos = new List<Period>();
            List<object> jsonResult = new List<object>();
            SchoolYearService _schoolYearService = new SchoolYearService();
            #endregion
            #region Obtener ano escolar y lista de periodos
            anoEscolarActivo = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(idColegio);
            listaLapsos = anoEscolarActivo.Periods;
            #endregion

            foreach (Period lapso in listaLapsos)
            {

                jsonResult.Add(new
                {
                    nombre = lapso.Name,
                    idLapso = lapso.PeriodId,

                });
            }
            return Json(jsonResult);
        }
        public List<float> ObtenerPorcentajesPeriodos(DateTime fechaactual, SchoolYear anoescolar, 
            List<Period> listadeperiodos)
        {
            List<float> listaporcentajesperiodos = new List<float>();

            TimeSpan diaslapso1 = listadeperiodos[0].FinishDate.Date.Subtract(listadeperiodos[0].StartDate.Date);
            TimeSpan diaslapso2 = listadeperiodos[1].FinishDate.Date.Subtract(listadeperiodos[1].StartDate.Date);
            TimeSpan diaslapso3 = listadeperiodos[2].FinishDate.Date.Subtract(listadeperiodos[2].StartDate.Date);
            TimeSpan diasanoescolar = anoescolar.EndDate.Date.Subtract(anoescolar.StartDate.Date);

            if (fechaactual.Date >= listadeperiodos[0].StartDate.Date && 
                fechaactual.Date <= listadeperiodos[0].FinishDate.Date)
            {
                TimeSpan result = fechaactual.Date.Subtract(listadeperiodos[0].StartDate.Date);
                float porcentajelapso1 = (Convert.ToInt32(result.Days) * 100) / diaslapso1.Days;
                TimeSpan result2 = fechaactual.Date.Subtract(anoescolar.StartDate.Date);
                float porcentajeanoescolar = (Convert.ToInt32(result2.Days) * 100) / diasanoescolar.Days;
                listaporcentajesperiodos.Add(porcentajelapso1);
                listaporcentajesperiodos.Add(0);
                listaporcentajesperiodos.Add(0);
                listaporcentajesperiodos.Add(porcentajeanoescolar);
            }
            else
            {
                if (fechaactual.Date >= listadeperiodos[1].StartDate.Date && 
                    fechaactual.Date <= listadeperiodos[1].FinishDate.Date)
                {
                    TimeSpan result = fechaactual.Date.Subtract(listadeperiodos[1].StartDate.Date);
                    float porcentajelapso2 = (Convert.ToInt32(result.Days) * 100) / diaslapso2.Days;
                    TimeSpan result2 = fechaactual.Date.Subtract(anoescolar.StartDate.Date);
                    float porcentajeanoescolar = (Convert.ToInt32(result2.Days) * 100) / diasanoescolar.Days;
                    listaporcentajesperiodos.Add(100);
                    listaporcentajesperiodos.Add(porcentajelapso2);
                    listaporcentajesperiodos.Add(0);
                    listaporcentajesperiodos.Add(porcentajeanoescolar);
                }
                else
                {
                    if (fechaactual.Date >= listadeperiodos[2].StartDate.Date && 
                        fechaactual.Date <= listadeperiodos[2].FinishDate.Date)
                    {
                        TimeSpan result = fechaactual.Date.Subtract(listadeperiodos[2].StartDate.Date);
                        float porcentajelapso3 = (Convert.ToInt32(result.Days) * 100) / diaslapso3.Days;
                        TimeSpan result2 = fechaactual.Date.Subtract(anoescolar.StartDate.Date);
                        float porcentajeanoescolar = (Convert.ToInt32(result2.Days) * 100) / diasanoescolar.Days;
                        listaporcentajesperiodos.Add(100);
                        listaporcentajesperiodos.Add(100);
                        listaporcentajesperiodos.Add(porcentajelapso3);
                        listaporcentajesperiodos.Add(porcentajeanoescolar);
                    }
                }
            }
            return listaporcentajesperiodos;
        }

        public bool ActualizacionSesionColegio(int id)
        {
            SchoolYearService service = new SchoolYearService();
            SchoolYear schoolYear = service.ObtenerAnoEscolarActivoPorColegio(id); 

            Session["SchoolId"] = id;
            Session["SchoolYearId"] = schoolYear.SchoolYearId;
            Session["StartDate"] = schoolYear.StartDate;
            Session["DateOfCompletion"] = schoolYear.EndDate;

            return true;
        }
        public bool CerrarSesion()
        {
            Session.Clear();
            Session.Abandon();
            System.Web.Security.FormsAuthentication.SignOut();

            return true;
        }

        public void AddErrors(IdentityResult result)
        {
            string _errorAux = "";

            foreach (var error in result.Errors)
            {
                #region Transcribiendo los mensajes de errores
                if (error.StartsWith("Name"))
                {
                    _errorAux = "Ya existe ese nombre de usuario, por favor seleccionar otro.";
                    ModelState.AddModelError("", _errorAux);
                }
                else if (error.StartsWith("Email"))
                {
                    _errorAux = "Ya existe este correo, por favor insertar otro.";
                    ModelState.AddModelError("", _errorAux);
                }
                else if (error.StartsWith("Passwords must have"))
                {
                    _errorAux = "La contraseña debe tener al menos una letra minúscula ('a'-'z'), " +
                                "una mayúscula ('A'-'Z') y un caracter que no sea una letra.";
                    ModelState.AddModelError("", _errorAux);
                }
                #endregion

                else
                    ModelState.AddModelError("", error);
            }
        }        
    }
}