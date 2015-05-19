using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Period //Lapses
    {
        [Key]
        public int PeriodId { get; set; }

        [Required(ErrorMessage = "Por favor insertar el nombre del lapso", AllowEmptyStrings = false)]
        [StringLength(20, ErrorMessage = "El nombre debe tener un máximo de 20 caracteres")]
        [MaxLength(20)]
        public string Name { get; set; } //Constante

        [Required(ErrorMessage = "Por favor inserte la fecha de inicio del/los lapso(s) correspondiente(s)", 
            AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "Por favor inserte la fecha de finalización del/los lapso(s) correspondiente(s).", 
            AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]
        public DateTime FinishDate { get; set; }

        public SchoolYear SchoolYear { get; set; }

        public List<CASU> CASUs { get; set; }

        public Period()
        {
            this.CASUs = new List<CASU>();
        }
    }
}
