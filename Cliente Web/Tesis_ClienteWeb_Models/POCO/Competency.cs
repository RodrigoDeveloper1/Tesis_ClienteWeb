using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Competency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CompetencyId { get; set; }

        public string Description { get; set; }

        public Subject Subject { get; set; }

        public List<Indicator> Indicators { get; set; }

        public Competency()
        {
            Indicators = new List<Indicator>();
        }
    }
}
