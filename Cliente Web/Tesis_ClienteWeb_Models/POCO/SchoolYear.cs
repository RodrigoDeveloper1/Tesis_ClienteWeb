using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    /* No se puede crear un año escolar activo para un colegio el cual dicho colegio ya posea un año escolar 
     * activo*/
    public class SchoolYear
    {
        [Key]
        public int SchoolYearId { get; set; }

        [Display(Name = "Fecha de inicio:")]
        [Required(ErrorMessage = "Por favor inserte la fecha de inicio del año escolar.", AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha de finalización:")]
        [Required(ErrorMessage = "Por favor inserte la fecha de finalización del año escolar.", 
            AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Si el estatus es true, el año escolar está activo, si es false, el año escolar está está inactivo y
        /// no se le puede agregar valores.
        /// </summary>
        public bool Status { get; set; } 

        public School School { get; set; }

        public List<Period> Periods { get; set; }
        public List<Event> Events { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<PsychologicalTest> PsychologicalTests { get; set; }

        public SchoolYear()
        {
            this.Periods = new List<Period>();
            this.Events = new List<Event>();
            this.Notifications = new List<Notification>();
            this.PsychologicalTests = new List<PsychologicalTest>();
        }
    }
}
