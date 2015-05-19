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
        private CourseService _courseService;
        private SubjectService _subjectService;
        private SchoolYearService _schoolYearService;

        #region Pantalla Estadísticas
        public ActionResult EstadisticasEvaluaciones(EstadisticasModel model)
        {
            ObteniendoSesion();
            #region inicializando variable
            _courseService = (_courseService == null ? new CourseService() : _courseService);
            _subjectService = (_subjectService == null ? new SubjectService() : _subjectService);
            _schoolYearService = new SchoolYearService();
            List<Course> listaCursos;
            List<Subject> listaMaterias;
            #endregion
            string idsession = (string)Session["UserId"];
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
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
            ObteniendoSesion();
            #region inicializando variable
            _courseService = (_courseService == null ? new CourseService() : _courseService);
            _subjectService = (_subjectService == null ? new SubjectService() : _subjectService);
            _schoolYearService = new SchoolYearService();
            List<Course> listaCursos;
            List<Subject> listaMaterias;
            #endregion
            string idsession = (string)Session["UserId"];
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
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
            ObteniendoSesion();
            #region inicializando variable
            _courseService = (_courseService == null ? new CourseService() : _courseService);
            _schoolYearService = new SchoolYearService();
            List<Course> listaCursos;
            #endregion
            string idsession = (string)Session["UserId"];
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            model.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");           
            List<Period> listaLapsos = new List<Period>();
            model.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            return View(model);
        }



        #endregion

        #region prueba pdf

        #endregion
    }
}
