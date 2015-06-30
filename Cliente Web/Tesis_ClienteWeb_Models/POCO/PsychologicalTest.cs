using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class PsychologicalTest
    {
        [Key]
        public int PsychologicalTestId { get; set; }

        [Required(ErrorMessage = "Por favor inserte el nombre del test.", AllowEmptyStrings = false)]
        public string TestName { get; set; }

        [Required(ErrorMessage = "Por favor inserte la fecha de creación del test.", AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]
        public DateTime DateOfCreation { get; set; }

        public SchoolYear SchoolYear { get; set; }

        public List<PsychologicalTest_Score> PsychologicalTest_Scores { get; set; }

        public PsychologicalTest()
        {
            this.PsychologicalTest_Scores = new List<PsychologicalTest_Score>();
        }
    }
}
