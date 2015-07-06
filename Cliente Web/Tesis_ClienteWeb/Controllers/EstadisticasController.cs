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
    public class EstadisticasController : MaestraController
    {
        private string _controlador = "Estadisticas";
        private BridgeController _puente = new BridgeController();

        public ActionResult EstadisticasEvaluaciones(EstadisticasModel model)
        {
            ConfiguracionInicial(_controlador, "EstadisticasEvaluaciones");
            
            #region Declaración de variables
            CourseService courseService = new CourseService();
            SubjectService subjectService = new SubjectService();
            SchoolYearService schoolYearService = new SchoolYearService();
            List<Course> listaCursos;
            List<Subject> listaMaterias;
            #endregion

            listaCursos = courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            model.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");

            listaMaterias = new List<Subject>();
            model.selectListMaterias = new SelectList(listaMaterias, "MateriaId", "Name");

            List<Period> listaLapsos = new List<Period>();
            model.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");

            List<Assessment> listaEvaluaciones = new List<Assessment>();
            model.selectListEvaluaciones = new SelectList(listaEvaluaciones, "AssessmentId", "Name");

            return View(model);
        }
        
        public ActionResult EstadisticasMaterias(EstadisticasModel model)
        {
            ConfiguracionInicial(_controlador, "EstadisticasMaterias");
            #region Declaración de variables
            CourseService courseService = new CourseService();
            List<Course> listaCursos;
            List<Subject> listaMaterias;
            #endregion

            listaCursos = courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            model.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");

            listaMaterias = new List<Subject>();
            model.selectListMaterias = new SelectList(listaMaterias, "MateriaId", "Name");

            List<Period> listaLapsos = new List<Period>();
            model.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");

           return View(model);
        }

        public ActionResult EstadisticasCursos(EstadisticasModel model)
        {
            ConfiguracionInicial(_controlador, "EstadisticasCursos");

            #region Declaración de variables
            CourseService courseService = new CourseService();
            List<Course> listaCursos;
            #endregion

            listaCursos = courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            model.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");           

            List<Period> listaLapsos = new List<Period>();
            model.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");

            return View(model);
        }
    }
}
