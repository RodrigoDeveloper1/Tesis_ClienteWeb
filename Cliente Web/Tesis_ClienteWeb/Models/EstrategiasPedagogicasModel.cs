using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tesis_ClienteWeb.Models
{
    public class EstrategiasPedagogicasModel : MaestraModel
    {
        [Display(Name = "Lista de cursos:")]
        [Required(ErrorMessage = "Por favor seleccione el curso.")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        [Display(Name = "Lista de materias")]
        [Required(ErrorMessage = "Por favor seleccione la materia.")]
        public int idMateria { get; set; }
        public SelectList selectListMaterias { get; set; }

        [Display(Name = "Lista de lapsos:")]
        [Required(ErrorMessage = "Por favor seleccione el lapso.")]
        public int idLapso { get; set; }
        public SelectList selectListLapsos { get; set; }

        public EstrategiasPedagogicasModel()
        {
            this.selectListCursos = new SelectList(new Dictionary<string, string>());
            this.selectListMaterias = new SelectList(new Dictionary<string, string>());
            this.selectListLapsos = new SelectList(new Dictionary<string, string>());
        }
    }
}