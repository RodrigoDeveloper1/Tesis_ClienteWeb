using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    public class RepresentantesController : MaestraController
    {
        private string _controlador = "Representantes";
        private BridgeController _puente = new BridgeController();

        [HttpGet]
        public ActionResult ListadoRepresentantes()
        {
            ConfiguracionInicial(_controlador, "ListadoRepresentantesDocentes");

            #region Inicializando variables
            GestionRepresentantesModel model = new GestionRepresentantesModel();
            List<Course> listaCursos;
            RepresentativeService representativeService = new RepresentativeService();
            CourseService courseService = new CourseService();
            #endregion
            
            listaCursos = courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;

            model.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");

            return View(model);
        }




        //Por revisar - Rodrigo Uzcátegui (06-07-15)

        #region Pantalla Modificar Representantes
        [HttpGet]
        public ActionResult ModificarRepresentantes(int id)
        {
            RepresentativeService representantesService = new RepresentativeService();
            RepresentantesModel model = new RepresentantesModel();
            Representative representante = new Representative();
            representante = representantesService.ObtenerRepresentantePorId(id);
            model.Representante = representante;
            return View(model);
        }

        #endregion
        #region Gestión de Representantes

        [HttpPost]
        public void CrearRepresentante(string name)
        {
            RepresentativeService representantesService = new RepresentativeService();
            Representative representante = new Representative()
            {
                //Name = name
               
            };
            representantesService.GuardarRepresentante(representante);
        }

        [HttpGet]
        public ActionResult EliminarRepresentante(int id)
        {
            RepresentativeService representantesService = new RepresentativeService();
            representantesService.EliminarRepresentante(id);
            return RedirectToAction("Representantes", "Representantes");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarRepresentantes(RepresentantesModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            RepresentativeService representantesService = new RepresentativeService();
            Representative representanteNuevo = new Representative()
            {
                RepresentativeId = model.Representante.RepresentativeId,
                //Name = model.Representante.Name

            };

            representantesService.ModificarRepresentante(representanteNuevo, representanteNuevo.RepresentativeId);
            return RedirectToAction("Representantes", "Representantes");
        }

        #endregion

        #region Otros Metodos

        [HttpPost]
        public JsonResult ObtenerTablaRepresentantesPorIdAlumno(int idEstudiante)
        {
            #region Declaración de variables
            List<Representative> listaRepresentantes = new List<Representative>();
            Student Alumno = new Student();
            List<object> jsonResult = new List<object>();
            StudentService studentService = new StudentService();
            RepresentativeService representativeService = new RepresentativeService();
            #endregion
            #region Obteniendo lista de estudiantes y curso
            listaRepresentantes = 
                representativeService.ObtenerListaRepresentantesPorAlumno(idEstudiante)
                    .OrderBy(m => m.LastName)
                    .ThenBy(m => m.SecondLastName)
                    .ToList();
            #endregion
            #region Transformando lista de representantes a JsonResult
            foreach (Representative representante in listaRepresentantes)
            {

                jsonResult.Add(new
                {
                    apellido1 = representante.LastName,
                    apellido2 = representante.SecondLastName,
                    nombre1 = representante.Name,
                    idEstudiante = representante.RepresentativeId
                });
            }

            #endregion
            return Json(jsonResult);
        }

        #endregion
    }
}