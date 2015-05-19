using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class Values
    {
        public int idEstudiante { get; set; }
        public int idLapso { get; set; }
        public int idMateria { get; set; }
        public double acumulativo { get; set; }

        public Values(int idEstudiante, int idLapso, int idMateria, double acumulativo)
        {
            this.idEstudiante = idEstudiante;
            this.idLapso = idLapso;
            this.idMateria = idMateria;
            this.acumulativo = acumulativo;
        }
    }
    public class Values_2
    {
        public int idEstudiante { get; set; }
        public int idMateria { get; set; }
        public double acumulativo { get; set; }

        public Values_2(int idEstudiante, int idMateria, double acumulativo)
        {
            this.idEstudiante = idEstudiante;
            this.idMateria = idMateria;
            this.acumulativo = acumulativo;
        }
    }

    public class ScoreService
    {
        private UnitOfWork _unidad;

        //Constructores
        public ScoreService()
        {
            this._unidad = new UnitOfWork();
        }
        public ScoreService(UnitOfWork unidad)
        {
            this._unidad = unidad;
        }

        #region CRUD
        /// <summary>
        /// Método CRUD - Guarda la nota
        /// </summary>
        /// <param name="score">La nota a guardar</param>
        /// <returns>True = Guardado correcto</returns>
        public bool GuardarScore(Score score)
        {
            try
            {
                _unidad.RepositorioScore.Add(score);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Método CRUD - Modificar la nota
        /// </summary>
        /// <param name="score">La nota a modificar</param>
        /// <returns>True = Modificado correcto</returns>
        public bool ModificarScore(Score score)
        {
            try
            {
                _unidad.RepositorioScore.Modify(score);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #region Obtener notas
        /// <summary>
        /// Método que obtiene la nota específica según el alumno y la evaluación en cuestión.
        /// </summary>
        /// <param name="idAlumno">El id del alumno</param>
        /// <param name="idEvaluacion">El id de la evaluación</param>
        /// <returns>La nota respectiva</returns>
        public Score ObtenerNotaPor_Alumno_Evaluacion(int idAlumno, int idEvaluacion)
        {
            Score nota = (
                from Score c in _unidad.RepositorioScore._dbset
                    .Include("Student")
                    .Include("Assessment")
                where c.StudentId == idAlumno && c.AssessmentId == idEvaluacion
                select c)
                    .FirstOrDefault<Score>();

            return nota;
        }
        
        /// <summary>
        /// Método que obtiene la lista de notas asociadas a un alumno por una materia en particular.
        /// Rodrigo Uzcátegui & Fabio Puchetti - 04-04-15
        /// </summary>
        /// <param name="idEstudiante">Id del estudiante</param>
        /// <param name="idMateria">Id de la materia</param>
        /// <returns>La lista de notas del alumno respectivo.</returns>
        public List<Score> ObtenerNotasPor_Alumno_Materia(int idEstudiante, int idMateria)
        {
            #region Declaraciónj de variables
            SubjectService subjectService = new SubjectService(this._unidad);
            Subject materia = subjectService.ObtenerMateriaPorId(idMateria);
            List<Score> listaNotas;
            #endregion

            #region Bachillerato
            if (materia.Grade > 6)
            {
                listaNotas = (
                from Score score in _unidad.RepositorioScore._dbset
                    .Include("Student")
                    .Include("Assessment.CASU")
                where score.StudentId == idEstudiante &&
                      score.Assessment.CASU.SubjectId == idMateria
                select score)
                    .OrderByDescending(m => m.NumberScore)
                    .ToList<Score>();
            }
            #endregion
            #region Primaria
            else 
            {
                listaNotas = (
                from Score score in _unidad.RepositorioScore._dbset
                    .Include("Student")
                    .Include("Assessment.CASU")
                where score.StudentId == idEstudiante &&
                      score.Assessment.CASU.SubjectId == idMateria
                select score)
                    .OrderByDescending(m => m.LetterScore)
                    .ToList<Score>();
            }
            #endregion

            return listaNotas;
        }
        /// <summary>
        /// Método que obtiene la lista de notas asociadas a un alumno por una materia en particular.
        /// Rodrigo Uzcátegui & Fabio Puchetti - 05-04-15
        /// </summary>
        /// <param name="idEstudiante">Id del estudiante</param>
        /// <param name="idMateria">Id de la materia</param>
        /// /// <param name="idLapso">Id del lapso</param>
        /// <returns>La lista de notas del alumno respectivo.</returns>
        public List<Score> ObtenerNotasPor_Alumno_Materia_Lapso(int idEstudiante, int idMateria, int idLapso)
        {
            #region Declaraciónj de variables
            SubjectService subjectService = new SubjectService(this._unidad);
            Subject materia = subjectService.ObtenerMateriaPorId(idMateria);
            List<Score> listaNotas;
            #endregion

            #region Bachillerato
            if (materia.Grade > 6)
            {
                listaNotas = (
                from Score score in _unidad.RepositorioScore._dbset
                    .Include("Student")
                    .Include("Assessment.CASU")
                where score.StudentId == idEstudiante &&
                      score.Assessment.CASU.SubjectId == idMateria &&
                      score.Assessment.CASU.PeriodId == idLapso
                select score)
                    .OrderByDescending(m => m.NumberScore)
                    .ToList<Score>();
            }
            #endregion
            #region Primaria
            else 
            {
                listaNotas = (
                from Score score in _unidad.RepositorioScore._dbset
                    .Include("Student")
                    .Include("Assessment.CASU")
                where score.StudentId == idEstudiante &&
                      score.Assessment.CASU.SubjectId == idMateria
                select score)
                    .OrderByDescending(m => m.LetterScore)
                    .ToList<Score>();
            }
            #endregion

            return listaNotas;
        }
        /// <summary>
        /// Método que obtiene la lista de notas según una evaluación en particular.
        /// Rodrigo Uzcátegui - 27-04-15
        /// </summary>
        /// <param name="idEvaluacion">El id de la evaluación</param>
        /// <returns>La lista de notas respectiva</returns>
        public List<Score> ObtenerNotasPor_Evaluacion(int idEvaluacion)
        {
            List<Score> listaNotas = (
                from Score c in _unidad.RepositorioScore._dbset
                    .Include("Student")
                    .Include("Assessment")
                where c.AssessmentId == idEvaluacion
                select c)                    
                    .ToList<Score>();

            return listaNotas;
        }
        #endregion
        #region Otros métodos
        /// <summary>
        /// Método que obtiene las notas definitivas de cada alumno para una materia, en un lapso en particular,
        /// de un curso en particular.
        /// Rodrigo Uzcátegui - 04-05-15.
        /// 
        /// Nota (05-05-15): Si el resultado del cálculo es 0, es porque el alumno no se le asignó ninguna nota
        /// </summary>
        /// <param name="idCurso">Id del curso</param>
        /// <param name="idLapso">Id del lapso</param>
        /// <param name="idMateria">Id de la materia</param>
        /// <param name="idDocente">Id del docente</param>
        /// <returns>Un diccionario que indica en la llave, el id del estudiante, y en el valor, su definitiva
        /// </returns>
        public Dictionary<int, double> ObtenerDefinitivasPor_Curso_Lapso_Materia_Docente(int idCurso, 
            int idLapso, int idMateria, string idDocente)
        {
            #region Declaración de variables
            Dictionary<int, double> diccionario = new Dictionary<int, double>();
            AssessmentService assessmentService = new AssessmentService(this._unidad);
            Assessment assessment;
            Score score;
            double calculo = 0;
            #endregion
            #region Obteniendo el casu respectivo
            CASU casu = (
                from CASU casuAux in _unidad.RepositorioCASU._dbset
                    .Include("Assessments")
                    .Include("Course.Students")
                where casuAux.CourseId == idCurso &&
                      casuAux.PeriodId == idLapso &&
                      casuAux.SubjectId == idMateria &&
                      casuAux.UserId == idDocente
                select casuAux)
                    .FirstOrDefault<CASU>();
            #endregion
            #region Obteniendo la lista de estudiantes & lista de evaluaciones respectivas
            List<Student> listaEstudiantes = casu.Course.Students;
            List<Assessment> listaEvaluaciones = casu.Assessments;
            #endregion
            #region Ciclo de cálculo de definitivas
            foreach (Student student in listaEstudiantes)
            {
                foreach (Assessment assessmentAux in listaEvaluaciones)
                {
                    assessment = assessmentService.ObtenerEvaluacionPor_Id(assessmentAux.AssessmentId);
                    score = assessment.Scores.Where(m => m.StudentId == student.StudentId).FirstOrDefault<Score>();
                    if (score != null) //Nota no vacía
                        calculo += ((double)(score.NumberScore * assessment.Percentage) / 100);
                };

                diccionario.Add(student.StudentId, calculo);
                calculo = 0;
            }
            #endregion

            return diccionario;
        }
        /// <summary>
        /// Método que obtiene las notas definitivas de cada alumno para un CASU en particular.
        /// Rodrigo Uzcátegui - 07-05-15.
        /// 
        /// Nota (05-05-15): Si el resultado del cálculo es 0, es porque el alumno no se le asignó ninguna nota
        /// </summary>
        /// <param name="CASU">El CASU respectivo</param>
        /// <returns>Un diccionario que indica en la llave, el id del estudiante, y en el valor, su definitiva
        /// </returns>
        public Dictionary<int, double> ObtenerDefinitivasPor_CASU(CASU casu)
        {
            #region Declaración de variables
            Dictionary<int, double> diccionario = new Dictionary<int, double>();
            AssessmentService assessmentService = new AssessmentService(this._unidad);
            Assessment assessment;
            Score score;
            double calculo = 0;
            #endregion
            #region Obteniendo la lista de estudiantes & lista de evaluaciones respectivas
            List<Student> listaEstudiantes = casu.Course.Students;
            List<Assessment> listaEvaluaciones = casu.Assessments;
            #endregion
            #region Ciclo de cálculo de definitivas
            foreach (Student student in listaEstudiantes)
            {
                foreach (Assessment assessmentAux in listaEvaluaciones)
                {
                    assessment = assessmentService.ObtenerEvaluacionPor_Id(assessmentAux.AssessmentId);
                    score = assessment.Scores.Where(m => m.StudentId == student.StudentId).FirstOrDefault<Score>();
                    if (score != null) //Nota no vacía
                        calculo += ((double)(score.NumberScore * assessment.Percentage) / 100);
                };

                diccionario.Add(student.StudentId, calculo);
                calculo = 0;
            }
            #endregion

            return diccionario;
        }

        public Dictionary<int, double> ObtenerDefinitivaPor_Curso(int idCurso)
        {
            #region Declaración de servicios
            CourseService courseService = new CourseService(this._unidad);
            CASUService casuService = new CASUService(this._unidad);
            AssessmentService assessmentService = new AssessmentService(this._unidad);
            #endregion
            #region Declaración de variables
            CASU casu = new CASU();
            Assessment assessment = new Assessment();
            Course curso = courseService.ObtenerCursoPor_Id(idCurso);
            List<Student> listaEstudiantes = curso.Students;
            List<Subject> listaMaterias = new List<Subject>();
            Score score = new Score();
            double acumulativoEvaluaciones = 0;
            bool control = true;

            ///Guardará la lista de cada estudiante, con el acumulativo del total de cada materia por lapso.
            List<Values> listaValues = new List<Values>();

            ///Guardará la lista de cada estudiante, con el promedio obtenido en la definitiva de cada materia
            ///en los 3 lapsos.
            List<Values_2> listaValues_2 = new List<Values_2>();

            Dictionary<int, double> diccionarioDefinitivas = new Dictionary<int, double>();
            Dictionary<int, double> diccionarioDefinitivas2 = new Dictionary<int, double>();
            #endregion

            foreach (Student student in listaEstudiantes)
            {
                foreach (CASU casuAux in curso.CASUs)
                {
                    casu = casuService.ObtenerCASUPor_Ids(casuAux.CourseId, casuAux.PeriodId, casuAux.SubjectId);
                                        
                    foreach (Assessment assessmentAux in casu.Assessments)
                    {
                        assessment = assessmentService.ObtenerEvaluacionPor_Id(assessmentAux.AssessmentId);
                        score = assessment.Scores.Where(m => m.StudentId == student.StudentId).FirstOrDefault();
                        if (score == null)
                            score = new Score() { LetterScore = "E", NumberScore = 01 };
                        acumulativoEvaluaciones += score.NumberScore * ((double)assessment.Percentage / 100);
                    }
                    listaValues.Add(new Values(student.StudentId, casuAux.PeriodId, casuAux.SubjectId, 
                        acumulativoEvaluaciones));
                    acumulativoEvaluaciones = 0;

                    if (casuAux.Period.Name.Equals("1er Lapso"))
                    {
                        listaValues_2.Add(new Values_2(student.StudentId, casuAux.SubjectId, 0));
                        if(control)
                            listaMaterias.Add(casu.Subject);
                    }
                }
                diccionarioDefinitivas.Add(student.StudentId, 0);
                control = false;
            }

            foreach(Values value in listaValues)
            {
                listaValues_2.Where(m => m.idEstudiante == value.idEstudiante && m.idMateria == value.idMateria)
                    .FirstOrDefault().acumulativo += value.acumulativo;
            }

            foreach(Values_2 values_2 in listaValues_2)
            {
                values_2.acumulativo = (double)values_2.acumulativo / 3;
                diccionarioDefinitivas[values_2.idEstudiante] += values_2.acumulativo;
            }

            foreach (KeyValuePair<int, double> valor in diccionarioDefinitivas)
            {
                diccionarioDefinitivas2.Add(valor.Key, (double)valor.Value / listaMaterias.Count());
            }

            return diccionarioDefinitivas2;
        }
        #endregion
    }
}