using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class IndicatorAssignation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IndicatorAssignationId { get; set; }

        //Required - Fluent API
        public int IndicatorId { get; set; }
        public int CompetencyId { get; set; }
        public int AssessmentId { get; set; }
        public IndicatorAssessment IndicatorAssessment { get; set; }

        public string LetterScore { get; set; }

        /// <summary>
        /// Valor asociado a ese indicador sobre la nota obtenida
        /// </summary>
        public int Assignation { get; set; }

        public IndicatorAssignation()
        {

        }
    }
}
