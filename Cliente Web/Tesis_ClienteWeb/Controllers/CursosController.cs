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
    public class CursosController : MaestraController
    {
        private string _controlador = "Cursos";
        private BridgeController _puente = new BridgeController();

        [HttpGet]
        public ActionResult CrearCurso()
        {
            ConfiguracionInicial(_controlador, "CrearCurso");
            CursosModel modelo = new CursosModel();
            #region Mensajes TempData
            if (TempData["NuevoCurso"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevoCurso"].ToString();
            }
            if (TempData["NuevoCursoError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevoCursoError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ColegioNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ColegioNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["CursoNoAgregado"] != null)
            {
                ModelState.AddModelError("", TempData["CursoNoAgregado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["MateriasNoAgregadas"] != null)
            {
                ModelState.AddModelError("", TempData["MateriasNoAgregadas"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion
            return View(modelo);
        }

        [HttpPost]
        public bool CrearCurso(List<Object> values, List<Object> materias)
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
            #region Validación de curso añadidos
            if (values.Count == 1)
            {
                TempData["CursoNoAgregado"] = "Por favor inserte el curso.";
                return false;
            }
            #endregion
            #region Validación de materias añadidas
            if (materias == null || materias.Count == 0)
            {
                TempData["MateriasNoAgregadas"] = "Por favor inserte al menos una materia.";
                return false;
            }
            #endregion
            #region Inicializando servicios
            UnitOfWork _unidad = new UnitOfWork();
            CourseService _courseService = new CourseService(_unidad);
            PeriodService _periodService = new PeriodService(_unidad);
            SchoolYearService _schoolYearService = new SchoolYearService(_unidad);
            CASUService _casuService = new CASUService(_unidad);
            //   _userService = new UserService(_unidad);
            #endregion
            #region Obteniendo año escolar
            SchoolYear añoEscolar = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(schoolId);
            List<Period> periodos = añoEscolar.Periods;
            #endregion
            #region Ciclo de asignación
            try
            {
                #region Creación de nuevo curso
                Course curso = new Course();
                curso.Name = values[1].ToString();
                curso.Grade = Convert.ToInt32(values[2].ToString());
                curso.Section = values[3].ToString();
                #endregion
                _courseService.GuardarCourse(curso);

                for (int i = 0; i <= materias.Count - 1; i++)
                {
                    string idMateria = materias[i].ToString();


                    #region Agregando casus a la bd

                    try
                    {
                        CASU casu1 = new CASU();
                        casu1.CourseId = curso.CourseId;
                        casu1.PeriodId = periodos[0].PeriodId;
                        casu1.SubjectId = Convert.ToInt32(idMateria);
                        _casuService.GuardarCASU(casu1);

                        CASU casu2 = new CASU();
                        casu2.CourseId = curso.CourseId;
                        casu2.PeriodId = periodos[1].PeriodId;
                        casu2.SubjectId = Convert.ToInt32(idMateria);
                        _casuService.GuardarCASU(casu2);

                        CASU casu3 = new CASU();
                        casu3.CourseId = curso.CourseId;
                        casu3.PeriodId = periodos[2].PeriodId;
                        casu3.SubjectId = Convert.ToInt32(idMateria);
                        _casuService.GuardarCASU(casu3);

                    }
                    catch (Exception e)
                    {
                        TempData["NuevoCursoError"] = e.Message;
                        return false;
                    }



                    #endregion
                }
            #endregion
                TempData["NuevoCurso"] = "Se agregó correctamente el curso";
                return true;
            }
            catch (Exception e)
            {
                TempData["NuevoCursoError"] = e.Message;
                return false;
            }
        }

        [HttpGet]
        public ActionResult GestionCursos()
        {
            ConfiguracionInicial(_controlador, "GestionCursos");
            CourseService courseController = new CourseService();
            CursosModel modelo = new CursosModel();
            #region Mensajes TempData
            if (TempData["NuevoCurso"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevoCurso"].ToString();
            }
            if (TempData["NuevoCursoError"] != null)
            {
                ModelState.AddModelError("", TempData["NuevoCursoError"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["ColegioNoSeleccionado"] != null)
            {
                ModelState.AddModelError("", TempData["ColegioNoSeleccionado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["CursoNoAgregado"] != null)
            {
                ModelState.AddModelError("", TempData["CursoNoAgregado"].ToString());
                modelo.MostrarErrores = "block";
            }
            if (TempData["MateriasNoAgregadas"] != null)
            {
                ModelState.AddModelError("", TempData["MateriasNoAgregadas"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion
            return View(modelo);
        }

        [HttpGet]
        public bool EliminarCurso(int id)
        {
            ConfiguracionInicial(_controlador, "EliminarCurso");
            CourseService _courseService = new CourseService();
            try
            {
                _courseService.EliminarCourse(id);
                TempData["NuevoCurso"] = "Se ha eliminado correctamente el curso.";
            }
            catch (Exception e)
            {
                TempData["NuevoCursoError"] = e.Message;
            }
            return true;
        }
        
        
        
        
        
        
        
        
        
        
        //Por revisar - Rodrigo Uzcátegui 12-05-15
        #region Pantalla Cursos
        public ActionResult ListaCursos(CursosModel modelo)
        {
            ObteniendoSesion();


            #region Inicializando variables
            CursosModel model = new CursosModel();
            List<Course> listaCursos;
            CourseService _courseService = new CourseService();
            #endregion
            #region Inicializando SelectList de Cursos
            string idsession = (string)Session["UserId"];
            int idcolegio = (int)Session["SchoolId"];
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            SchoolYearService _schoolYearService = new SchoolYearService();
            model.listaCursos = listaCursos;
            model.SchoolYear = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(idcolegio);
            #endregion

            return View(model);
        }
        #endregion
        #region Pantalla Modificar curso

        [HttpGet]
        public ActionResult ModificarCurso(int id)
        {
            CourseService courseController = new CourseService();
            CursosModel model = new CursosModel();              
            Course curso = new Course();
            curso = courseController.ObtenerCursoPor_Id(id);
            model.Course = curso;              
            return View(model);     
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarCurso(CursosModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            CourseService courseController = new CourseService();
            Course cursoNuevo = new Course()
            {
                CourseId = model.Course.CourseId,
                Name = model.Course.Name,
                //StartDate = Convert.ToDateTime(model.Course.StartDate),
                //EndDate = Convert.ToDateTime(model.Course.EndDate),
                //Status = true
            };
            courseController.ModificarCourse(cursoNuevo);
            return RedirectToAction("Inicio", "Index");
        }

        #endregion
        #region pantalla gestion cursos
        
        #endregion
        #region Gestión de Cursos
        public int DevuelveCursoPorId(string name)
        {
            ObteniendoSesion();

            CourseService courseController = new CourseService();
            Course cursoparaid = courseController.ObtenerCursoPor_Nombre_AnoEscolar_Usuario(name, 
                _session.SCHOOLYEARID, _session.USERID);
            return cursoparaid.CourseId; 
        }

       
        
#endregion
        #region Gestión de Alumnos Dentro de Curso

        [HttpPost]
        public ActionResult CargarAlumnosCurso(string name, string firstlastname, string secondlastname,
                                               string firstname, string secondname)
        
        {
            var idcursoforanea = DevuelveCursoPorId(name);
            StudentService studentController = new StudentService();
            Student alumnoNuevo = new Student()
            {
                //ListNumber = idcursoforanea,
                FirstLastName = firstlastname,
                SecondLastName = secondlastname,
                FirstName = firstname,
                SecondName = secondname,                
            };

            studentController.GuardarStudent(alumnoNuevo);
            return RedirectToAction("Cursos", "ListaCursos");

        }

        #endregion

        #region Otros métodos
        public JsonResult ObtenerSelectListCursosActivos(int idColegio)
        {
            #region Declaración de variables
            List<string> listaCursos = new List<string>();
            CourseService _courseService = new CourseService();
            #endregion
            #region Obtener lista de cursos
            //listaCursos = _courseService.ObtenerListaNombresCursosActivosPorColegio(idColegio).ToList<string>();
            #endregion

            return Json(listaCursos);
        }

        [HttpPost]
        public JsonResult ObtenerSelectListCursosPorColegio(int idColegio)
        {
            #region Declaración de variables
            CourseService _courseService = new CourseService();
            SchoolYearService _schoolYearService = new SchoolYearService();

            List<object> jsonResult = new List<object>();
            List<Course> listaCursos = new List<Course>();

            SchoolYear anoEscolarActivo = new SchoolYear();
            #endregion
            #region Obtener lista de cursos
            anoEscolarActivo = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(idColegio);
            listaCursos = _courseService.ObtenerListaCursosPorColegio(idColegio).ToList<Course>();
            #endregion
            #region Validación lista de cursos vacía
            if (listaCursos.Count == 0)
            {
                jsonResult.Add(new
                {
                    success = false
                });
            }
            #endregion
            #region Construyendo objeto JSON
            else
            {
                foreach (Course curso in listaCursos)
                {
                    jsonResult.Add(new
                    {
                        success = true,
                        nombre = curso.Name,
                        anoescolar = anoEscolarActivo.StartDate.ToShortDateString() + " - " +
                        anoEscolarActivo.EndDate.ToShortDateString(),
                        grado = curso.Grade,
                        seccion = curso.Section,
                        idCurso = curso.CourseId,
                    });
                }
            }
            #endregion
            
            return Json(jsonResult);
        }

        #endregion
    }
}
