using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class CalificacionesModel : MaestraListaColegiosModel
    {
        [Display(Name = "Lista de cursos:")]
        [Required(ErrorMessage = "Por favor seleccione el curso.")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        [Display(Name = "Lista Materias")]
        [Required(ErrorMessage = "Por favor seleccione la materia.")]
        public int idMateria { get; set; }
        public SelectList selectListMaterias { get; set; }

        [Display(Name = "Lista de lapsos:")]
        [Required(ErrorMessage = "Por favor seleccione el lapso.")]
        public int idLapso { get; set; }
        public SelectList selectListLapsos { get; set; }

        [Required(ErrorMessage = "Por favor seleccione un estudiante.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de estudiantes:")]
        public int idEstudiante { get; set; }
        public SelectList selectListEstudiantes { get; set; }

        [Required(ErrorMessage = "Por favor seleccione una evaluación.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de evaluaciones:")]
        public int idEvaluacion { get; set; }
        public SelectList selectListEvaluaciones { get; set; }
        public float NumberScore { get; set; }

        public string LetterScore { get; set; }

        [Display(Name = "Nota del estudiante:")]
        public string Nota { get; set; }
    }
}