using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Assessment //Asignaciones/Evaluaciones
    {
        [Key]
        public int AssessmentId { get; set; }

        [Required(ErrorMessage = "Por favor insertar el nombre de la evaluación", AllowEmptyStrings = false)]
        [StringLength(80, ErrorMessage = "El nombre debe tener un máximo de 80 caracteres")]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Por favor insertar el porcentaje de la evaluación", AllowEmptyStrings = false)]
        public int Percentage { get; set; }

        [Required(ErrorMessage = "Por favor insertar fecha de inicio de la evaluación", AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required(
            ErrorMessage = "Por favor insertar la fecha de finalización de la evaluación", 
            AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]               
        public DateTime FinishDate { get; set; }    /* Si el tipo de evaluación termina el mismo día, la fecha 
                                                     * de finalización es igual que la fecha de inicio         
                                                     */
        
        [StringLength(20, ErrorMessage = "La hora debe tener un máximo de 20 caracteres")]
        [MaxLength(20)]
        public string StartHour { get; set; }
        
        [StringLength(20, ErrorMessage = "La hora debe tener un máximo de 20 caracteres")]
        [MaxLength(20)]
        public string EndHour { get; set; }

        [Display(Name = "Tipo de actividad:")]
        [Required(ErrorMessage = "Por favor seleccione el tipo de actividad.", AllowEmptyStrings = true)]
        public string Activity { get; set; } //El tipo de evaluación. Ej: Taller, Exposición, Dramatización...

        [Display(Name = "Tipo de técnica:")]
        [Required(ErrorMessage = "Por favor seleccione el tipo de técnica.", AllowEmptyStrings = true)]
        public string Technique { get; set; } /* Medio utilizado para apreciar el rendimiento del alumndo
                                               */

        [Display(Name = "Tipo de instrumento:")]
        [Required(ErrorMessage = "Por favor seleccione el tipo de instrumento.", AllowEmptyStrings = true)]
        public string Instrument { get; set; } /* Medios que sirven para aplicar las técnicas.
                                                */

        [Required(ErrorMessage = "Por favor indicar el período, el curso y la materia asociada a la" + 
            " asignación", AllowEmptyStrings = false)]
        /// <summary>
        /// La asignación siempre debe ir asignada a un Curso, Período y Materia
        /// </summary>
        public CASU CASU { get; set; }

        public List<Score> Scores { get; set; }

        public Event Event { get; set; }

        public Assessment()
        {
            this.Scores = new List<Score>();
        }
     }
}