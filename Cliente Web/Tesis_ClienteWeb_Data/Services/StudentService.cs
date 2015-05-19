using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class StudentService
    {
        #region Configuración inicial
        private SessionVariablesRepository _session;
        private UnitOfWork _unidad;

        public StudentService()
        {
            this._unidad = new UnitOfWork();
            _InicializadorVariablesSesion();
        }
        public StudentService(UnitOfWork unidad)
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
        public bool GuardarStudent(Student student)
        {
            //if (student.Course != null)
            //{
                //unidad.RepositorioCourse.Modify(student.Course);
            //}

            _unidad.RepositorioStudent.Add(student);

            try
            {
                _unidad.Save();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Método CRUD - Modifica el estudiante.
        /// </summary>
        /// <param name="estudiante">El objeto estudiante</param>
        /// <returns>True = estudiante modificado.</returns>
        public bool ModificarEstudiante(Student estudiante)
        {
            try
            {
                _unidad.RepositorioStudent.Modify(estudiante);
                _unidad.Save();

                return true;
            }
            catch (NullReferenceException e)
            {
                // 
                // Excepción encapsulada por: Rodrigo Uzcátegui - 11-04-15
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool ModificarStudent(Student student, int id)
        {
            try
            {
                _unidad.RepositorioStudent.Update(student, id);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public bool EliminarStudent(int studentID)
        {
            var aux = _unidad.RepositorioStudent.GetAll().Where(u => u.StudentId == studentID).Count();
            {
                _unidad.RepositorioStudent.Delete(u => u.StudentId == studentID);
                _unidad.Save();

                return true;
            }
        }
        #endregion
        #region Obtener estudiantes
        public Student ObtenerAlumnoPorId(int id)
        {
            Student alumno = (from Student c in _unidad.RepositorioStudent._dbset
                                  .Include("Courses")
                                  .Include("Representatives")
                                  .Include("Scores")
                              where c.StudentId == id
                              select c).FirstOrDefault<Student>();

            return alumno;
        }        
        public IQueryable<Student> ObtenerListaEstudiantesPorNombreCurso(int idColegio, string nombreCurso)
        {
            IQueryable<Student> listaEstudiantes = null; /*= (from Student s in unidad.RepositorioStudent.
                                                        _dbset.Include(("Representatives"))
                                                    where s.Course.Name == nombreCurso &&
                                                          s.Course.Status == true &&
                                                          s.Course.School.SchoolId == idColegio
                                                    select s).OrderBy(m => m.FirstLastName); */

            return listaEstudiantes;
        }
        public ICollection<Student> ObtenerListaAlumnos()
        {
            try
            {
                return _unidad.RepositorioStudent.GetAll().OrderBy(m => m.FirstLastName).ThenBy(m => m.SecondLastName).ToList<Student>();
            }
            catch (InvalidOperationException e)
            {
                //Carga inicial del proyecto
                //return new List<Subject>();
                throw e;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
        public List<Student> ObtenerListaEstudiantesConNotas(int idCurso)
        {

            List<Student> listaEstudiantes = new List<Student>();
            List<Student> lista = (
                 from Course course in _unidad.RepositorioCourse._dbset
                     .Include("Students")
                 where course.CourseId == idCurso
                 select course.Students)
                     .FirstOrDefault()
                     .OrderBy(m => m.NumberList)
                     .ToList<Student>();

            foreach (Student estudiante in lista)
            {
                Student estudianteNotas = ObtenerAlumnoPorId(estudiante.StudentId);
                if (estudianteNotas.Scores.Count != 0)
                    listaEstudiantes.Add(estudianteNotas);
            }

            return listaEstudiantes;
        }

        /// <summary>
        /// Método que obtiene la lista de estudiantes con sus notas por materia.
        /// Rodrigo Uzcátegui - 03-04-15
        /// </summary>
        /// <param name="idCurso">El id del curso</param>
        /// <param name="idMateria">El id de la materia</param>
        /// <returns>La lista de alumnos respectiva</returns>
        public List<Student> ObtenerListaEstudiantesConNotasPor_Materia(int idCurso, int idMateria)
        {
            ScoreService scoreService = new ScoreService(this._unidad);
            List<Student> listaEstudiantes = new List<Student>();

            List<Student> lista = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                        .Include("Course.Students")
                where casu.CourseId == idCurso &&
                      casu.SubjectId == idMateria
                select casu.Course.Students)
                        .FirstOrDefault()
                        .ToHashSet<Student>()
                        .ToList<Student>();

            foreach(Student estudiante in lista)
            {
                estudiante.Scores = scoreService.ObtenerNotasPor_Alumno_Materia(estudiante.StudentId, idMateria);

                if(estudiante.Scores.Count != 0)
                    listaEstudiantes.Add(estudiante);
            }

            return listaEstudiantes;
        }

        /// <summary>
        /// Método que obtiene la lista de estudiantes con sus notas por materia y lapso.
        /// Rodrigo Uzcátegui & Fabio Puchetti - 05-04-15
        /// </summary>
        /// <param name="idCurso">El id del curso</param>
        /// <param name="idMateria">El id de la materia</param>
        /// <param name="idLapso">El id del lapso</param>
        /// <returns>La lista de alumnos respectiva</returns>
        public List<Student> ObtenerListaEstudiantesConNotasPor_Materia_Lapso(int idCurso, int idMateria, 
            int idLapso)
        {
            ScoreService scoreService = new ScoreService(this._unidad);
            List<Student> listaEstudiantes = new List<Student>();

            List<Student> lista = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                        .Include("Course.Students")
                where casu.CourseId == idCurso &&
                      casu.SubjectId == idMateria &&
                      casu.PeriodId == idLapso
                select casu.Course.Students)
                        .FirstOrDefault()
                        .ToHashSet<Student>()
                .ToList<Student>();
                      

            foreach(Student estudiante in lista)
            {
                estudiante.Scores = scoreService.ObtenerNotasPor_Alumno_Materia_Lapso(estudiante.StudentId, 
                    idMateria, idLapso);

                if(estudiante.Scores.Count != 0)
                    listaEstudiantes.Add(estudiante);
            }

            return listaEstudiantes;
        }

        /// <summary>
        /// Método que obtiene la lista de estudiantes por curso
        /// 
        /// Rodrigo Uzcátegui - 27-02-15
        /// </summary>
        /// <param name="idCurso">El id del curso para obtener los estudiantes</param>
        /// <returns>La lista de estudiantes respectiva</returns>
        public List<Student> ObtenerListaEstudiantePorCurso(int idCurso)
        {
            Course curso = new CourseService().ObtenerCursoPor_Id(idCurso);
            List<Student> listaEstudiante = curso.Students
                                                .OrderBy(m => m.NumberList)
                                                .ToList<Student>();

            List<Student> listaEstudiantesAux = new List<Student>();
            Student studentAux = new Student();

            foreach(Student estudiante in listaEstudiante)
            {
                studentAux = this.ObtenerAlumnoPorId(estudiante.StudentId);
                listaEstudiantesAux.Add(studentAux);
            }

            return listaEstudiantesAux;
        }

        /// <summary>
        /// Método que obtiene la lista de estudiantes (junto con toda su información de un año escolar en 
        /// particular.
        /// Rodrigo Uzcátegui - 04-04-15
        /// </summary>
        /// <param name="idAnoEscolar">Id del año escolar</param>
        /// <returns>La lista de estudiantes respectivo</returns>
        public List<Student> ObtenerListaEstudiantePor_AnoEscolar(int idAnoEscolar)
        {
            List<Student> lista = new List<Student>();
            List<Student> listaEstudiante = (from CASU casu in _unidad.RepositorioCASU._dbset
                                                .Include("Course.Students")
                                             where casu.Period.SchoolYear.SchoolYearId == idAnoEscolar
                                             select casu.Course.Students)
                                                .FirstOrDefault()
                                                .ToHashSet<Student>()
                                                .ToList<Student>();

            foreach(Student estudiante in listaEstudiante)
            {
                Student studentAux = this.ObtenerAlumnoPorId(estudiante.StudentId);
                lista.Add(studentAux);
            }

            return lista;
        }
        #endregion
        #region Otros métodos
        /// <summary>
        /// Método que obtiene la lista de todos los representantes de todos los cursos correspondientes al 
        /// año escolar pasado por parámetro.
        /// Rodrigo Uzcátegui - 03-04-15
        /// </summary>
        /// <param name="idAnoEscolar">Id del año escolar</param>
        /// <returns>La lista de representantes</returns>
        public List<Representative> ObtenerListaRepresentantesPor_AnoEscolar(int idAnoEscolar)
        {
            List<Representative> listaRepresentantes = new List<Representative>();
            List<Student> listaEstudiantes = new List<Student>();
            HashSet<Course> listaCursos = new HashSet<Course>();

            listaCursos = (from CASU casu in _unidad.RepositorioCASU._dbset
                                .Include("Course.Students")
                           where casu.Period.SchoolYear.SchoolYearId == idAnoEscolar
                           orderby casu.Course.Grade
                           select casu.Course)
                                .ToHashSet<Course>();

            foreach(Course curso in listaCursos)
            {
                listaEstudiantes = this.ObtenerListaEstudiantePorCurso(curso.CourseId);

                foreach (Student estudiante in listaEstudiantes)
                {
                    foreach (Representative representante in estudiante.Representatives)
                    {
                        listaRepresentantes.Add(representante);
                    }
                }
            }

            return listaRepresentantes;
        }



        #endregion
    }
}