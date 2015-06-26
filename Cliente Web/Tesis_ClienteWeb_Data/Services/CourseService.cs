using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class CourseService
    {
        private UnitOfWork _unidad;

        //Constructores
        public CourseService()
        {
            this._unidad = new UnitOfWork();
        }
        public CourseService(UnitOfWork _unidad)
        {
            this._unidad = _unidad;
        }

        #region CRUD
        /// <summary>
        /// CRUD - Guardar curso
        /// </summary>
        /// <param name="course">El curso a guardar</param>
        /// <returns>True: Se guardó con éxito. False: No se guardó</returns>
        public bool GuardarCourse(Course course)
        {            
            try
            {
                _unidad.RepositorioCourse.Add(course);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// CRUD - Modificar Curso
        /// </summary>
        /// <param name="course">El curso a modificar</param>
        /// <returns>True: Se modificó con éxito. False: No se modificó</returns>
        public bool ModificarCourse(Course course)
        {
            try
            {
                _unidad.RepositorioCourse.Modify(course);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// CRUD - Eliminar curso
        /// </summary>
        /// <param name="id">El curso a eliminar</param>
        /// <returns>True: Se eliminó correctamente. False: No se eliminó</returns>
        public bool EliminarCourse(int id)
        {
            try
            {
                _unidad.RepositorioCourse.Delete(u => u.CourseId == id);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Obtener cursos
        /// <summary>
        /// Método que obtiene el curso en específico.
        /// Nota: Obtiene la lista de CASUS en detalle - Rodrigo Uzcátegui. 24-05-15
        /// </summary>
        /// <param name="id">El id del curso</param>
        /// <returns>El curso</returns>
        public Course ObtenerCursoPor_Id(int id)
        {
            CASUService casuService = new CASUService(this._unidad);

            Course curso = (
                from Course c in _unidad.RepositorioCourse._dbset
                    .Include("Students")
                    .Include("CASUs")
                where c.CourseId == id
                select c)
                    .FirstOrDefault<Course>();

            for (int i = 0; i <= curso.CASUs.Count() - 1; i++ )
            {
                curso.CASUs[i] = casuService.ObtenerCASUPor_Ids(curso.CASUs[i].CourseId,
                    curso.CASUs[i].PeriodId, curso.CASUs[i].SubjectId);
            }

            return curso;
        }
        /// <summary>
        /// Método que obtiene el curso por el nombre, el año escolar y el docente asociado
        /// </summary>
        /// <param name="name">El nombre del curso</param>
        /// /// <param name="idAnoEscolar">El id del año escolar</param>
        /// /// <param name="idDocente">El id del docente respectivo</param>
        /// <returns>El curso en específico.</returns>
        public Course ObtenerCursoPor_Nombre_AnoEscolar_Usuario(string nombre, int idAnoEscolar, string idDocente)
        {
            Course curso = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                where casu.Course.Name == nombre &&
                      casu.Period.SchoolYear.SchoolYearId == idAnoEscolar &&
                      casu.Teacher.Id == idDocente
                select casu.Course)
                    .FirstOrDefault<Course>();

            return curso;
        }
        /// <summary>
        /// Método que obtiene el curso activo según el estudiante y el año escolar. 
        /// sRodrigo Uzcátegui. 22-02-15
        /// </summary>
        /// <param name="idEstudiante">El id del estudiante</param>
        /// <param name="idAnoEscolar">El id del año escolar</param>
        /// <returns>El curso activo de aquel estudiante</returns>
        public Course ObtenerCursoPor_Estudiante_AnoEscolar(int idEstudiante, int idAnoEscolar)
        {
            #region Declaración de variables
            Course curso = null;
            bool auxPaso = true;
            Student estudiante;
            List<Course> listaCursos;
            #endregion

            #region Obteniendo el estudiante respectivo
            estudiante = new StudentService().ObtenerAlumnoPorId(idEstudiante);
            #endregion
            #region Obteniendo la lista de cursos del año escolar
            listaCursos = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Course.Students")
                    .Include("Course.CASUs")
                where casu.Period.SchoolYear.SchoolYearId == idAnoEscolar
                select casu.Course)
                    .ToHashSet<Course>()
                    .ToList<Course>();
            #endregion
            #region Obteniendo el curso específico
            foreach (Course course in listaCursos)
            {
                foreach (Course course2 in estudiante.Courses)
                {
                    if (course2.CourseId == course.CourseId)
                    {
                        curso = course2;
                        auxPaso = false;
                        break;
                    }
                }

                if (!auxPaso)
                    break;

            }
            #endregion

            return curso;
        }
        /// <summary>
        /// Método que obtiene el curso activo de un usuario cuya notificación haya sido remitida para ese 
        /// usuario docente.
        /// 
        /// Rodrigo Uzcátegui. 22-02-15
        /// </summary>
        /// <param name="idUsuario">El id del usuario</param>
        /// <param name="notificacion">El objeto notificación</param>
        /// <param name="idAnoEscolar">El id del año escolar (opcional)</param>
        /// <returns>El curso activo respectivo a ese docente.</returns>
        public Course ObtenerCursoPor_Docente_Notificacion_AnoEscolar(string idUsuario,
            Notification notificacion, int idAnoEscolar)
        {
            Course curso = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                where casu.Period.SchoolYear.SchoolYearId == idAnoEscolar && //Validación de año escolar
                      casu.TeacherId == idUsuario && //Validación del usuario
                      casu.Subject.Name == notificacion.Attribution /* Estableciendo que si la notificación
                                                                     * tiene que ver con una materia en 
                                                                     * específico, resultará ese curso.
                                                                     */
                select casu.Course).FirstOrDefault<Course>();

            /* Puede ser nulo porque quizás esa notificación no fue enviada para un usuario de un curso en
             * particular, sino que la atribución de esa notificación fue "N/A".
             */
            if (curso == null)
                return null;
            else
                return curso;
        }
        /// <summary>
        /// Método que obtiene la lista de cursos actuales asociados al docente, según el año escolar del momento.
        /// Rodrigo Uzcátegui - 15-05-15
        /// </summary>
        /// <param name="idDocente">Id del docente</param>
        /// <param name="idAnoEscolar">Id del año escolar</param>
        /// <returns>La lista de cursos respectiva.</returns>
        public List<Course> ObtenerListaCursoPor_Docente_AnoEscolar(string idDocente, int idAnoEscolar)
        {
            List<CASU> listaCASU = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Course")
                    .Include("Period.SchoolYear")
                where casu.Period.SchoolYear.SchoolYearId == idAnoEscolar &&
                      casu.TeacherId == idDocente
                select casu)
                    .ToList<CASU>();

            List<Course> listaCursos = new List<Course>();

            foreach(CASU casu in listaCASU)
            {
                listaCursos.Add(casu.Course);
            }

            return listaCursos;
        }








        
        

        

        

        /// <summary>
        /// Método que obtiene la lista de cursos de un docente
        /// </summary>
        /// <param name="id">El id del docente</param>
        /// <returns>La lista de cursos del docente</returns>
        public List<Course> ObtenerListaCursosPor_Docente(string id, int idAnoEscolar)
        {//Creo que aquí hay un error
            List<Course> listaCursos = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Period.SchoolYear")
                where casu.Teacher.Id == id &&
                      casu.Period.SchoolYear.SchoolYearId == idAnoEscolar
                select casu.Course)
                    .ToHashSet<Course>()
                    .ToList<Course>();
                    
            return listaCursos ;
        }

        public List<User> ObtenerListaProfesoresPorLapsoCursoMateria(int idLapso, int idCurso, int idMateria)
        {
            List<User> listaProfesores = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Subject")
                    .Include("Period")
                    .Include("Course")
                    .Include("Teacher")
                where casu.Period.PeriodId == idLapso && 
                      casu.Course.CourseId == idCurso && 
                      casu.Subject.SubjectId == idMateria
                select casu.Teacher)
                    .ToHashSet<User>()
                    .ToList<User>();

            return listaProfesores;
        }
        public List<Course> ObtenerListaCursosPorColegio(int idColegio)
        {
            School colegio = new SchoolService().ObtenerColegioPorId(idColegio);
            List<Course> listaCursos = new List<Course>();
            SchoolYearService _schoolYearService = new SchoolYearService(this._unidad);

            SchoolYear anoEscolarActivo = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(idColegio);
            listaCursos = ObtenerListaCursosPorAnoEscolar(anoEscolarActivo.SchoolYearId).ToList<Course>();

            return listaCursos;
        }

        public HashSet<Course> ObtenerListaCursosPorLapso(int idLapso)
        {
            HashSet<Course> listaCursos = new HashSet<Course>();

            listaCursos = (from CASU casu in _unidad.RepositorioCASU._dbset
                               .Include("Course")
                           where casu.Period.PeriodId == idLapso 
                           select casu.Course).OrderBy(m => m.Name).ToHashSet<Course>();

            return listaCursos;
        }

        public List<Course> ObtenerListaCursosPorAnoEscolar(int idAnoEscolar)
        {   
            Period periodo = (from Period p in _unidad.RepositorioPeriod._dbset
                                  .Include("CASUs")
                              where p.SchoolYear.SchoolYearId == idAnoEscolar
                              select p).FirstOrDefault<Period>();

            List<Course> listaCursos = new List<Course>();

            try
            {
                listaCursos = (from CASU casu in _unidad.RepositorioCASU._dbset
                               where casu.PeriodId == periodo.PeriodId
                               select casu.Course)
                                    .OrderBy(m => m.Name)
                                    .ToHashSet<Course>()
                                    .ToList<Course>();

                return listaCursos;
            }
            catch (TargetException)
            {
                /* Excepción que aparece cuando en no existen valores en la lista de cursos
                 * Rodrigo Uzcátegui - 29-03-15
                 */
                listaCursos = new List<Course>();
                return listaCursos;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Método que obtiene la lista de cursos sin estudiantes según el año escolar.
        /// Rodrigo Uzcátegui - 09-04-15
        /// </summary>
        /// <param name="idAnoEscolar">Id del año escolar</param>
        /// <returns>La lista de cursos respectiva</returns>
        public List<Course> ObtenerListaCursosPorAnoEscolarSinEstudiantes(int idAnoEscolar)
        {
            List<Course> listaCursos = (from CASU casu in _unidad.RepositorioCASU._dbset
                                        where casu.Period.SchoolYear.SchoolYearId == idAnoEscolar &&
                                              casu.Course.Students.Count == 0 //Que no tenga estudiantes
                                        select casu.Course)
                                            .ToHashSet<Course>()
                                            .OrderBy(m => m.Name)
                                            .ToList<Course>();

            return listaCursos;
        }

        public Dictionary<int, string> ObtenerDiccionarioCursosPorAnoEscolar(int idAnoEscolar)
        {
            Dictionary<int, string> diccionario = new Dictionary<int, string>();

            Period periodo = (from Period p in _unidad.RepositorioPeriod._dbset
                                  .Include("CASUs")
                              where p.SchoolYear.SchoolYearId == idAnoEscolar &&
                                    p.StartDate >= p.SchoolYear.StartDate &&
                                    p.FinishDate <= p.SchoolYear.EndDate
                              select p).FirstOrDefault<Period>();

            List<Course> listaCursos = (from CASU casu in _unidad.RepositorioCASU._dbset
                                        where casu.PeriodId == periodo.PeriodId
                                        select casu.Course)
                                            .OrderBy(m => m.Name)
                                            .ToHashSet()
                                            .ToList<Course>();

            foreach(Course curso in listaCursos)
            {
                diccionario.Add(curso.CourseId, curso.Name);
            }

            return diccionario;
        }

        #endregion
        #region Otros métodos
        /// <summary>
        /// Método que obtiene el número de estudiantes asociados a un curso
        /// </summary>
        /// <param name="idCurso">El id del curso</param>
        /// <returns>El número de estudiantes asociados</returns>
        public int ObtenerNumeroDeEstudiantesPorCurso(int idCurso)
        {
            Course curso = this.ObtenerCursoPor_Id(idCurso); 
            int numalumnos = curso.Students.Count();

            return numalumnos;
        }
        #endregion
    }
}