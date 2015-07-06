using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class EvaluacionModel : MaestraListaColegiosModel
    {
        public Assessment Evaluacion { get; set; }              

        public List<Assessment> listaEvaluaciones { get; set; }
      
        [Display(Name = "Lista de tipos:")]
        public string TipoEvaluacion { get; set; }
        public SelectList selectListTipos{ get; set; }

        [Display(Name = "Lista de instrumentos:")]
        public string InstrumentoEvaluacion { get; set; }
        public SelectList selectListInstrumentos { get; set; }

        [Display(Name = "Lista de técnicas:")]
        public string TecnicaEvaluacion { get; set; }
        public SelectList selectListTecnicas { get; set; }

        public List<Course> listaCursos { get; set; }
        public List<Subject> listaMaterias { get; set; }
        public List<Period> listaLapsos { get; set; }

        [Display(Name = "Lista de cursos:")]
        [Required(ErrorMessage = "Por favor seleccione el curso.")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        [Display(Name = "Lista de profesores:")]
        [Required(ErrorMessage = "Por favor seleccione el profesor.")]
        public string idProfesor { get; set; }
        public SelectList selectListProfesores { get; set; }

        [Display(Name = "Lista de materias:")]
        [Required(ErrorMessage = "Por favor seleccione la materia.")]
        public int idMateria { get; set; }
        public SelectList selectListMaterias { get; set; }      

        [Display(Name = "Lista de lapsos:")]
        [Required(ErrorMessage = "Por favor seleccione el lapso.")]
        public int idLapso { get; set; }
        public SelectList selectListLapsos { get; set; }

        [Display(Name = "Nombre de la evaluación:")]      
        public string Name { get; set; }

        public Course Course { get; set; }

        public Subject SchoolSubject { get; set; }

        [Display(Name = "%: ")] 
        public int Percentage { get; set; }

        [Display(Name = "Fecha inicio:")] 
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha fin:")] 
        public DateTime FinishDate { get; set; }

        [Display(Name = "Hora inicio:")] 
        public string StartHour { get; set; }

        [Display(Name = "Hora fin:")] 
        public string EndHour { get; set; }

        public List<string> listaTiposNormal;

        public List<string> listaInstrumentosNormal;

        public List<string> listaTecnicasNormal;

        public EvaluacionModel()
        {
            selectListTipos = new SelectList(new Dictionary<string, string>());
            selectListInstrumentos = new SelectList(new Dictionary<string, string>());
            selectListTecnicas = new SelectList(new Dictionary<string, string>());
            selectListCursos = new SelectList(new Dictionary<string, string>());
            selectListProfesores = new SelectList(new Dictionary<string, string>());
            selectListMaterias = new SelectList(new Dictionary<string, string>());
            selectListLapsos = new SelectList(new Dictionary<string, string>());

            listaTiposNormal = new List<string>();
            listaInstrumentosNormal = new List<string>();
            listaTecnicasNormal = new List<string>();
        }
    }

    public class CrearEvaluacionAvanzadaModel : MaestraListaColegiosModel
    {
        public Assessment Evaluacion { get; set; }

        [Display(Name = "Lista de tipos:")]
        public string TipoEvaluacion { get; set; }
        public SelectList selectListTipos { get; set; }

        [Display(Name = "Lista de instrumentos:")]
        public string InstrumentoEvaluacion { get; set; }
        public SelectList selectListInstrumentos { get; set; }

        [Display(Name = "Lista de técnicas:")]
        public string TecnicaEvaluacion { get; set; }
        public SelectList selectListTecnicas { get; set; }

        public List<Course> listaCursos { get; set; }
        public List<Subject> listaMaterias { get; set; }
        public List<Period> listaLapsos { get; set; }

        [Display(Name = "Lista de cursos:")]
        [Required(ErrorMessage = "Por favor seleccione el curso.")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        [Display(Name = "Lista de materias:")]
        [Required(ErrorMessage = "Por favor seleccione la materia.")]
        public int idMateria { get; set; }
        public SelectList selectListMaterias { get; set; }

        [Display(Name = "Lista de lapsos:")]
        [Required(ErrorMessage = "Por favor seleccione el lapso.")]
        public int idLapso { get; set; }
        public SelectList selectListLapsos { get; set; }

        [Display(Name = "Nombre de la evaluación:")]
        public string Name { get; set; }

        public Course Course { get; set; }

        public Subject SchoolSubject { get; set; }

        [Display(Name = "%: ")]
        public int Percentage { get; set; }

        [Display(Name = "Fecha inicio:")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha fin:")]
        public DateTime FinishDate { get; set; }

        [Display(Name = "Hora inicio:")]
        public string StartHour { get; set; }

        [Display(Name = "Hora fin:")]
        public string EndHour { get; set; }

        public List<string> listaTiposNormal;

        public List<string> listaInstrumentosNormal;

        public List<string> listaTecnicasNormal;

        public CrearEvaluacionAvanzadaModel()
        {
            selectListTipos = new SelectList(new Dictionary<string, string>());
            selectListInstrumentos = new SelectList(new Dictionary<string, string>());
            selectListTecnicas = new SelectList(new Dictionary<string, string>());
            selectListCursos = new SelectList(new Dictionary<string, string>());
            selectListMaterias = new SelectList(new Dictionary<string, string>());
            selectListLapsos = new SelectList(new Dictionary<string, string>());

            listaTiposNormal = new List<string>();
            listaInstrumentosNormal = new List<string>();
            listaTecnicasNormal = new List<string>();
        }
    }

    public class MatrizIndicadoresLiteralesModel : MaestraModel
    {
        public Assessment assessment { get; set; }

        [Required(ErrorMessage = "Por favor seleccione una competencia.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de competencias:")]
        public int idCompetencia { get; set; }
        public SelectList selectListCompetencies { get; set; }

        public MatrizIndicadoresLiteralesModel(Assessment assessment)
        {
            this.assessment = assessment;
            this.selectListCompetencies = new SelectList(new Dictionary<string, string>());
        }
    }
}