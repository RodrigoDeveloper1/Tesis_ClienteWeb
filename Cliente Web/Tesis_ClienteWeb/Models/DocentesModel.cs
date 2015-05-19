using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tesis_ClienteWeb.Models
{
    public class AgregarDocenteModel : MaestraListaColegiosModel
    {                
        [Display(Name = "Año escolar activo respectivo:")]
        public string labelAnoEscolar { get; set; }
        public int idAnoEscolar { get; set; }

        [Required(ErrorMessage = "Por favor seleccione un curso.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de cursos:")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        [Display(Name = "Lista de materias:")]
        public int idMateria { get; set; }
        public SelectList selectListMaterias { get; set; }

        [Display(Name = "Lista de docentes:")]
        public int idDocente { get; set; }
        public SelectList selectListDocentes { get; set; }

        public AgregarDocenteModel()
        {
            this.selectListCursos = new SelectList(new Dictionary<string, string>());
            this.selectListMaterias = new SelectList(new Dictionary<string, string>());
            this.selectListDocentes = new SelectList(new Dictionary<string, string>());
        }
    }
}