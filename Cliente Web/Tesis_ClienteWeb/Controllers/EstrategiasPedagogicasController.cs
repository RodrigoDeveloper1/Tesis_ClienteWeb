using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    public class EstrategiasPedagogicasController : MaestraController
    {
        private string _controlador = "EstrategiasPedagogicas";
        private BridgeController _puente = new BridgeController();

        [HttpGet]
        public ActionResult EstrategiasPedagogicas()
        {
            ConfiguracionInicial(_controlador, "EstrategiasPedagogicas");

            #region Declaración de variables
            EstrategiasPedagogicasModel modelo = new EstrategiasPedagogicasModel();
            CourseService courseService = new CourseService();
            List<Course> listaCursos = new List<Course>();
            List<Subject> listaMaterias = new List<Subject>();
            List<Period> listaLapsos = new List<Period>();
            #endregion
            #region Obteniendo la lista de cursos
            List<Course> listaAux = 
                courseService.ObtenerListaCursosPor_Docente(_session.USERID, _session.SCHOOLYEARID);

            foreach (Course course in listaAux)
            {
                if (course.Grade <= 6)
                    listaCursos.Add(course);
            }
            #endregion
            #region Inicializando las listas de la vista
            modelo.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            modelo.selectListMaterias = new SelectList(listaMaterias, "MateriaId", "Name");
            modelo.selectListLapsos = new SelectList(listaLapsos, "PeriodId", "Name");
            #endregion

            return View(modelo);
        }

        [HttpPost]
        public JsonResult EstrategiasPedagogicas_Reporte(int idCurso, int idLapso, int idMateria)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            CourseService courseService = new CourseService();
            SubjectService subjectService = new SubjectService();
            #endregion
            #region Obteniendo datos del curso
            Course course = courseService.ObtenerCursoPor_Id(idCurso);
            int grade = course.Grade;
            #endregion
            #region Obteniendo datos de la materia
            Subject subject = subjectService.ObtenerMateriaPorId(idMateria);
            string subjectName = subject.Name;
            #endregion
            #region Definiendo el path
            string fileName = "";
            if (subjectName.Equals("Castellano")) 
                fileName = ConstantRepository.ReportsSE_Castellano;
            else if (subjectName.Equals("Matemática")) 
                fileName = ConstantRepository.ReportsSE_Matematica;
            else if (subjectName.Equals("Ciencias de la naturaleza") || subjectName.Equals("Ciencias")) 
                fileName = ConstantRepository.ReportsSE_Ciencias_Naturaleza;
            else if (subjectName.Equals("Ciencias Sociales") || subjectName.Equals("Sociales")) 
                fileName = ConstantRepository.ReportsSE_Ciencias_Sociales;
            else if (subjectName.Equals("Educación Estética") || subjectName.Equals("Estética")) 
                fileName = ConstantRepository.ReportsSE_Estetica;
            else if (subjectName.Equals("Educación Estética")) 
                fileName = ConstantRepository.ReportsSE_Estetica;

            string path =
                Path.Combine(Server.MapPath(ConstantRepository.ReportsSE_Path), grade.ToString(), fileName);
            #endregion
            #region Definiendo el resultado
            if (fileName.Equals(""))
                jsonResult.Add(new { Success = false });
            else
                jsonResult.Add(new { Success = true, Path = path });
            #endregion

            return Json(jsonResult);
        }
    }
}