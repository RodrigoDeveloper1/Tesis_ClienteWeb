using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class SubjectService
    {
        #region Configuración inicial
        private SessionVariablesRepository _session;
        private UnitOfWork _unidad;

        public SubjectService()
        {
            this._unidad = new UnitOfWork();
            _InicializadorVariablesSesion();
        }

        public SubjectService(UnitOfWork unidad)
        {
            this._unidad = unidad;
            _InicializadorVariablesSesion();
        }

        /// <summary>
        /// Método interno que inicializa todas las variables de la sesión activa
        /// </summary>
        private void _InicializadorVariablesSesion()
        {
            _session = new SessionVariablesRepository();
        }
        #endregion

        #region CRUD
        public bool GuardarMateria(Subject materia)
        {
            try
            {
                _unidad.RepositorioSubject.Add(materia);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                //return false; Se editó para arreglar warnings del proyecto - Rodrigo Uzcátegui - 03-02-15
                throw e;
            }
        }
        /// <summary>
        /// Método CRUD - Modificar materia
        /// </summary>
        /// <param name="schoolsubject">La materia a modificar</param>
        /// <returns>True = Materia modificada correctamente</returns>
        public bool ModificarSchoolSubject(Subject schoolsubject)
        {
            try
            {
                _unidad.RepositorioSubject.Modify(schoolsubject);
                _unidad.Save();                                
            }
            catch (DbEntityValidationException e)
            {
                //Excepción encapsulada por: Rodrigo Uzcátegui - 12-05-15
                //Entra en la excepción por valores required vacíos o nulos para materia.
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
        public bool EliminarSchoolSubject(int id)
        {
            try
            {
                _unidad.RepositorioSubject.Delete(u => u.SubjectId == id);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #region Obtener materias
        public Subject ObtenerMateriaPorId(int id)
        {
            Subject materia = (from Subject s in _unidad.RepositorioSubject._dbset
                                 .Include("CASUs")
                            where s.SubjectId == id
                            select s).FirstOrDefault<Subject>();
            return materia;
        }
        public List<Subject> ObtenerListaMateriasPorLapsoYCurso(int idLapso, int idCurso)
        {
            HashSet<Subject> listaMaterias = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Subject")
                    .Include("Period")
                    .Include("Course")
                where casu.Period.PeriodId == idLapso && 
                      casu.Course.CourseId == idCurso
                select casu.Subject)
                    .OrderBy(m => m.Name)
                    .ToHashSet<Subject>();

            return listaMaterias.ToList<Subject>();
        }

        

        public Subject GetSchoolSubject(int id)
        {

            Subject materiaseleccionada = _unidad.RepositorioSubject.GetById(id);
            return materiaseleccionada;

        }

        /// <summary>
        /// Método que obtiene todas las listas de las materias en bd.
        /// </summary>
        /// <returns>La lista de materias del proyecto.</returns>
        public List<string> ObtenerListaNombreDeMaterias()
        {
            List<string> listaMaterias = (
                from Subject s in _unidad.RepositorioSubject.GetAll().OrderBy(m => m.Name)
                select s.Name).ToHashSet<string>().ToList<string>();

            return listaMaterias;
        }
        public List<Subject> ObtenerListaMateriasPorColegio(int idColegio)
        {

            List<Subject> listaMaterias = (from Subject s in _unidad.RepositorioSubject._dbset
                                        .Include("School")
                                        where s.School.SchoolId == idColegio

                                        select s).ToList<Subject>();


            return listaMaterias.ToList<Subject>();
        }

        /// <summary>
        /// Método que obtiene la lista de materias con docentes por lapso y curso asociado.
        /// Rodrigo Uzcátegui - 03-04-15
        /// </summary>
        /// <param name="idLapso">Id del lapso</param>
        /// <param name="idCurso">Id del curso</param>
        /// <returns>La lista de materias del curso</returns>
        public List<Subject> ObtenerListaMateriasConDocentesPor_Lapso_Curso(int idLapso, int idCurso)
        {
            HashSet<Subject> listaMaterias = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Subject")
                    .Include("Period")
                    .Include("Course")
                    .Include("Teacher")
                where casu.Period.PeriodId == idLapso && 
                      casu.Course.CourseId == idCurso &&
                      casu.Teacher != null
                select casu.Subject)
                    .OrderBy(m => m.Name)
                    .ToHashSet<Subject>();

            return listaMaterias.ToList<Subject>();
        }

        /// <summary>
        /// Método que devuelve la lita de materias del colegio y el grado respectivo.
        /// Rodrigo Uzcátegui - 02-04-15
        /// </summary>
        /// <param name="idColegio">El id del colegio</param>
        /// <param name="grado">El grado</param>
        /// <returns>La lista de materias</returns>
        public List<Subject> ObtenerListaMateriasPor_Colegio_Grado(int idColegio, int grado)
        {
            List<Subject> listaMaterias = (
                from Subject s in _unidad.RepositorioSubject._dbset
                    .Include("School")
                where s.School.SchoolId == idColegio &&
                      s.Grade == grado
                select s).ToList<Subject>();

            return listaMaterias;
        }
        
        /// <summary>
        /// Método que obtiene la lista de materias desde el casu con el colegio, el grado y el curso.
        /// Rodrigo Uzcátegui - 02-05-15
        /// </summary>
        /// <param name="idColegio">Id del colegio</param>
        /// <param name="grado">Grado de las materias</param>
        /// <param name="idCurso">Id del curso</param>
        /// <returns>La lista de materias</returns>
        public List<Subject> ObtenerListaMateriasCASUPor_Colegio_Grado(int idColegio, int grado, int idCurso)
        {
            List<Subject> listaMaterias = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Subject")
                    .Include("Teacher")
                where casu.TeacherId == null &&
                      casu.CourseId == idCurso &&
                      casu.Subject.School.SchoolId == idColegio &&
                      casu.Subject.Grade == grado
                select casu.Subject)
                    .OrderBy(m => m.Name)
                    .ToHashSet<Subject>()
                    .ToList<Subject>();

            return listaMaterias;
        }

        public List<Subject> ObtenerListaMateriasPorCurso(int idCurso, string idUser)
        {
            //Editado 18-01-15. No probado
            HashSet<Subject> listaMaterias = (from CASU casu in _unidad.RepositorioCASU._dbset
                                 .Include("Subject")
                                 .Include("Teacher")
                                 .Include("Course")
                                              where casu.Teacher.Id == idUser && casu.Course.CourseId == idCurso

                                              select casu.Subject).ToHashSet<Subject>();


            return listaMaterias.ToList<Subject>();
        }

        /// <summary>
        /// Método que obtiene la lista de materias según el curso y el usuario de la sesión.
        /// Rodrigo Uzcátegui & Fabio Puchetti - 05-04-15
        /// </summary>
        /// <param name="idCurso">Id del curso</param>
        /// <returns>La lista de materias respectiva</returns>
        public List<Subject> ObtenerListaMateriasPor_Curso_SDocente(int idCurso)
        {
            List<Subject> listaMaterias = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Subject")
                    .Include("Teacher")
                    .Include("Course")
                where casu.CourseId == idCurso &&
                      casu.TeacherId == _session.USERID
                select casu.Subject)
                      .ToHashSet<Subject>()
                      .OrderBy(m => m.Name)
                      .ToList<Subject>();
            
            return listaMaterias;
        }

        /// <summary>
        /// Método que obtiene la lista de materias por curso.
        /// Rodrigo Uzcátegui y Fabio Puchetti - 05-04-15
        /// </summary>
        /// <param name="idCurso">El id del curso</param>
        /// <returns>La lista de materias del curso respectivo.</returns>
        public List<Subject> ObtenerListaMateriasPor_Curso(int idCurso)
        {
            List<Subject> listaMaterias = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Subject")
                    .Include("Teacher")
                    .Include("Course")
                where casu.CourseId == idCurso
                select casu.Subject)
                      .ToHashSet<Subject>()
                      .OrderBy(m => m.Name)
                      .ToList<Subject>();

            return listaMaterias;
        }


        public List<string> ObtenerListaNombreMateriasPor_SAnoEscolar()
        {
            List<string> lista = (from CASU casu in _unidad.RepositorioCASU._dbset
                                  where casu.Period.SchoolYear.SchoolYearId == _session.SCHOOLYEARID &&
                                        casu.Period.SchoolYear.School.SchoolId == _session.SCHOOLID
                                  orderby casu.Subject.Name
                                  select casu.Subject.Name)
                                        .ToHashSet<string>()
                                        .ToList<string>();

            return lista;
        }
        
        /// <summary>
        /// Método que obtiene la lista de las materias del año escolar de la sesión
        /// 
        /// Rodrigo Uzcátegui - 27-02-15
        /// </summary>
        /// <returns>La lista de materias del año escolar de la sesión</returns>
        public List<Subject> ObtenerListaMaterias()
        {
            List<Subject> lista = (from CASU casu in _unidad.RepositorioCASU._dbset
                                  where casu.Period.SchoolYear.SchoolYearId == _session.SCHOOLYEARID &&
                                        casu.Period.SchoolYear.School.SchoolId == _session.SCHOOLID
                                  orderby casu.Subject.Name
                                  select casu.Subject)
                                    .ToHashSet()
                                    .ToList<Subject>();

            return lista;
        }
        #endregion
        #region Otros métodos
        #endregion
    }
}