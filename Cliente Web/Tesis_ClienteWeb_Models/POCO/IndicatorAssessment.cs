using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class IndicatorAssessment
    {
        //Required - Fluent API
        public int IndicatorId { get; set; }
        public int CompetencyId { get; set; }
        public Indicator Indicator { get; set; }

        //Required - Fluent API
        public int AssessmentId { get; set; }
        public Assessment Assessment { get; set; }

        public List<IndicatorAssignation> IndicatorAsignations { get; set; }

        public IndicatorAssessment()
        {
            IndicatorAsignations = new List<IndicatorAssignation>();
        }
    }
}
