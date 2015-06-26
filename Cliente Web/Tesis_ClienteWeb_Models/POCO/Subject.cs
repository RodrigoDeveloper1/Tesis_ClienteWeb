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

        [Display(Name= "Objetivo general:")]
        public string GeneralPurpose { get; set; }

        public int Grade { get; set; }

        public School School { get; set; }
        
        public List<CASU> CASUs { get; set; }

        public List<ContentBlock> ContentBlocks { get; set; }

        public List<Competency> Competencies { get; set; }

        public Subject()
        {
            this.CASUs = new List<CASU>();
            this.ContentBlocks = new List<ContentBlock>();
            this.Competencies = new List<Competency>();
        }
    }
}
