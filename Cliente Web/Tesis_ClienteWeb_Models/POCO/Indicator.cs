using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Indicator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IndicatorId { get; set; }

        //Required - Fluent APi
        public int CompetencyId { get; set; }
        public Competency Competency { get; set; }

        public string Description { get; set; }

        public List<IndicatorAssessment> IndicatorAssessments { get; set; }

        public Indicator()
        {
            IndicatorAssessments = new List<IndicatorAssessment>();
        }
    }
}
