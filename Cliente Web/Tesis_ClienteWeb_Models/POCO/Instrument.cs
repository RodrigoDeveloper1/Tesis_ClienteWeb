using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Instrument
    {
        [Key]
        public int InstrumentId { get; set; }

        [Required(ErrorMessage = "Por favor insertar el nombre del Instrumento", AllowEmptyStrings = false)]
        [StringLength(80, ErrorMessage = "El nombre debe tener un máximo de 80 caracteres")]
        [MaxLength(80)]
        public string Name { get; set; }

        public virtual List<Assessment> Assessments { get; set; }
    }
}
