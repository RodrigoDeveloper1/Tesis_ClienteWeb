using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tesis_ClienteWeb_Models.POCO
{
    public  class School
    {
        [Key]
        public int SchoolId { get; set; }

        [Index("School_NameIndex", IsUnique = true)]
        [Display(Name = "Nombre del colegio:")]
        [Required(ErrorMessage = "Por favor insertar el nombre del colegio.", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "El nombre debe tener un máximo de 100 caracteres")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "Fecha de creación:")]
        [Required(ErrorMessage = "Por favor especificar la fecha de creación del colegio.", 
            AllowEmptyStrings = false)]
        public DateTime DateOfCreation { get; set; }

        [Display(Name = "Estatus del colegio:")]
        public bool Status { get; set; }

        public List<SchoolYear> SchoolYears { get; set; } /* Un colegio solo puede tener un año escolar activo
                                                           * por año.
                                                           */

        public List<User> Users { get; set; }

        public List<Subject> Subjects { get; set; }

        public School()
        {
            this.DateOfCreation = DateTime.Now;
         
            this.SchoolYears = new List<SchoolYear>();
            this.Users = new List<User>();
            this.Subjects = new List<Subject>();
        }
    }
}
