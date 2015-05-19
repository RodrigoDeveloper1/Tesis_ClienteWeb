using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class CASUService
    {
        private UnitOfWork _unidad;

        //Constructores
        public CASUService()
        {
            this._unidad = new UnitOfWork();
        }
        public CASUService(UnitOfWork _unidad)
        {
            this._unidad = _unidad;
        }

        #region CRUD
        /// <summary>
        /// CRUD - Guardar CASU
        /// </summary>
        /// <param name="assessment">El CASU a guardar</param>
        /// <returns>True: Se guardó con éxito. False: No se guardó</returns>
        public bool GuardarCASU(CASU casu)
        {
            try
            {
                _unidad.RepositorioCASU.Add(casu);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Método CRUD - Modificar CASU
        /// </summary>
        /// <param name="casu">El CASU a modificar</param>
        /// <returns>True = Modificado correcto.</returns>
        public bool ModificarCASU(CASU casu)
        {
            try
            {
                _unidad.RepositorioCASU.Modify(casu);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Obtener CASUs
        /// <summary>
        /// Método que obtiene el CASU respectivo según los id primarios.
        /// Rodrigo Uzcátegui - 02-05-15
        /// </summary>
        /// <param name="courseId">Id del curso</param>
        /// <param name="periodId">Id del lapso</param>
        /// <param name="subjectId">Id de la materia</param>
        /// <returns>El CASU respectivo</returns>
        public CASU ObtenerCASUPor_Ids(int courseId, int periodId, int subjectId)
        {
            CASU casu = (
                from CASU c in _unidad.RepositorioCASU._dbset
                    .Include("Course.Students")
                    .Include("Period.SchoolYear.School.Subjects")
                    .Include("Subject")
                    .Include("User")
                    .Include("Assessments")
                where c.CourseId == courseId &&
                      c.PeriodId == periodId &&
                      c.SubjectId == subjectId
                select c)
                    .FirstOrDefault<CASU>();

            return casu;
        }
        /// <summary>
        /// Método que obtiene el CASU según los ids respectivos. Utiliza el id del docente como valor extra
        /// para definir el CASU.
        /// </summary>
        /// <param name="courseId">El id del curso</param>
        /// <param name="periodId">El id del período</param>
        /// <param name="subjectId">El id de la materia</param>
        /// <param name="userId">El id del docente</param>
        /// <returns>El CASU respectivo.</returns>
        public CASU ObtenerCASUPor_Ids(int courseId, int periodId, int subjectId, string userId)
        {
            CASU casu = (
                from CASU c in _unidad.RepositorioCASU._dbset
                    .Include("Course")
                    .Include("Period.SchoolYear")
                    .Include("Subject")
                    .Include("User")
                    .Include("Assessments")
                where c.CourseId == courseId && 
                      c.PeriodId == periodId && 
                      c.SubjectId == subjectId && 
                      c.UserId == userId
                select c)
                    .FirstOrDefault<CASU>();

            return casu;
        }
        /// <summary>
        /// Método que obtiene la lista de CASUS según el lapso en cuestión
        /// </summary>
        /// <param name="idLapso">Id del lapso.</param>
        /// <returns>La lista de CASUs respectiva.</returns>
        public List<CASU> ObtenerListaCASUPor_Periodo(int idLapso)
        {
            List<CASU> lista = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                where casu.Period.PeriodId == idLapso
                select casu).ToList<CASU>();

            return lista;
        }
        /// <summary>
        /// Método que obtiene la lista de casus por curso y materia.
        /// Rodrigo Uzcátegui - 02-04-15
        /// </summary>
        /// <param name="idCurso">El id del curso</param>
        /// <param name="idMateria">El id de la materia</param>
        /// <returns>La lista de CASUS respectiva</returns>
        public List<CASU> ObtenerListaCASUPor_Curso_Materia(int idCurso, int idMateria)
        {
            List<CASU> lista = (from CASU casu in _unidad.RepositorioCASU._dbset
                                    .Include("Course")
                                    .Include("Period.SchoolYear")
                                    .Include("Subject")
                                    .Include("User")
                                    .Include("Assessments")
                                where casu.Course.CourseId == idCurso &&
                                      casu.Subject.SubjectId == idMateria
                                select casu)
                                    .ToList<CASU>();

            return lista;
        }
        #endregion
    }
}