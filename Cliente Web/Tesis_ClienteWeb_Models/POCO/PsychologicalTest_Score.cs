using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class PsychologicalTest_Score
    {
        /// <summary>
        /// Dominio:
        /// 1: Razonamiento Verbal
        /// 2: Razonamiento Matemático
        /// </summary>
        //Required - FluentAPI
        public int ReasoningType { get; set; }
        
        //Required - FluentAPI
        public int StudentId { get; set; }
        public Student Student { get; set; }

        //Required - FluentAPI
        public int PsychologicalTestId { get; set; }
        public PsychologicalTest PsychologicalTest { get; set; }

        [Required]
        public float Score { get; set; }
    }
}
