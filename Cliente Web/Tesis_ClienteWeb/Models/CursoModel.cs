using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class CursosModel : MaestraListaColegiosModel
    {

        public List<Course> listaCursos { get; set; }

        public Course Course { get; set; }

        public List<Subject> listaMaterias { get; set; }

        public SchoolYear SchoolYear { get; set; }

        public string CourseId { get; set; }

        public string Name { get; set; }

        public string NumeroAlumnos { get; set; }

        public string Grade { get; set; }

        public string Section { get; set; }


        public int StudentTotal { get; set; }
             
        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string Status { get; set; }

        public int idAnoEscolar { get; set; }
        [Display(Name = "Año escolar activo respectivo:")]
        public string labelAnoEscolar { get; set; }

        // ALumnos

        public int ListNumber { get; set; }

        public string FirstLastName { get; set; }

        public string SecondLastName { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }


    }
}