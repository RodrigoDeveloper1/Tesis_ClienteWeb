using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class GestionRepresentantesModel : MaestraListaColegiosModel
    {
        public int RepresentativeId { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Por favor seleccione un curso.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de cursos activos:")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }


        public Representative Representante { get; set; }

        public List<Representative> listaRepresentantes { get; set; }
    }
}