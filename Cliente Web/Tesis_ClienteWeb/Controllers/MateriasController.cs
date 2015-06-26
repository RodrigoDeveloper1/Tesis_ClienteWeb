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
    public class MateriasController : MaestraController
    {
        private string _controlador = "Materias";
        private BridgeController _puente = new BridgeController();

        [HttpGet]
        public ActionResult CrearMateria()
        {
            ConfiguracionInicial(_controlador, "CrearMateria");
            MateriasModel modelo = new MateriasModel();
            #region Mensajes TempData
            if (TempData["NuevaMateria"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["NuevaMateria"].ToString();
            }
            if (TempData["MateriaNuevaError"] != null)
            {
                ModelState.AddModelError("", TempData["MateriaNuevaError"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion

            List<Course> listaCursos = new List<Course>();
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            List<User> listaProfesores = new List<User>();
            modelo.selectListProfesores = new SelectList(listaProfesores, "UserId", "Name");

            return View(modelo);
        }

        [HttpPost]
        public ActionResult CrearMateria(MateriasModel modelo)
        {
            ConfiguracionInicial(_controlador, "CrearMateria_POST");
            #region Obteniendo ids
            int colegioId = modelo.idColegio;
            #endregion
            #region Inicializando servicios
            UnitOfWork _unidad = new UnitOfWork();
            SubjectService _subjectService = new SubjectService(_unidad);
            SchoolService _schoolService = new SchoolService(_unidad);
            #endregion
            #region obteniendo colegio y grabando materia
            #endregion
            #region Guardando la materia
            try
            {
                Subject materia = new Subject();
                materia.Name = modelo.Name;
                materia.SubjectCode = modelo.SubjectCode;
                materia.GeneralPurpose = modelo.Pensum;
                materia.Grade = modelo.Grade;

                School colegio = new School();
                colegio = _schoolService.ObtenerColegioPorId(colegioId);
                colegio.Subjects.Add(materia);
                _schoolService.ModificarColegio(colegio);
                #region TempData de guardado exitoso
                TempData["NuevaMateria"] = "Se agregó correctamente la materia '" + materia.Name + "'";
                #endregion
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["MateriaNuevaError"] = e.Message;
            }
            #endregion

            return RedirectToAction("CrearMateria");
        }

        [HttpGet]
        public bool EliminarMateria(int id)
        {
            ConfiguracionInicial(_controlador, "EliminarMateria");

            try
            {
                SubjectService materiasController = new SubjectService();
                materiasController.EliminarSchoolSubject(id);
                TempData["NuevaMateria"] = "Se ha eliminado correctamente la materia.";
                return true;
            }
            catch (Exception e)
            {
                TempData["MateriaNuevaError"] = e.Message;
                throw e;
            }
        }

        [HttpGet]
        public ActionResult ModificarMateria()
        {
            ConfiguracionInicial(_controlador, "ModificarMateria");
            MateriasModel modelo = new MateriasModel();

            #region Mensajes TempData
            if (TempData["ModificadaMateria"] != null)
            {
                modelo.MostrarAclamaciones = "block";
                modelo.MensajeAclamacion = TempData["ModificadaMateria"].ToString();
            }
            if (TempData["MateriaError"] != null)
            {
                ModelState.AddModelError("", TempData["MateriaError"].ToString());
                modelo.MostrarErrores = "block";
            }
            #endregion

            List<Course> listaCursos = new List<Course>();
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            List<Period> listaLapsos = new List<Period>();
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            List<User> listaProfesores = new List<User>();
            modelo.selectListProfesores = new SelectList(listaProfesores, "UserId", "Name");

            return View(modelo);
        }

        [HttpPost]
        public bool ModificarMateria(string nombre, string codigo, string pensum, int idMateria,string idCurso, 
            string idProfesor, string idColegio)
        {
            //Si entra en [HttpGet] ModificarMateria(), no necesita de ConfiguracionInicial();
            #region Inicializando servicios
            UnitOfWork _unidad = new UnitOfWork();
            SubjectService _subjectService = new SubjectService(_unidad);
            #endregion
            #region Definiendo nueva materia
            try
            {
                Subject materia = _subjectService.ObtenerMateriaPorId(idMateria);
                materia.Name = nombre;
                materia.SubjectCode = codigo;
                materia.GeneralPurpose = pensum;

                _subjectService.ModificarSchoolSubject(materia);
                TempData["ModificadaMateria"] = "Se modificó correctamente la materia '" + materia.Name + "'";
            }
            catch (Exception e)
            {
                TempData["MateriaError"] = e.Message;
                throw e;
            }
            #endregion

            return true;
        }









        //Por revisar - Rodrigo Uzcátegui - 12/05/15
        public ActionResult Materias()
        {
            ObteniendoSesion();
            #region Inicializando variables
            MateriasModel model = new MateriasModel();
            List<Course> listaCursos;
            SubjectService _subjectService = new SubjectService();
            CourseService _courseService = new CourseService();
            #endregion
            #region Inicializando SelectList de Cursos
            string idsession = (string)Session["UserId"];
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            model.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
           // model.listaMaterias = _subjectService.ObtenerListaMaterias().ToList();
            #endregion

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarMaterias(MateriasModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SubjectService materiasController = new SubjectService();
            Subject materiaNueva = new Subject()
            {
                //SchoolSubjectdId = model.Materia.SchoolSubjectdId,
                //Name = model.Materia.Name
            };
            //materiasController.ModificarSchoolSubject(materiaNueva, materiaNueva.SchoolSubjectdId);
            return RedirectToAction("Materias", "Materias");
        }
    }
}
