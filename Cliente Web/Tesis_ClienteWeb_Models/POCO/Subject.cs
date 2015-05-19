using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Display(Name = "Nombre de la materia:")]
        [Required(ErrorMessage = "Por favor inserte el nombre de la materia.", AllowEmptyStrings = false)]
        [StringLength(70, 
            MinimumLength = 5,
            ErrorMessage = "El nombre de la materia debe tener un mínimo de {2} caracteres y un máximo de " + 
            "{1} caracteres.")]
        [MaxLength(70)]
        public string Name { get; set; }

        public string SubjectCode { get; set; }

        [Display(Name= "Pensum de la materia:")]
        [StringLength(700, 
            ErrorMessage = "El pensum de la materia debe tener un máximo de {1} caracteres.")]
        public string Pensum { get; set; }

        public int Grade { get; set; }

        public School School { get; set; }
        
        public List<CASU> CASUs { get; set; }

        public Subject()
        {
            this.CASUs = new List<CASU>();
        }
    }
}
