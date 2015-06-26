using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class AssessmentService
    {
        private UnitOfWork _unidad;

        //Constructores
        public AssessmentService()
        {
            this._unidad = new UnitOfWork();
        }
        public AssessmentService(UnitOfWork _unidad)
        {
            this._unidad = _unidad;
        }

        #region CRUD
        /// <summary>
        /// CRUD - Guardar evaluación
        /// </summary>
        /// <param name="assessment">La asignación a guardar</param>
        /// <returns>True: Se guardó con éxito</returns>
        public bool GuardarAssessment(Assessment assessment)
        {            
            try
            {                
                _unidad.RepositorioAssessment.Add(assessment);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// CRUD - Modificar evaluación
        /// </summary>
        /// <param name="assessment">La evaluación a modificar</param>
        /// <returns>True: Se modificó con éxito.</returns>
        public bool ModificarAssessment(Assessment assessment)
        {
            try
            {
                _unidad.RepositorioAssessment.Modify(assessment);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// CRUD - Eliminar evaluación
        /// </summary>
        /// <param name="id">El id de la evalución</param>
        /// <returns>True: Se eliminó correctamente.</returns>
        public bool EliminarAssessment(int id)
        {
            try
            {
                _unidad.RepositorioAssessment.Delete(u => u.AssessmentId == id);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Obtener Evaluaciones
        /// <summary>
        /// Método que obtiene la evaluación en específico.
        /// </summary>
        /// <param name="id">El id de la evaluación</param>
        /// <returns>La evaluación</returns>
        public Assessment ObtenerEvaluacionPor_Id(int id)
        {            
            Assessment assessment = (
                from Assessment evaluacion in _unidad.RepositorioAssessment._dbset
                    .Include("CASU.Course.Students")
                    .Include("CASU.Period.SchoolYear.School.Subjects")
                    .Include("CASU.Period.SchoolYear.School.SchoolYears")
                    .Include("CASU.Subject")
                    .Include("CASU.Teacher")
                    .Include("Scores")
                    .Include("Event")
                    .Include("IndicatorAssessments")
                where evaluacion.AssessmentId == id
                select evaluacion)
                    .FirstOrDefault<Assessment>();

            return assessment;
        }
        /// <summary>
        /// Método que obtiene la última evaluación realizada por un docente en particular.
        /// Rodrigo Uzcátegui - 27-04-15
        /// </summary>
        /// <param name="idDocente">El id del docente</param>
        /// <param name="idAnoEscolar">El año escolar en cuestión</param>
        /// <returns>La última evaluación realizada por el docente.</returns>
        public Assessment ObtenerUltimaEvaluacionPor_Docente_AnoEscolar(string idDocente, int idAnoEscolar)
        {
            #region Declaración de variables
            Assessment assessment;
            Assessment lastAssessment = null;
            List<Assessment> listaEvaluaciones = new List<Assessment>();
            #endregion
            #region Obteniendo la lista de CASUS del representante
            List<CASU> listaCASU = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Course")
                    .Include("Assessments")
                    .Include("Period.SchoolYear")
                    .Include("Teacher")
                    .Include("Subject")
                where casu.TeacherId == idDocente &&
                      casu.Period.SchoolYear.SchoolYearId == idAnoEscolar
                select casu)
                    .ToList<CASU>();
            #endregion
            #region Ciclo de obtención de todas las evaluaciones con notas (evaluaciones que ya se cargaron)
            foreach (CASU casu in listaCASU)
            {
                foreach (Assessment assessmentAux in casu.Assessments)
                {
                    assessment = this.ObtenerEvaluacionPor_Id(assessmentAux.AssessmentId);
                    if (assessment.Scores.Count() != 0)
                        listaEvaluaciones.Add(assessment);
                }
            }
            #endregion
            #region Reordenando la lista de evaluaciones
            listaEvaluaciones = listaEvaluaciones.OrderByDescending(m => m.FinishDate)
                .ThenByDescending(m => m.EndHour).ToList();
            #endregion
            #region Obteniendo la última evaluación
            if (listaEvaluaciones.Count() != 0)
                lastAssessment = listaEvaluaciones.First<Assessment>();
            #endregion

            return lastAssessment;
        }
        /// <summary>
        /// Método que obtiene la lista de evaluaciones definidas para el curso respectivo.
        /// Rodrigo Uzcátegui - 13-05-15
        /// </summary>
        /// <param name="idCurso">El id del curso</param>
        /// <returns>La lista de evaluaciones asociadas</returns>
        public List<Assessment> ObtenerListaEvaluacionesPor_Curso(int idCurso)
        {
            CourseService courseService = new CourseService(this._unidad);
            Course course = courseService.ObtenerCursoPor_Id(idCurso);
            List<Assessment> listaEvaluaciones = new List<Assessment>();
            Assessment assessmentAux;

            foreach(CASU casu in course.CASUs)
            {
                foreach(Assessment assesssment in casu.Assessments)
                {
                    assessmentAux = this.ObtenerEvaluacionPor_Id(assesssment.AssessmentId);
                    listaEvaluaciones.Add(assessmentAux);
                }
            }

            return listaEvaluaciones;
        }
        /// <summary>
        /// Método que obtiene la lista de todas las evaluaciones de un profesor.
        /// Nota (05-05-15): Este método devuelve la lista de evaluaciones del curso y de la materia, en todos 
        /// los lapsos. Rodrigo Uzcátegui.
        /// </summary>
        /// <returns>La lista de evaluaciones del profesor.</returns>
        public List<Assessment> ObtenerListaEvaluacionesPor_Curso_Materia_Docente(int idMateria, int idCurso, 
            string idDocente)
        {
            List<Assessment> listaEvaluaciones = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Subject")
                    .Include("Course")
                    .Include("Teacher")
                    .Include("Assessments")
                where casu.Teacher.Id == idDocente &&
                      casu.Course.CourseId == idCurso &&
                      casu.Subject.SubjectId == idMateria
                select casu.Assessments)
                    .FirstOrDefault()
                    .OrderBy(m => m.StartDate)
                    .ThenBy(t => t.StartHour)
                    .ToList<Assessment>();

            return listaEvaluaciones;
        }
        /// <summary>
        /// Método que obtiene la lista de evaluaciones según los parámetros solicitados.
        /// NOTA: No se está utilizando el id del docente - Rodrigo Uzcátegui - 03-04-15
        /// NOTA2: Se agregó el id del docente en consideración. Se debe evaluar el comportamiento de este método.
        /// Rodrigo Uzcátegui - 27-04-15
        /// </summary>
        /// <param name="idMateria">El id de la materia</param>
        /// <param name="idCurso">El id del curso</param>
        /// <param name="idProfesor">El id del docente</param>
        /// <param name="idLapso">El id del lapso</param>
        /// <returns>La lista de evaluaciones respectiva</returns>
        public List<Assessment> ObtenerListaEvaluacionesPor_Curso_Materia_Docente_Lapso(int idMateria,
            int idCurso, string idProfesor, int idLapso)
        {
            List<Assessment> lista = new List<Assessment>();

            List<Assessment> listaEvaluaciones = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Period.SchoolYear.School")
                    .Include("Course.CASU")
                    .Include("Subject")
                    .Include("Teacher")
                    .Include("Assessments")
                where casu.PeriodId == idLapso &&
                      casu.CourseId == idCurso &&
                      casu.SubjectId == idMateria &&
                      casu.TeacherId == idProfesor
                select casu.Assessments)
                    .FirstOrDefault()
                    .OrderBy(m => m.FinishDate)
                    .ThenBy(t => t.StartHour)
                    .ToList<Assessment>();

            foreach(Assessment evaluacion in listaEvaluaciones)
            {
                lista.Add(this.ObtenerEvaluacionPor_Id(evaluacion.AssessmentId));
            }

            return lista;
        }
        /// <summary>
        /// Método que obtiene la lista de evaluaciones por curso, materia y lapso
        /// Nota (05-05-15): La lista trate todas las evaluaciones del curso, materia y lapso sin importar el 
        /// docente.
        /// Rodrigo Uzcátegui - 03-04-15
        /// </summary>
        /// <param name="idCurso">El id del curso</param>
        /// <param name="idMateria">Id de la materia</param>
        /// <param name="idLapso">Id del lapso</param>
        /// <returns>La lista de evaluaciones respectiva.</returns>
        public List<Assessment> ObtenerListaEvaluacionesPor_Curso_Materia_Lapso(int idCurso, int idMateria, 
            int idLapso)
        {
            try
            {
                List<Assessment> listaEvaluaciones = (
                    from CASU casu in _unidad.RepositorioCASU._dbset
                        .Include("Period.SchoolYear.School")
                        .Include("Course")
                        .Include("Subject")
                        .Include("Teacher")
                        .Include("Assessments")
                    where casu.PeriodId == idLapso &&
                          casu.CourseId == idCurso &&
                          casu.SubjectId == idMateria
                    select casu.Assessments)
                        .FirstOrDefault()
                        .OrderBy(m => m.StartDate)
                        .ThenBy(t => t.StartHour)
                        .ToList<Assessment>();

                return listaEvaluaciones;
            }
            catch (ArgumentNullException)
            {
                // Entra cuando la materia no tiene evaluaciones en ese periodo 
                // Fabio Puchetti
                // 05/04/2015
                
                return new List<Assessment>();
            }
            catch (Exception e)
            {
                throw e;

            };
        }
        /// <summary>
        /// Método que obtiene la lista de evaluaciones según los datos pasados por parámetro.
        /// Rodrigo Uzcátegui & Fabio Puchetti - 05-04-15
        /// </summary>
        /// <param name="idCurso">El id del curso</param>
        /// <param name="idMateria">Id de la materia</param>
        /// <param name="nombreLapso">Nombre del lapso (1er Lapso, 2do Lapso, 3er Lapso)</param>
        /// <returns>La lista de evaluaciones respectiva.</returns>
        public List<Assessment> ObtenerListaEvaluacionesPor_Curso_Materia_Lapso(int idCurso, int idMateria, 
            string nombreLapso)
        {
            List<Assessment> lista = new List<Assessment>();

            try
            {
                List<Assessment> listaEvaluaciones = (
                    from CASU casu in _unidad.RepositorioCASU._dbset
                        .Include("Period.SchoolYear.School")
                        .Include("Course")
                        .Include("Subject")
                        .Include("Teacher")
                        .Include("Assessments")
                    where casu.Period.Name == nombreLapso &&
                          casu.CourseId == idCurso &&
                          casu.SubjectId == idMateria
                    select casu.Assessments)
                        .FirstOrDefault()
                        .OrderBy(m => m.StartDate)
                        .ThenBy(t => t.StartHour)
                        .ToList<Assessment>();

                foreach (Assessment evaluacion in listaEvaluaciones)
                {
                    lista.Add(this.ObtenerEvaluacionPor_Id(evaluacion.AssessmentId));
                }

                return listaEvaluaciones;
            }
            catch (ArgumentNullException)
            {
                // Entra cuando la materia no tiene evaluaciones en ese periodo 
                // Fabio Puchetti
                // 05/04/2015

                //return null;
                return new List<Assessment>();
            }
            catch (Exception e)
            {
                throw e;
            };
        }       
        /// <summary>
        /// Método que obtiene la lista de evaluaciones ya realizadas por el docente, durante un curso en 
        /// particular. La condición que se está utilizando para definir si una evaluación ya se realizó es
        /// buscar si ésta ya posee notas asociadas.
        /// Rodrigo Uzcátegui - 27-04-15
        /// </summary>
        /// <param name="idCurso">El id del curso</param>
        /// <param name="idLapso">El id del lapso</param>
        /// <param name="idMateria">El id de la materia</param>
        /// <param name="idDocente">El id del docente</param>
        /// <returns>La lista de evaluaciones realizadas del curso (CASU) respectivo</returns>
        public List<Assessment> ObtenerListaEvaluacionesRealizadasPor_Curso_Lapso_Materia_Docente(int idCurso, 
            int idLapso, int idMateria, string idDocente)
        {
            #region Declaración de variables
            List<Assessment> listaEvaluaciones = new List<Assessment>();
            Assessment assessmentAux;
            #endregion

            List<Assessment> lista = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Assessments")
                where casu.CourseId == idCurso &&
                      casu.PeriodId == idLapso &&
                      casu.SubjectId == idMateria &&
                      casu.TeacherId == idDocente
                select casu.Assessments)
                    .FirstOrDefault()
                    .ToList<Assessment>();

            foreach(Assessment assessment in lista)
            {
                assessmentAux = this.ObtenerEvaluacionPor_Id(assessment.AssessmentId);

                if (assessment.Scores.Count() != 0)
                    listaEvaluaciones.Add(assessment);
            }

            return listaEvaluaciones;
        }
        #endregion

        #region Otros métodos
        /// <summary>
        /// Método que obtiene el porcentaje de alumnos aprobados según un curso en particular.
        /// Rodrigo Uzcátegui - 30-04-15
        /// </summary>
        /// <param name="idLapso">Id del lapso</param>
        /// <param name="idCurso">Id del curso</param>
        /// <param name="idDocente">Id del docente</param>
        /// <returns>Porcentaje de alumnos aprobados del salón</returns>
        public double ObtenerPorcentajeAlumnosAprobadosPor_Lapso_Curso_Docente(int idLapso, int idCurso, 
            string idDocente)
        {
            #region Declaración de variables
            StudentService studentService = new StudentService(this._unidad);
            List<Student> listaAprobados = new List<Student>();
            Assessment assessment;
            Student estudiante;
            Score score = new Score();
            double sumatoria = 0;
            #endregion
            #region Obteniendo el CASU respectivo
            CASU casuAux = (
                from CASU casu in _unidad.RepositorioCASU._dbset
                    .Include("Assessments")
                    .Include("Course.Students")
                where casu.CourseId == idCurso &&
                      casu.PeriodId == idLapso &&
                      casu.TeacherId == idDocente
                select casu)
                    .FirstOrDefault();
            #endregion
            #region Obteniendo datos desde el CASU
            List<Assessment> listaEvaluaciones = casuAux.Assessments;
            List<Student> listaEstudiantes = casuAux.Course.Students;
            int grado = casuAux.Course.Grade;
            #endregion

            foreach (Student estudianteAux in listaEstudiantes)
            {
                estudiante = studentService.ObtenerAlumnoPorId(estudianteAux.StudentId);

                foreach(Assessment evaluacionAux in listaEvaluaciones)
                {
                    assessment = this.ObtenerEvaluacionPor_Id(evaluacionAux.AssessmentId);
                    if (assessment.Scores.Count() != 0) //Que tenga notas adheridas
                    {
                        score = assessment.Scores.Where(m => m.StudentId == estudiante.StudentId).FirstOrDefault();
                        if(score == null)
                            score = new Score() { NumberScore = 0, LetterScore = "E" };
                    }
                    else
                        score = new Score() { NumberScore = 0, LetterScore = "E" };

                    if (grado > 6) //Bachillerato
                        sumatoria += (double)(score.NumberScore * assessment.Percentage)/100;
                    else //Primaria
                        sumatoria += (double)score.ToIntLetterScore(score.LetterScore);
                }

                #region Bachillerato
                if (grado > 6 && Math.Round(sumatoria) >= 10)
                    listaAprobados.Add(estudiante);
                #endregion
                #region Primaria
                else if (grado <= 6 && sumatoria > listaEvaluaciones.Count())
                    listaAprobados.Add(estudiante);
                #endregion

                sumatoria = 0; //Reiniciando variable
            }

            double porcentajeAprobados = (double)(listaAprobados.Count() * 100) / listaEstudiantes.Count();

            return porcentajeAprobados;
        }
        /// <summary>
        /// Método que obtiene el porcentaje acumulado de las evaluaciones definidas en el CASU respectivo.
        /// Rodrigo Uzcátegui - 12-05-15
        /// </summary>
        /// <param name="idLapso">El id del lapso.</param>
        /// <param name="idCurso">El id del curso.</param>
        /// <param name="idMateria">El id de la materia.</param>
        /// <returns>Valor entero que define el acumulado de porcentaje de las evaluaciones respectivas.</returns>
        public int ObtenerTotalPorcentajeEvaluacionesPor_Lapso_Curso_Materia(int idLapso, int idCurso, 
            int idMateria)
        {
            CASUService casuService = new CASUService(this._unidad);
            CASU casu = casuService.ObtenerCASUPor_Ids(idCurso, idLapso, idMateria);
            List<Assessment> listaEvaluaciones = casu.Assessments;
            int totalPorcentaje = 0;

            foreach(Assessment assessment in listaEvaluaciones)
            {
                totalPorcentaje += assessment.Percentage;
            }

            return totalPorcentaje;
        }
        #endregion
    }
}